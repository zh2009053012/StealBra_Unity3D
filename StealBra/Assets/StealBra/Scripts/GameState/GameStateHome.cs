using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStateHome : IStateBase {


	private GameStateHome()
	{}

	private static GameStateHome m_instance;
	private static object m_lockHelper = new object();
	public static GameStateHome Instance()
	{
		if (null == m_instance) {
			lock (m_lockHelper) {
				if (null == m_instance) {
					m_instance = new GameStateHome ();
				}
			}
		}
		return m_instance;
	}
	//
	//GameHome m_owner;
	GameHomeUI m_uiCtr;
	public void Enter(GameStateBase owner)
	{
		//m_owner = (GameHome)owner;
		GameObject prefab = Resources.Load("UI/HomeCanvas")as GameObject;
		GameObject player = GameObject.Instantiate (prefab);
		m_uiCtr = player.GetComponent<GameHomeUI> ();
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
		if(message.Equals("PlayGame")){

			GameStateManager.Instance().FSM.ChangeState(GameStateLevel.Instance());
		}
	}
}
