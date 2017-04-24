using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;

public class LevelData{
	public int id;
	public int starNum;
	public string levelFileName;
	public bool isLock;
	public int countDown;
}

public class GameData {
	public static LevelData CurLevel;
	public static List<LevelData> LevelDataList = new List<LevelData>();
	public static void ClearLevel(int levelID, int starNum){
		if(levelID > 0 && levelID <= LevelDataList.Count){
			LevelDataList[levelID-1].starNum = starNum;
			if(levelID+1 <= LevelDataList.Count){
				//now there is only 5 levels
				if (levelID + 1 <= 5) {
					LevelDataList [levelID].isLock = false;
				}
			}
			WriteLevelData();
		}
	}
	public static void ResetLevelData(){
		LevelDataList.Clear ();
		for (int i = 0; i < 50; i++) {
			LevelData level = new LevelData ();
			level.countDown = 500 - 5 * i;
			level.id = i + 1;
			level.isLock = true;
			if (i == 0) {
				level.isLock = false;
			}
			level.starNum = 0;
			level.levelFileName = "scene" + level.id;
			LevelDataList.Add (level);
		}
		WriteLevelData ();
	}

	public static void ReadLevelData(){
		LevelDataList.Clear();

		string path = Application.dataPath+"/level.txt";
		FileStream fs = new FileStream (path, FileMode.Open);

		byte[] data = new byte[fs.Length];
		fs.Seek (0, SeekOrigin.Begin);
		fs.Read (data, 0, (int)fs.Length);
		string jsonStr = Encoding.ASCII.GetString (data);

		LevelDataList = JsonMapper.ToObject<List<LevelData>> (jsonStr);
		fs.Close ();
	}
	public static void WriteLevelData(){
		//
		string path = Application.dataPath+"/level.txt";
		FileStream fs = new FileStream (path, FileMode.Create);
		string jsonStr = JsonMapper.ToJson (LevelDataList);
		byte[] data = Encoding.ASCII.GetBytes (jsonStr);
		fs.Write (data, 0, data.Length);
		fs.Flush ();
		fs.Close ();
	}
}
