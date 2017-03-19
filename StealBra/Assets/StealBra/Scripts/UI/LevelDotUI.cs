using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LevelDotUI : MonoBehaviour {
	[SerializeField]
	protected GameObject m_dotPrefab;

	protected List<DotImageUI> m_dotList = new List<DotImageUI>();
	public void Init(int num){
		if(num <= 0)
			return;
		Clear();
		for(int i=0; i<num; i++){
			GameObject go = GameObject.Instantiate(m_dotPrefab);
			go.transform.parent = transform;
			go.transform.localScale = Vector3.one;
			DotImageUI ctr = go.GetComponent<DotImageUI>();
			m_dotList.Add(ctr);
		}
		ShowAllDotOff();
	}
	void Clear(){
		for(int i=0; i<m_dotList.Count; i++){
			GameObject.Destroy(m_dotList[i].gameObject);
		}
		m_dotList.Clear();
	}
	public void SetCurPage(int curIndex){
		if(curIndex < 0 || curIndex >= m_dotList.Count)
			return;
		ShowAllDotOff();
		m_dotList[curIndex].ShowDotOn(true);
	}
	void ShowAllDotOff(){
		for(int i=0; i<m_dotList.Count; i++){
			m_dotList[i].ShowDotOn(false);
		}
	}
}
