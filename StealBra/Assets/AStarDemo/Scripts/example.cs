using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class example : MonoBehaviour {
	Map map;
	List<MapCell> path = new List<MapCell>();
	// Use this for initialization
	void Start () {
		map = MapStream.Read (Application.dataPath+"/scene1.txt");
		map.CalculateCellPosition (new Vector2 (2.56f, 2.56f), Vector2.zero);
		path = AStar.CalculatePath (map.GetCell (0, 0), map.GetCell (map.Row - 1, map.Column - 1), map);

	}
	
	void OnDrawGizmos(){
		for (int i = 0; i < path.Count-1; i++) {
			Gizmos.color = Color.red;
			Gizmos.DrawLine (path [i].GetPosition(), path [i + 1].GetPosition());
		}
	}
}
