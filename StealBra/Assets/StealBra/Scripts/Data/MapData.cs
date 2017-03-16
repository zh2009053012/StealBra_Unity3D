using UnityEngine;
using System.Collections.Generic;

public class MapData {

	public List<MapCellData> cells;
	public List<DecorationData> decorations;
	public int row;
	public int column;
	public double cellWidth;
	public double cellHeight;
	public double startPosX;
	public double startPosY;
	public double startPosZ;
	public MapData(){
		row = 0;
		column = 0;
		cellWidth = 0;
		cellHeight = 0;
		startPosX = 0;
		startPosY = 0;
		startPosZ = 0;
		cells = new List<MapCellData> ();
		decorations = new List<DecorationData> ();
	}
}
