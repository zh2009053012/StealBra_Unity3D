using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerData))]
public class PlayerCtr : MonoBehaviour {
	private PlayerData m_data;
	protected bool m_canControl=true;
	public bool CanControl{
		get{ return m_canControl;}
		set{m_canControl = value;}
	}
	public int Row{
		get{return m_data.Row;}
	}
	public int Column{
		get{return m_data.Column;}
	}
	private float vInput, hInput;
	private bool isJump;
	// Use this for initialization
	public void Init(int row, int col, MapCtr map){
		m_data = GetComponent<PlayerData> ();
		m_data.Init (row, col, map);
		m_data.RegisterMoveOverEvent(MoveOver);
		m_data.RegisterStateChangeEvent(StateChange);
	}
	public void AddMoveSpeed(float add){
		m_data.AddMoveSpeed(add);
	}
	void MoveOver(PlayerData pd){
		object[] p = new object[2];
		p[0] = (object)pd.Row;
		p[1] = (object)pd.Column;
		if((CELL_TYPE)pd.MapCell.CellData.cellType == CELL_TYPE.BRA){
			pd.MapCell.ChangeToNone(false);
			GameStateManager.Instance().FSM.CurrentState.Message("GetBra", p);
			AudioManager.Instance.PlayAudio("get");
		}else if((CELL_TYPE)pd.MapCell.CellData.cellType == CELL_TYPE.UNDERPANT){
			pd.MapCell.ChangeToNone(false);
			GameStateManager.Instance().FSM.CurrentState.Message("GetUnderpant", p);
			AudioManager.Instance.PlayAudio("get");
		}else if((CELL_TYPE)pd.MapCell.CellData.cellType == CELL_TYPE.SOCKS){
			pd.MapCell.ChangeToNone(false);
			GameStateManager.Instance().FSM.CurrentState.Message("GetSocks", p);
			AudioManager.Instance.PlayAudio("get");
		}
		if(pd.Map.CanExit && (CELL_ADDITION)pd.MapCell.CellData.cellAdd == CELL_ADDITION.EXIT){
			GameStateManager.Instance().FSM.CurrentState.Message("Vectory", p);
		}
		//
	}
	void CheckDead(){
		
	}
	//
	protected PlayerData.PlayerState m_preState = PlayerData.PlayerState.STAND;
	void StateChange(PlayerData pd){
		if( pd.State != m_preState)
		{
			StopStateAudio(m_preState);
			m_preState = pd.State;
		}
		PlayStateAudio(pd.State);


	}
	void PlayStateAudio(PlayerData.PlayerState state){
		switch(state){
		case PlayerData.PlayerState.ATTACK:
			AudioManager.Instance.PlayAudio("attack");
			break;
		case PlayerData.PlayerState.CLIMB:
			AudioManager.Instance.PlayAudio("climb", true);
			break;
		case PlayerData.PlayerState.DEAD:
			AudioManager.Instance.PlayAudio("dead");
			break;
		case PlayerData.PlayerState.RUN:
			//AudioManager.Instance.PlayAudio("run", true);
			break;
		case PlayerData.PlayerState.STAND:
			
			break;
		}
	}
	void StopStateAudio(PlayerData.PlayerState state){
		switch(state){
		case PlayerData.PlayerState.ATTACK:
			break;
		case PlayerData.PlayerState.CLIMB:
			AudioManager.Instance.StopAudio("climb");
			break;
		case PlayerData.PlayerState.DEAD:
			break;
		case PlayerData.PlayerState.RUN:
			//AudioManager.Instance.StopAudio("run");
			break;
		case PlayerData.PlayerState.STAND:

			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		return;
		if(!CanControl)
			return;
		if (Input.GetKeyDown (KeyCode.I)) {
			m_data.CheckAttack ();
		}
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
		if(Input.GetKeyDown(KeyCode.Space)){
			isJump = true;
		}else{
			isJump = false;
		}
//		hInput = Input.GetAxis("Horizontal");
//		vInput = Input.GetAxis("Vertical");
		m_data.Move (hInput, vInput, true);
	}
}
