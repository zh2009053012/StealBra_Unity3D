using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MapCellDisplay : MonoBehaviour {

	public MapDisplay Owner;
	public int Row, Column;

	public bool IsToLeft=true;
	public bool IsToRight=true;
	public bool IsToUp=true;
	public bool IsToDown=true;
	public bool IsToLeftUp=true;
	public bool IsToLeftDown=true;
	public bool IsToRightUp=true;
	public bool IsToRightDown=true;

	private bool[,] m_passArray = new bool[3,3];

	public bool IsAllOpen = true;
	public bool IsAllClose = false;
	public bool IsObstacle = false;

	void ArrayToBoolValue(){
		IsToLeftUp = m_passArray[0, 0];
		IsToUp = m_passArray[1, 0];
		IsToRightUp = m_passArray[2, 0];
		IsToLeft = m_passArray[0, 1];
		IsToRight = m_passArray[2, 1];
		IsToLeftDown = m_passArray[0, 2];
		IsToDown = m_passArray[1, 2];
		IsToRightDown = m_passArray[2, 2];
	}
	void BoolValueToArray(){

		m_passArray[0, 0] = IsToLeftUp;
		m_passArray[1, 0] = IsToUp;
		m_passArray[2, 0] =IsToRightUp;
		m_passArray[0, 1] =IsToLeft;
		m_passArray[2, 1] =IsToRight;
		m_passArray[0, 2] =IsToLeftDown;
		m_passArray[1, 2] =IsToDown;
		m_passArray[2, 2] =IsToRightDown;
	}

	public void SetSelfObstacle(){
		IsAllClose = true;
		IsAllOpen = false;
		IsToLeftUp = false;
		IsToUp = false;
		IsToRightUp = false;
		IsToLeft = false;
		IsToRight = false;
		IsToLeftDown = false;
		IsToDown = false;
		IsToRightDown = false;
	}
	public void SetNeighborObstacle(int neighborRow, int neighborCol){
		BoolValueToArray();
		int x = neighborCol - Column + 1;
		int y = neighborRow - Row + 1;
		m_passArray[x, y] = false;
		ArrayToBoolValue();
	}

	public MapCell ToMapCell(){
		MapCell mc = new MapCell ();
		mc.IsToLeft = IsToLeft;
		mc.IsToRight = IsToRight;
		mc.IsToUp = IsToUp;
		mc.IsToDown = IsToDown;
		mc.IsToLeftUp = IsToLeftUp;
		mc.IsToLeftDown = IsToLeftDown;
		mc.IsToRightUp = IsToRightUp;
		mc.IsToRightDown = IsToRightDown;
		return mc;
	}
	public void FromMapCell(MapCell mc){
		IsToLeft = mc.IsToLeft;
		IsToRight = mc.IsToRight;
		IsToUp = mc.IsToUp;
		IsToDown = mc.IsToDown;
		IsToLeftUp = mc.IsToLeftUp;
		IsToLeftDown = mc.IsToLeftDown;
		IsToRightUp = mc.IsToRightUp;
		IsToRightDown = mc.IsToRightDown;

		ShowLeft ();
		ShowLeftDown ();
		ShowLeftUp ();
		ShowRight ();
		ShowRightDown ();
		ShowRightUp ();
		ShowUp ();
		ShowDown ();
	}

	[SerializeField]
	private GameObject m_lineLeft;
	[SerializeField]
	private GameObject m_lineRight;
	[SerializeField]
	private GameObject m_lineUp;
	[SerializeField]
	private GameObject m_lineDown;
	[SerializeField]
	private GameObject m_lineLeftUp;
	[SerializeField]
	private GameObject m_lineLeftDown;
	[SerializeField]
	private GameObject m_lineRightUp;
	[SerializeField]
	private GameObject m_lineRightDown;

	[SerializeField]
	private GameObject m_arrowLeft;
	[SerializeField]
	private GameObject m_arrowRight;
	[SerializeField]
	private GameObject m_arrowUp;
	[SerializeField]
	private GameObject m_arrowDown;
	[SerializeField]
	private GameObject m_arrowLeftUp;
	[SerializeField]
	private GameObject m_arrowLeftDown;
	[SerializeField]
	private GameObject m_arrowRightUp;
	[SerializeField]
	private GameObject m_arrowRightDown;


	public void Init(){
		BoolValueToArray();

		m_lineLeft = transform.FindChild ("line_left").gameObject;
		m_lineRight = transform.FindChild ("line_right").gameObject;
		m_lineUp = transform.FindChild ("line_up").gameObject;
		m_lineDown = transform.FindChild ("line_down").gameObject;
		m_lineLeftUp = transform.FindChild ("line_left_up").gameObject;
		m_lineLeftDown = transform.FindChild ("line_left_down").gameObject;
		m_lineRightUp = transform.FindChild ("line_right_up").gameObject;
		m_lineRightDown = transform.FindChild ("line_right_down").gameObject;

		m_arrowLeft = transform.FindChild ("arrow_left").gameObject;
		m_arrowRight = transform.FindChild ("arrow_right").gameObject;
		m_arrowUp = transform.FindChild ("arrow_up").gameObject;
		m_arrowDown = transform.FindChild ("arrow_down").gameObject;
		m_arrowLeftUp = transform.FindChild ("arrow_left_up").gameObject;
		m_arrowLeftDown = transform.FindChild ("arrow_left_down").gameObject;
		m_arrowRightUp = transform.FindChild ("arrow_right_up").gameObject;
		m_arrowRightDown = transform.FindChild ("arrow_right_down").gameObject;
	}
		
	public void ShowLeft(){
		if(m_lineLeft!=null)
			m_lineLeft.GetComponent<SpriteRenderer> ().enabled = IsToLeft;
		if(m_arrowLeft!=null)
			m_arrowLeft.GetComponent<SpriteRenderer> ().enabled = IsToLeft;
	}
	public void ShowLeftUp(){
		if(m_lineLeftUp!=null)
			m_lineLeftUp.GetComponent<SpriteRenderer> ().enabled = IsToLeftUp;
		if(m_arrowLeftUp!=null)
			m_arrowLeftUp.GetComponent<SpriteRenderer> ().enabled = IsToLeftUp;
	}
	public void ShowLeftDown(){
		if(m_lineLeftDown!=null)
			m_lineLeftDown.GetComponent<SpriteRenderer> ().enabled = IsToLeftDown;
		if(m_arrowLeftDown!=null)
			m_arrowLeftDown.GetComponent<SpriteRenderer> ().enabled = IsToLeftDown;
	}
	public void ShowRight(){
		if(m_lineRight!=null)
		m_lineRight.GetComponent<SpriteRenderer> ().enabled = IsToRight;
		if(m_arrowRight!=null)
		m_arrowRight.GetComponent<SpriteRenderer> ().enabled = IsToRight;
	}
	public void ShowRightUp(){
		if(m_lineRightUp!=null)
		m_lineRightUp.GetComponent<SpriteRenderer> ().enabled = IsToRightUp;
		if(m_arrowRightUp!=null)
		m_arrowRightUp.GetComponent<SpriteRenderer> ().enabled = IsToRightUp;
	}
	public void ShowRightDown(){
		if(m_lineRightDown!=null)
		m_lineRightDown.GetComponent<SpriteRenderer> ().enabled = IsToRightDown;
		if(m_arrowRightDown!=null)
		m_arrowRightDown.GetComponent<SpriteRenderer> ().enabled = IsToRightDown;
	}
	public void ShowUp(){
		if(m_lineUp!=null)
		m_lineUp.GetComponent<SpriteRenderer> ().enabled = IsToUp;
		if(m_arrowUp!=null)
		m_arrowUp.GetComponent<SpriteRenderer> ().enabled = IsToUp;
	}
	public void ShowDown(){
		if(m_lineDown!=null)
		m_lineDown.GetComponent<SpriteRenderer> ().enabled = IsToDown;
		if(m_arrowDown!=null)
		m_arrowDown.GetComponent<SpriteRenderer> ().enabled = IsToDown;
	}
}
