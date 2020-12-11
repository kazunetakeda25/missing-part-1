using UnityEngine;
using System.Collections;

public class AARPGEpisodeComplete : AARPanelGenerator {
	
	public const string EPISODE_COMPLETE_TEXT = "[#FF0000]{0} [#FFFFFF]COMPLETE...";
	
	public Episode episode;
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
		panel.episodeComplete.Text = string.Format(EPISODE_COMPLETE_TEXT, GetEpisodeString());
	}
	
	private string GetEpisodeString() {
		switch(episode) {
		case Episode.Episode1:
			return "EPISODE 1";
		case Episode.Episode2:
			return "EPISODE 2";
		case Episode.Episode3:
			return "EPISODE 3";
		}
		
		return "Episode Missing";
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
