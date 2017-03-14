using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 1\ sprite drawcall optimize
/// 2\ SetObstacle  optimize
/// </summary>

[ExecuteInEditMode]
public class MapDisplay : MonoBehaviour {

	public uint m_row;
	public uint m_column;
	public MapCellDisplay[,] m_cellArray;
	public string m_filePath;
	private int m_mapRow, m_mapCol;

	public void WallAround(){
		for (int i = 0; i < m_row; i++) {
			for (int j = 0; j < m_column; j++) {
				if(i == 0 || i == m_row-1 || j == 0 || j == m_column-1)
					SetObstacle(i, j);
			}
		}
		Recreate(Selection.activeGameObject);
	}
	public void SetObstacle(int row, int col){
		m_cellArray[col, row].SetSelfObstacle();

		for(int i=row-1; i<row+2; i++){
			for(int j=col-1; j<col+2; j++){
				if(i >= 0 && i < m_row && j >= 0 && j < m_column){
					m_cellArray[j, i].SetNeighborObstacle(row, col);
				}
			}
		}
	}
	public void Recreate(GameObject curSel){
		MapCellDisplay mcd = curSel.GetComponent<MapCellDisplay>();
		int col = -1;
		int row = -1;
		if(mcd != null){
			col = mcd.Column;
			row = mcd.Row;
		}
		MapCatch();
		ClearMap();
		ReadCatch();
		if(col >= 0 && row >= 0)
			Selection.activeGameObject = m_cellArray[col, row].gameObject;
	}
	public void ClearMap(){
		m_cellArray = null;
		int count = transform.childCount;
		for (int i = count-1; i >= 0; i--) {
			GameObject.DestroyImmediate (transform.GetChild(i).gameObject);
		}
		m_filePath = "";
	}

	public void CreateMap(){
		ClearMap ();

		m_cellArray = new MapCellDisplay[m_column, m_row];

		GameObject prefab = Resources.Load ("cell")as GameObject;
		SpriteRenderer sr = prefab.GetComponent<SpriteRenderer> ();
		Vector3 size = sr.bounds.size;
		for (int i = 0; i < m_row; i++) {
			for (int j = 0; j < m_column; j++) {
				GameObject go = GameObject.Instantiate (prefab);

				go.transform.parent = transform;
				go.transform.localScale = Vector3.one;
				go.transform.localPosition = new Vector3 (size.x*j, -size.y*i, 0);
				go.name = i + "_" + j;

				m_cellArray [j, i] = go.GetComponent<MapCellDisplay> ();
				m_cellArray[j, i].Owner = this;
				m_cellArray[j, i].Row = i;
				m_cellArray[j, i].Column = j;
			}
		}
	}

	Map ToMap(){
		Map map = new Map (m_row, m_column);
		for (int i = 0; i < m_row; i++) {
			for (int j = 0; j < m_column; j++) {
				map.SetCell ((uint)i, (uint)j, m_cellArray [j, i].ToMapCell());
			}
		}
		return map;
	}
	void FromMap(Map map){
		ClearMap ();
		m_column = map.Column;
		m_row = map.Row;
		CreateMap ();
		for (int i = 0; i < m_row; i++) {
			for (int j = 0; j < m_column; j++) {
				m_cellArray [j, i].FromMapCell (map.GetCell ((uint)i, (uint)j));
			}
		}
	}

	public void SaveMap(){
		if (m_cellArray == null)
			return;
		if (string.IsNullOrEmpty (m_filePath)) {
			m_filePath = EditorUtility.SaveFilePanel ("Save map file", Application.dataPath, "", "txt");
		}
		if (string.IsNullOrEmpty (m_filePath)) {
			return;
		}
		MapStream.Write (ToMap (), m_filePath);
		Debug.Log ("Save success.");
	}
	void MapCatch(){
		MapStream.Write (ToMap (), Application.dataPath+"/MapCatch");
	}
	void ReadCatch(){
		Map map = MapStream.Read (Application.dataPath+"/MapCatch");
		FromMap (map);
		FileUtil.DeleteFileOrDirectory(Application.dataPath+"/MapCatch");
	}

	public void ReadMap(){
		if(!string.IsNullOrEmpty(m_filePath))
			SaveMap ();
		m_filePath = EditorUtility.OpenFilePanel ("Read map file", Application.dataPath, "txt");
		if (string.IsNullOrEmpty (m_filePath)) {
			return ;
		}
		Map map = MapStream.Read (m_filePath);
		FromMap (map);

	}
}
