using UnityEngine;
using System.Collections;

[AddComponentMenu("UI/AAR Pane Types/AARPane Objective Feedback")]
public class ObjectiveFeedback
{
	public class QuestionVariables 
	{
		public string question;
		public string answerAffirmative;
		public string answerNegative;
		public string answerUnknown;
		public string wrapUp;
	}
	
	public const string GENERAL_OPENING = "[#555555]You answered the question, [#FFFFFF]{0}[#555555] with [#FFFFFF]{1}[#555555] \n\n";

	public const string WRONG = "[#555555]This is a common, but [#FFFFFF]incorrect[#555555] answer.\n\n";
	public const string WRONG_BUT_NOT_FAE = "[#555555]This answer avoids falling for the fundamental attribution error, but is still [#FFFFFF]incorrect[#555555].\n\n";
	public const string CORRECT = "[#555555]This is the [#FFFFFF]correct[#555555] answer.\n\n";

	public const string GENERAL_CLOSING = "\n\nTo avoid the fundamental attribution error, [#FFFFFF]remember to think about how most people would act[#555555] in the situation you are judging.  [#FFFFFF]If most people would act that way[#555555], a person's behavior is unlikely to tell you much about his or her personality.";
	
	public const string V2_QUESTION = "\"Do you think Terry is a nervous person?\"";
	public const string V2_ANSWER_AFFIRMATIVE = "\"She is a nervous person.\"";
	public const string V2_ANSWER_UNKNOWN = "\"I can't tell.\"";
	public const string V2_ANSWER_NEGATIVE = "\"No, she's not a nervous person.\"";
	public const string V2_WRAPUP = "Terry does appear nervous when riding a creaky elevator. However, most people would be nervous in that situation as well. There is not enough evidence to infer that Terry is a nervous person in general.";
	public static QuestionVariables vignette2Question = new QuestionVariables();
	
	public const string V4_QUESTION = "\"Do you think Stephanie is an evasive person?\"";
	public const string V4_ANSWER_NEGATIVE = "\"I don't think she is an evasive person.\"";
	public const string V4_ANSWER_UNKNOWN = "\"I'm not sure.\"";
	public const string V4_ANSWER_AFFIRMATIVE = "\"I agree, she is an evasive person!\"";
	public const string V4_WRAPUP = "Stephanie seems evasive on the phone. For this reason, many people assume that she is generally an evasive person. However, she was in a public place (given the background noise) and may not have been able to speak freely. One phone discussion is not enough to infer whether she is generally an evasive person or not. ";
	public static QuestionVariables vignette4Question = new QuestionVariables();
	
	public const string V5_QUESTION = "\"Do you think he is a really nice guy?\"";
	public const string V5_ANSWER_AFFIRMATIVE = "\"He does seem like a nice guy.\"";
	public const string V5_ANSWER_NEGATIVE = "\"He doesn't look too nice to me.\"";
	public const string V5_ANSWER_UNKNOWN = "\"I'm not sure.\"";
	public const string V5_WRAPUP = "Richard Soriano might appear to be a nice person at a company party, but that would be expected of most people celebrating a company milestone. He might be putting up a front and really not be so nice, but we cannot be sure. There is not enough information to infer whether he is either a good or bad person.";
	public static QuestionVariables vignette5Question = new QuestionVariables();
	
	public const string V7_QUESTION = "\"Do you think Terry believes the company is blameless?\"";
	public const string V7_ANSWER_AFFIRMATIVE = "\"Terry doesn't believe the company was responsible.\"";
	public const string V7_ANSWER_NEGATIVE = "\"I think Terry blames the company for the Exceleron scandal.\"";
	public const string V7_ANSWER_UNKNOWN = "\"I can't tell from this letter.\"";
	public const string V7_WRAPUP = "Terry wrote the press release supporting the company's position as part of her job. It represents the company's view, not necessarily Terry's personal opinion or belief. ";
	public static QuestionVariables vignette7Question = new QuestionVariables();
	
	public const string V8_QUESTION = "\"So, what do you think? Are they planning something dangerous?\"";
	public const string V8_ANSWER_AFFIRMATIVE = "\"I think the two men have dangerous intentions.\"";
	public const string V8_ANSWER_NEGATIVE = "\"I think they are just planning some kind of business venture.\"";
	public const string V8_ANSWER_UNKNOWN = "\"I don't know.\"";
	public const string V8_WRAPUP = "Neither you, nor the waitress have enough information to conclude what the two men are thinking at this point.";
	public static QuestionVariables vignette8Question = new QuestionVariables();
	
	public const string V9_QUESTION = "\"Does that man seem to like that girl?\"";
	public const string V9_ANSWER_AFFIRMATIVE = "\"I think he likes her.\"";
	public const string V9_ANSWER_NEGATIVE = "\"No, he doesn't like her.\"";
	public const string V9_ANSWER_UNKNOWN = "\"I'm not sure...\"";
	public const string V9_WRAPUP = "Terry was seen being friendly with Richard Soriano, a company Vice President. Richard may have been nice because of the setting. There is not enough information to show that Richard dislikes or likes Terry.";
	public static QuestionVariables vignette9Question = new QuestionVariables();
	
	private static void PopulateQuestionVariables() {
		vignette2Question.question = V2_QUESTION;
		vignette2Question.answerAffirmative = V2_ANSWER_AFFIRMATIVE;
		vignette2Question.answerNegative = V2_ANSWER_NEGATIVE;
		vignette2Question.answerUnknown = V2_ANSWER_UNKNOWN;
		vignette2Question.wrapUp = V2_WRAPUP;
		
		vignette4Question.question = V4_QUESTION;
		vignette4Question.answerAffirmative = V4_ANSWER_AFFIRMATIVE;
		vignette4Question.answerNegative = V4_ANSWER_NEGATIVE;
		vignette4Question.answerUnknown = V4_ANSWER_UNKNOWN;
		vignette4Question.wrapUp = V4_WRAPUP;
		
		vignette5Question.question = V5_QUESTION;
		vignette5Question.answerAffirmative = V5_ANSWER_AFFIRMATIVE;
		vignette5Question.answerNegative = V5_ANSWER_NEGATIVE;
		vignette5Question.answerUnknown = V5_ANSWER_UNKNOWN;
		vignette5Question.wrapUp = V5_WRAPUP;		
		
		vignette7Question.question = V7_QUESTION;
		vignette7Question.answerAffirmative = V7_ANSWER_AFFIRMATIVE;
		vignette7Question.answerNegative = V7_ANSWER_NEGATIVE;
		vignette7Question.answerUnknown = V7_ANSWER_UNKNOWN;
		vignette7Question.wrapUp = V7_WRAPUP;
		
		vignette8Question.question = V8_QUESTION;
		vignette8Question.answerAffirmative = V8_ANSWER_AFFIRMATIVE;
		vignette8Question.answerNegative = V8_ANSWER_NEGATIVE;
		vignette8Question.answerUnknown = V8_ANSWER_UNKNOWN;
		vignette8Question.wrapUp = V8_WRAPUP;
		
		vignette9Question.question = V9_QUESTION;
		vignette9Question.answerAffirmative = V9_ANSWER_AFFIRMATIVE;
		vignette9Question.answerNegative = V9_ANSWER_NEGATIVE;
		vignette9Question.answerUnknown = V9_ANSWER_UNKNOWN;
		vignette9Question.wrapUp = V9_WRAPUP;		
	}
	
	public static string GetPerformanceText(Vignette.VignetteID vignette)
	{
		
		PopulateQuestionVariables();
		
		string perfText = "NOT YET IMPLEMENTED.";
		
		switch( vignette )
		{
		case Vignette.VignetteID.E1vNervousElevator:
			perfText = CreateQuestionText(Vignette.VignetteID.E1vNervousElevator, vignette2Question);
			break;
		case Vignette.VignetteID.E1vStephEvasive:
			perfText = CreateQuestionText(Vignette.VignetteID.E1vStephEvasive, vignette4Question);
			break;
		case Vignette.VignetteID.E2vSorianoNice:
			perfText = CreateQuestionText(Vignette.VignetteID.E2vSorianoNice, vignette5Question);
			break;
		case Vignette.VignetteID.E2vPressRelease:
			perfText = CreateQuestionText(Vignette.VignetteID.E2vPressRelease, vignette7Question);
			break;
		case Vignette.VignetteID.E3vSuspiciousMen:
			perfText = CreateQuestionText(Vignette.VignetteID.E3vSuspiciousMen, vignette8Question);
			break;
		case Vignette.VignetteID.E3vCoupleRomance:
			perfText = CreateQuestionText(Vignette.VignetteID.E3vCoupleRomance, vignette9Question);
			break;
		default:
			Debug.Log( "Incorrect Vignette Quesiton Passed." );
			break;
		}
		
		return perfText;
	}
	
	private static string CreateQuestionText(Vignette.VignetteID vignette, QuestionVariables questionVars)
	{
		string recordedAnswer = "";
		string answerCorrectness = "";
		
		BiasChoice answerBias = GetCorrectAnswer(vignette);
		switch(answerBias)
		{
		case BiasChoice.Ambiguous:
			recordedAnswer = questionVars.answerUnknown;
			answerCorrectness = CORRECT;
			break;
		case BiasChoice.Confirming:
			recordedAnswer = questionVars.answerAffirmative;
			answerCorrectness = WRONG;
			break;
		case BiasChoice.Disconfirming:
			recordedAnswer = questionVars.answerNegative;
			answerCorrectness = WRONG_BUT_NOT_FAE;
			break;
		case BiasChoice.None:			
			return "Bias Choice not saved!!";
		}
		
		string questionAnalysisText = string.Format(GENERAL_OPENING, questionVars.question, recordedAnswer);
		
		questionAnalysisText += answerCorrectness;
		questionAnalysisText += questionVars.wrapUp;
		questionAnalysisText += GENERAL_CLOSING;
		
		return questionAnalysisText;
	}
	
	private static BiasChoice GetCorrectAnswer(Vignette.VignetteID vignette) {
		SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
		switch(vignette) {
		case Vignette.VignetteID.E1vNervousElevator:
			return currentSubject.E1vNervousElevatorAnswer;
		case Vignette.VignetteID.E1vStephEvasive:
			return currentSubject.E1vStephEvasiveAnswer;
		case Vignette.VignetteID.E2vSorianoNice:
			return currentSubject.E2vSorianoNiceAnswer;
		case Vignette.VignetteID.E2vPressRelease:
			return currentSubject.E2vPressReleaseAnswer;
		case Vignette.VignetteID.E3vCoupleRomance:
			return currentSubject.E3vCoupleRomanceAnswer;
		case Vignette.VignetteID.E3vSuspiciousMen:
			return currentSubject.E3vSuspiciousMenAnswer;
		default:
			Debug.LogError("Incorrect Vignette passed.");
			return BiasChoice.None;
		}
	}
	
}

