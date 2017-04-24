using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FightUI : MonoBehaviour {
	[SerializeField]
	protected Text m_underpantNumText;
	[SerializeField]
	protected Text m_braNumText;
	[SerializeField]
	protected Text m_countDownText;
	[SerializeField]
	protected Slider m_countDownSlider;

	public void SetUnderpantNum(int underpant){
		m_underpantNumText.text = underpant.ToString();
	}
	public void SetBraNum(int braNum){
		m_braNumText.text  = braNum.ToString();
	}
	public void SetCountDown(int cur, int total){
		cur = cur > total ? total : cur;
		m_countDownText.text = cur.ToString ();
		m_countDownSlider.value = (float)cur / (float)total;
	}

	public void OnBackBtnClick(){
		GameStateManager.Instance().FSM.CurrentState.Message("BackBtnClick", null);
		AudioManager.Instance.PlayAudio("hit");
	}
}
