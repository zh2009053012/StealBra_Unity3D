using UnityEngine;
using System.Collections;

public class GameStateStart : IStateBase {

	private GameStateStart()
	{}

	private static GameStateStart m_instance;
	private static object m_lockHelper = new object();
	public static GameStateStart Instance()
	{
		if (null == m_instance) {
			lock (m_lockHelper) {
				if (null == m_instance) {
					m_instance = new GameStateStart ();
				}
			}
		}
		return m_instance;
	}
	//
	GameStart m_owner;
	public void Enter(GameStateBase owner)
	{
		m_owner = (GameStart)owner;

		GameObject prefab = Resources.Load("Canvas")as GameObject;
		GameObject go = GameObject.Instantiate(prefab);

	}

	public void Execute(GameStateBase owner)
	{

	}

	public void Exit(GameStateBase owner)
	{

	}

	public void Message(string message, object[] parameters)
	{

	}
}
