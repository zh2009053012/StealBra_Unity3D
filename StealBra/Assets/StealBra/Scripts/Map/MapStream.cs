using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using LitJson;

public class MapStream {

	public static void Write(MapData map, string path){
		FileStream fs = new FileStream (path, FileMode.Create);
		string jsonStr = JsonMapper.ToJson (map);
		byte[] data = Encoding.ASCII.GetBytes (jsonStr);
		fs.Write (data, 0, data.Length);
		fs.Flush ();
		fs.Close ();
	}
	public static MapData Read(string path){
		FileStream fs = new FileStream (path, FileMode.Open);

		byte[] data = new byte[fs.Length];
		fs.Seek (0, SeekOrigin.Begin);
		fs.Read (data, 0, (int)fs.Length);
		string jsonStr = Encoding.ASCII.GetString (data);

		MapData map = JsonMapper.ToObject<MapData> (jsonStr);
		fs.Close ();

		return map;
	}
}
