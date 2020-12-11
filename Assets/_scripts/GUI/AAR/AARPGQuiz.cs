using UnityEngine;
using System.Collections;

public class AARPGQuiz : AARPanelGenerator {
	
	public Vignette.VignetteID vignette;
	public Quiz quiz;
	public bool playerGetsMultipleChances;
	
	private bool showingQuiz;
	private int selectedAnswer;
	private int attempts = 0;
	private bool correct = false;
	
	public override void ActivatePanel ()
	{
		aarMaster.ShowTitle(LogicFeedback.GetLogicTitle(vignette));
		base.ActivatePanel ();
	}
	
	public override void SetupPanel() {
		panel.SetupAARComponents(AARPanel.PanelType.BiasFeedback, AARPanel.NextButtonType.Next);
	}
	
	public override void CustomizePanel ()
	{
		panel.DisableNextButton();
		
		showingQuiz = true;
		
		panel.subText1.Text = "\n" + QuizData.GetQuizQuestionText(quiz);
		panel.header2.Text = "";
		panel.subText2.Text = "";
		
		SetupVerticalRadios();
	}
	
	public override void NextButtonPressed ()
	{
		attempts++;
		
		selectedAnswer = GetRadioValue(panel.verticalRadioButtons);
		
		if(!showingQuiz) {
			base.NextButtonPressed ();
			return;
		}
		
		if(selectedAnswer == QuizData.GetCorrectQuizAnswer(quiz))
		{
			correct = true;
			PrepForQuizAnswer();
			SetupForCorrectAnswer();
		}
		else
		{
			correct = false;
			PrepForQuizAnswer();
			if(playerGetsMultipleChances)
				SetupForReplay();
			else
				SetupForIncorrectAnswer();
		}
	}
	
	private void PrepForQuizAnswer() 
	{
		showingQuiz = false;
		panel.HideRadios(panel.verticalRadioButtons);
		panel.subText1.Text = "";
		panel.header2.Text = "";
		panel.subText2.Text = "";
		
		if(CheckForReport())
			ReportEvent.QuizResults(quiz, QuizData.GetQuizAnswerText(quiz)[selectedAnswer - 1], selectedAnswer, correct, attempts);
	}
	
	private bool CheckForReport() {
		if(!playerGetsMultipleChances)
			return true;
		
		if(playerGetsMultipleChances && correct)
			return true;
		
		return false;
	}
	
	private void SetupForCorrectAnswer()
	{
		panel.subText2.Text = QuizData.GetCorrectResponse(quiz);
	}
	
	private void SetupForIncorrectAnswer() 
	{
		string answerChosen = QuizData.GetQuizAnswerText(quiz)[selectedAnswer - 1];
		panel.subText2.Text = string.Format(QuizData.GetIncorrectResponse(quiz), answerChosen);
	}
	
	private void SetupForReplay()
	{
		showingQuiz = true;
		
		panel.DisableNextButton();
		panel.ClearRadios(panel.verticalRadioButtons);
		SetupVerticalRadios();
		panel.subText1.Text = "\n" + QuizData.GetIncorrectResponse(quiz) + "\n" + QuizData.GetQuizQuestionText(quiz);
		panel.header2.Text = "";
		panel.subText2.Text = "";
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
	
	private void SetupVerticalRadios()
	{
		foreach(UIRadioBtn radio in panel.verticalRadioButtons)
			radio.Hide(true);
		
		string[] answers = QuizData.GetQuizAnswerText(quiz);
		
		for (int i = 1; i < answers.Length + 1; i++) {
			panel.verticalRadioButtons[i].Hide(false);
			panel.verticalRadioButtons[i].spriteText.Text = answers[i - 1];
		}
	}
	
}
