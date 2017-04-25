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
	[SerializeField]
	protected Animator m_aniCtr;

	public void PlayCountDownEffect(){
		if (!m_aniCtr.GetBool ("isPlay"))
			m_aniCtr.SetBool ("isPlay", true);
	}
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
