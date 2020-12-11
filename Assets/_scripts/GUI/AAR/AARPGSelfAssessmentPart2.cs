using UnityEngine;
using System.Collections;

public class AARPGSelfAssessmentPart2 : AARPanelGenerator {
	
	public const string TITLE_CONF_BIAS = "[#FF0000]Confirmation Bias";
	public const string TITLE_FAE = "[#FF0000]Fundamental Attribution Error";
	public const string TITLE_SUFFIX = " Self-Assessment[#555555]";
	
	public const string TXT_DESCRIPTION = "Please answer the following question.";
	public const string TXT_QUESTION_TITLE = "QUESTION 2 OF 2:";
	
	public const string TXT_RADIO1 = "[#FFFFFF]much less [#555555]{0}.";
	public const string TXT_RADIO2 = "[#FFFFFF]less [#555555]{0}.";
	public const string TXT_RADIO3 = "[#FFFFFF]slightly less [#555555]{0}.";
	public const string TXT_RADIO4 = "[#FFFFFF]just as much [#555555]{0}.";
	public const string TXT_RADIO5 = "[#FFFFFF]slightly more [#555555]{0}.";
	public const string TXT_RADIO6 = "[#FFFFFF]more [#555555]{0}.";
	public const string TXT_RADIO7 = "[#FFFFFF]much more [#555555]{0}.";
	
	
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
		panel.SetupAARComponents(AARPanel.PanelType.BiasSelfAssessmentPart2, AARPanel.NextButtonType.Done);
	}
	
	public override void CustomizePanel ()
	{
		panel.DisableNextButton();
		panel.subText1.Text = TXT_DESCRIPTION + "\n\n" + TXT_QUESTION_TITLE + "\n" + questionText + "\n[#FFFFFF]Relative to the average player, I showed: ";
		panel.header2.Text = "";
		panel.subText2.Text = "";
		SetRadioTexts();
		panel.ShowPreviousButton();
	}
	
	private void SetRadioTexts() {
		panel.verticalRadioButtons[0].spriteText.Text = string.Format(TXT_RADIO1, GenerateBiasText());
		panel.verticalRadioButtons[1].spriteText.Text = string.Format(TXT_RADIO2, GenerateBiasText());
		panel.verticalRadioButtons[2].spriteText.Text = string.Format(TXT_RADIO3, GenerateBiasText());
		panel.verticalRadioButtons[3].spriteText.Text = string.Format(TXT_RADIO4, GenerateBiasText());
		panel.verticalRadioButtons[4].spriteText.Text = string.Format(TXT_RADIO5, GenerateBiasText());
		panel.verticalRadioButtons[5].spriteText.Text = string.Format(TXT_RADIO6, GenerateBiasText());
		panel.verticalRadioButtons[6].spriteText.Text = string.Format(TXT_RADIO7, GenerateBiasText());
	}
	
	private string GenerateBiasText() {
		if(biasType == BiasType.FundamentalAttributionError)
			return "fundamental attribution error";
		
		return "confirmation bias";
	}
	
	public override void NextButtonPressed ()
	{
		SelfReviewQuestion selfReviewQuestion = SelfReviewQuestion.Conf2;
		if(biasType == BiasType.FundamentalAttributionError)
			selfReviewQuestion = SelfReviewQuestion.FAE2;
		
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
