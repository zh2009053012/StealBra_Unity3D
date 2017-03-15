using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapDisplay))]
public class MapEditor : Editor {
	private SerializedObject generator;
	private MapDisplay m_mapDisplay;

	private SerializedProperty m_row;
	private SerializedProperty m_column;

	void OnEnable(){
		generator = new SerializedObject (target);
		m_mapDisplay = (MapDisplay)target;

		m_row = generator.FindProperty ("m_row");
		m_column = generator.FindProperty ("m_column");
	}
	public override void OnInspectorGUI(){
		generator.Update ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField (new GUIContent("Row"), GUILayout.Width(30));
		EditorGUILayout.PropertyField (m_row, GUIContent.none, GUILayout.Width (80));
		EditorGUILayout.LabelField (new GUIContent("Column"), GUILayout.Width(50));
		EditorGUILayout.PropertyField (m_column, GUIContent.none, GUILayout.Width (80));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button (new GUIContent ("Clear Map"), GUILayout.Width (100))) {
			m_mapDisplay.ClearMap ();
		}
		if (GUILayout.Button (new GUIContent ("Create Map"), GUILayout.Width (100))) {
			m_mapDisplay.CreateMap ();
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField (new GUIContent("FilePath:"), GUILayout.Width(55));
		EditorGUILayout.LabelField (new GUIContent(m_mapDisplay.m_filePath), GUILayout.MinWidth(80));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button (new GUIContent ("Read Map"), GUILayout.Width (100))) {
			m_mapDisplay.ReadMap ();
		}
		if (GUILayout.Button (new GUIContent ("Save Map"), GUILayout.Width (100))) {
			m_mapDisplay.SaveMap ();
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button (new GUIContent ("Wall Around"), GUILayout.Width (100))) {
			m_mapDisplay.WallAround();
		}
		EditorGUILayout.EndHorizontal ();

		generator.ApplyModifiedProperties ();
	}
}
