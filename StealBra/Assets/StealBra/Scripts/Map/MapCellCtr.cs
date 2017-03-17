using UnityEngine;
using System.Collections;


public class MapCellCtr : MonoBehaviour {

	public MapCellData CellData;
	public MapCtr Owner;
	protected SpriteRenderer m_render;
	public SpriteRenderer Render{
		get{
			if (null == m_render) {
				m_render = GetComponent<SpriteRenderer> ();
			}
			return m_render;
		}
	}

	public Vector3 Position{
		get{ 
			return Owner.StartPos + new Vector3 (Owner.CellSize.x*CellData.col, Owner.CellSize.y*CellData.row, 0);
		}
	}

	public void Init(MapCtr owner, MapCellData data){
		CellData = data;
		Owner = owner;
		//
		if (!string.IsNullOrEmpty (data.resName))
			Render.sprite = Resources.Load ("Sprites/" + data.resName, typeof(Sprite))as Sprite;
		else
			Render.sprite = null;
		transform.position = owner.StartPos + new Vector3 (data.col*owner.CellSize.x, data.row*owner.CellSize.y, owner.StartPos.z);
		Render.sortingOrder = data.sortOrder;
		Render.sortingLayerName = data.sortLayer;
	}
	//
	protected int m_preType;
	public void ChangeToNone(bool canRevert = true){
		if ((CELL_TYPE)CellData.cellType == CELL_TYPE.WALL) {
			
			SpriteMap2AStarMap.ResetAStarMapCell (Owner.Map, CellData, ref Owner.PathfindingMap);
		}
		//
		Render.sprite = null;
		m_preType = CellData.cellType; 
		CellData.cellType = (int)CELL_TYPE.NONE;
		if(canRevert)
			StartCoroutine (Revert (10));
	}

	IEnumerator Revert(float second){
		yield return new WaitForSeconds (second);

		if (!string.IsNullOrEmpty (CellData.resName))
			Render.sprite = Resources.Load ("Sprites/" + CellData.resName, typeof(Sprite))as Sprite;
		CellData.cellType = m_preType;
		//
		if ((CELL_TYPE)CellData.cellType == CELL_TYPE.WALL) {

			SpriteMap2AStarMap.ResetAStarMapCell (Owner.Map, CellData, ref Owner.PathfindingMap);
		}
		//
		//GameStateManager.Instance ().FSM.CurrentState.Message ("", null);
	}
}
