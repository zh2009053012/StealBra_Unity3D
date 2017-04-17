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
	protected bool m_canControl=true;
	public bool CanControl{
		get{ return m_canControl;}
		set{m_canControl = value;}
	}
	[SerializeField]
	protected float m_g=-9.8f;
	[SerializeField]
	protected float m_jumpForce = 250;//m = 1
	[SerializeField]
	protected float m_moveSpeed=3;
	[SerializeField]
	protected GameObject m_body;
	public void AddMoveSpeed(float add){
		m_moveSpeed += add;
	}
	protected Animator m_ani;
	protected bool m_isGround;
	protected Vector2 m_dir;
	public void Init(int row, int col, MapCtr map){
		m_isGround = false;
		m_row = row;
		m_col = col;
		m_mapCtr = map;
		m_curCell = m_mapCtr.GetMapCell (row, col);
		transform.position = m_curCell.Position;
		m_ani = GetComponentInChildren<Animator> ();
	}
	private float m_vInput, m_hInput;
	[SerializeField]
	private int m_look=1;
	private float m_verticalAccelerate = 0;
	private bool m_isClimb = false;
	private bool m_isRun = false;
	private bool m_isHitInput = false;
	private bool m_isAttack = false;

	public void OnAttackOver(){
		m_isAttack = false;
		m_ani.SetBool ("attack", m_isAttack);
	}

	void FixedUpdate(){
		//上格子
		Vector2 upPos = new Vector2(transform.position.x, transform.position.y + (float)m_mapCtr.Map.cellHeight*0.5f);  
		MapCellCtr upCell = m_mapCtr.GetMapCell(upPos);
		CELL_TYPE upCellType = (CELL_TYPE)upCell.CellData.cellType;
		//下
		MapCellCtr downCell = m_mapCtr.GetMapCell(new Vector2(transform.position.x, transform.position.y - (float)m_mapCtr.Map.cellHeight*0.5f));
		CELL_TYPE downCellType = (CELL_TYPE)downCell.CellData.cellType;
		//右
		Vector2 rightPos = new Vector2(transform.position.x +  (float)m_mapCtr.Map.cellWidth*0.5f, transform.position.y);  
		MapCellCtr rightCell = m_mapCtr.GetMapCell(rightPos);
		CELL_TYPE rightCellType = (CELL_TYPE)rightCell.CellData.cellType;
		//左
		Vector2 leftPos = new Vector2(transform.position.x - (float) m_mapCtr.Map.cellWidth*0.5f, transform.position.y);  
		MapCellCtr leftCell = m_mapCtr.GetMapCell(leftPos);
		CELL_TYPE leftCellType = (CELL_TYPE)leftCell.CellData.cellType;
		//foot
		MapCellCtr footCell = m_mapCtr.GetMapCell(new Vector2(transform.position.x, transform.position.y - (float)m_mapCtr.Map.cellHeight*0.5f + 0.1f));
		CELL_TYPE footCellType = (CELL_TYPE)footCell.CellData.cellType;
		//cur
		m_curCell = m_mapCtr.GetMapCell(new Vector2(transform.position.x, transform.position.y));
		CELL_TYPE curCellType = (CELL_TYPE)m_curCell.CellData.cellType;
		m_row = m_curCell.CellData.row;
		m_col = m_curCell.CellData.col;

		//检查是否劈砖
		float angleY = m_body.transform.localRotation.eulerAngles.y;
		angleY = angleY > 180 ? (angleY-360):angleY;
		if(angleY > 10){
			m_look = 1;
		}else if(angleY < -10){
			m_look = -1;
		}else{
			m_look = 0;
		}
		if(m_isHitInput){
			m_isHitInput = false;
			if(downCellType == CELL_TYPE.WALL || downCellType == CELL_TYPE.STONE){
				if(m_look != 0){
					MapCellCtr target = m_mapCtr.GetMapCell (m_row, m_col+m_look);
					if ((CELL_TYPE)target.CellData.cellType == CELL_TYPE.NONE) {
						MapCellCtr targetDown = m_mapCtr.GetMapCell (m_row-1, m_col+m_look);
						if ((CELL_TYPE)targetDown.CellData.cellType == CELL_TYPE.WALL) {
							m_isAttack = true;
							m_ani.SetBool ("run", false);
							m_ani.SetBool ("attack", m_isAttack);
							targetDown.ChangeToNone (true);
							AudioManager.Instance.PlayAudio("attack");
						}
					}
				}
			}
		}
		if(m_isAttack){
			return;
		}

		//计算重力
		m_verticalAccelerate += m_g;
		if(m_verticalAccelerate > 0){
			if(upCellType == CELL_TYPE.STONE || upCellType == CELL_TYPE.WALL){
				m_verticalAccelerate = 0;
			}
		}else if(m_verticalAccelerate < 0){
			if(downCellType == CELL_TYPE.STONE || downCellType == CELL_TYPE.WALL || 
				downCellType == CELL_TYPE.LADDER || downCellType == CELL_TYPE.POLE){
				m_verticalAccelerate = 0;
				m_isGround = true;
			}else{
				m_isGround = false;
			}
			//
			if(downCellType == CELL_TYPE.STONE || downCellType == CELL_TYPE.WALL){
				transform.position = new Vector3(transform.position.x, downCell.Position.y+(float)m_mapCtr.Map.cellHeight-0.01f, transform.position.z);
			}
		}else{
			m_isGround = true;
		}
		//计算左右位移和上下楼梯
		if(m_dir.x > 0){
			if(rightCellType == CELL_TYPE.STONE || rightCellType == CELL_TYPE.WALL){
				m_dir = new Vector2(0, m_dir.y);
			}
		}else if(m_dir.x < 0){
			if(leftCellType == CELL_TYPE.STONE || leftCellType == CELL_TYPE.WALL){
				m_dir = new Vector2(0, m_dir.y);
			}
		}

		//
		if(m_dir.y > 0){

			if(footCellType != CELL_TYPE.LADDER){
				m_dir = new Vector2(m_dir.x, 0);
			}
		}else{
			
			if(downCellType == CELL_TYPE.STONE || downCellType == CELL_TYPE.WALL)
			{
				m_dir = new Vector2(m_dir.x, 0);
			}

		}
		//最后综合考虑重力和输入的上下左右
		Vector2 velocity = m_verticalAccelerate*new Vector2(0, 1)*Time.fixedDeltaTime + m_dir*m_moveSpeed;
		Vector2 dist = velocity*Time.fixedDeltaTime;
		transform.position += new Vector3(dist.x, dist.y, 0);

		//animation
		//是否播放run动画
		if(Mathf.Abs(m_dir.x) > 0 && m_isGround){
			if( (footCellType != CELL_TYPE.LADDER && footCellType != CELL_TYPE.POLE) || 
				(downCellType == CELL_TYPE.WALL || downCellType == CELL_TYPE.STONE)){
				m_isRun = true;
			}else{
				m_isRun = false;
			}
		}else{
			m_isRun = false;
		}
		//是否播放climb动画
		if(m_isGround &&  footCellType == CELL_TYPE.LADDER)
		{
			if(downCellType == CELL_TYPE.STONE || downCellType == CELL_TYPE.WALL)
				m_isClimb = false;
			else
				m_isClimb = true;
		}
//		else if(m_isGround && curCellType == CELL_TYPE.POLE && Mathf.Abs(m_dir.x) > 0){
//			m_isClimb = true;
//		}
		else{
			m_isClimb = false;
		}

		m_ani.SetBool("climb", m_isClimb);
		if(m_isClimb){
			if(Mathf.Abs(m_dir.y) > 0)
				AudioManager.Instance.PlayAudio("climb", true);
			else
				AudioManager.Instance.StopAudio("climb");
			m_body.transform.localEulerAngles = new Vector3 (0, 0, 0);
		}else{
			AudioManager.Instance.StopAudio("climb");
			//m_body.transform.localEulerAngles = new Vector3 (0, 90, 0);
		}

		m_ani.SetBool("run", m_isRun);
		if(m_isRun){
			AudioManager.Instance.PlayAudio("run", true);
			if(velocity.x > 0){
				m_body.transform.localEulerAngles = new Vector3 (0, 90, 0);
			}else{
				m_body.transform.localEulerAngles = new Vector3 (0, -90, 0);
			}
		}else{
			AudioManager.Instance.StopAudio("run");
		}
		//是否播放jump动画
		if(m_verticalAccelerate > 0 && !m_isGround && !m_ani.GetBool("jump")){
			m_ani.SetBool("jump", true);
			AudioManager.Instance.PlayAudio("jump");
		}
		if(m_isGround){
			m_ani.SetBool("jump", false);
		}
		if(m_verticalAccelerate < 0 && !m_isGround){
			if(m_dir.x > 0){
				m_body.transform.localEulerAngles = new Vector3 (0, 90, 0);
			}else if(m_dir.x < 0){
				m_body.transform.localEulerAngles = new Vector3 (0, -90, 0);
			}
		}
		//检查当前格子
		object[] p = new object[2];
		p[0] = (object)Row;
		p[1] = (object)Column;
		if(curCellType == CELL_TYPE.BRA){
			MapCell.ChangeToNone(false);
			GameStateManager.Instance().FSM.CurrentState.Message("GetBra", p);
			AudioManager.Instance.PlayAudio("get");
		}else if(curCellType == CELL_TYPE.UNDERPANT){
			MapCell.ChangeToNone(false);
			GameStateManager.Instance().FSM.CurrentState.Message("GetUnderpant", p);
			AudioManager.Instance.PlayAudio("get");
		}else if(curCellType == CELL_TYPE.SOCKS){
			MapCell.ChangeToNone(false);
			GameStateManager.Instance().FSM.CurrentState.Message("GetSocks", p);
			AudioManager.Instance.PlayAudio("get");
		}

	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			m_vInput = 1;
		} else if (Input.GetKey (KeyCode.S)) {
			m_vInput = -1;
		} else if (Input.GetKey (KeyCode.A)) {
			m_hInput = -1;
		} else if (Input.GetKey (KeyCode.D)) {
			m_hInput = 1;
		}
		if (Input.GetKeyUp (KeyCode.W)) {
			m_vInput = 0;
		}
		if (Input.GetKeyUp (KeyCode.S)) {
			m_vInput = 0;
		}
		if (Input.GetKeyUp (KeyCode.A)) {
			m_hInput = 0;
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			m_hInput = 0;
		}
		if(m_isGround && Input.GetKeyDown(KeyCode.Space)){
			m_verticalAccelerate = m_jumpForce;
			m_isGround = false;
		}
		m_dir = new Vector2(m_hInput, m_vInput);
		if(!m_isAttack && m_isGround && Input.GetKeyDown(KeyCode.I)){
			m_isHitInput = true;
		}
	}
}
