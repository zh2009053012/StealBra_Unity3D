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
	MapCtr m_mapCtr;
	PlayerCtr m_playerCtr;
	public void Enter(GameStateBase owner)
	{
		m_owner = (GameStart)owner;

		GameObject map = new GameObject ();
		map.name = "map";
		m_mapCtr = map.AddComponent<MapCtr> ();
		m_mapCtr.ReadMap ("scene1");
		//
		GameObject prefab = Resources.Load("Player")as GameObject;
		GameObject player = GameObject.Instantiate (prefab);
		m_playerCtr = player.GetComponent<PlayerCtr> ();
		m_playerCtr.Init (2, 2, m_mapCtr);
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
