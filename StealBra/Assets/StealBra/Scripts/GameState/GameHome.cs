using UnityEngine;
using System.Collections;

public class GameHome : GameStateBase {


	// Use this for initialization
	void Start () {
		FSM = new StateMachine (this);
		GameStateManager.Instance ().FSM = FSM;

		FSM.ChangeState (GameStateHome.Instance ());
		//		FSM.GlobalState = GameGlobalState.Instance ();
		//		FSM.GlobalState.Enter (FSM.Owner);
	}


}
