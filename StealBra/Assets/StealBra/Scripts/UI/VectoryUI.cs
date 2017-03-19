using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VectoryUI : MonoBehaviour {
	[SerializeField]
	protected List<GameObject> m_starList = new List<GameObject>();

	public void SetStarNum(int num){
		for(int i=0; i<m_starList.Count; i++){
			m_starList[i].SetActive(false);
		}
		for(int i=0; i<num && i<m_starList.Count; i++){
			m_starList[i].SetActive(true);
		}
	}
	public void OnLevelBtnClick(){
		GameStateManager.Instance().FSM.CurrentState.Message("ShowLevelMenu", null);
	}
	public void OnRetryBtnClick(){
		GameStateManager.Instance().FSM.CurrentState.Message("RetryBtnClick", null);
	}
}
