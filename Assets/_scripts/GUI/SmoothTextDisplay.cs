using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteText))]
public class SmoothTextDisplay : MonoBehaviour {

	private SpriteText m_text;
	public float fadeDuration;
	
	private string m_textToSet;
	
	private void Awake() {
		m_text = this.gameObject.GetComponent<SpriteText>();
		m_text.SetColor(ColorTools.SetColorAlpha(m_text.Color, 0));
	}
	
	public SpriteText GetSpriteText() {
		return m_text;
	}
	
	public void SetText(string textToSet) {
		m_textToSet = textToSet;
		if(m_text.Color.a == 1) {
			SmoothFadeOut();
		} else {
			SetTextAndFadeIn();
		}
	}
	
	private void SetTextAndFadeIn() {
		m_text.Text = m_textToSet;
		FadeSpriteText.FadeText(m_text, 1, fadeDuration);
	}
	
	private void SmoothFadeOut() {
		FadeSpriteText.FadeText(m_text, 0, fadeDuration, FadeOutComplete);
	}
	
	public void FadeOutComplete() {
		SetTextAndFadeIn();
	}
	
}