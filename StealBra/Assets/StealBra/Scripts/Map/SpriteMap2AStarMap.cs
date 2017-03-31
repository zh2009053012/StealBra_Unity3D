using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using System.Text;

public class SpriteMap2AStarMap {

	public static AStarMap SpriteMapToAStarMap(MapData map){
		AStarMap astar = new AStarMap (map.row, map.column);
		for (int i = 0; i < map.row; i++) {
			for (int j = 0; j < map.column; j++) {
				SetAStarMapCell_First (map, map.cells [i * map.column + j], ref astar);
			}
		}
		for (int i = 0; i < map.row; i++) {
			for (int j = 0; j < map.column; j++) {
				SetAStarMapCell_Second (map, map.cells [i * map.column + j], ref astar);
			}
		}

		return astar;
	}
	public static void ResetAStarMapCell(MapData map, MapCellData cell, ref AStarMap astarMap){
		SetAStarMapCell_First (map, cell, ref astarMap);
		SetAStarMapCell_Second (map, cell, ref astarMap);
	}
	static void SetAStarMapCell_Second(MapData map, MapCellData cell, ref AStarMap astarMap){

		// check second time
		CELL_TYPE curType = (CELL_TYPE)cell.cellType;
		if (curType == CELL_TYPE.STONE || curType == CELL_TYPE.WALL)
			return;
		if (cell.row - 1 >= 0) {
			MapCellData downCell = map.cells [(cell.row - 1) * map.column + cell.col];
			CELL_TYPE downType = (CELL_TYPE)downCell.cellType;
			if (downType == CELL_TYPE.WALL || downType == CELL_TYPE.STONE ||
				downType == CELL_TYPE.LADDER) {
				//right
				if (cell.col + 1 < map.column) {
					MapCellData rightCell = map.cells [(cell.row) * map.column + cell.col + 1];
					CELL_TYPE rightType = (CELL_TYPE)rightCell.cellType;
					if (rightType != CELL_TYPE.WALL && rightType != CELL_TYPE.STONE) {
						//astarMapCell.IsToRight = true;
						astarMap.GetCell (rightCell.row, rightCell.col).IsToLeft = true;
					}
				}
				//left
				if (cell.col - 1 >= 0) {
					MapCellData leftCell = map.cells [(cell.row) * map.column + cell.col - 1];
					CELL_TYPE leftType = (CELL_TYPE)leftCell.cellType;
					if (leftType != CELL_TYPE.WALL && leftType != CELL_TYPE.STONE) {
						//astarMapCell.IsToLeft = true;
						astarMap.GetCell (leftCell.row, leftCell.col).IsToRight = true;
					}
				}
			}
		}
	}
	static void SetAStarMapCell_First(MapData map, MapCellData cell, ref AStarMap astarMap){
		AStarMapCell astarMapCell = astarMap.GetCell (cell.row, cell.col);
		astarMapCell.IsToLeftUp = false;
		astarMapCell.IsToLeftDown = false;
		astarMapCell.IsToRightUp = false;
		astarMapCell.IsToRightDown = false;

		//check up
		if (cell.row + 1 < map.row) {
			MapCellData upCell = map.cells [(cell.row + 1) * map.column + cell.col];
			AStarMapCell upAStarCell = astarMap.GetCell (cell.row + 1, cell.col);
			switch ((CELL_TYPE)cell.cellType) {
			case CELL_TYPE.WALL:
				upAStarCell.IsToDown = false;
				astarMapCell.IsToUp = false;
				break;
			case CELL_TYPE.STONE:
				upAStarCell.IsToDown = false;
				astarMapCell.IsToUp = false;
				break;
			case CELL_TYPE.LADDER:
				switch ((CELL_TYPE)upCell.cellType) {
				case CELL_TYPE.WALL:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = false;
					break;
				case CELL_TYPE.STONE:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = false;
					break;
				case CELL_TYPE.LADDER:
					upAStarCell.IsToDown = true;
					astarMapCell.IsToUp = true;
					break;
				case CELL_TYPE.POLE:
					upAStarCell.IsToDown = true;
					astarMapCell.IsToUp = true;
					break;
				case CELL_TYPE.NONE:
					upAStarCell.IsToDown = true;
					astarMapCell.IsToUp = true;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					upAStarCell.IsToDown = true;
					astarMapCell.IsToUp = true;
					break;
				}
				break;
			case CELL_TYPE.POLE:
				switch ((CELL_TYPE)upCell.cellType) {
				case CELL_TYPE.WALL:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = false;
					break;
				case CELL_TYPE.STONE:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = false;
					break;
				case CELL_TYPE.LADDER:
					upAStarCell.IsToDown = true;
					astarMapCell.IsToUp = true;
					break;
				case CELL_TYPE.POLE:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = false;
					break;
				case CELL_TYPE.NONE:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = true;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = true;
					break;
				}
				break;
			case CELL_TYPE.NONE:
				switch ((CELL_TYPE)upCell.cellType) {
				case CELL_TYPE.WALL:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = false;
					break;
				case CELL_TYPE.STONE:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = false;
					break;
				case CELL_TYPE.LADDER:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = true;
					break;
				case CELL_TYPE.POLE:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = true;
					break;
				case CELL_TYPE.NONE:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = true;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = true;
					break;
				}
				break;
			default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
				switch ((CELL_TYPE)upCell.cellType) {
				case CELL_TYPE.WALL:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = false;
					break;
				case CELL_TYPE.STONE:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = false;
					break;
				case CELL_TYPE.LADDER:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = true;
					break;
				case CELL_TYPE.POLE:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = true;
					break;
				case CELL_TYPE.NONE:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = true;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					upAStarCell.IsToDown = false;
					astarMapCell.IsToUp = true;
					break;
				}
				break;
			}
		}
		//down
		if (cell.row - 1 >= 0) {
			MapCellData downCell = map.cells [(cell.row - 1) * map.column + cell.col];
			AStarMapCell downAStarCell = astarMap.GetCell (cell.row - 1, cell.col);
			switch ((CELL_TYPE)cell.cellType) {
			case CELL_TYPE.WALL:
				astarMapCell.IsToDown = false;
				downAStarCell.IsToUp = false;
				break;
			case CELL_TYPE.STONE:
				astarMapCell.IsToDown = false;
				downAStarCell.IsToUp = false;
				break;
			case CELL_TYPE.LADDER:
				switch ((CELL_TYPE)downCell.cellType) {
				case CELL_TYPE.WALL:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = false;
					break;
				case CELL_TYPE.STONE:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = false;
					break;
				case CELL_TYPE.LADDER:
					astarMapCell.IsToDown = true;
					downAStarCell.IsToUp = true;
					break;
				case CELL_TYPE.POLE:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = true;
					break;
				case CELL_TYPE.NONE:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = true;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = true;
					break;
				}
				break;
			case CELL_TYPE.POLE:
				switch ((CELL_TYPE)downCell.cellType) {
				case CELL_TYPE.WALL:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = false;
					break;
				case CELL_TYPE.STONE:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = false;
					break;
				case CELL_TYPE.LADDER:
					astarMapCell.IsToDown = true;
					downAStarCell.IsToUp = true;
					break;
				case CELL_TYPE.POLE:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = true;
					break;
				case CELL_TYPE.NONE:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = true;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = true;
					break;
				}
				break;
			case CELL_TYPE.NONE:
				switch ((CELL_TYPE)downCell.cellType) {
				case CELL_TYPE.WALL:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = false;
					break;
				case CELL_TYPE.STONE:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = false;
					break;
				case CELL_TYPE.LADDER:
					astarMapCell.IsToDown = true;
					downAStarCell.IsToUp = true;
					break;
				case CELL_TYPE.POLE:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = true;
					break;
				case CELL_TYPE.NONE:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = true;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = true;
					break;
				}
				break;
			default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
				switch ((CELL_TYPE)downCell.cellType) {
				case CELL_TYPE.WALL:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = false;
					break;
				case CELL_TYPE.STONE:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = false;
					break;
				case CELL_TYPE.LADDER:
					astarMapCell.IsToDown = true;
					downAStarCell.IsToUp = true;
					break;
				case CELL_TYPE.POLE:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = true;
					break;
				case CELL_TYPE.NONE:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = true;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					astarMapCell.IsToDown = false;
					downAStarCell.IsToUp = true;
					break;
				}
				break;
			}
		}
		//check left
		if (cell.col - 1 >= 0) {
			MapCellData leftCell = map.cells [(cell.row ) * map.column + cell.col-1];
			AStarMapCell leftAStarCell = astarMap.GetCell (cell.row, cell.col - 1);
			switch ((CELL_TYPE)cell.cellType) {
			case CELL_TYPE.WALL:
				leftAStarCell.IsToRight = false;
				astarMapCell.IsToLeft = false;
				break;
			case CELL_TYPE.STONE:
				leftAStarCell.IsToRight = false;
				astarMapCell.IsToLeft = false;
				break;
			case CELL_TYPE.LADDER:
				switch ((CELL_TYPE)leftCell.cellType) {
				case CELL_TYPE.WALL:
					leftAStarCell.IsToRight = false;
					astarMapCell.IsToLeft = false;
					break;
				case CELL_TYPE.STONE:
					leftAStarCell.IsToRight = false;
					astarMapCell.IsToLeft = false;
					break;
				case CELL_TYPE.LADDER:
					leftAStarCell.IsToRight = true;
					astarMapCell.IsToLeft = true;
					break;
				case CELL_TYPE.POLE:
					leftAStarCell.IsToRight = true;
					astarMapCell.IsToLeft = true;
					break;
				case CELL_TYPE.NONE:
					leftAStarCell.IsToRight = true;
					astarMapCell.IsToLeft = false;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					leftAStarCell.IsToRight = true;
					astarMapCell.IsToLeft = false;
					break;
				}
				break;
			case CELL_TYPE.POLE:
				switch ((CELL_TYPE)leftCell.cellType) {
				case CELL_TYPE.WALL:
					leftAStarCell.IsToRight = false;
					astarMapCell.IsToLeft = false;
					break;
				case CELL_TYPE.STONE:
					leftAStarCell.IsToRight = false;
					astarMapCell.IsToLeft = false;
					break;
				case CELL_TYPE.LADDER:
					leftAStarCell.IsToRight = true;
					astarMapCell.IsToLeft = true;
					break;
				case CELL_TYPE.POLE:
					leftAStarCell.IsToRight = true;
					astarMapCell.IsToLeft = true;
					break;
				case CELL_TYPE.NONE:
					leftAStarCell.IsToRight = true;
					astarMapCell.IsToLeft = false;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					leftAStarCell.IsToRight = true;
					astarMapCell.IsToLeft = false;
					break;
				}
				break;
			default://case CELL_TYPE.NONE || CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
				switch ((CELL_TYPE)leftCell.cellType) {
				case CELL_TYPE.WALL:
					leftAStarCell.IsToRight = false;
					astarMapCell.IsToLeft = false;
					break;
				case CELL_TYPE.STONE:
					leftAStarCell.IsToRight = false;
					astarMapCell.IsToLeft = false;
					break;
				case CELL_TYPE.LADDER:
					leftAStarCell.IsToRight = false;
					astarMapCell.IsToLeft = true;
					break;
				case CELL_TYPE.POLE:
					leftAStarCell.IsToRight = false;
					astarMapCell.IsToLeft = true;
					break;
				case CELL_TYPE.NONE:
					leftAStarCell.IsToRight = false;
					astarMapCell.IsToLeft = false;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					leftAStarCell.IsToRight = false;
					astarMapCell.IsToLeft = false;
					break;
				}
				break;
			}
		}
		//check right
		if (cell.col + 1 < map.column) {
			MapCellData rightCell = map.cells [(cell.row) * map.column + cell.col+1];
			AStarMapCell rightAStarCell = astarMap.GetCell (cell.row, cell.col+1);
			switch ((CELL_TYPE)cell.cellType) {
			case CELL_TYPE.WALL:
				astarMapCell.IsToRight = false;
				rightAStarCell.IsToLeft = false;
				break;
			case CELL_TYPE.STONE:
				astarMapCell.IsToRight = false;
				rightAStarCell.IsToLeft = false;
				break;
			case CELL_TYPE.LADDER:
				switch ((CELL_TYPE)rightCell.cellType) {
				case CELL_TYPE.WALL:
					astarMapCell.IsToRight = false;
					rightAStarCell.IsToLeft = false;
					break;
				case CELL_TYPE.STONE:
					astarMapCell.IsToRight = false;
					rightAStarCell.IsToLeft = false;
					break;
				case CELL_TYPE.LADDER:
					astarMapCell.IsToRight = true;
					rightAStarCell.IsToLeft = true;
					break;
				case CELL_TYPE.POLE:
					astarMapCell.IsToRight = true;
					rightAStarCell.IsToLeft = true;
					break;
				case CELL_TYPE.NONE:
					astarMapCell.IsToRight = false;
					rightAStarCell.IsToLeft = true;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					astarMapCell.IsToRight = false;
					rightAStarCell.IsToLeft = true;
					break;
				}
				break;
			case CELL_TYPE.POLE:
				switch ((CELL_TYPE)rightCell.cellType) {
				case CELL_TYPE.WALL:
					astarMapCell.IsToRight = false;
					rightAStarCell.IsToLeft = false;
					break;
				case CELL_TYPE.STONE:
					astarMapCell.IsToRight = false;
					rightAStarCell.IsToLeft = false;
					break;
				case CELL_TYPE.LADDER:
					astarMapCell.IsToRight = true;
					rightAStarCell.IsToLeft = true;
					break;
				case CELL_TYPE.POLE:
					astarMapCell.IsToRight = true;
					rightAStarCell.IsToLeft = true;
					break;
				case CELL_TYPE.NONE:
					astarMapCell.IsToRight = false;
					rightAStarCell.IsToLeft = true;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					astarMapCell.IsToRight = false;
					rightAStarCell.IsToLeft = true;
					break;
				}
				break;
			default://case CELL_TYPE.NONE || CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
				switch ((CELL_TYPE)rightCell.cellType) {
				case CELL_TYPE.WALL:
					astarMapCell.IsToRight = false;
					rightAStarCell.IsToLeft = false;
					break;
				case CELL_TYPE.STONE:
					astarMapCell.IsToRight = false;
					rightAStarCell.IsToLeft = false;
					break;
				case CELL_TYPE.LADDER:
					astarMapCell.IsToRight = true;
					rightAStarCell.IsToLeft = false;
					break;
				case CELL_TYPE.POLE:
					astarMapCell.IsToRight = true;
					rightAStarCell.IsToLeft = false;
					break;
				case CELL_TYPE.NONE:
					astarMapCell.IsToRight = false;
					rightAStarCell.IsToLeft = false;
					break;
				default://case CELL_TYPE.BRA || CELL_TYPE.UNDERPANT || CELL_TYPE.SOCKS:
					astarMapCell.IsToRight = false;
					rightAStarCell.IsToLeft = false;
					break;
				}
				break;
			}
		}
			
	}
}
