using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {
	[SerializeField]
	protected GameObject m_body;
	//stay pos
	protected int m_row, m_col;
	public int Row{
		get{ return m_row;}
	}
	public int Column{
		get{ return m_col;}
	}

	protected MapCtr m_mapCtr;
	protected MapCellCtr m_curCell;
	//
	[SerializeField]
	protected float m_moveSpeed=3;
	[SerializeField]
	protected float m_climbSpeed=2;
	//
	protected Vector3 m_targetPos;
	protected bool m_isMoving = false;
	protected Vector3 m_dir;
	//
	protected float vInput;
	protected float hInput;
	//
	protected Animator m_ani;
	protected bool m_isAttack = false;
	protected bool m_isLookLeft = false;

	public void Init(int row, int col, MapCtr map){
		m_row = row;
		m_col = col;
		m_dir = new Vector3 (1, 0, 0);
		m_mapCtr = map;
		m_curCell = m_mapCtr.GetMapCell (row, col);
		transform.position = m_curCell.Position;
		m_ani = GetComponentInChildren<Animator> ();
		CheckGround ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_isMoving) {
			float step = m_moveSpeed * Time.deltaTime;
			transform.position += m_dir * step;
			if (Vector2.Distance (new Vector2 (transform.position.x, transform.position.y), new Vector2 (m_targetPos.x, m_targetPos.y)) < step*1.5f) {
				transform.position = new Vector3 (m_targetPos.x, m_targetPos.y, transform.position.z);
				m_row += (int)m_dir.y;
				m_col += (int)m_dir.x;
				m_curCell = m_mapCtr.GetMapCell (m_row, m_col);
				m_isMoving = false;
				CheckGround ();
			}
		}
	}
	public void OnAttackOver(){
		m_isAttack = false;
		m_ani.SetBool ("attack", m_isAttack);
	}
	public void CheckAttack(){
		if (m_isMoving || m_ani.GetBool("climb"))
			return;
		//stand on the wall and target must be wall
		CELL_TYPE underType = (CELL_TYPE)m_mapCtr.GetMapCell (m_row - 1, m_col).CellData.cellType;
		if (underType == CELL_TYPE.WALL || underType == CELL_TYPE.STONE) {
			Debug.Log ("under type ok");
			int look;
			if (m_isLookLeft)
				look = -1;
			else
				look = 1;
			MapCellCtr target = m_mapCtr.GetMapCell (m_row, m_col+look);
			if ((CELL_TYPE)target.CellData.cellType == CELL_TYPE.NONE) {
				Debug.Log ("dir ok");
				MapCellCtr targetDown = m_mapCtr.GetMapCell (m_row-1, m_col+look);
				if ((CELL_TYPE)targetDown.CellData.cellType == CELL_TYPE.WALL) {
					Debug.Log ("dir down ok");
					m_isAttack = true;
					m_ani.SetBool ("run", false);
					m_ani.SetBool ("attack", m_isAttack);
					targetDown.ChangeToNone ();
				}
			}
		}
	}
	void CheckGround(){
		//not pole ladder && can move down
		m_ani.SetBool ("jump", false);
		if((CELL_TYPE) m_curCell.CellData.cellType != CELL_TYPE.POLE && (CELL_TYPE) m_curCell.CellData.cellType != CELL_TYPE.LADDER){
			CELL_TYPE type = (CELL_TYPE)m_mapCtr.GetMapCell (m_row-1, m_col).CellData.cellType;
			if(type != CELL_TYPE.LADDER)
				MoveDown ();
		}else if((CELL_TYPE) m_curCell.CellData.cellType == CELL_TYPE.POLE){
			transform.localEulerAngles = new Vector3 (0, 0, 90);
			m_body.transform.localEulerAngles = new Vector3 (0, 90, 0);
			m_ani.SetBool ("run", false);
			m_ani.SetBool ("climb", true);
			m_isLookLeft = true;
		}
	}
	void MoveLeft(){
		CELL_TYPE type = (CELL_TYPE)m_mapCtr.GetMapCell (m_row, m_col-1).CellData.cellType;
		if ( type != CELL_TYPE.STONE && type != CELL_TYPE.WALL ) {
			MapCellCtr target = m_mapCtr.GetMapCell (m_row, m_col - 1);
			if (null != target) {
				m_targetPos = target.Position;
				m_isMoving = true;
				m_dir = new Vector3 (-1, 0, 0);
				if ((CELL_TYPE)target.CellData.cellType == CELL_TYPE.POLE) {
					transform.localEulerAngles = new Vector3 (0, 0, 90);
					m_body.transform.localEulerAngles = new Vector3 (0, 90, 0);
					m_ani.SetBool ("run", false);
					m_ani.SetBool ("climb", true);
					m_isLookLeft = true;
				} else {
					transform.localEulerAngles = new Vector3 (0, 0, 0);
					m_body.transform.localEulerAngles = new Vector3 (0, -90, 0);
					m_isLookLeft = true;
					m_ani.SetBool ("run", true);
					m_ani.SetBool ("climb", false);
				}
					
			} else {
				Debug.LogWarning ("warning: astarMap not match spriteMap:"+m_row+","+(m_col+1));
			}
		}
	}
	void MoveRight(){
		CELL_TYPE type = (CELL_TYPE)m_mapCtr.GetMapCell (m_row, m_col+1).CellData.cellType;
		if ( type != CELL_TYPE.STONE && type != CELL_TYPE.WALL ) {
			MapCellCtr target = m_mapCtr.GetMapCell (m_row, m_col + 1);
			if (null != target) {
				m_targetPos = target.Position;
				m_isMoving = true;
				m_dir = new Vector3 (1, 0, 0);
				if ((CELL_TYPE)target.CellData.cellType == CELL_TYPE.POLE) {
					transform.localEulerAngles = new Vector3 (0, 0, 90);
					m_body.transform.localEulerAngles = new Vector3 (0, 90, 0);
					m_ani.SetBool ("run", false);
					m_ani.SetBool ("climb", true);
					m_isLookLeft = true;
				} else {
					transform.localEulerAngles = new Vector3 (0, 0, 0);
					m_body.transform.localEulerAngles = new Vector3 (0, 90, 0);
					m_isLookLeft = false;
					m_ani.SetBool ("run", true);
					m_ani.SetBool ("climb", false);
				}

			} else {
				Debug.LogWarning ("warning: astarMap not match spriteMap:"+m_row+","+(m_col+1));
			}
		}
	}
	void MoveUp(){
		CELL_TYPE type = (CELL_TYPE)m_mapCtr.GetMapCell (m_row+1, m_col).CellData.cellType;
		if ( type != CELL_TYPE.STONE && type != CELL_TYPE.WALL && (CELL_TYPE)m_curCell.CellData.cellType == CELL_TYPE.LADDER) {
			MapCellCtr target = m_mapCtr.GetMapCell (m_row+1, m_col);
			if (null != target) {
				m_targetPos = target.Position;
				m_isMoving = true;
				m_dir = new Vector3 (0, 1, 0);
				m_ani.SetBool ("run", false);
				m_ani.SetBool ("climb", true);
				m_isLookLeft = false;
				transform.localEulerAngles = new Vector3 (0, 0, 0);
			} else {
				Debug.LogWarning ("warning: astarMap not match spriteMap:"+(m_row+1)+","+(m_col));
			}
		}
	}
	void MoveDown(){
		CELL_TYPE type = (CELL_TYPE)m_mapCtr.GetMapCell (m_row-1, m_col).CellData.cellType;
		if (type != CELL_TYPE.STONE && type != CELL_TYPE.WALL) {
			MapCellCtr target = m_mapCtr.GetMapCell (m_row - 1, m_col);
			if (null != target) {
				m_targetPos = target.Position;
				m_isMoving = true;
				m_dir = new Vector3 (0, -1, 0);
				m_isLookLeft = false;
				if (type == CELL_TYPE.NONE || type == CELL_TYPE.POLE) {
					m_ani.SetBool ("jump", true);
				} else {
					m_ani.SetBool ("run", false);
					m_ani.SetBool ("climb", true);
				}
				transform.localEulerAngles = new Vector3 (0, 0, 0);
			} else {
				Debug.LogWarning ("warning: astarMap not match spriteMap:" + (m_row - 1) + "," + (m_col));
			}
		} else {
			m_ani.SetBool ("climb", false);
		}
	}

	public void Move(float horizontalInput, float verticalInput){
		
		if (m_isMoving || m_isAttack)
			return;
		hInput = horizontalInput;
		vInput = verticalInput;
		if (Mathf.Abs (hInput) > 0.5f) {
			if(hInput > 0)
			{
				MoveRight ();
			}else{
				MoveLeft ();
			}
		}
		//
		if (Mathf.Abs (vInput) > 0.5f) {
			if(vInput > 0)
			{
				MoveUp ();
			}else{
				MoveDown ();
			}
		}
	}
}
