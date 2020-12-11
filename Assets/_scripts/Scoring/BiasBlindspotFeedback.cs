using UnityEngine;
using System;
using System.Collections;
using System.IO;

[AddComponentMenu("UI/AAR Pane Types/AARPane Bias Blind Spot Feedback")]
public class BiasBlindSpotFeedback 
{
	public enum BBSTopic
	{
		ConfBiasE1,
		FAEE1,
		ConfBiasE2,
		FAEE2,
		ConfBiasE3,
		FAEE3,
		NONE
	}
	
	private float AMBIGOUS_ANSWER_PERCENTAGE_FOR_ABOVE_AVERAGE = 0.8f;
	private float AMBIGOUS_ANSWER_PERCENTAGE_FOR_AVERAGE = 0.4f;
	public const float BACKUP_AVERAGE_PLAYER_BIAS = 0.5f;	
	
	public BBSTopic biasTopic;
	private SubjectData currentSubject;
	
	public string GetPerformanceText(Episode episode, BiasType biasType)
	{
		currentSubject = SessionManager.GetSessionManager().currentSubject;
		biasTopic = DetermineBBSTopic(episode, biasType);
		
		string perfText = "NOT YET IMPLEMENTED.";
		
		switch( biasTopic )
		{
		case BBSTopic.ConfBiasE1:			
			perfText = GetConfirmationBiasFeedbackTextEp1(currentSubject);
			break;
		case BBSTopic.FAEE1:
			perfText = GetFAEBiasFeedbackTextEp1(currentSubject);
			break;
		case BBSTopic.ConfBiasE2:
			perfText = GetConfirmationBiasFeedbackTextEp2(currentSubject);
			break;
		case BBSTopic.FAEE2:
			perfText = GetFAEBiasFeedbackTextEp2(currentSubject);
			break;
		case BBSTopic.ConfBiasE3:
			perfText = GetConfirmationBiasFeedbackTextEP3(currentSubject);
			break;
		case BBSTopic.FAEE3:
			perfText = GetFAEBiasFeedbackTextEp3(currentSubject);
			break;
		default:
			Debug.LogError( "Unknown/unsupported Bias Blind Spot Topic encountered!" );
			break;
		}
		
		return perfText;
	}
	
	private string GetConfirmationBiasFeedbackTextEp1(SubjectData currentSubject) 
	{	
		//Pull Results of V1 & V3 searches
		float e1v1bias = GetSearchBias(Vignette.VignetteID.E1vTerrysApartmentSearch);
		float e1v3bias = GetSearchBias(Vignette.VignetteID.E1vHomeOfficeSearch);
		
		//Episode 1 has two search Vignettes so we need to average them together.
		float[] searchPerformances = {e1v1bias, e1v3bias};
		
		//Now average all of the Episode 1 answers together to generate our second performance marker.
		BiasChoice[] playerResponses = { currentSubject.E1VTerrysApartmentSearchAnswer, currentSubject.E1vHomeOfficeSearchAnswer };
		
		PlayerRating[] playerSelfAssessmentRatings = 
		{
			//QuestionReportingTools.GetPlayerSelfRating(AARScreen.Question.ConfirmationBiasE1Q1, currentSubject.GetEpisodeData(Episode.Episode1).selfAssessment.confSelfReview1),
			QuestionReportingTools.GetPlayerSelfRating(AARScreen.Question.ConfirmationBiasE1Q2, currentSubject.GetEpisodeData(Episode.Episode1).selfAssessment.confSelfReview2)
		};
		
		return GenerateGenericConfirmationBiasFeedbackText(searchPerformances, playerResponses, playerSelfAssessmentRatings, Episode.Episode1);
	}
	
	private string GetConfirmationBiasFeedbackTextEp2(SubjectData currentSubject)
	{
		float[] searchPerformances = {GetSearchBias(Vignette.VignetteID.E2vGPCOfficeSearch)};
		
		BiasChoice[] playerResponses = {currentSubject.E2vGPCOfficeSearchAnswer};
		
		PlayerRating[] playerSelfAssessmentRatings = 
		{
			//QuestionReportingTools.GetPlayerSelfRating(AARScreen.Question.ConfirmationBiasE2Q1, currentSubject.GetEpisodeData(Episode.Episode2).selfAssessment.confSelfReview1),
			QuestionReportingTools.GetPlayerSelfRating(AARScreen.Question.ConfirmationBiasE2Q2, currentSubject.GetEpisodeData(Episode.Episode2).selfAssessment.confSelfReview2)
		};
		
		return GenerateGenericConfirmationBiasFeedbackText(searchPerformances, playerResponses, playerSelfAssessmentRatings, Episode.Episode2);
	}
	
	private string GetConfirmationBiasFeedbackTextEP3(SubjectData currentSubject)
	{
		float[] searchPerformances = {GetSearchBias(Vignette.VignetteID.E3vChrisBriefcaseSearch)};
		
		BiasChoice[] playerResponses = {currentSubject.E3vChrisBriefcaseSearchAnswer};
		
		PlayerRating[] playerSelfAssessmentRatings =
		{
			//QuestionReportingTools.GetPlayerSelfRating(AARScreen.Question.ConfirmationBiasE2Q1, currentSubject.GetEpisodeData(Episode.Episode3).selfAssessment.confSelfReview1),
			QuestionReportingTools.GetPlayerSelfRating(AARScreen.Question.ConfirmationBiasE2Q2, currentSubject.GetEpisodeData(Episode.Episode3).selfAssessment.confSelfReview2)			
		};
		
		return GenerateGenericConfirmationBiasFeedbackText(searchPerformances, playerResponses, playerSelfAssessmentRatings, Episode.Episode3);
	}
	
	private string GenerateGenericConfirmationBiasFeedbackText(float[] searches, BiasChoice[] answers, PlayerRating[] selfAssessment, Episode episode) 
	{
		
		PlayerRating averageSearchEpBias = CompareSearchPerformanceToAveragePlayer(searches);
		PlayerRating averageResponsePerformance = GetPlayerAnswerPerformanceRating(answers);
		
		//Average the two performance ratings together
		PlayerRating[] playerRatings = {averageSearchEpBias, averageResponsePerformance};
		PlayerRating finalConfirmationBiasRating = PlayerRanker.AveragePlayerRatings(playerRatings);
		
		//Average the Player's Self Review Questions together
		PlayerRating averageSelfReviewRating = PlayerRanker.AveragePlayerRatings(selfAssessment);
		Debug.Log("Average Conf Bias Self Review Rating: " + averageSelfReviewRating);
		ReportEpisodeConfirmationBias(averageSelfReviewRating, finalConfirmationBiasRating, episode);
		
		return AARStringGenerator.GenerateBiasString(BiasType.ConfirmationBias, finalConfirmationBiasRating, averageSelfReviewRating);
	}
	
	private void ReportEpisodeConfirmationBias(PlayerRating playerRating, PlayerRating finalConfirmationBiasRating, Episode episode) {
		
		bool hasBlindspot = false;
		if(PlayerRanker.PlayerRatingToInt(playerRating) > PlayerRanker.PlayerRatingToInt(finalConfirmationBiasRating))
			hasBlindspot = true;
		
		SubjectData.EpisodeData epData = currentSubject.GetEpisodeData(episode);
		
		epData.biasRating.Conf = finalConfirmationBiasRating;
		epData.blindspot.confBlindspot = hasBlindspot;		
		
		ReportEvent.ReportCalculatedBiasScore(episode, BiasType.ConfirmationBias, finalConfirmationBiasRating);
		ReportEvent.ReportBlindspot(episode, BiasType.ConfirmationBias, hasBlindspot);
	}
	
	private string GetFAEBiasFeedbackTextEp1(SubjectData currentSubject) 
	{
		//Collect all relevant FAE Feedback Answers
		BiasChoice[] playerResponses = {currentSubject.E1vNervousElevatorAnswer, currentSubject.E1vStephEvasiveAnswer};
		
		PlayerRating[] playerSelfAssessmentRatings =
		{
			//QuestionReportingTools.GetPlayerSelfRating(AARScreen.Question.FundamentalAttributionErrorE1Q1, currentSubject.GetEpisodeData(Episode.Episode1).selfAssessment.faeSelfReview1),
			QuestionReportingTools.GetPlayerSelfRating(AARScreen.Question.FundamentalAttributionErrorE1Q2, currentSubject.GetEpisodeData(Episode.Episode1).selfAssessment.faeSelfReview2)
		};
		
		return GenerateGenericFAEFeedbackText(playerResponses, playerSelfAssessmentRatings, Episode.Episode1);
	}
	
	private string GetFAEBiasFeedbackTextEp2(SubjectData currentSubject) 
	{
		//Collect all relevant FAE Feedback Answers
		BiasChoice[] playerResponses = {currentSubject.E2vPressReleaseAnswer, currentSubject.E2vSorianoNiceAnswer};
		
		PlayerRating[] playerSelfAssessmentRatings = 
		{
			//QuestionReportingTools.GetPlayerSelfRating(AARScreen.Question.FundamentalAttributionErrorE2Q1, currentSubject.GetEpisodeData(Episode.Episode2).selfAssessment.faeSelfReview1),
			QuestionReportingTools.GetPlayerSelfRating(AARScreen.Question.FundamentalAttributionErrorE2Q2, currentSubject.GetEpisodeData(Episode.Episode2).selfAssessment.faeSelfReview2)
		};
		
		return GenerateGenericFAEFeedbackText(playerResponses, playerSelfAssessmentRatings, Episode.Episode2);
	}
	
	private string GetFAEBiasFeedbackTextEp3(SubjectData currentSubject) 
	{
		BiasChoice[] playerResponses = {currentSubject.E3vCoupleRomanceAnswer, currentSubject.E3vSuspiciousMenAnswer};
		
		PlayerRating[] playerSelfAssessmentRatings = 
		{
			//QuestionReportingTools.GetPlayerSelfRating(AARScreen.Question.FundamentalAttributionErrorE3Q1, currentSubject.GetEpisodeData(Episode.Episode3).selfAssessment.faeSelfReview1),
			QuestionReportingTools.GetPlayerSelfRating(AARScreen.Question.FundamentalAttributionErrorE3Q2, currentSubject.GetEpisodeData(Episode.Episode3).selfAssessment.faeSelfReview2),
		};
		
		return GenerateGenericFAEFeedbackText(playerResponses, playerSelfAssessmentRatings, Episode.Episode3);
	}
	
	private string GenerateGenericFAEFeedbackText(BiasChoice[] answers, PlayerRating[] selfAssessment, Episode episode)
	{
		PlayerRating averageResponsePerformance = GetPlayerAnswerPerformanceRating(answers);
		
		//Average the Player's Self Review Questions together
		PlayerRating averageSelfReviewRating = PlayerRanker.AveragePlayerRatings(selfAssessment);
		Debug.Log("Average FAE Bias Self Review Rating: " + averageSelfReviewRating);
		ReportFAEBias(averageSelfReviewRating, averageResponsePerformance, episode);
		
		return AARStringGenerator.GenerateBiasString(BiasType.FundamentalAttributionError, averageResponsePerformance, averageSelfReviewRating);
	}
	
	private void ReportFAEBias(PlayerRating playerRating, PlayerRating averageResponsePerformance, Episode episode) {
		
		bool hasBlindspot = false;
		
		if(PlayerRanker.PlayerRatingToInt(playerRating) > PlayerRanker.PlayerRatingToInt(averageResponsePerformance))
			hasBlindspot = true;
		
		SubjectData.EpisodeData epData = currentSubject.GetEpisodeData(episode);
		epData.biasRating.FAE = averageResponsePerformance;
		epData.blindspot.faeBlindspot = hasBlindspot;
		
		ReportEvent.ReportCalculatedBiasScore(episode, BiasType.FundamentalAttributionError, averageResponsePerformance);
		ReportEvent.ReportBlindspot(episode, BiasType.FundamentalAttributionError, hasBlindspot);
	}	
	
	private PlayerRating CompareSearchPerformanceToAveragePlayer(float[] biasScores)
	{
		//Safety Exit since we're dividing by a Array Length
		if(biasScores.Length == 0)
			return PlayerRating.NotYetEvaluated;
		
		float biasSum = 0f;
		foreach(float score in biasScores)
		{
			biasSum += score;
		}
		
		float biasAverage = biasSum / biasScores.Length;
		
		return ComparePerformanceAgainstAverage(biasAverage);
	}
	
	private PlayerRating ComparePerformanceAgainstAverage(float biasAverage)
	{
		PlayerRating searchRating = PlayerRating.NotYetEvaluated;
		
		if(biasAverage >= AMBIGOUS_ANSWER_PERCENTAGE_FOR_ABOVE_AVERAGE) 
			searchRating = PlayerRating.AboveAverage;
		
		if(biasAverage > AMBIGOUS_ANSWER_PERCENTAGE_FOR_AVERAGE && biasAverage < AMBIGOUS_ANSWER_PERCENTAGE_FOR_ABOVE_AVERAGE)
			searchRating = PlayerRating.Average;
		
		if(biasAverage <= AMBIGOUS_ANSWER_PERCENTAGE_FOR_AVERAGE)
			searchRating = PlayerRating.BelowAverage;
		
		Debug.Log("This Search has been rated : " + searchRating.ToString());
		
		return searchRating;
	}
	
	private PlayerRating GetPlayerAnswerPerformanceRating(BiasChoice[] choices) 
	{
		if(choices.Length == 0)
		{
			Debug.LogError("No Choices passed for Conf Bias Rating!");
			return PlayerRating.NotYetEvaluated;;
		}
		
		int ambigousCount = 0;
		
		foreach (BiasChoice choice in choices) 
		{
			if(choice == BiasChoice.Ambiguous)
			{
				ambigousCount++;
			}
		}
		
		if(ambigousCount == choices.Length)
			return PlayerRating.AboveAverage;
		else if(ambigousCount >= (choices.Length / 2)) //Got half or better right
			return PlayerRating.Average;
		else //Got less than half right
			return PlayerRating.BelowAverage;
	}
	
	private float GetSearchBias(Vignette.VignetteID vignette) 
	{
		SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
		float biasAmount = -1;
		
		switch(vignette)
		{
		case Vignette.VignetteID.E1vHomeOfficeSearch:
			biasAmount = currentSubject.e1v3ExplorationScore.AmbigiousBiasScore;
			break;
		case Vignette.VignetteID.E1vTerrysApartmentSearch:
			biasAmount = currentSubject.e1v1ExplorationScore.AmbigiousBiasScore;
			break;
		case Vignette.VignetteID.E2vGPCOfficeSearch:
			biasAmount = currentSubject.e2v7ExplorationScore.AmbigiousBiasScore;
			break;
		case Vignette.VignetteID.E3vChrisBriefcaseSearch:
			biasAmount = currentSubject.e3v10ExplorationScore.AmbigiousBiasScore;
			break;
		default:
			Debug.LogError("Incorrect Vignette Passed to Fuzzy Feedback!!");
			break;
		}
		
		return biasAmount;
	}
	
	private BBSTopic DetermineBBSTopic(Episode episode, BiasType biasType)
	{
		BBSTopic topic = BBSTopic.NONE;
		
		switch(episode)
		{
		case Episode.Episode1:
			
			if(biasType == BiasType.ConfirmationBias)
				topic = BBSTopic.ConfBiasE1;
			else
				topic = BBSTopic.FAEE1;
			
			break;
		case Episode.Episode2:
			
			if(biasType == BiasType.ConfirmationBias)
				topic = BBSTopic.ConfBiasE2;
			else
				topic = BBSTopic.FAEE2;
			
			break;
		case Episode.Episode3:
			
			if(biasType == BiasType.ConfirmationBias)
				topic = BBSTopic.ConfBiasE3;
			else
				topic = BBSTopic.FAEE3;
			
			break;
		}
		
		return topic;
	}	
	
}

