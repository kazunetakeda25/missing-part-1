using UnityEngine;
using System.Collections;

public class AARPGBlindspotFeedback : AARPanelGenerator {
	
	public const string TITLE = "Feedback on [#FF0000]Bias Blind Spot[#FFFFFF] - ";
	public const string SUBTITLE_CONF_BIAS = "Confirmation Bias";
	public const string SUBTITLE_FAE = "Fundamental Attribution Error";
	
	public BiasType biasType;
	public Episode episode;
	
	public override void ActivatePanel ()
	{
		aarMaster.ShowTitle(TITLE + GetSubtitle());
		
		//Unlike every other panel, this needs to come in at Activation, since we don't have the Self Review answers yet!
		panel.subText1.Text = "";
		panel.header2.Text = GenerateDynamicFeedbackText();
		panel.subText2.Text = "";
		
		//FAE is second so send the Event then.
		if(biasType == BiasType.FundamentalAttributionError)
			ReportSimpleEpisodeScore();
		
		base.ActivatePanel ();
	}
	
	public override void SetupPanel() {
		panel.SetupAARComponents(AARPanel.PanelType.BiasBlindspot, AARPanel.NextButtonType.Next);
	}
	
	public override void CustomizePanel ()
	{

	}
	
	private string GetSubtitle() {
		switch(biasType) {
		case BiasType.ConfirmationBias:
			return SUBTITLE_CONF_BIAS;
		case BiasType.FundamentalAttributionError:
			return SUBTITLE_FAE;
		}
		
		return "Error Generating Title";
	}
	
	private string GenerateDynamicFeedbackText() {
		
		BiasBlindSpotFeedback blindspotFeedback = new BiasBlindSpotFeedback();
		
		return blindspotFeedback.GetPerformanceText(episode, biasType);
	}
	
	public override void NextButtonPressed ()
	{
		base.NextButtonPressed ();
	}
	
	private void ReportSimpleEpisodeScore() {
		SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
		
		switch(episode)
		{
		case Episode.Episode1:
			currentSubject.ReportSimpleEpisode1Score();
			break;
		case Episode.Episode2:
			currentSubject.ReportSimpleEpisode2Score();
			break;
		case Episode.Episode3:
			currentSubject.ReportSimpleEpisode3Score();
			break;
		}
	}
	
}
