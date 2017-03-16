using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[ExecuteInEditMode]
public class MapDisplay : MonoBehaviour {
	
	private MapCellDisplay[,] cells;
	public int row=1;
	public int column=1;
	public Vector2 cellSize = Vector2.one;
	public Vector3 startPos;
	public string mapCellPrefab="MapCellDisplay";
	public string mapFilePath;

	public MapData ToMapData(){
		MapData md = new MapData ();
		md.row = row;
		md.column = column;
		md.cellWidth = (double)cellSize.x;
		md.cellHeight = (double)cellSize.y;
		md.startPosX = (double)startPos.x;
		md.startPosY = (double)startPos.y;
		md.startPosZ = (double)startPos.z;
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < column; j++) {
				if (null != cells [j, i]) {
					md.cells.Add (cells [j, i].CellData);
				}
			}
		}
		SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer> ();
		for (int i = 0; i < children.Length; i++) {
			if (children [i].gameObject.GetComponent<MapCellDisplay> () == null) {
				DecorationData dd = new DecorationData ();
				if (children [i].sprite != null) {
					dd.resName = children [i].sprite.name;
				}
				dd.sortLayer = children [i].sortingLayerName;
				dd.sortOrder = children [i].sortingOrder;
				Transform ddt = children [i].transform;
				dd.posX = (double)ddt.position.x;
				dd.posY = (double)ddt.position.y;
				dd.posZ = (double)ddt.position.z;
				dd.eulerX = (double)ddt.rotation.eulerAngles.x;
				dd.eulerY = (double)ddt.rotation.eulerAngles.y;
				dd.eulerZ = (double)ddt.rotation.eulerAngles.z;
				dd.scaleX = (double)ddt.lossyScale.x;
				dd.scaleY = (double)ddt.lossyScale.y;
				dd.scaleZ = (double)ddt.lossyScale.z;
				md.decorations.Add (dd);
			}
		}
		return md;
	}

	public void ClearMapCell(){
		cells = null;
		int count = transform.childCount;
		for (int i = count-1; i >= 0; i--) {
			GameObject.DestroyImmediate (transform.GetChild(i).gameObject);
		}
	}

	public void CreateMap(int row, int col, Vector2 cellSize){
		startPos = transform.position;
		this.cellSize = cellSize;
		this.row = row;
		this.column = col;
		cells = new MapCellDisplay[col, row];

		GameObject prefab = Resources.Load (mapCellPrefab)as GameObject;
		if (prefab == null) {
			Debug.LogError ("Can not load prefab:"+mapCellPrefab);
			return;
		}
		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				GameObject go = GameObject.Instantiate (prefab);
				go.transform.parent = this.transform;
				go.name = i + "_" + j;
				MapCellDisplay mcd = go.GetComponent<MapCellDisplay> ();
				MapCellData data = new MapCellData ();
				data.row = i;
				data.col = j;
				data.sortOrder = 0;
				data.sortLayer = go.GetComponent<SpriteRenderer> ().sortingLayerName;
				data.resName = go.GetComponent<SpriteRenderer> ().sprite.name;
				data.cellType = (int)CELL_TYPE.WALL;
				mcd.Init (this, data);
				cells [j, i] = mcd;
			}
		}
	}
	void FromMapData(MapData md){
		GameObject prefab = Resources.Load (mapCellPrefab)as GameObject;
		if (prefab == null) {
			Debug.LogError ("Can not load prefab:"+mapCellPrefab);
			return;
		}

		startPos = new Vector3 ((float)md.startPosX, (float)md.startPosY, (float)md.startPosZ);
		transform.position = startPos;
		this.cellSize = new Vector2 ((float)md.cellWidth, (float)md.cellHeight);
		this.row = md.row;
		this.column = md.column;
		cells = new MapCellDisplay[column, row];
		//
		foreach (MapCellData item in md.cells) {
			GameObject go = GameObject.Instantiate (prefab);
			go.transform.parent = this.transform;
			go.name = item.col + "_" + item.row;
			MapCellDisplay mcd = go.GetComponent<MapCellDisplay> ();
			mcd.Init (this, item);
			cells [item.col, item.row] = mcd;
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
	public void ReadMap(){
		mapFilePath = EditorUtility.OpenFilePanel ("Read map file", Application.dataPath, "txt");
		if (string.IsNullOrEmpty (mapFilePath)) {
			return ;
		}
		ClearMapCell ();
		MapData map = MapStream.Read (mapFilePath);
		FromMapData (map);
	}

	public void SaveMap(){
		if (cells == null)
			return;
		if (string.IsNullOrEmpty (mapFilePath)) {
			mapFilePath = EditorUtility.SaveFilePanel ("Save map file", Application.dataPath, "", "txt");
		}
		if (string.IsNullOrEmpty (mapFilePath)) {
			return;
		}
		MapStream.Write (ToMapData (), mapFilePath);
		Debug.Log ("Save success.");
	}
}
