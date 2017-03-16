using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 1\ sprite drawcall optimize  over
/// 2\ SetObstacle  optimize
/// </summary>

[ExecuteInEditMode]
public class AStarMapDisplay : MonoBehaviour {

	public int m_row;
	public int m_column;
	public AStarMapCellDisplay[,] m_cellArray;
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
		AStarMapCellDisplay mcd = curSel.GetComponent<AStarMapCellDisplay>();
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

		m_cellArray = new AStarMapCellDisplay[m_column, m_row];

		GameObject prefab = Resources.Load ("cell")as GameObject;
		SpriteRenderer sr = prefab.GetComponent<SpriteRenderer> ();
		Vector3 size = sr.bounds.size;
		for (int i = 0; i < m_row; i++) {
			for (int j = 0; j < m_column; j++) {
				GameObject go = GameObject.Instantiate (prefab);

				go.transform.parent = transform;
				go.transform.localScale = Vector3.one;
				go.transform.localPosition = new Vector3 (size.x*j, size.y*i, 0);
				go.name = i + "_" + j;
				go.GetComponent<Renderer> ().sharedMaterial = CreateMat (go.name);

				m_cellArray [j, i] = go.GetComponent<AStarMapCellDisplay> ();
				m_cellArray[j, i].Owner = this;
				m_cellArray[j, i].Row = i;
				m_cellArray[j, i].Column = j;
			}
		}
		AssetDatabase.Refresh ();
	}
	//
	Texture cell, line, mask1, mask2;
	void LoadTexture(){
		string path = "Assets/AStarDemo/Textures/cell/";
		cell = AssetDatabase.LoadAssetAtPath (path+"cell.png", typeof(Texture))as Texture;
		line = AssetDatabase.LoadAssetAtPath (path+"line_arrow.png", typeof(Texture))as Texture;
		mask1 = AssetDatabase.LoadAssetAtPath (path+"mask_1.tga", typeof(Texture))as Texture;
		mask2 = AssetDatabase.LoadAssetAtPath (path+"mask_2.tga", typeof(Texture))as Texture;
	}
	Material CreateMat(string name){
		if (cell == null || line == null || mask1 == null || mask2 == null)
			LoadTexture ();
		string path = "Assets/AStarDemo/Materials/" + name + ".mat";
		Material mat = AssetDatabase.LoadAssetAtPath<Material> (path);
		if (mat == null) {
			mat = new Material (Shader.Find ("Sprites/Cell"));
			mat.mainTexture = cell;
			mat.SetTexture ("_Mask1Tex", mask1);
			mat.SetTexture ("_Mask2Tex", mask2);
			mat.SetTexture ("_LineTex", line);
			AssetDatabase.CreateAsset (mat, path);
		}
		mat.SetVector ("_Mask1", Vector4.one);
		mat.SetVector ("_Mask2", Vector4.one);

		return mat;
	}

	AStarMap ToMap(){
		AStarMap map = new AStarMap (m_row, m_column);
		for (int i = 0; i < m_row; i++) {
			for (int j = 0; j < m_column; j++) {
				map.SetCell (i, j, m_cellArray [j, i].ToMapCell());
			}
		}
		return map;
	}
	void FromMap(AStarMap map){
		ClearMap ();
		m_column = map.Column;
		m_row = map.Row;
		CreateMap ();
		for (int i = 0; i < m_row; i++) {
			for (int j = 0; j < m_column; j++) {
				m_cellArray [j, i].FromMapCell (map.GetCell (i, j));
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
		AStarMapStream.Write (ToMap (), m_filePath);
		Debug.Log ("Save success.");
	}
	void MapCatch(){
		AStarMapStream.Write (ToMap (), Application.dataPath+"/MapCatch");
	}
	void ReadCatch(){
		AStarMap map = AStarMapStream.Read (Application.dataPath+"/MapCatch");
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
		AStarMap map = AStarMapStream.Read (m_filePath);
		FromMap (map);

	}
}
