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
[System.Serializable]
public class MapCellData {

	public int cellType;
	public string resName;
	public int row;
	public int col;
	public string sortLayer;
	public int sortOrder;
	public MapCellData(){
		cellType = 0;
		resName = "";
		row = 0;
		col = 0;
		sortLayer = "";
		sortOrder = 0;
	}
}
