using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerData))]
public class PlayerCtr : MonoBehaviour {
	private PlayerData m_data;
	public int Row{
		get{return m_data.Row;}
	}
	public int Column{
		get{return m_data.Column;}
	}
	private float vInput, hInput;
	// Use this for initialization
	public void Init(int row, int col, MapCtr map){
		m_data = GetComponent<PlayerData> ();
		m_data.Init (row, col, map);
		m_data.RegisterMoveOverEvent(MoveOver);
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
		}else if((CELL_TYPE)pd.MapCell.CellData.cellType == CELL_TYPE.UNDERPANT){
			pd.MapCell.ChangeToNone(false);
			GameStateManager.Instance().FSM.CurrentState.Message("GetUnderPant", p);
		}else if((CELL_TYPE)pd.MapCell.CellData.cellType == CELL_TYPE.SOCKS){
			pd.MapCell.ChangeToNone(false);
			GameStateManager.Instance().FSM.CurrentState.Message("GetSocks", p);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
			m_data.CheckAttack ();
		}
		if (Input.GetKeyDown (KeyCode.W)) {
			vInput = 1;
		} else if (Input.GetKeyDown (KeyCode.S)) {
			vInput = -1;
		} else if (Input.GetKeyDown (KeyCode.A)) {
			hInput = -1;
		} else if (Input.GetKeyDown (KeyCode.D)) {
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

//		hInput = Input.GetAxis("Horizontal");
//		vInput = Input.GetAxis("Vertical");
		m_data.Move (hInput, vInput);
	}
}
