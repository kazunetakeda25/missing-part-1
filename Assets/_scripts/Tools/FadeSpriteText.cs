using UnityEngine;
using System.Collections;

public class FadeSpriteText : MonoBehaviour {
	
	private const string FADE_SPRITETEXT_NAME = "SpriteText Fader";
	
	public delegate void ActionDelegate();
	
	private float m_duration = 0.0f;
	private SpriteText m_text;
	private float m_targetAlpha;
	private float m_multiplier = 0;
	private ActionDelegate m_Action;
	
	public static void FadeText(SpriteText text, float targetAlpha, float duration) {
		FadeText(text, targetAlpha, duration, ActionEater);
	}
	
	public static void FadeText(SpriteText text, float targetAlpha, float duration, ActionDelegate act)
	{
		GameObject faderGO = new GameObject(FADE_SPRITETEXT_NAME);
		FadeSpriteText newFader =  faderGO.AddComponent<FadeSpriteText>();
		newFader.Fade(text, targetAlpha, duration, act);
	}
	
	public void Fade(SpriteText text, float targetAlpha, float duration, ActionDelegate act) {
		m_Action = act;
		m_duration = duration;
		m_text = text;
		m_targetAlpha = targetAlpha;
		
		DetermineMultiplier();
	}
	
	private void DetermineMultiplier() {
		if(m_text.Color.a > m_targetAlpha) 
			m_multiplier = -1;
		else
			m_multiplier = 1;
	}
	
	public void Update ()
	{
		ApplyNewAlpha(CalculateNewAlpha());
		CheckFadeComplete();
	}
	
	private float CalculateNewAlpha() {
		float newAlpha = m_text.Color.a;
		float stepAmount =  Time.deltaTime / m_duration;
		newAlpha += stepAmount * m_multiplier;
		newAlpha = Mathf.Clamp(newAlpha, 0, 1);
		return newAlpha;
	}
	
	private void ApplyNewAlpha(float newAlpha) {
		m_text.SetColor(ColorTools.SetColorAlpha(m_text.Color, newAlpha));
	}
	
	private void CheckFadeComplete() {
		if(m_multiplier == 1 && m_text.Color.a >= m_targetAlpha) {
			FadeComplete();
		}
		
		if(m_multiplier == -1 && m_text.Color.a <= m_targetAlpha) {
			FadeComplete();
		}
	}
	
	private void FadeComplete() {
		m_Action();
		Destroy(this.gameObject);
	}
	
	private static void ActionEater() {
		
	}	
	
}
