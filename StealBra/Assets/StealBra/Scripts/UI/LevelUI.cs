using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour {
	[SerializeField]
	protected GameObject m_clearLevel;
	[SerializeField]
	protected GameObject m_lockLevel;
	[SerializeField]
	protected Text m_levelText;
	[SerializeField]
	protected List<GameObject> m_starList = new List<GameObject>();

	protected LevelData m_data;
	public LevelData Data{
		get{return m_data;}
	}
	public void Init(LevelData data){
		m_data = data;
		m_levelText.text = m_data.id.ToString();
		if(m_data.isLock){
			m_lockLevel.SetActive(true);
			m_clearLevel.SetActive(false);
		}else{
			m_lockLevel.SetActive(false);
			m_clearLevel.SetActive(true);
			for(int i=0; i<m_starList.Count; i++){
				if(m_data.starNum >= i+1){
					m_starList[i].SetActive(true);
				}else{
					m_starList[i].SetActive(false);
				}
			}
		}
	}
	public void OnClick(){
		Debug.Log("on clik");
		object[] p = new object[1];
		p[0] = (object)m_data;
		GameStateManager.Instance().FSM.CurrentState.Message("SelectLevel", p);
	}
}
