using UnityEngine;
using System.Collections;

public class AARPanel : MonoBehaviour {
	
	public enum PanelType {
		GameplayFeedback5,
		GameplayFeedback7,
		Intro,
		Movie,
		MovieWithSubtext,
		Vanilla,
		BiasSelfAssessmentPart1,
		BiasSelfAssessmentPart2,
		BiasFeedback,
		BiasBlindspot,
		EndOfLevel,
		Quiz
	}
	
	public enum NextButtonType {
		None,
		Next,
		Done,
		Quit
	}
	
	public UIButton prevButton;
	public SpriteText prevButtonTxt;
	public UIButton nextButton;
	public SpriteText nextButtonTxt;
	public UIButton skipButton;
	public SpriteText skipButtonTxt;
	
	public UIRadioBtn[] verticalRadioButtons;
	
	public SpriteText subText1;
	
	public SpriteText header2;
	public SpriteText subText2;
	
	public UIRadioBtn[] horizontalRadio7;
	public SpriteText horizontalRadio7LeftLabel;
	public SpriteText horizontalRadio7RightLabel;
	public UIRadioBtn[] horizontalRadio5;
	public SpriteText horizontalRadio5LeftLabel;
	public SpriteText horizontalRadio5RightLabel;
	
	public MoviePlayer moviePlayer;
	public SpriteText movieSubtitle;
	
	public SpriteText episodeComplete;
	
	public void HideRadios(UIRadioBtn[] radios) 
	{
		foreach(UIRadioBtn button in radios)
			button.Hide(true);
	}
	
	public void ClearRadios(UIRadioBtn[] radios) 
	{
		foreach(UIRadioBtn button in radios)
			button.Value = false;
	}
	
	public void SetupAARComponents(PanelType panelType, NextButtonType nextButton) {
		
		switch(panelType) {
		case PanelType.GameplayFeedback7:
			ShowHeader2();
			ShowHorizontalRadios7();
			ShowNextButton(nextButton);
			break;
		case PanelType.GameplayFeedback5:
			ShowHeader2();
			ShowHorizontalRadios5();
			ShowNextButton(nextButton);
			break;
		case PanelType.Intro:
			ShowIntroTexts();
			break;
		case PanelType.Movie:
			break;
		case PanelType.MovieWithSubtext:
			ShowMovieSubtitle();
			break;
		case PanelType.Vanilla:
			ShowAllHeaderText();
			break;
		case PanelType.BiasSelfAssessmentPart1:
			ShowAllHeaderText();
			ShowVerticalRadios(5);
			break;
		case PanelType.BiasSelfAssessmentPart2:
			ShowAllHeaderText();
			ShowVerticalRadios(7);
			break;
		case PanelType.BiasFeedback:
			ShowAllHeaderText();
			break;
		case PanelType.BiasBlindspot:
			ShowAllHeaderText();
			break;
		case PanelType.EndOfLevel:
			ShowAllHeaderText();
			break;
		case PanelType.Quiz:
			ShowAllHeaderText();
			ShowVerticalRadios(7);
			break;
		}
		
		ShowNextButton(nextButton);
		
		if(Application.isEditor || Debug.isDebugBuild) {
			//ShowSkipButton();
		}
	}
	
	//All gui features start hidden, so we just need to turn them on.
	
	private void ShowIntroTexts() {
		episodeComplete.Hide(false);
	}
	
	private void ShowAllHeaderText() {
		ShowTopHeader();
		ShowMidHeader();
	}
	
	private void ShowTopHeader() {
		ShowSubText1();
	}
	
	private void ShowMidHeader() {
		ShowHeader2();
		ShowSubText2();
	}
	
	private void ShowHeader2() {
		header2.Hide(false);
	}
	
	private void ShowSubText1() {
		subText1.Hide(false);
	}
	
	private void ShowSubText2() {
		subText2.Hide(false);
	}
	
	private void ShowMovieSubtitle() {
		movieSubtitle.Hide(false);
	}
	
	private void ShowHorizontalRadios5() {
		foreach(UIRadioBtn radio in horizontalRadio5)
			radio.Hide(false);
		horizontalRadio5LeftLabel.Hide(false);
		horizontalRadio5RightLabel.Hide(false);
	}
	
	private void ShowHorizontalRadios7() {
		foreach(UIRadioBtn radio in horizontalRadio7)
			radio.Hide(false);
		horizontalRadio7LeftLabel.Hide(false);
		horizontalRadio7RightLabel.Hide(false);
	}
	
	private void ShowVerticalRadios(int numberToShow) {
		for (int i = 0; i < numberToShow; i++) {
			verticalRadioButtons[i].Hide(false);
		}
	}
	
	private void ShowNextButton(NextButtonType nextButtonType) {
		
		switch(nextButtonType) {
		case NextButtonType.None:
			return;
		case NextButtonType.Done:
			nextButtonTxt.Text = "done";
			break;
		case NextButtonType.Next:
			nextButtonTxt.Text = "next";
			break;
		case NextButtonType.Quit:
			nextButtonTxt.Text = "quit";
			break;
		}
		
		nextButton.Hide(false);
		nextButtonTxt.Hide(false);
	}
	
	public void ShowPreviousButton() {
		prevButton.Hide(false);
		prevButtonTxt.Hide(false);
	}
	
	private void ShowSkipButton() {
		skipButton.Hide(false);
		skipButtonTxt.Hide(false);
	}
	
	public void DisableNextButton() {
		nextButton.controlIsEnabled = false;
		nextButtonTxt.SetColor( new Color(0.1f, 0.1f, 0.1f, 0.5f) );
	}
	
	public void VanishNexButtonText() {
		nextButtonTxt.SetColor( new Color(nextButton.color.r, nextButton.color.b, nextButton.color.g, 0f) );
	}
	
	public void EnableNextButton() {
		nextButton.controlIsEnabled = true;
		nextButtonTxt.SetColor( new Color(1.0f, 1.0f, 1.0f, 1.0f));
	}
	
}
