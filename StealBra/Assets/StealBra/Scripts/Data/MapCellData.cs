using UnityEngine;
using System.Collections;

public enum CELL_TYPE{
	WALL,
	STONE,
	LADDER,
	POLE,
	BRA,
	UNDERPANT,
	SOCKS,
	NONE,//air
}
public enum CELL_ADDITION{
	NONE,
	PLAYER,
	DOG,
	EXIT,
}
[System.Serializable]
public class MapCellData {

	public int cellType;
	public int cellAdd;
	public string resName;
	public int row;
	public int col;
	public string sortLayer;
	public int sortOrder;
	public MapCellData(){
		cellType = 0;
		cellAdd = 0;
		resName = "";
		row = 0;
		col = 0;
		sortLayer = "";
		sortOrder = 0;
	}
}
