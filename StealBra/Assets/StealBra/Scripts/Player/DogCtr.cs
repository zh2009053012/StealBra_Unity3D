using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerData))]
public class DogCtr : MonoBehaviour {
	protected bool m_canControl=true;
	//dead : false; not dead : true;
	public bool CanControl{
		get{return m_canControl;}
		set{
			m_canControl = value;
			if(!m_canControl){
				AudioManager.Instance.StopAudio("dog");
			}
		}
	}
	private PlayerData m_data;
	private AStarMap m_astarMap;
	private float vInput, hInput;
	private PlayerCtrEx m_target;
	private int startRow, startCol;
	private bool m_isDead = false;
	public int Row{
		get{return m_data.Row;}
	}
	public int Column{
		get{return m_data.Column;}
	}
	public void Init(int row, int col, MapCtr map, AStarMap astarMap, PlayerCtrEx target, bool isLeft){
		startRow = row;
		startCol = col;
		m_data = GetComponent<PlayerData> ();
		m_data.Init (row, col, map);
		m_astarMap = astarMap;
		m_target = target;

		m_data.RegisterMoveOverEvent(AutoMoveOver);
		if(isLeft){
			if(m_astarMap.GetCell(m_data.Row, m_data.Column).IsToLeft)
				m_data.Move(-1, 0);
			else
				m_data.Move(1, 0);
		}else{
			if(m_astarMap.GetCell(m_data.Row, m_data.Column).IsToRight)
				m_data.Move(1, 0);
			else
				m_data.Move(-1, 0);
		}
	}
	void Update(){
//		if(m_target != null && Time.frameCount%5 == 0){
//			if(Vector3.Distance(m_target.gameObject.transform.position, this.gameObject.transform.position) <= 
//				m_data.Map.CellSize.x){
//				SendGameOverMsg();
//			}
//		}
	}
	void SendGameOverMsg(){
		object[] p = new object[2];
		p[0] = (object)m_data.Row;
		p[1] = (object)m_data.Column;
		GameStateManager.Instance().FSM.CurrentState.Message("PlayerDead", p);
	}
	public void AutoMoveOver(PlayerData pd){
		if (!CanControl)
			return;
		object[] p = new object[2];
		p[0] = (object)pd.Row;
		p[1] = (object)pd.Column;
//		if(pd.Row == m_target.Row && pd.Column == m_target.Column){
//			GameStateManager.Instance().FSM.CurrentState.Message("PlayerDead", p);
//			return;
//		}

		if(m_data.IsLookLeft){
			if(m_astarMap.GetCell(m_data.Row, m_data.Column).IsToLeft)
				m_data.Move(-1, 0);
			else
				m_data.Move(1, 0);
		}else{
			if(m_astarMap.GetCell(m_data.Row, m_data.Column).IsToRight)
				m_data.Move(1, 0);
			else
				m_data.Move(-1, 0);
		}
	}
	public void DoDead(){
		CanControl = false;
		DeadEffect deCtr = gameObject.GetComponent<DeadEffect> ();
		if (null == deCtr) {
			deCtr = gameObject.AddComponent<DeadEffect> ();
		}
		deCtr.Play (2, OnDeadCallback);
	}
	void OnDeadCallback(GameObject go){
		StartCoroutine (Resurrection(10));
	}
	IEnumerator Resurrection(float second){
		yield return new WaitForSeconds (second);

		DeadEffect deCtr = gameObject.GetComponent<DeadEffect> ();
		if (null != deCtr) {
			deCtr.Stop ();
		}
		Reset ();
		CanControl = true;
		if(m_astarMap.GetCell(m_data.Row, m_data.Column).IsToLeft){
			m_data.Move(-1, 0);
		}else{
			m_data.Move(1, 0);
		}
	}
	// Use this for initialization
	public void Init(int row, int col, MapCtr map, AStarMap astarMap, PlayerCtrEx target){
		startRow = row;
		startCol = col;
		m_data = GetComponent<PlayerData> ();
		m_data.Init (row, col, map);
		m_astarMap = astarMap;
		m_target = target;

		m_data.RegisterMoveOverEvent(MoveOver);
		FindPath();
	}
	public void Reset(){
		m_data.Init (startRow, startCol, m_data.Map);

		FindPath();
	}
	void MoveOver(PlayerData pd){
		if(null == m_target || !CanControl)
			return;
		object[] p = new object[2];
		p[0] = (object)pd.Row;
		p[1] = (object)pd.Column;
		if(pd.Row == m_target.Row && pd.Column == m_target.Column){
			//GameStateManager.Instance().FSM.CurrentState.Message("PlayerDead", p);
		}else{
			FindPath();
		}
	}
	//
	void FindPath(){
		if(null == m_target || !CanControl)
			return;
		AStarMapCell start = m_astarMap.GetCell( m_data.Row, m_data.Column);
		AStarMapCell end = m_astarMap.GetCell( m_target.Row, m_target.Column);
		List<AStarMapCell> list = AStar.CalculatePath(start, end, m_astarMap);
		if(list.Count > 0){
			m_data.Move(list[0].Column - m_data.Column, list[0].Row - m_data.Row);
			AudioManager.Instance.PlayAudio("dog", true);
		}else{
			if (m_data.IsLookLeft) {
				m_data.Move (-1, 0);
			}else{
				m_data.Move (1, 0);
			}
			StartCoroutine(RetryFindPath(1));
		}
	}
	IEnumerator RetryFindPath(float second){
		yield return new WaitForSeconds(second);
		FindPath();
	}
}
