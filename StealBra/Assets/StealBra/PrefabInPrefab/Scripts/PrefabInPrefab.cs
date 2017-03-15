using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PrefabInPrefab : MonoBehaviour
{
	public GameObject Child
	{
		get
		{
			if(Application.isPlaying && generatedObject == null && prefab != null)
			{
				/*
				You can use Script Execution Order Settings.
				http://docs.unity3d.com/Documentation/Components/class-ScriptExecution.html
				*/
				Debug.LogError("Prefab In Prefab is Uninitialized. You can use this after Awake().");
			}
			return generatedObject;
		}
	}
	public T GetComponentFromChild<T>() where T : Component { return Child.GetComponent<T>(); }

	[SerializeField] GameObject prefab;
	[SerializeField] bool moveComponents = true;
	private GameObject generatedObject;

	void Awake()
	{
#if UNITY_EDITOR
		if(!Application.isPlaying)
		{
			StartInEditMode();
			return;
		}
#endif
		if(prefab == null) return;
		InstantiatePrefab();
	}

	GameObject InstantiatePrefab()
	{
		generatedObject = Instantiate(prefab) as GameObject;

		// calc transform to base parent
		generatedObject.transform.parent = this.transform.parent.transform;
		generatedObject.transform.position = this.transform.position;
		generatedObject.transform.rotation = this.transform.rotation;
		generatedObject.transform.localScale = this.transform.localScale;

		// change parent
		generatedObject.transform.parent = this.transform;

		if(moveComponents) MoveComponents(this.gameObject, generatedObject);
		return generatedObject;
	}

	void MoveComponents(GameObject from, GameObject to)
	{
		var components = from.GetComponents(typeof(Component));
		foreach(var component in components)
		{
			if(component as Transform != null || component as PrefabInPrefab != null) continue;

			Type type = component.GetType();
			var copy = to.AddComponent(type);
			var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			foreach (var field in fields) field.SetValue(copy, field.GetValue(component));
			if(Application.isPlaying) Destroy(component);
		}
	}

#if UNITY_EDITOR
	// ==============
	//  in edit mode
	// ==============

	private string lastPrefabUpdateTime;
	[SerializeField] bool previewInEditor = true;

	private bool visibleVirtualPrefab
	{
		get { return previewInEditor && this.gameObject.activeInHierarchy && this.enabled; }
	}

	void StartInEditMode() { DrawDontEditablePrefab(); }
	void OnDisable() { DrawDontEditablePrefab(); }
	void OnEnable() { DrawDontEditablePrefab(); }
	void ParamChanged() { ForceDrawDontEditablePrefab(); }

	public void ForceDrawDontEditablePrefab()
	{
		lastPrefabUpdateTime = "";
		DrawDontEditablePrefab();
	}

	public void DrawDontEditablePrefab()
	{
		if(Application.isPlaying) return;
		if(prefab == null || !visibleVirtualPrefab)
		{
			if(Child != null)
			{
				// param changed
				DeleteChildren();
				UpdateGameView();
			}
			return;
		}
		if(!PrefabUpdated()) return;
		if(ValidationError()) return;

		DeleteChildren();

		var generatedObject = InstantiatePrefab();
		// 自分の1つ上のGameObjectが所属しているPrefabのRootの親の下.
		var foundRoot = PrefabUtility.FindPrefabRoot(transform.parent.gameObject).transform.parent;
		if(foundRoot == null)
		{
			// 親オブジェクトは、ドラッグアンドドロップした瞬間は見つからない
			EditorApplication.delayCall += () =>
			{
				if(generatedObject == null) return;
				var parent = PrefabUtility.FindPrefabRoot(transform.parent.gameObject).transform.parent;
				generatedObject.transform.parent = parent == null ? null : parent.transform;
			};
		}
		else
		{
			generatedObject.transform.parent = foundRoot.transform;
		}
		generatedObject.name = string.Format(">PrefabInPrefab{0}", GetInstanceID());
		generatedObject.tag = "EditorOnly";
		foreach(var childTransform in generatedObject.GetComponentsInChildren<Transform>())
		{
			childTransform.gameObject.hideFlags = HideFlags.HideAndDontSave;
		}

		var child = generatedObject.AddComponent<PrefabInPrefabAsset.VirtualPrefab>();
		child.stepparent = this.gameObject;
		child.original = this;
		child.UpdateTransform();

		UpdateGameView();
	}

	bool PrefabUpdated()
	{
		var prefabUpdateTime = GetPrefabUpdateTime();
		if(lastPrefabUpdateTime == prefabUpdateTime && Child != null) return false;
		lastPrefabUpdateTime = prefabUpdateTime;
		return true;
	}

	void DeleteChildren()
	{
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("EditorOnly"))
		{
			if(obj.name != string.Format(">PrefabInPrefab{0}", GetInstanceID())) continue;
			DestroyImmediate(obj);
		}
	}

	public string GetPrefabFilePath()
	{
		return AssetDatabase.GetAssetPath(prefab);
	}

	string GetPrefabUpdateTime()
	{
		string result;
		using(System.IO.FileStream fs = new System.IO.FileStream(GetPrefabFilePath(), System.IO.FileMode.Open, System.IO.FileAccess.Read))
		{
			using(System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create())
			{
				byte[] bs = sha1.ComputeHash(fs);
				result = BitConverter.ToString(bs).ToLower().Replace("-","");
			}
		}
		
		return result;
	}

	void UpdateGameView()
	{
		if(Application.isPlaying) return;
		UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
		SceneView.RepaintAll();

		// force redraw anything(ex. NGUI's UICamera)
		var dummy = new GameObject();
		dummy.transform.parent = null;
		DestroyImmediate(dummy);
	}

	bool ValidationError()
	{
		// check circular reference
		if(CheckCircularReference(this, null))
		{
			Debug.LogError("Can't circular reference.");
			Reset();
			return true;
		}

		// This game object can't be root.
		// Because this is not in prefab.
		if(this.transform.parent == null)
		{
			// copy&paseした時に、なぜか一瞬だけparentがnullになるので、
			// 少し遅らせる.
			EditorApplication.delayCall += () =>
			{
				if(this.transform.parent == null)
				{
					Debug.LogError("Can't attach PrefabInPrefab to root gameobject.");
					Reset();
				}
				else
				{
					ForceDrawDontEditablePrefab();
				}
			};

			// stop
			return true;
		}

		return false;
	}

	void Reset()
	{
		prefab = null;
		DeleteChildren();
	}

	bool CheckCircularReference(PrefabInPrefab target, List<int> usedPrefabs)
	{
		if(target.prefab == null) return false;
		if(usedPrefabs == null) usedPrefabs = new List<int>();

		int id = target.prefab.GetInstanceID();
		if(usedPrefabs.Contains(id)) return true;
		usedPrefabs.Add(id);

		foreach(var nextTarget in ((GameObject)target.prefab).GetComponentsInChildren<PrefabInPrefab>(true))
		{
			if(nextTarget == this) continue;
			if(CheckCircularReference(nextTarget, new List<int>(usedPrefabs))) return true;
		}

		return false;
	}
#endif
}
