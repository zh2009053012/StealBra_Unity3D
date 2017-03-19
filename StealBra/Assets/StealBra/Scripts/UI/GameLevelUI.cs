using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLevelUI : MonoBehaviour {
	[SerializeField]
	protected LevelDotUI m_dotCtr;
	[SerializeField]
	protected Transform m_contents;
	[SerializeField]
	protected GameObject m_levelItemPrefab;
	[SerializeField]
	protected DotImageUI m_leftBtnCtr;
	[SerializeField]
	protected DotImageUI m_rightBtnCtr;
	protected int pageNum=0;
	protected int curPage = 0;
	protected List<LevelUI> m_list = new List<LevelUI>();

	public void Init(){
		GameData.ReadLevelData();
		pageNum = Mathf.FloorToInt( GameData.LevelDataList.Count/10.0f);
		int ingress=-1;
		foreach(LevelData item in GameData.LevelDataList){
			if(!item.isLock){
				ingress++;
			}else{
				break;
			}
		}
		curPage = Mathf.FloorToInt(ingress/10.0f);
		m_dotCtr.Init(pageNum);

		ShowPage(curPage);
		//
		ShowLeftRightBtn();
	}
	bool ShowPage(int page){
		if(page < 0 || page >= pageNum)
			return false;
		curPage = page;
		m_dotCtr.SetCurPage(curPage);

		for(int i=curPage*10; i<(curPage+1)*10; i++){
			if(i<GameData.LevelDataList.Count){
				if(i-curPage*10 < m_list.Count){
					m_list[i-curPage*10].Init(GameData.LevelDataList[i]);
				}else{
					GameObject go = GameObject.Instantiate(m_levelItemPrefab);
					go.transform.parent = m_contents;
					go.transform.localScale = Vector3.one;
					LevelUI uiCtr = go.GetComponent<LevelUI>();
					uiCtr.Init(GameData.LevelDataList[i]);
					m_list.Add(uiCtr);
				}
			}else if(i-curPage*10 < m_list.Count){
				GameObject.Destroy(m_list[i-curPage*10].gameObject);
				m_list[i-curPage*10] = null;
			}
		}
		//
		for(int i=m_list.Count-1; i>=0; i--)
		{
			if(m_list[i] == null)
				m_list.RemoveAt(i);
		}
		return true;
	}
	void ShowLeftRightBtn(){
		if(curPage == 0){
			m_leftBtnCtr.ShowDotOn(false);
		}else{
			m_leftBtnCtr.ShowDotOn(true);
		}
		if(curPage == pageNum-1){
			m_rightBtnCtr.ShowDotOn(false);
		}else{
			m_rightBtnCtr.ShowDotOn(true);
		}
	}

	public void OnLeftBtnClick(){
		
		ShowPage(curPage-1);
		ShowLeftRightBtn();
		AudioManager.Instance.PlayAudio("hit");
	}
	public void OnRightBtnClick(){
		
		ShowPage(curPage+1);
		ShowLeftRightBtn();
		AudioManager.Instance.PlayAudio("hit");
	}
	public void OnBackBtnClick(){
		AudioManager.Instance.PlayAudio("hit");
		GameStateManager.Instance().FSM.CurrentState.Message("Back", null);
	}

}
