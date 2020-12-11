using UnityEngine;
using System.Collections;

public class AARPGGameplayFeedback : AARPanelGenerator {
	
	public enum FeedbackType {
		Engaged,
		Effort,
		NextEpisode
	}
	
	private const string PANEL_TITLE = "GAMEPLAY FEEDBACK";
	public string feedbackQuestion;
	public string leftLabel;
	public string rightLabel;
	public FeedbackType feedbackType;
	public Episode episode;
	public bool previousButtonAllowed;
	
	public override void ActivatePanel ()
	{
		aarMaster.ShowTitle(PANEL_TITLE);
		base.ActivatePanel ();
	}
	
	public override void SetupPanel() {
		AARPanel.PanelType panelType = AARPanel.PanelType.GameplayFeedback5;
		if(feedbackType == FeedbackType.Engaged)
			panelType = AARPanel.PanelType.GameplayFeedback5;
		
		if(feedbackType == FeedbackType.NextEpisode) {
			panel.SetupAARComponents(panelType, AARPanel.NextButtonType.Done);
		} else {
			panel.SetupAARComponents(panelType, AARPanel.NextButtonType.Next);
		}
	}
	
	public override void CustomizePanel() {
		
		if(previousButtonAllowed)
			panel.ShowPreviousButton();
		
		panel.DisableNextButton();
		panel.VanishNexButtonText();
		panel.header2.Text = feedbackQuestion;
	
		panel.horizontalRadio5LeftLabel.Text = leftLabel;
		panel.horizontalRadio5RightLabel.Text = rightLabel;
	}
	
	private void Update() 
	{
		if(!panel.nextButton.controlIsEnabled) 
		{
			if(	CheckToSeeIfAnyRadioIsSelected(panel.horizontalRadio5) ||
				CheckToSeeIfAnyRadioIsSelected(panel.horizontalRadio7))
			{
				panel.EnableNextButton();
			}
		}
	}
	
	public override void NextButtonPressed ()
	{
		SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
		int episodeIndex = GetEpisodeIndex();
		int score = 0;
		
		switch(feedbackType)
		{
		case FeedbackType.NextEpisode:
			score = Get5RadioScore();
			currentSubject.episodeData[episodeIndex].episodeFeedback.challenge = score;
			ReportEpisodeFeedback();
			break;
		case FeedbackType.Effort:
			score = Get5RadioScore();
			currentSubject.episodeData[episodeIndex].episodeFeedback.effort = score;
			break;
		case FeedbackType.Engaged:
			score = Get5RadioScore();
			currentSubject.episodeData[episodeIndex].episodeFeedback.nextEpisode = score;
			break;
		}
		
		base.NextButtonPressed();
	}
	
	public void FadeIn() {
		
	}
	
	public void FadeOut() {
			
	}
	
	private int Get5RadioScore() {
		for (int i = 0; i < panel.horizontalRadio5.Length; i++) {
			if(panel.horizontalRadio5[i].Value == true)
				return i;
		}		
		
		Debug.LogError("No Radios selected!");
		return 0;
	}
	
	private int Get7RadioScore() {
		for (int i = 0; i < panel.horizontalRadio7.Length; i++) {
			if(panel.horizontalRadio7[i].Value == true)
				return i;
		}
		
		Debug.LogError("No Radios selected!");
		return 0;
	}
	
	private int GetEpisodeIndex() 
	{
		Episode episode = SessionManager.GetSessionManager().vignetteManager.GetEpisode();
		
		switch(episode) 
		{
		case Episode.Episode1:
			return 0;
		case Episode.Episode2:
			return 1;
		case Episode.Episode3:
			return 2;
		}
		
		return 0;
	}
	
	private void ReportEpisodeFeedback()
	{
		SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
		
		Episode episode = SessionManager.GetSessionManager().vignetteManager.GetEpisode();
		int episodeIndex = GetEpisodeIndex();
		
		int effort = currentSubject.episodeData[episodeIndex].episodeFeedback.effort;
		int engagement = currentSubject.episodeData[episodeIndex].episodeFeedback.nextEpisode;
		int challenge = currentSubject.episodeData[episodeIndex].episodeFeedback.challenge;
		
		ReportEvent.EpisodeFeedback(episode, engagement, effort, challenge);
	}
}
