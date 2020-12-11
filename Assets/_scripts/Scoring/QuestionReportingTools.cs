using UnityEngine;
using System.Collections;

public class QuestionReportingTools {
	
	//BackCODE: This class is for gathering and generating Question Scoring Data.  These are mostly called from SubjectData
	
	/*
	public static Message GenerateFeedbackQuestionMessage(Episode episode, SubjectData.EpisodeFeedback epFeedback) 
	{
		
		return new Message_EpisodeFeedback(
			episode, 
			epFeedback.engagement,
			epFeedback.effort,
			epFeedback.challenge);
			
	}
	*/
	
	
	public static PlayerRating GetPlayerSelfRating(AARScreen.Question question, int index) 
	{
		PlayerRating selfRating = PlayerRating.NotYetEvaluated;
		switch(question) {
		case AARScreen.Question.ConfirmationBiasE1Q1:
			selfRating = ConvertPlayerSelfRating(QuestionType.Short, index);
			break;
		case AARScreen.Question.ConfirmationBiasE1Q2:
			selfRating = ConvertPlayerSelfRating(QuestionType.Long, index);
			break;
		case AARScreen.Question.FundamentalAttributionErrorE1Q1:
			selfRating = ConvertPlayerSelfRating(QuestionType.Short, index);
			break;
		case AARScreen.Question.FundamentalAttributionErrorE1Q2:
			selfRating = ConvertPlayerSelfRating(QuestionType.Long, index);
			break;
		case AARScreen.Question.ConfirmationBiasE2Q1:
			selfRating = ConvertPlayerSelfRating(QuestionType.Short, index);
			break;
		case AARScreen.Question.ConfirmationBiasE2Q2:
			selfRating = ConvertPlayerSelfRating(QuestionType.Long, index);
			break;
		case AARScreen.Question.FundamentalAttributionErrorE2Q1:
			selfRating = ConvertPlayerSelfRating(QuestionType.Short, index);
			break;
		case AARScreen.Question.FundamentalAttributionErrorE2Q2:
			selfRating = ConvertPlayerSelfRating(QuestionType.Long, index);
			break;
		case AARScreen.Question.ConfirmationBiasE3Q1:
			selfRating = ConvertPlayerSelfRating(QuestionType.Short, index);
			break;
		case AARScreen.Question.ConfirmationBiasE3Q2:
			selfRating = ConvertPlayerSelfRating(QuestionType.Long, index);
			break;
		case AARScreen.Question.FundamentalAttributionErrorE3Q1:
			selfRating = ConvertPlayerSelfRating(QuestionType.Short, index);
			break;
		case AARScreen.Question.FundamentalAttributionErrorE3Q2:
			selfRating = ConvertPlayerSelfRating(QuestionType.Long, index);
			break;
		default:
			Debug.LogError("Question Type incorrectly sent: " + question);
			break;
		}
		
		return selfRating;
	}
	
	
	private static string ConvertQuestionIndexToFeedbackAnswer(int index) 
	{
		string feedbackAnswer = "";
		switch(index) {
		case 0:
			feedbackAnswer = "Strongly Disagree";
			break;
		case 1:
			feedbackAnswer = "Disagree";
			break;
		case 2:
			feedbackAnswer = "Neutral";
			break;
		case 3:
			feedbackAnswer = "Agree";
			break;
		case 4:
			feedbackAnswer = "Strongly Agree";
			break;
		}
		return feedbackAnswer;
	}
	
	private static PlayerRating ConvertPlayerSelfRating(QuestionType type, int index) 
	{
		PlayerRating selfRating = PlayerRating.NotYetEvaluated;
		
		switch(type) 
		{
		case QuestionType.Short:
			if(index > 2)
			{
				selfRating = PlayerRating.BelowAverage;
			}
			else if(index == 2)
			{
				selfRating = PlayerRating.Average;
			}
			else
			{
				selfRating = PlayerRating.AboveAverage;
			}			
			break;
		case QuestionType.Long:
			if(index > 3)
			{
				selfRating = PlayerRating.BelowAverage;
			} 
			else if(index == 3)
			{
				selfRating = PlayerRating.Average;
			}
			else
			{
				selfRating = PlayerRating.AboveAverage;
			}			
			break;
		}
		
		return selfRating;
	}
	
	private static string GenerateSelfEvaluationString(AARScreen.Question question) 
	{
		string questionAndAnswerText;
		switch(question) 
		{
		case AARScreen.Question.ConfirmationBiasE1Q1:
			questionAndAnswerText = AARQuestionText.GetSelfReviewQuestionText(AARScreen.Question.ConfirmationBiasE1Q1);
			break;
		case AARScreen.Question.ConfirmationBiasE1Q2:
			questionAndAnswerText = AARQuestionText.GetSelfReviewQuestionText(AARScreen.Question.ConfirmationBiasE1Q2);
			break;
		case AARScreen.Question.FundamentalAttributionErrorE1Q1:
			questionAndAnswerText = AARQuestionText.GetSelfReviewQuestionText(AARScreen.Question.FundamentalAttributionErrorE1Q1);
			break;
		case AARScreen.Question.FundamentalAttributionErrorE1Q2:
			questionAndAnswerText = AARQuestionText.GetSelfReviewQuestionText(AARScreen.Question.FundamentalAttributionErrorE1Q2);
			break;
		case AARScreen.Question.ConfirmationBiasE2Q1:
			questionAndAnswerText = AARQuestionText.GetSelfReviewQuestionText(AARScreen.Question.ConfirmationBiasE2Q1);
			break;
		case AARScreen.Question.ConfirmationBiasE2Q2:
			questionAndAnswerText = AARQuestionText.GetSelfReviewQuestionText(AARScreen.Question.ConfirmationBiasE2Q2);
			break;
		case AARScreen.Question.FundamentalAttributionErrorE2Q1:
			questionAndAnswerText = AARQuestionText.GetSelfReviewQuestionText(AARScreen.Question.FundamentalAttributionErrorE2Q1);
			break;
		case AARScreen.Question.FundamentalAttributionErrorE2Q2:
			questionAndAnswerText = AARQuestionText.GetSelfReviewQuestionText(AARScreen.Question.FundamentalAttributionErrorE2Q2);
			break;
		case AARScreen.Question.ConfirmationBiasE3Q1:
			questionAndAnswerText = AARQuestionText.GetSelfReviewQuestionText(AARScreen.Question.ConfirmationBiasE3Q1);
			break;
		case AARScreen.Question.ConfirmationBiasE3Q2:
			questionAndAnswerText = AARQuestionText.GetSelfReviewQuestionText(AARScreen.Question.ConfirmationBiasE3Q2);
			break;
		case AARScreen.Question.FundamentalAttributionErrorE3Q1:
			questionAndAnswerText = AARQuestionText.GetSelfReviewQuestionText(AARScreen.Question.FundamentalAttributionErrorE3Q1);
			break;
		case AARScreen.Question.FundamentalAttributionErrorE3Q2:
			questionAndAnswerText = AARQuestionText.GetSelfReviewQuestionText(AARScreen.Question.FundamentalAttributionErrorE3Q2);
			break;					
		default:
			Debug.LogError("Incorrect Question Value Passed: " + question);
			return "";
		}
		
		questionAndAnswerText += " : ";
		return questionAndAnswerText;
	}	
	
}
