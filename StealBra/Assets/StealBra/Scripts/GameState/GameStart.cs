using UnityEngine;
using System.Collections;

public class GameStart : GameStateBase {

	// Use this for initialization
	void Start () {
		FSM = new StateMachine (this);
		GameStateManager.Instance ().FSM = FSM;

		FSM.ChangeState (GameStateStart.Instance ());
//		FSM.GlobalState = GameGlobalState.Instance ();
//		FSM.GlobalState.Enter (FSM.Owner);
	}

}
