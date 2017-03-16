using UnityEngine;
using System.Collections;

[SerializeField]
public class AStarMapCell {
	protected bool[,] m_passArray = new bool[3,3];
	public void PrintPassArray(){
		for(int i=0; i<3; i++){
			for(int j=0; j<3; j++){
				Debug.Log(i+","+j+":"+m_passArray[i, j].ToString());
			}
		}
	}
	protected int m_row;
	public int Row{
		get{ return m_row;}
		set{ m_row = value;}
	}
	protected int m_column;
	public int Column{
		get{return m_column;}
		set{ m_column = value;}
	}
	protected Vector3 m_position;
	public Vector3 GetPosition(){
		return m_position;
	}
	public void SetPosition(Vector3 pos)
	{
		m_position = pos;
	}
	protected AStarMapCell m_parent;
	public AStarMapCell GetParent(){
		return m_parent;
	}
	public void SetParent(AStarMapCell mc){
		m_parent = mc;
	}

	protected int m_h;
	public int H{
		get{ return m_h; }
		set{ m_h = value;}
	}
	public void CalculateH(AStarMapCell end){
		int dis = Mathf.Abs (end.Row - Row) + Mathf.Abs (end.Column - Column);
		H = dis * 10;
	}
	public int F{
		get{ return G + H;}
	}
	protected int m_g;
	public int G{
		get{ return m_g;}
		set{ m_g = value;}
	}
	public void CalculateG(AStarMapCell neighbor){
		G = neighbor.G + CalculateNeighborDist (neighbor);
	}
	public int CalculateNeighborDist(AStarMapCell neighbor){
		int disY = Mathf.Abs (neighbor.Row - Row);
		int disX = Mathf.Abs (neighbor.Column - Column);
		if (disX > 1 || disY > 1 || (disX == 0 && disY == 0))
			return 0;
		if (disX == 1 && disY == 1) {
			return 14;
		} else {
			return 10;
		}
	}

	public bool IsInCloseList = false;
	public bool IsInOpenList = false;

	public bool IsToLeft{
		get{ return m_passArray [0, 1];}
		set{ m_passArray [0, 1] = value;}
	}
	public bool IsToRight{
		get{ return m_passArray [2, 1];}
		set{ m_passArray [2, 1] = value;}
	}
	public bool IsToUp{
		get{ return m_passArray [1, 2];}
		set{ m_passArray [1, 2] = value;}
	}
	public bool IsToDown{
		get{ return m_passArray [1, 0];}
		set{ m_passArray [1, 0] = value;}
	}
	public bool IsToLeftUp{
		get{ return m_passArray [0, 0];}
		set{ m_passArray [0, 0] = value;}
	}
	public bool IsToLeftDown{
		get{ return m_passArray [0, 2];}
		set{ m_passArray [0, 2] = value;}
	}
	public bool IsToRightUp{
		get{ return m_passArray [2, 0];}
		set{ m_passArray [2, 0] = value;}
	}
	public bool IsToRightDown{
		get{ return m_passArray [2, 2];}
		set{ m_passArray [2, 2] = value;}
	}
	public AStarMapCell(){}

	public bool CanArrive(AStarMapCell target){
		int x = target.Column - Column + 1;
		int y = target.Row - Row + 1;
		if (x >= 0 && x < 3 && y >= 0 && y < 3) {
			return m_passArray [x, y];
		} else {
			return false;
		}
	}
}
