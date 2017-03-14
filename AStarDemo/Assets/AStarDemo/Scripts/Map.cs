using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map {

	protected uint row;
	public uint Row{
		get{ return row;}
		set{ row = value;}
	}

	protected uint column;
	public uint Column{
		get{ return column;}
		set{ column = value;}
	}

	public List<MapCell> cells ;
	public MapCell GetCell(uint row, uint column){
		if (row > this.row || column > this.column) {
			return null;
		}
		return cells [(int)(row * this.column + column)];
	}
	public bool SetCell(uint row, uint column, MapCell mc){
		if (row > this.row || column > this.column) {
			return false;
		}
		cells [(int)(row * this.column + column)] = mc;
		return true;
	}

	public Map(uint row, uint column){
		this.row = row;
		this.column = column;
		cells = new List<MapCell>();

		for (int i = 0; i < column; i++) {
			for (int j = 0; j < row; j++) {
				cells.Add (new MapCell ());
			}
		}
	}
	public Map(){
		row = 0;
		column = 0;
		cells = new List<MapCell>();
	}
}
