using UnityEngine;
using System.Collections;

public class AARPGBigSplash : AARPanelGenerator {
	
	public string textToDisplay;
	public float timeToDisplay;
	public float fadeDuration;
	public float endDelay;
	private bool fadeText = false;
	
	public override void ActivatePanel ()
	{
		aarMaster.HideTitle();
		FadeSpriteText.FadeText(panel.episodeComplete, 1, fadeDuration, FadeInComplete);
		base.ActivatePanel ();
	}
	
	public override void SetupPanel() {
		panel.SetupAARComponents(AARPanel.PanelType.Intro, AARPanel.NextButtonType.None);
	}
	
	public override void CustomizePanel ()
	{
		panel.episodeComplete.Text = textToDisplay;
	}

	public void FadeInComplete() {
		StartCoroutine(DisplayText());
	}
	
	private IEnumerator DisplayText() {
		yield return new WaitForSeconds(timeToDisplay);
		FadeSpriteText.FadeText(panel.episodeComplete, 0, fadeDuration, FadeOutComplete);
	}
	
	public void FadeOutComplete() {
		StartCoroutine(NextButtonDelay());
	}
	
	private IEnumerator NextButtonDelay() {
		yield return new WaitForSeconds(endDelay);
		NextButtonPressed();
	}
	
}
