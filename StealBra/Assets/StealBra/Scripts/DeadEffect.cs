using UnityEngine;
using System.Collections;

public class DeadEffect : MonoBehaviour {

	private float DeadTime=1;
	public string ParamName = "_Cutoff";
	private Renderer[] m_renders;
	private float m_startTime;
	private bool m_isPlaying = false;

	private System.Action<GameObject> m_deadAniOverEvent;
	public void Play(float deadTime, System.Action<GameObject> deadOverCallback){
		m_renders = GetComponentsInChildren<Renderer>();
		m_startTime = Time.time;
		m_deadAniOverEvent -= deadOverCallback;
		m_deadAniOverEvent += deadOverCallback;
		DeadTime = deadTime;
		m_isPlaying = true;
	}
	public void Stop(){
		m_isPlaying = false;
		for(int i = 0; i<m_renders.Length; i++)
		{
			for(int j = 0; j < m_renders[i].materials.Length; j++)
			{
				m_renders[i].materials[j].SetFloat (ParamName, 0);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(m_renders == null)
		{
			return;
		}
			
		if(m_isPlaying)
		{
			float t = (Time.time - m_startTime)/DeadTime;
			for(int i = 0; i<m_renders.Length; i++)
			{
				for(int j = 0; j < m_renders[i].materials.Length; j++)
				{
					m_renders[i].materials[j].SetFloat (ParamName, t);
				}
			}
			if (t >= 1) {
				m_isPlaying = false;
				if (null != m_deadAniOverEvent) {
					m_deadAniOverEvent.Invoke (gameObject);
				} 
			}
		}
	}
}
