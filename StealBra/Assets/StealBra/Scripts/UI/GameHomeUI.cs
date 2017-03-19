using UnityEngine;
using System.Collections;

public class GameHomeUI : MonoBehaviour {

	public void OnPlayBtnClick(){
		AudioManager.Instance.PlayAudio("hit");
		GameStateManager.Instance().FSM.CurrentState.Message("PlayGame", null);
	}
}
