using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FightUI : MonoBehaviour {
	[SerializeField]
	protected Text m_underpantNumText;
	[SerializeField]
	protected Text m_braNumText;

	public void SetUnderpantNum(int underpant){
		m_underpantNumText.text = underpant.ToString();
	}
	public void SetBraNum(int braNum){
		m_braNumText.text  = braNum.ToString();
	}

	public void OnBackBtnClick(){
		GameStateManager.Instance().FSM.CurrentState.Message("BackBtnClick", null);
		AudioManager.Instance.PlayAudio("hit");
	}
}
