using UnityEngine;
using System.Collections;

public class PlayerCtrEx : MonoBehaviour {
	protected int m_row, m_col;
	public int Row{
		get{ return m_row;}
	}
	public int Column{
		get{ return m_col;}
	}
	protected MapCtr m_mapCtr;
	public MapCtr Map{
		get{return m_mapCtr;}
	}
	protected MapCellCtr m_curCell;
	public MapCellCtr MapCell{
		get{return m_curCell;}
	}
	[SerializeField]
	protected float m_g=-9.8f;
	[SerializeField]
	protected float m_jumpForce = 400;//m = 1
	[SerializeField]
	protected float m_moveSpeed=2;
	public void AddMoveSpeed(float add){
		m_moveSpeed += add;
	}
	protected Animator m_ani;
	protected bool m_isGround;
	protected Vector2 m_dir;
	protected GameObject upGO, downGO, leftGO, rightGO;
	public void Init(int row, int col, MapCtr map){
		m_isGround = false;
		m_row = row;
		m_col = col;
		m_mapCtr = map;
		m_curCell = m_mapCtr.GetMapCell (row, col);
		transform.position = m_curCell.Position;
		m_ani = GetComponentInChildren<Animator> ();
		//
		upGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
		upGO.transform.localScale = Vector3.one*0.1f;
		downGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
		downGO.transform.localScale = Vector3.one*0.1f;
		leftGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
		leftGO.transform.localScale = Vector3.one*0.1f;
		rightGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
		rightGO.transform.localScale = Vector3.one*0.1f;
	}
	private float vInput, hInput;
	private float m_curA = 0;

	void FixedUpdate(){
		m_isGround = false;

		m_curA += m_g;

		Vector2 velocity = m_curA*new Vector2(0, 1)*Time.fixedDeltaTime + m_dir*m_moveSpeed;
		//Debug.Log("a:"+m_curA+" velocity:"+velocity+" time:"+Time.fixedDeltaTime+" dir:"+m_dir);
		if(velocity.x > 0){
			Vector2 rightPos = new Vector2(transform.position.x +  (float)m_mapCtr.Map.cellWidth*0.5f, transform.position.y);  
			rightGO.transform.position = new Vector3(rightPos.x, rightPos.y, 0);
			MapCellCtr rightCell = m_mapCtr.GetMapCell(rightPos);
			CELL_TYPE cellType = (CELL_TYPE)rightCell.CellData.cellType;
			if(cellType == CELL_TYPE.STONE || cellType == CELL_TYPE.WALL){
				velocity = new Vector2(0, velocity.y);
			}
		}else{
			Vector2 leftPos = new Vector2(transform.position.x - (float) m_mapCtr.Map.cellWidth*0.5f, transform.position.y);  
			leftGO.transform.position = new Vector3(leftPos.x, leftPos.y, 0);
			MapCellCtr leftCell = m_mapCtr.GetMapCell(leftPos);
			CELL_TYPE cellType = (CELL_TYPE)leftCell.CellData.cellType;
			if(cellType == CELL_TYPE.STONE || cellType == CELL_TYPE.WALL){
				velocity = new Vector2(0, velocity.y);
			}
		}

		//
		if(velocity.y > 0){
			Vector2 upPos = new Vector2(transform.position.x, transform.position.y + (float)m_mapCtr.Map.cellHeight*0.5f);  
			upGO.transform.position = new Vector3(upPos.x, upPos.y, 0);
			MapCellCtr upCell = m_mapCtr.GetMapCell(upPos);
			CELL_TYPE cellType = (CELL_TYPE)upCell.CellData.cellType;
			if(cellType == CELL_TYPE.STONE || cellType == CELL_TYPE.WALL){
				velocity = new Vector2(velocity.x, 0);
				m_curA = 0;
			}
		}else{
			Vector2 downPos = new Vector2(transform.position.x, transform.position.y - (float)m_mapCtr.Map.cellHeight*0.5f);  
			downGO.transform.position = new Vector3(downPos.x, downPos.y, 0);
			MapCellCtr downCell = m_mapCtr.GetMapCell(downPos);
			CELL_TYPE cellType = (CELL_TYPE)downCell.CellData.cellType;
			if(cellType == CELL_TYPE.STONE || cellType == CELL_TYPE.WALL || cellType == CELL_TYPE.LADDER ||
				cellType == CELL_TYPE.POLE){
				velocity = new Vector2(velocity.x, 0);
				m_isGround = true;
				if(m_curA < m_g){
					transform.position = new Vector3(transform.position.x, downCell.Position.y+(float)m_mapCtr.Map.cellHeight-0.01f, transform.position.z);
				}
				m_curA = 0;
			}
		}

		Vector2 dist = velocity*Time.fixedDeltaTime;
		transform.position += new Vector3(dist.x, dist.y, 0);

	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			vInput = 1;
		} else if (Input.GetKey (KeyCode.S)) {
			vInput = -1;
		} else if (Input.GetKey (KeyCode.A)) {
			hInput = -1;
		} else if (Input.GetKey (KeyCode.D)) {
			hInput = 1;
		}
		if (Input.GetKeyUp (KeyCode.W)) {
			vInput = 0;
		}
		if (Input.GetKeyUp (KeyCode.S)) {
			vInput = 0;
		}
		if (Input.GetKeyUp (KeyCode.A)) {
			hInput = 0;
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			hInput = 0;
		}
		if(m_isGround && Input.GetKeyDown(KeyCode.Space)){
			m_curA = m_jumpForce;
		}
		m_dir = new Vector2(hInput, vInput);
	}
}
