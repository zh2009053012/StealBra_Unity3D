using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapDisplay))]
public class MapEditor : Editor {

	private SerializedObject generator;
	private MapDisplay m_mapDisplay;

	private SerializedProperty row;
	private SerializedProperty column;
	private SerializedProperty cellSize;
	private SerializedProperty mapCellPrefab;
	private SerializedProperty mapFilePath;

	void OnEnable(){
		generator = new SerializedObject (target);
		m_mapDisplay = (MapDisplay)target;

		row = generator.FindProperty ("row");
		column = generator.FindProperty ("column");
		cellSize = generator.FindProperty ("cellSize");
		mapCellPrefab = generator.FindProperty ("mapCellPrefab");
		mapFilePath = generator.FindProperty ("mapFilePath");
	}
	public override void OnInspectorGUI(){
		generator.Update ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField (new GUIContent("Row"), GUILayout.Width(40) );
		EditorGUILayout.PropertyField (row, GUIContent.none, GUILayout.Width (80));
		EditorGUILayout.LabelField (new GUIContent("Column"), GUILayout.Width(50) );
		EditorGUILayout.PropertyField (column, GUIContent.none, GUILayout.Width (80));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField (new GUIContent("CellSize"), GUILayout.Width(80) );
		EditorGUILayout.PropertyField (cellSize, GUIContent.none, GUILayout.Width (160));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField (new GUIContent("MapCellPrefab"), GUILayout.Width(100));
		EditorGUILayout.PropertyField (mapCellPrefab, GUIContent.none, GUILayout.Width (200));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField (new GUIContent("file path"), GUILayout.Width(100));
		EditorGUILayout.LabelField (new GUIContent(mapFilePath.stringValue), GUILayout.Width(200));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button (new GUIContent ("Clear MapCell"), GUILayout.Width (100))) {
			m_mapDisplay.ClearMapCell ();
		}
		if (GUILayout.Button (new GUIContent ("Create Map"), GUILayout.Width (100))) {
			m_mapDisplay.CreateMap (row.intValue, column.intValue, cellSize.vector2Value);
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button (new GUIContent ("Read Map"), GUILayout.Width (100))) {
			m_mapDisplay.ReadMap();
		}
		if (GUILayout.Button (new GUIContent ("Save Map"), GUILayout.Width (100))) {
			m_mapDisplay.SaveMap();
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button (new GUIContent ("ToAStarMap"), GUILayout.Width (100))) {
			m_mapDisplay.AutoAStarMap ();
		}
		EditorGUILayout.EndHorizontal ();

		generator.ApplyModifiedProperties ();
	}
}
