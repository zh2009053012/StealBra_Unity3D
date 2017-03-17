using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AStarMapCellDisplay))]
public class AStarMapCellEditor : Editor {

	private SerializedObject generator;
	private AStarMapCellDisplay m_mapDisplay;

	private SerializedProperty IsToLeft;
	private SerializedProperty IsToRight;
	private SerializedProperty IsToUp;
	private SerializedProperty IsToDown;
	private SerializedProperty IsToLeftUp;
	private SerializedProperty IsToLeftDown;
	private SerializedProperty IsToRightUp;
	private SerializedProperty IsToRightDown;

	private SerializedProperty IsAllOpen;
	private SerializedProperty IsAllClose;
	private SerializedProperty IsObstacle;

	void OnEnable(){
		generator = new SerializedObject (target);
		m_mapDisplay = (AStarMapCellDisplay)target;
		m_mapDisplay.Init ();

		IsToLeft = generator.FindProperty ("IsToLeft");
		IsToRight = generator.FindProperty ("IsToRight");
		IsToUp = generator.FindProperty ("IsToUp");
		IsToDown = generator.FindProperty ("IsToDown");
		IsToLeftUp = generator.FindProperty ("IsToLeftUp");
		IsToLeftDown = generator.FindProperty ("IsToLeftDown");
		IsToRightUp = generator.FindProperty ("IsToRightUp");
		IsToRightDown = generator.FindProperty ("IsToRightDown");

		IsAllOpen = generator.FindProperty ("IsAllOpen");
		IsAllClose = generator.FindProperty ("IsAllClose");
		IsObstacle = generator.FindProperty("IsObstacle");
	}
	public override void OnInspectorGUI(){
		generator.Update ();
		if(Selection.activeGameObject != m_mapDisplay.gameObject)
			return;
		//

		EditorGUILayout.BeginHorizontal();
		IsToLeftUp.boolValue = EditorGUILayout.ToggleLeft (new GUIContent("LeftUp"), IsToLeftUp.boolValue, GUILayout.Width(80));
		IsToUp.boolValue = EditorGUILayout.ToggleLeft (new GUIContent("Up"), IsToUp.boolValue, GUILayout.Width(80));
		IsToRightUp.boolValue = EditorGUILayout.ToggleLeft (new GUIContent("RightUp"), IsToRightUp.boolValue, GUILayout.Width(80));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		IsToLeft.boolValue = EditorGUILayout.ToggleLeft (new GUIContent("Left"), IsToLeft.boolValue, GUILayout.Width(164));
		IsToRight.boolValue = EditorGUILayout.ToggleLeft (new GUIContent("Right"), IsToRight.boolValue, GUILayout.Width(80));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal();
		IsToLeftDown.boolValue = EditorGUILayout.ToggleLeft (new GUIContent("LeftDown"), IsToLeftDown.boolValue, GUILayout.Width(80));
		IsToDown.boolValue = EditorGUILayout.ToggleLeft (new GUIContent("Down"), IsToDown.boolValue, GUILayout.Width(80));
		IsToRightDown.boolValue = EditorGUILayout.ToggleLeft (new GUIContent("RightDown"), IsToRightDown.boolValue, GUILayout.Width(80));
		EditorGUILayout.EndHorizontal ();

		if (IsToLeft.boolValue || IsToLeftUp.boolValue || IsToLeftDown.boolValue || IsToUp.boolValue || 
			IsToDown.boolValue || IsToRight.boolValue || IsToRightDown.boolValue || IsToRightUp.boolValue) {
			IsAllClose.boolValue = false;
		}
		if (!IsToLeft.boolValue || !IsToLeftUp.boolValue || !IsToLeftDown.boolValue || !IsToUp.boolValue ||
		   !IsToDown.boolValue || !IsToRight.boolValue || !IsToRightDown.boolValue || !IsToRightUp.boolValue) {
			IsAllOpen.boolValue = false;
		}

		EditorGUILayout.LabelField (new GUIContent("GLOBAL"), GUILayout.Width(80));
		EditorGUILayout.BeginHorizontal();
		bool isAllOpen = EditorGUILayout.ToggleLeft (new GUIContent("AllOpen"), IsAllOpen.boolValue, GUILayout.Width(80));
		bool isAllClose = EditorGUILayout.ToggleLeft (new GUIContent("AllClose"), IsAllClose.boolValue, GUILayout.Width(80));
		IsObstacle.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Obstacle"), IsObstacle.boolValue, GUILayout.Width(80));

		if (isAllOpen && !IsAllOpen.boolValue) {
			IsAllOpen.boolValue = isAllOpen;
			IsAllClose.boolValue = false;
			IsToLeftUp.boolValue = true;
			IsToUp.boolValue = true;
			IsToRightUp.boolValue = true;
			IsToLeft.boolValue = true;
			IsToRight.boolValue = true;
			IsToLeftDown.boolValue = true;
			IsToDown.boolValue = true;
			IsToRightDown.boolValue = true;
		}else if (isAllClose && !IsAllClose.boolValue) {
			IsAllClose.boolValue = isAllClose;
			IsAllOpen.boolValue = false;
			IsToLeftUp.boolValue = false;
			IsToUp.boolValue = false;
			IsToRightUp.boolValue = false;
			IsToLeft.boolValue = false;
			IsToRight.boolValue = false;
			IsToLeftDown.boolValue = false;
			IsToDown.boolValue = false;
			IsToRightDown.boolValue = false;
		}
		EditorGUILayout.EndHorizontal ();

		generator.ApplyModifiedProperties ();

		if(IsObstacle.boolValue){
			m_mapDisplay.Owner.SetObstacle(m_mapDisplay.Row, m_mapDisplay.Column);
		}
		m_mapDisplay.ShowLeft ();
		m_mapDisplay.ShowUp ();
		m_mapDisplay.ShowRightUp ();
		m_mapDisplay.ShowLeftUp ();
		m_mapDisplay.ShowRight ();
		m_mapDisplay.ShowLeftDown ();
		m_mapDisplay.ShowDown ();
		m_mapDisplay.ShowRightDown ();
	}
}
