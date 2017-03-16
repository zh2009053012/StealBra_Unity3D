using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class example : MonoBehaviour {
	AStarMap map;
	List<AStarMapCell> path = new List<AStarMapCell>();
	// Use this for initialization
	void Start () {
		map = AStarMapStream.Read (Application.dataPath+"/scene1.txt");
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
