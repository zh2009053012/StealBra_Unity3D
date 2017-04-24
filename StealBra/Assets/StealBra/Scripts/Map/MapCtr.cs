using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapCtr : MonoBehaviour {
	protected PlayerCtrEx m_player;
	public PlayerCtrEx Player{
		get{return m_player;}
	}
	public List<DogCtr> m_dogList = new List<DogCtr>();
	public DogCtr GetDog(int row, int col){
		for(int i=0; i<m_dogList.Count; i++){
			if(m_dogList[i].Row == row && m_dogList[i].Column == col){
				return m_dogList[i];
			}
		}
		return null;
	}
	public bool IsMeetWithDog(Vector3 playerPos){
		for (int i = 0; i < m_dogList.Count; i++) {
			if (Vector3.Distance (playerPos, m_dogList [i].transform.position) <= m_map.cellWidth * 0.75f) {
				return true;
			}
		}
		return false;
	}
	//
	protected int exitRow, exitCol;
	public int ExitRow{
		get{
			return exitRow;
		}
	}
	public int ExitCol{
		get{return exitCol;}
	}
	protected int exitEndRow, exitEndCol;
	public int ExitEndRow{
		get{ return exitEndRow;}
		private set{exitEndRow = value;}
	}
	public int ExitEndCol{
		get{ return exitEndCol;}
		private set{ exitEndCol = value;}
	}
	protected bool canExit=false;
	public bool CanExit{
		get{return canExit;}
	}
	public void ShowExit(bool isShow){
		canExit = isShow;
		if(isShow){
			//cells[exitCol, exitRow].Render.sprite = Resources.Load ("Sprites/ladder", typeof(Sprite))as Sprite;
			for (int i = exitRow; i < m_map.row; i++) {
				cells [exitCol, i].ChangeToLadder ();
			}
			ExitEndRow = cells [exitCol, m_map.row-1].CellData.row;
			ExitEndCol = cells [exitCol, m_map.row-1].CellData.col;
		}else{
			//cells[exitCol, exitRow].Render.sprite = null;
		}
	}
	//
	protected MapCellCtr[,] cells;
	public AStarMap PathfindingMap;
	protected List<GameObject> m_decorationList = new List<GameObject>();
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
	public MapCellCtr GetMapCell(Vector2 pos){
		Vector2 offset = pos - new Vector2((float)m_map.startPosX, (float)m_map.startPosY);
		int row = (int)( offset.y / (float)m_map.cellHeight + 0.5f );
		int col = (int) (offset.x / (float)m_map.cellWidth + 0.5f);
		return GetMapCell (row, col);
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
	protected int m_braUnderpantNum;
	public int BraUnderpantNum{
		get{ return m_braUnderpantNum;}
		private set{ m_braUnderpantNum = value;}
	}

	public void Clear(){
		if(null != Player)
			GameObject.Destroy(Player.gameObject);
		foreach(DogCtr dog in m_dogList){
			GameObject.Destroy(dog.gameObject);
		}
		m_player = null;
		m_dogList.Clear();
		//
		for(int i=0; i<m_map.row; i++){
			for(int j=0; j<m_map.column; j++){
				GameObject.Destroy(cells[j, i].gameObject);
				cells[j, i]= null;
			}
		}
		foreach(GameObject go in m_decorationList){
			GameObject.Destroy(go);
		}
		m_decorationList.Clear();
		GameObject.Destroy (this.gameObject);
	}

	void FromMapData(MapData md){
		m_map = md;
		transform.position = new Vector3 ((float)md.startPosX, (float)md.startPosY, (float)md.startPosZ);

		cells = new MapCellCtr[m_map.column, m_map.row];
		BraUnderpantNum = 0;
		//
		foreach (MapCellData item in md.cells) {
			GameObject go = new GameObject ();
			go.AddComponent<SpriteRenderer> ();
			MapCellCtr mcc = go.AddComponent<MapCellCtr> ();
			go.transform.parent = this.transform;
			go.name = item.col + "_" + item.row;
			mcc.Init (this, item);
			cells [item.col, item.row] = mcc;
			//
			if ((CELL_TYPE)mcc.CellData.cellType == CELL_TYPE.BRA ||
			   (CELL_TYPE)mcc.CellData.cellType == CELL_TYPE.UNDERPANT) {
				BraUnderpantNum += 1;
			}
		}
		foreach(MapCellData item in md.cells){
			switch((CELL_ADDITION)item.cellAdd){
			case CELL_ADDITION.EXIT:
				exitRow = item.row;
				exitCol = item.col;
				ShowExit(false);
				break;
			case CELL_ADDITION.PLAYER:
				LoadPlayer(item.row, item.col);
				break;
			case CELL_ADDITION.DOG:
				LoadDog(item.row, item.col);
				break;
			}
		}
		//
		m_decorationList.Clear();
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
			m_decorationList.Add(go);
		}
	}
	void LoadPlayer(int row, int col){
		GameObject prefab = Resources.Load("Player")as GameObject;
		GameObject player = GameObject.Instantiate (prefab);
		m_player = player.GetComponent<PlayerCtrEx> ();
		m_player.Init (row, col, this);
	}
	void LoadDog(int row, int col){
		GameObject dogPrefab = Resources.Load("Dog")as GameObject;
		GameObject dog = GameObject.Instantiate (dogPrefab);
		DogCtr m_dogCtr = dog.GetComponent<DogCtr> ();
		m_dogCtr.Init (row, col, this, PathfindingMap, m_player, Random.Range(0, 2)==1);
		m_dogList.Add(m_dogCtr);
	}
	public void ReadMap(string fileName){
		Debug.Log ("MapCtr::ReadMap");
		Debug.Log (GameDebug.GetInvokeClassAndMethodName(1));
		Debug.Log (GameDebug.GetInvokeClassAndMethodName(2));
		Debug.Log (GameDebug.GetInvokeClassAndMethodName(3));
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
