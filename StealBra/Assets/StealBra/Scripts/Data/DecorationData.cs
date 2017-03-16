using UnityEngine;
using System.Collections;

public class DecorationData {
	public string resName;
	public string sortLayer;
	public int sortOrder;
	public double posX;
	public double posY;
	public double posZ;
	public double eulerX;
	public double eulerY;
	public double eulerZ;
	public double scaleX;
	public double scaleY;
	public double scaleZ;
	public DecorationData(){
		resName = "";
		sortLayer = "";
		sortOrder = 0;
		posX = 0;
		posY = 0;
		posZ = 0;
		eulerX = 0;
		eulerY = 0;
		eulerZ = 0;
		scaleX = 1;
		scaleY = 1;
		scaleZ = 1;
	}
}
