using UnityEngine;
using System.Collections;

public class MapCtr : MonoBehaviour {
	protected MapCellCtr[,] cells;
	public AStarMap PathfindingMap;
	protected MapData m_map;
	public MapData Map{
		get{ return m_map;}
	}
	public MapCellCtr GetMapCell(int row, int col){
		if (row >= 0 && col >= 0 && row < Row && col < Column) {
			return cells [col, row];
		}
		return null;
	}
	public Vector3 StartPos{
		get{ 
			return new Vector3 ((float)m_map.startPosX, (float)m_map.startPosY, (float)m_map.startPosZ);
		}
	}
	public Vector2 CellSize{
		get{ 
			return new Vector2 ((float)m_map.cellWidth, (float)m_map.cellHeight);
		}
	}
	public int Row{
		get{ return m_map.row;}
	}
	public int Column{
		get{return m_map.column;}
	}

	void FromMapData(MapData md){
		m_map = md;
		transform.position = new Vector3 ((float)md.startPosX, (float)md.startPosY, (float)md.startPosZ);

		cells = new MapCellCtr[m_map.column, m_map.row];
		//
		foreach (MapCellData item in md.cells) {
			GameObject go = new GameObject ();
			go.AddComponent<SpriteRenderer> ();
			MapCellCtr mcc = go.AddComponent<MapCellCtr> ();
			go.transform.parent = this.transform;
			go.name = item.col + "_" + item.row;
			mcc.Init (this, item);
			cells [item.col, item.row] = mcc;
		}
		//
		foreach(DecorationData item in md.decorations){
			GameObject go = new GameObject ();
			go.AddComponent<SpriteRenderer> ();
			go.transform.parent = this.transform;
			go.name = item.resName;
			go.transform.position = new Vector3 ((float)item.posX, (float)item.posY, (float)item.posZ);
			go.transform.rotation = Quaternion.Euler( new Vector3 ((float)item.eulerX, (float)item.eulerY, (float)item.eulerZ));
			go.transform.localScale = new Vector3 ((float)item.scaleX, (float)item.scaleY, (float)item.scaleZ);
			if (!string.IsNullOrEmpty (item.resName))
				go.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Sprites/" + item.resName, typeof(Sprite))as Sprite;
			else
				go.GetComponent<SpriteRenderer> ().sprite = null;
			go.GetComponent<SpriteRenderer> ().sortingOrder = item.sortOrder;
			go.GetComponent<SpriteRenderer> ().sortingLayerName = item.sortLayer;
		}
	}
	public void ReadMap(string fileName){
		if (string.IsNullOrEmpty (fileName)) {
			return ;
		}
		Debug.Log (Application.dataPath);
		string dir = Application.dataPath + "/";
		PathfindingMap = AStarMapStream.Read (dir+fileName+"_astar.txt");
		MapData map = MapStream.Read (dir+fileName+".txt");
		FromMapData (map);
	}
}
