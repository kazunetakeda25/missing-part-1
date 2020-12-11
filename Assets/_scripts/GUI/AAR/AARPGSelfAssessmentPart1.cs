using UnityEngine;
using System.Collections;

public class AARPGSelfAssessmentPart1 : AARPanelGenerator {
	
	public const string TITLE_CONF_BIAS = "[#FF0000]Confirmation Bias";
	public const string TITLE_FAE = "[#FF0000]Fundamental Attribution Error";
	public const string TITLE_SUFFIX = " Self-Assessment[#555555]";
	
	public const string TXT_DESCRIPTION = "Please select the answer that most closely matches how you view your performance based on the choices given.";
	public const string TXT_QUESTION_TITLE = "QUESTION 1 OF 2:";
	
	public const string TXT_RADIO1 = "Not at all";
	public const string TXT_RADIO2 = "Probably not";
	public const string TXT_RADIO3 = "Possibly";
	public const string TXT_RADIO4 = "Probably";
	public const string TXT_RADIO5 = "Definitely";
	
	public BiasType biasType;
	public Episode episode;
	public string questionText;
	
	public override void ActivatePanel ()
	{
		aarMaster.ShowTitle(GenerateTitle());
		base.ActivatePanel ();
	}
	
	private string GenerateTitle() {
		switch(biasType) {
		case BiasType.ConfirmationBias:
			return TITLE_CONF_BIAS + TITLE_SUFFIX;
		case BiasType.FundamentalAttributionError:
			return TITLE_FAE + TITLE_SUFFIX;
		}
		
		return "Error Generating Title";
	}
	
	public override void SetupPanel() {
		panel.SetupAARComponents(AARPanel.PanelType.BiasSelfAssessmentPart1, AARPanel.NextButtonType.Next);
	}
	
	public override void CustomizePanel ()
	{
		panel.DisableNextButton();
		panel.subText1.Text = TXT_DESCRIPTION;
		panel.header2.Text = TXT_QUESTION_TITLE;
		panel.subText2.Text = questionText;
		SetRadioTexts();
	}
	
	private void SetRadioTexts() {
		panel.verticalRadioButtons[0].spriteText.Text = TXT_RADIO1;
		panel.verticalRadioButtons[1].spriteText.Text = TXT_RADIO2;
		panel.verticalRadioButtons[2].spriteText.Text = TXT_RADIO3;
		panel.verticalRadioButtons[3].spriteText.Text = TXT_RADIO4;
		panel.verticalRadioButtons[4].spriteText.Text = TXT_RADIO5;
	}
	
	public override void NextButtonPressed ()
	{
		SelfReviewQuestion selfReviewQuestion = SelfReviewQuestion.Conf1;
		if(biasType == BiasType.FundamentalAttributionError)
			selfReviewQuestion = SelfReviewQuestion.FAE1;
		
		SessionManager.GetSessionManager().currentSubject.ReportSelfReviewAnswer(episode, selfReviewQuestion, GetRadioValue(panel.verticalRadioButtons));
		
		base.NextButtonPressed ();
	}
	
	private void Update() {
		if(panel != null) {
			if(!panel.nextButton.controlIsEnabled) 
			{
				if(	CheckToSeeIfAnyRadioIsSelected(panel.verticalRadioButtons))
				{
					panel.EnableNextButton();
				}
			}
		}
	}	
	
}
