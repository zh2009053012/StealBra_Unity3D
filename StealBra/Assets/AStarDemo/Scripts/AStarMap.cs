using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarMap {

	protected int row;
	public int Row{
		get{ return row;}
		set{ row = value;}
	}

	protected int column;
	public int Column{
		get{ return column;}
		set{ column = value;}
	}

	protected Vector2 cellSize;
	public Vector2 GetCellSize(){
		return cellSize;
	}

	protected Vector2 startPos;//left_down corner
	/// <summary>
	/// left_down corner position
	/// </summary>
	/// <value>The start position.</value>
	public Vector2 GetStartPos(){
		return startPos;
	}

	public List<AStarMapCell> cells ;
	public AStarMapCell GetCell(int row, int column){
		if (row > this.row || column > this.column) {
			return null;
		}
		return cells [row * this.column + column];
	}
	public AStarMapCell GetCell(Vector2 pos){
		Vector2 offset = pos - startPos;
		int row = Mathf.FloorToInt( offset.y / cellSize.y );
		int col = Mathf.FloorToInt (offset.x / cellSize.x);
		return GetCell (row, col);
	}
	public bool SetCell(int row, int column, AStarMapCell mc){
		if (row > this.row || column > this.column) {
			return false;
		}
		cells [row * this.column + column] = mc;
		return true;
	}

	public AStarMap(int row, int column){
		this.row = row;
		this.column = column;
		cells = new List<AStarMapCell>();

		for (int i = 0; i < column; i++) {
			for (int j = 0; j < row; j++) {
				cells.Add (new AStarMapCell ());
			}
		}
	}
	public AStarMap(int row, int column, Vector2 cellSize, Vector2 startPos){
		this.row = row;
		this.column = column;
		cells = new List<AStarMapCell>();

		for (int i = 0; i < column; i++) {
			for (int j = 0; j < row; j++) {
				cells.Add (new AStarMapCell ());
			}
		}
		this.cellSize = cellSize;
		this.startPos = startPos;
	}
	public AStarMap(){
		row = 0;
		column = 0;
		cells = new List<AStarMapCell>();
	}
	public void CalculateCellPosition(Vector2 cellSize, Vector2 startPos){
		this.cellSize = cellSize;
		this.startPos = startPos;
		for (int i = 0; i < column; i++) {
			for (int j = 0; j < row; j++) {
				cells [j * this.column + i].SetPosition ( startPos + new Vector2 (cellSize.x*i, cellSize.y*j));
			}
		}
	}
}
