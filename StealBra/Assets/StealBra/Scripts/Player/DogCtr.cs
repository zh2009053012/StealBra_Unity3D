using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerData))]
public class DogCtr : MonoBehaviour {

	private PlayerData m_data;
	private AStarMap m_astarMap;
	private float vInput, hInput;
	private PlayerCtr m_target;
	// Use this for initialization
	public void Init(int row, int col, MapCtr map, AStarMap astarMap, PlayerCtr target){
		m_data = GetComponent<PlayerData> ();
		m_data.Init (row, col, map);
		m_astarMap = astarMap;
		m_target = target;

		m_data.RegisterMoveOverEvent(MoveOver);
		FindPath();
	}
	void MoveOver(PlayerData pd){
		object[] p = new object[2];
		p[0] = (object)pd.Row;
		p[1] = (object)pd.Column;
		if(pd.Row == m_target.Row && pd.Column == m_target.Column){
			GameStateManager.Instance().FSM.CurrentState.Message("GetPlayer", p);
		}else{
			FindPath();
		}
	}
	//
	void FindPath(){
		AStarMapCell start = m_astarMap.GetCell( m_data.Row, m_data.Column);
		AStarMapCell end = m_astarMap.GetCell( m_target.Row, m_target.Column);
		List<AStarMapCell> list = AStar.CalculatePath(start, end, m_astarMap);
		if(list.Count > 0){
			m_data.Move(list[0].Column - m_data.Column, list[0].Row - m_data.Row);
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
