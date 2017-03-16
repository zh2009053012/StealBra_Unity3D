using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AStarMapCellDisplay : MonoBehaviour {

	public AStarMapDisplay Owner;
	public int Row, Column;

	public bool IsToLeft=true;
	public bool IsToRight=true;
	public bool IsToUp=true;
	public bool IsToDown=true;
	public bool IsToLeftUp=true;
	public bool IsToLeftDown=true;
	public bool IsToRightUp=true;
	public bool IsToRightDown=true;

//	private bool[,] m_passArray = new bool[3,3];

	public bool IsAllOpen = true;
	public bool IsAllClose = false;
	public bool IsObstacle = false;

//	void ArrayToBoolValue(){
//		IsToLeftUp = m_passArray[0, 0];
//		IsToUp = m_passArray[1, 0];
//		IsToRightUp = m_passArray[2, 0];
//		IsToLeft = m_passArray[0, 1];
//		IsToRight = m_passArray[2, 1];
//		IsToLeftDown = m_passArray[0, 2];
//		IsToDown = m_passArray[1, 2];
//		IsToRightDown = m_passArray[2, 2];
//	}
//	void BoolValueToArray(){
//
//		m_passArray[0, 0] = IsToLeftUp;
//		m_passArray[1, 0] = IsToUp;
//		m_passArray[2, 0] =IsToRightUp;
//		m_passArray[0, 1] =IsToLeft;
//		m_passArray[2, 1] =IsToRight;
//		m_passArray[0, 2] =IsToLeftDown;
//		m_passArray[1, 2] =IsToDown;
//		m_passArray[2, 2] =IsToRightDown;
//	}

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

		//
		ShowLeft ();
		ShowLeftDown ();
		ShowLeftUp ();
		ShowRight ();
		ShowRightDown ();
		ShowRightUp ();
		ShowUp ();
		ShowDown ();
	}
	public void SetNeighborObstacle(int neighborRow, int neighborCol){

		int x = neighborCol - Column;
		int y = neighborRow - Row;

		if (x == -1 && y == -1) {
			IsToLeftDown = false;
			ShowLeftDown ();
		} else if (x == -1 && y == 0) {
			IsToLeft = false;
			ShowLeft ();
		} else if (x == -1 && y == 1) {
			
			IsToLeftUp = false;
			ShowLeftUp ();
		} else if (x == 0 && y == -1) {
			IsToDown = false;
			ShowDown ();
		} else if (x == 0 && y == 1) {
			IsToUp = false;
			ShowUp ();
		} else if (x == 1 && y == -1) {
			IsToRightDown = false;
			ShowRightDown ();
		} else if (x == 1 && y == 0) {
			IsToRight = false;
			ShowRight ();
		} else if (x == 1 && y == 1) {
			
			IsToRightUp = false;
			ShowRightUp ();
		}

	}

	public AStarMapCell ToMapCell(){
		AStarMapCell mc = new AStarMapCell ();
		mc.IsToLeft = IsToLeft;
		mc.IsToRight = IsToRight;
		mc.IsToUp = IsToUp;
		mc.IsToDown = IsToDown;
		mc.IsToLeftUp = IsToLeftUp;
		mc.IsToLeftDown = IsToLeftDown;
		mc.IsToRightUp = IsToRightUp;
		mc.IsToRightDown = IsToRightDown;
		mc.Row = Row;
		mc.Column = Column;
		return mc;
	}
	public void FromMapCell(AStarMapCell mc){
		IsToLeft = mc.IsToLeft;
		IsToRight = mc.IsToRight;
		IsToUp = mc.IsToUp;
		IsToDown = mc.IsToDown;
		IsToLeftUp = mc.IsToLeftUp;
		IsToLeftDown = mc.IsToLeftDown;
		IsToRightUp = mc.IsToRightUp;
		IsToRightDown = mc.IsToRightDown;
		Row = mc.Row;
		Column = mc.Column;

		ShowLeft ();
		ShowLeftDown ();
		ShowLeftUp ();
		ShowRight ();
		ShowRightDown ();
		ShowRightUp ();
		ShowUp ();
		ShowDown ();
	}


	public void Init(){
	//	BoolValueToArray();
	}

	Vector4 mask1 = Vector4.one;// left_up, right_up, left_down, right_down
	Vector4 mask2 = Vector4.one;//up, right, down, left
	float Bool2Float(bool b){
		if (b)
			return 1.0f;
		else
			return 0.0f;
	}
	void Update(){
		if (!IsObstacle) {
			ShowLeft ();
			ShowLeftDown ();
			ShowLeftUp ();
			ShowRight ();
			ShowRightDown ();
			ShowRightUp ();
			ShowUp ();
			ShowDown ();
		}
	}
	public void ShowLeft(){
		mask2 = new Vector4 (mask2.x, mask2.y, mask2.z, Bool2Float(IsToLeft));
		GetComponent<SpriteRenderer> ().sharedMaterial.SetVector ("_Mask2", mask2);
	}
	public void ShowLeftUp(){
		mask1 = new Vector4 (Bool2Float(IsToLeftUp), mask1.y, mask1.z, mask1.w);
		GetComponent<SpriteRenderer> ().sharedMaterial.SetVector ("_Mask1", mask1);
	}
	public void ShowLeftDown(){
		mask1 = new Vector4 (mask1.x, mask1.y, Bool2Float(IsToLeftDown), mask1.w);
		GetComponent<SpriteRenderer> ().sharedMaterial.SetVector ("_Mask1", mask1);
	}
	public void ShowRight(){
		mask2 = new Vector4 (mask2.x, Bool2Float(IsToRight), mask2.z, mask2.w);
		GetComponent<SpriteRenderer> ().sharedMaterial.SetVector ("_Mask2", mask2);
	}
	public void ShowRightUp(){
		mask1 = new Vector4 (mask1.x, Bool2Float(IsToRightUp), mask1.z, mask1.w);
		GetComponent<SpriteRenderer> ().sharedMaterial.SetVector ("_Mask1", mask1);
	}
	public void ShowRightDown(){
		mask1 = new Vector4 (mask1.x, mask1.y, mask1.z, Bool2Float(IsToRightDown));
		GetComponent<SpriteRenderer> ().sharedMaterial.SetVector ("_Mask1", mask1);
	}
	public void ShowUp(){
		mask2 = new Vector4 (Bool2Float(IsToUp), mask2.y, mask2.z, mask2.w);
		GetComponent<SpriteRenderer> ().sharedMaterial.SetVector ("_Mask2", mask2);
	}
	public void ShowDown(){
		mask2 = new Vector4 (mask2.x, mask2.y, Bool2Float(IsToDown), mask2.w);
		GetComponent<SpriteRenderer> ().sharedMaterial.SetVector ("_Mask2", mask2);
	}
}
