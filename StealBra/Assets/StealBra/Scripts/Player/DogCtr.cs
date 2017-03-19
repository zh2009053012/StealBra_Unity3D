﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerData))]
public class DogCtr : MonoBehaviour {
	protected bool m_canControl=true;
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
	private PlayerCtr m_target;
	private int startRow, startCol;
	public int Row{
		get{return m_data.Row;}
	}
	public int Column{
		get{return m_data.Column;}
	}
	// Use this for initialization
	public void Init(int row, int col, MapCtr map, AStarMap astarMap, PlayerCtr target){
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
			GameStateManager.Instance().FSM.CurrentState.Message("PlayerDead", p);
		}else{
			FindPath();
		}
	}
	//
	void FindPath(){
		if(null == m_target)
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
