using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar {

	private static List<MapCell> m_openList = new List<MapCell>();
	private static List<MapCell> m_closeList = new List<MapCell>();
	private static Map m_map;

	public static List<MapCell> CalculatePath(MapCell start, MapCell end, Map map){
		m_openList.Clear ();
		m_closeList.Clear ();
		m_map = map;
		ResetMap ();

		start.G = 0;
		start.CalculateH (end);

		m_openList.Add (start);
		start.IsInOpenList = true;

		while (true) {
			int nearestIndex = FindNearestCellInOpenList ();
			MapCell cur;
			if (nearestIndex >= 0) {
				cur = m_openList [nearestIndex];
				if (cur.Equals (end)) {
					return BacktrackPath (cur);
				}
			} else {
				return new List<MapCell> ();
			}

			for (int i = cur.Row - 1; i <= cur.Row + 1; i++) {
				for (int j = cur.Column - 1; j <= cur.Column + 1; j++) {
					if (i >= 0 && i < m_map.Row && j >= 0 && j < m_map.Column) {
						MapCell mc = m_map.GetCell (i, j);

						if (mc != null && !mc.IsInCloseList && cur.CanArrive(mc)) {
							if (mc.IsInOpenList) {
								int dist = cur.CalculateNeighborDist (mc);
								if (cur.G + dist < mc.G) {
									mc.G = cur.G + dist;
									mc.SetParent( cur);
								}
							} else {
								mc.SetParent( cur );
								mc.CalculateH (end);
								mc.CalculateG (cur);
								mc.IsInOpenList = true;
								m_openList.Add (mc);
							}
						}
					}
				}
			}
			m_openList.RemoveAt (nearestIndex);
			cur.IsInOpenList = false;
			m_closeList.Add (cur);
			cur.IsInCloseList = true;
		}
		return new List<MapCell> ();
	}
	private static void ResetMap(){
		for (int i = 0; i < m_map.Row; i++) {
			for (int j = 0; j < m_map.Column; j++) {
				MapCell mc = m_map.GetCell (i, j);
				if (mc != null) {
					mc.IsInOpenList = false;
					mc.IsInCloseList = false;
					mc.SetParent( null);
					mc.H = 0;
					mc.G = 0;
				}
			}
		}
	}
	private static List<MapCell> BacktrackPath(MapCell end){
		if (end == null)
			return new List<MapCell> ();
		List<MapCell> list = new List<MapCell> ();
		MapCell mc = end;
		while (mc.GetParent() != null) {
			list.Insert (0, mc);
			mc = mc.GetParent();
		}
		return list;
	}

	private static int FindNearestCellInOpenList(){
		int F = int.MaxValue;
		int index = -1;
		for (int i = 0; i < m_openList.Count; i++) {
			if (m_openList [i].F < F) {
				F = m_openList [i].F;
				index = i;
			}
		}
		return index;
	}
}
