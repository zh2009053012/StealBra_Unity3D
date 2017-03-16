using UnityEngine;
using System.Collections;
using UnityEngine.Events;
public class AnimationEvent : MonoBehaviour {
	public UnityEvent m_attackOverEvent;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void AttackOver(){
		if (m_attackOverEvent != null) {
			m_attackOverEvent.Invoke ();
		}
	}
}
