using UnityEngine;
using System.Collections;

public class GameStateStart : IStateBase {

	private GameStateStart()
	{}

	private static GameStateStart m_instance;
	private static object m_lockHelper = new object();
	public static GameStateStart Instance()
	{
		if (null == m_instance) {
			lock (m_lockHelper) {
				if (null == m_instance) {
					m_instance = new GameStateStart ();
				}
			}
		}
		return m_instance;
	}
	//
	GameStart m_owner;
	MapCtr m_mapCtr;
	FightUI m_uiCtr;
	FailureUI m_failCtr;
	VectoryUI m_vectoryCtr;
	int underpantNum=0, braNum=0;
	int m_countDownTotal = 0;
	float m_countDownCur = 0;
	bool m_isVectory = false;
	bool m_isFailed = false;

	public void Enter(GameStateBase owner)
	{
		underpantNum = 0;
		braNum = 0;
		m_owner = (GameStart)owner;
		m_isFailed = false;
		m_isVectory = false;
		//
		GameObject prefab = Resources.Load("UI/FightCanvas")as GameObject;
		GameObject go = GameObject.Instantiate(prefab);
		m_uiCtr= go.GetComponent<FightUI>();
		m_countDownTotal = GameData.CurLevel.countDown;
		m_countDownCur = m_countDownTotal;
		m_uiCtr.SetCountDown ((int)m_countDownCur, m_countDownTotal);
		//
		GameObject failPrefab = Resources.Load("UI/FailureCanvas")as GameObject;
		GameObject fail = GameObject.Instantiate(failPrefab);
		m_failCtr= fail.GetComponent<FailureUI>();
		m_failCtr.gameObject.SetActive(false);
		//
		GameObject vectoryPrefab = Resources.Load("UI/VectoryCanvas")as GameObject;
		GameObject vectory = GameObject.Instantiate(vectoryPrefab);
		m_vectoryCtr= vectory.GetComponent<VectoryUI>();
		m_vectoryCtr.gameObject.SetActive(false);
		//
		GameObject map = new GameObject ();
		map.name = "map";
		m_mapCtr = map.AddComponent<MapCtr> ();
		Debug.Log ("read map");
		m_mapCtr.ReadMap (GameData.CurLevel.levelFileName);
		//
		Camera.main.GetComponent<CameraFollow>().FollowTarget = m_mapCtr.Player.transform;
		AudioManager.Instance.PlayAudio ("bg", true);
	}

	public void Execute(GameStateBase owner)
	{
		//
		if (m_mapCtr.Player.CanControl) {
			m_countDownCur -= Time.deltaTime;
			m_uiCtr.SetCountDown ((int)m_countDownCur, m_countDownTotal);
			if (m_countDownCur <= 30) {
				m_uiCtr.PlayCountDownEffect ();
			}
			if (m_countDownCur <= 0) {
				Message ("PlayerDead", null);
			}
		}
	}

	public void Exit(GameStateBase owner)
	{
		AudioManager.Instance.StopAudio ("bg");
		m_mapCtr.Clear();
		Debug.Log ("clear map");
		if(null != m_uiCtr){
			GameObject.Destroy(m_uiCtr.gameObject);
			m_uiCtr =null;
		}
		if(null != m_failCtr){
			GameObject.Destroy(m_failCtr.gameObject);
			m_failCtr = null;
		}
		if(null != m_vectoryCtr){
			GameObject.Destroy(m_vectoryCtr.gameObject);
			m_vectoryCtr = null;
		}
	}

	public void Message(string message, object[] parameters)
	{
		if(message.Equals("MapCellRevert")){
			CheckDead(parameters);
		}else if(message.Equals("GetSocks")){
			m_mapCtr.Player.AddMoveSpeed(0.2f);
		}else if(message.Equals("BackBtnClick")){
			DoBack();
		}else if(message.Equals("GetUnderpant")){
			DoGetUnderpant();
		}else if(message.Equals("GetBra")){
			DoGetBra();
		}else if(message.Equals("Vectory")){
			DoVectory();
		}else if(message.Equals("PlayerDead")){
			DoPlayerDead();
		}else if(message.Equals("ShowLevelMenu")){
			DoShowLevel();
		}else if(message.Equals("RetryBtnClick")){
			DoRetryLevel();
		}

	}
	void DoShowLevel(){
		//SceneLoading.LoadSceneName = GlobalDefine.HomeSceneName;
		//UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(GlobalDefine.LoadSceneName);
		GameStateManager.Instance().FSM.ChangeState(GameStateLevel.Instance());
		Debug.Log ("DoShowLevel");
	}
	void DoRetryLevel(){
		GameStateManager.Instance().FSM.ChangeState(GameStateStart.Instance());
		Debug.Log ("DoRetryLevel");
	}
	void CheckDead(object[] p){
		int row = (int)p[0];
		int col = (int)p[1];
		if(m_mapCtr.Player.Row == row && m_mapCtr.Player.Column == col){
			DoPlayerDead();
		}
		foreach(DogCtr dog in m_mapCtr.m_dogList){
			if(dog.Row == row && dog.Column == col){
				dog.DoDead ();
			}
		}
	}

	void DoPlayerDead(){
		if (m_isFailed || m_isVectory) {
			return;
		}
		m_mapCtr.Player.CanControl = false;
		foreach(DogCtr dog in m_mapCtr.m_dogList){
			dog.CanControl = false;
		}
		AudioManager.Instance.PlayAudio("dead");
		DeadEffect deCtr = m_mapCtr.Player.gameObject.AddComponent<DeadEffect> ();
		deCtr.Play (2, (GameObject x)=>{
			m_failCtr.gameObject.SetActive(true);
		});
	}
	void DoVectory(){
		if (m_isFailed || m_isVectory) {
			return;
		}
		m_mapCtr.Player.CanControl = false;
		foreach(DogCtr dog in m_mapCtr.m_dogList){
			dog.CanControl = false;
		}
		AudioManager.Instance.PlayAudio("vectory",false);
		//
		int starNum = 1;
		if(braNum + underpantNum >= m_mapCtr.BraUnderpantNum){
			starNum= 3;
		}else if(braNum + underpantNum >= m_mapCtr.BraUnderpantNum*0.75f){
			starNum = 2;
		}else{
			starNum = 1;
		}
		m_vectoryCtr.gameObject.SetActive(true);
		m_vectoryCtr.SetStarNum(starNum);
		GameData.ClearLevel(GameData.CurLevel.id, starNum);
	}
	void DoGetBra(){
		braNum++;
		m_uiCtr.SetBraNum(braNum);
		CheckShowDoor();
	}
	void DoGetUnderpant(){
		underpantNum++;
		m_uiCtr.SetUnderpantNum(underpantNum);
		CheckShowDoor();
	}
	void CheckShowDoor(){
		//(int)(m_mapCtr.BraUnderpantNum*0.5f)
		if(!m_mapCtr.CanExit && underpantNum + braNum >= m_mapCtr.BraUnderpantNum*0.5f){
			m_mapCtr.ShowExit(true);
			AudioManager.Instance.PlayAudio("vectory", false);
		}
	}
	void DoBack(){
		SceneLoading.LoadSceneName = GlobalDefine.HomeSceneName;
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(GlobalDefine.LoadSceneName);
	}
}
