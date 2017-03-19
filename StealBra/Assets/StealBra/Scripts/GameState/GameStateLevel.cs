using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStateLevel : IStateBase {


	private GameStateLevel()
	{}

	private static GameStateLevel m_instance;
	private static object m_lockHelper = new object();
	public static GameStateLevel Instance()
	{
		if (null == m_instance) {
			lock (m_lockHelper) {
				if (null == m_instance) {
					m_instance = new GameStateLevel ();
				}
			}
		}
		return m_instance;
	}
	//
	GameLevelUI m_uiCtr;
	public void Enter(GameStateBase owner)
	{
		GameObject prefab = Resources.Load("UI/LevelCanvas")as GameObject;
		GameObject player = GameObject.Instantiate (prefab);
		m_uiCtr = player.GetComponent<GameLevelUI> ();
		m_uiCtr.Init();
	}

	public void Execute(GameStateBase owner)
	{

	}

	public void Exit(GameStateBase owner)
	{
		if(null != m_uiCtr){
			GameObject.Destroy(m_uiCtr.gameObject);
			m_uiCtr = null;
		}
	}

	public void Message(string message, object[] parameters)
	{
		if(message.Equals("Back")){
			GameStateManager.Instance().FSM.ChangeState(GameStateHome.Instance());
		}else if(message.Equals("SelectLevel")){
			DoSelectLevel(parameters);
		}
	}
	void DoSelectLevel(object[] p){
		LevelData ld = (LevelData)(p[0]);
		GameData.CurLevel = ld;
		SceneLoading.LoadSceneName = GlobalDefine.FightSceneName;
		SceneManager.LoadSceneAsync(GlobalDefine.LoadSceneName);
	}
}
