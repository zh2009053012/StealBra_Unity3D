using UnityEngine;
using System.Collections;

public class GameStateBase : MonoBehaviour {
	public StateMachine FSM;

	// Update is called once per frame
	void Update () {
		if (null != FSM)
			FSM.Update ();
	}
	void OnDestroy()
	{
		if (FSM != null && FSM.GlobalState != null) {
			FSM.GlobalState.Exit (FSM.Owner);
			FSM.CurrentState.Exit (FSM.Owner);
		}
	}
}
