using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DotImageUI : MonoBehaviour {
	[SerializeField]
	protected Sprite m_dotOnSprite;
	[SerializeField]
	protected Sprite m_dotOffSprite;

	public void ShowDotOn(bool isOn){
		if(isOn){
			GetComponent<Image>().sprite = m_dotOnSprite;
		}else{
			GetComponent<Image>().sprite = m_dotOffSprite;
		}
	}
}
