using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class MapCellDisplay : MonoBehaviour {
	protected MapDisplay m_owner;
	[SerializeField]
	protected CELL_TYPE m_type;
	[SerializeField]
	protected CELL_ADDITION m_addition;
	protected MapCellData m_data = new MapCellData();
	protected SpriteRenderer m_render;
	protected SpriteRenderer Render{
		get{
			if (null == m_render) {
				m_render = GetComponent<SpriteRenderer> ();
			}
			return m_render;
		}
	}
	public MapCellData CellData{
		get{
			m_data.cellType = (int)m_type;
			m_data.cellAdd = (int)m_addition;
			if (Render.sprite != null)
				m_data.resName = Render.sprite.name;
			else
				m_data.resName = "";
			m_data.sortOrder = Render.sortingOrder;
			m_data.sortLayer = Render.sortingLayerName;
			return m_data;
		}
	}

	public void Init(MapDisplay owner, MapCellData data){
		m_owner = owner;
		m_data = data;
		m_type = (CELL_TYPE)data.cellType;
		if (!string.IsNullOrEmpty (data.resName))
			Render.sprite = Resources.Load ("Sprites/" + data.resName, typeof(Sprite))as Sprite;
		else
			Render.sprite = null;
		transform.position = owner.startPos + new Vector3 (data.col*owner.cellSize.x, data.row*owner.cellSize.y, owner.startPos.z);
		//Render.sortingLayerID = SortingLayer.GetLayerValueFromID(data.sortLayer);
		Render.sortingOrder = data.sortOrder;
		Render.sortingLayerName = data.sortLayer;
	}
}
