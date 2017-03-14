using UnityEngine;
using System.Collections;

[SerializeField]
public class MapCell {

	public bool IsToLeft=true;
	public bool IsToRight=true;
	public bool IsToUp=true;
	public bool IsToDown = true;
	public bool IsToLeftUp=true;
	public bool IsToLeftDown = true;
	public bool IsToRightUp = true;
	public bool IsToRightDown = true;
	public MapCell(){}
}
