using UnityEngine;
using System.Collections;

public class FailureUI : MonoBehaviour {

	public void OnLevelBtnClick(){
		GameStateManager.Instance().FSM.CurrentState.Message("ShowLevelMenu", null);
	}
	public void OnRetryBtnClick(){
		GameStateManager.Instance().FSM.CurrentState.Message("RetryBtnClick", null);
	}
}
