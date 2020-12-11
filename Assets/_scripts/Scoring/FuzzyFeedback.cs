using UnityEngine;
using System;
using System.Collections;

//BACKCODE: THOM DENICK
//This class has been almost enitrely rewritten to dynamically generate the Fuzzy Feedback strings.

public class FuzzyFeedback
{
	
	public const string TXT_PLACEHOLDER = "Not yet Implemented.";
	public const string TXT_NO_CHOICE_RECORDED = "NO CHOICE RECORDED FOR THIS VIGNETTE.";
	
	public const string TXT_VIGNETTE1_INTRO_SENTENCE = "[#555555]Given conflicting evidence in the apartment, there is not enough information to say what happened to Terry. \n\n";
	public const string TXT_VIGNETTE3_INTRO_SENTENCE = "[#555555]Terry's home office contained evidence of multiple problems that she might have. Given conflicting evidence, there is not enough information to determine what her problem is. \n\n";
	public const string TXT_VIGNETTE6_INTRO_SENTENCE = "[#555555]Given conflicting evidence in the office, there is not enough information to determine if Terry's trouble stems from her cheating on her expense reports. \n\n";
	public const string TXT_VIGNETTE10_INTRO_SENTENCE = "[#555555]Given conflicting evidence in the briefcase, there is not enough information to determine what Chris's problem is. \n\n";
	
	public const string TXT_CORRECT_ANSWER = "You answered {0} question [#FFFFFF]correctly[#555555]. ";
	public const string TXT_INCORRECT_ANSWER = "You answered {0} question [#FFFFFF]incorrectly[#555555]. ";
	
	public const string TXT_QUESTION_ASKER_GENERIC = "the vignette";
	public const string TXT_QUESTION_ASKER_CHRIS = "Chris's";
	public const string TXT_QUESTION_ASKER_LANDLORD = "the landlord's";
	public const string TXT_QUESTION_ASKER_STEPHANIE = "Stephanie's";
	
	public const string TXT_HOWEVER = "However, ";
	public const string TXT_ADDITIONALLY = "Additionally, ";
	
	public const string TXT_GENERIC_SEARCH = "your search was ";
	
	public const string TXT_SEARCH_PERFECT = "[#FFFFFF]perfectly balanced[#555555]. ";
	public const string TXT_SEARCH_MILD = "[#FFFFFF]reasonably balanced[#555555]. ";
	public const string TXT_SEARCH_SOMEWHAT = "[#FFFFFF]moderately biased[#555555]. ";
	public const string TXT_SEARCH_SIGNFICANT = "[#FFFFFF]biased[#555555]. ";
	public const string TXT_SEARCH_INTENSE = "[#FFFFFF]extremely biased[#555555]. ";
	public const string TXT_SEARCH_COMPLETE = "[#FFFFFF]completely biased[#555555]. ";
	
	public const string TXT_GENERIC_BIAS_SUMMARY = "[#555555]You {1} {0} the hypothesis suggested by the story characters and {2} search for evidence supporting alternative possibilities.";
	
	public const string TXT_BIAS_EQUAL_PART1 = "[#FFFFFF]searched for and evaluated an equal amount of evidence [#555555]";
	public const string TXT_BIAS_LEVEL_PART1_MILD = "[#FFFFFF]searched for and evaluated only slightly more evidence[#555555]";
	public const string TXT_BIAS_LEVEL_PART1_SOMEWHAT = "[#FFFFFF]searched for and evaluated more evidence[#555555]";
	public const string TXT_BIAS_LEVEL_PART1_SIGNIFICANT = "[#FFFFFF]searched for and evaluated more evidence[#555555]";
	public const string TXT_BIAS_LEVEL_PART1_INTENSE = "[#FFFFFF]searched for and evaluated much more evidence[#555555]";
	public const string TXT_BIAS_LEVEL_PART1_COMPLETE = "[#FFFFFF]only searched for and evaluated evidence[#555555]";
	
	public const string TXT_BIAS_EQUAL_PART2 = "[#FFFFFF]performed a balanced[#555555]";
	public const string TXT_BIAS_LEVEL_PART2_MILD = "[#FFFFFF]engaged in a slightly less balanced[#555555]";
	public const string TXT_BIAS_LEVEL_PART2_SOMEWHAT = "[#FFFFFF]did not sufficiently[#555555]";
	public const string TXT_BIAS_LEVEL_PART2_SIGNIFICANT = "[#FFFFFF]did not sufficiently[#555555]";
	public const string TXT_BIAS_LEVEL_PART2_INTENSE = "[#FFFFFF]did not sufficiently[#555555]";
	public const string TXT_BIAS_LEVEL_PART2_COMPLETE = "[#FFFFFF]and did not[#555555]";	
	
	public const string TXT_CONFIRMING = "[#FFFFFF]confirming[#555555]";
	public const string TXT_DISCONFIRMING = "[#FFFFFF]disconfirming[#555555]";
	public const string TXT_EQUAL = "[#FFFFFF]confirming and disconfirming[#555555]";
	
	public const string TXT_BUT = "[#555555]However, you";
	public const string TXT_AND = "You also";
	
	public const string TXT_INSUFFICIENT_EVIDENCE = " [#FFFFFF]did not search for sufficient evidence [#555555]before answering the question. [#FFFFFF]Make sure to search for enough evidence before coming to a conclusion[#555555].";
	
	public static string GetPerformanceText(Vignette.VignetteID vignetteID)
	{
		string perfText = TXT_PLACEHOLDER;
		
		switch( vignetteID )
		{
		case Vignette.VignetteID.E1vTerrysApartmentSearch:
			perfText = GetConfBiasFeedback(Vignette.VignetteID.E1vTerrysApartmentSearch, TXT_GENERIC_BIAS_SUMMARY, TXT_VIGNETTE1_INTRO_SENTENCE, TXT_QUESTION_ASKER_CHRIS);
			break;
		case Vignette.VignetteID.E1vHomeOfficeSearch:
			perfText = GetConfBiasFeedback(Vignette.VignetteID.E1vHomeOfficeSearch, TXT_GENERIC_BIAS_SUMMARY, TXT_VIGNETTE3_INTRO_SENTENCE, TXT_QUESTION_ASKER_LANDLORD);
			break;
		case Vignette.VignetteID.E2vGPCOfficeSearch:
			perfText = GetConfBiasFeedback(Vignette.VignetteID.E2vGPCOfficeSearch, TXT_GENERIC_BIAS_SUMMARY, TXT_VIGNETTE6_INTRO_SENTENCE, TXT_QUESTION_ASKER_STEPHANIE);
			break;
		case Vignette.VignetteID.E3vChrisBriefcaseSearch:
			perfText = GetConfBiasFeedback(Vignette.VignetteID.E3vChrisBriefcaseSearch, TXT_GENERIC_BIAS_SUMMARY, TXT_VIGNETTE10_INTRO_SENTENCE, TXT_QUESTION_ASKER_STEPHANIE);
			break;
		default:
			Debug.LogError( "Attempting to get Fuzzy Feedback on a Vignette with no searching!!" );
			break;
		}
		
		return perfText;
	}
	
	private static string GetConfBiasFeedback( Vignette.VignetteID vignetteID, string masterString, string vignetteIntroString, string questionAsker )
	{
		LevelManager levelManager = LevelManager.FindLevelManager();
		if(levelManager == null) {
			return "No Level Manager running!!";
		}

//		//Get the score report.
//		ScoringManager scoreMgr = levelManager.ScoringManager;
//		
//		ScoreReport report = scoreMgr.GetScoresForMethod( vignetteID
//														, false	//Print individuals?
//														, false	//Print overall?
//														);
//		
//		EvaluationManager evalMgr = levelManager.EvaluationManager;
//		
//		FuzzyReasoningCognitiveBias bias = evalMgr.GetCognitiveBiasReasoning();
//		
//		FuzzyReasoningCognitiveBias.DefuzzifyResults result;	//Error in the assessment?
//		bool bPassedAlphaThreshold;	//Did we pass the cut?
//		
//		bias.GetFinalAssessment( report					//Score values
//							   , out result				//Was there an error?
//							   , out bPassedAlphaThreshold	//Did we make the cut?
//							   );		
//		
//		Debug.Log("Vignette: " + vignetteID + " has a Fuzzy Bias Result of: " + result.m_result);
//		Debug.Log("Result Final Domain Value: " + result.m_finalDomainVal);
//		Debug.Log("Result Highest Membership: " + result.m_highestMembership);
//		Debug.Log("Result Passed Alpha Threshold: " + bPassedAlphaThreshold);		
		
		SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
		BiasChoice choice = AnswerScoreTools.GetCorrectBiasChoice(vignetteID, currentSubject);
		VignetteScore vignetteScore = GetVignetteScore(vignetteID, currentSubject);
		bool bMadeAnyChoice = ( BiasChoice.None != choice );
		
		if ( false == bMadeAnyChoice )
		{
			return GetMissingDataDebugTextString();
		}
		else
		{		
			bool bWasRightChoice = ( BiasChoice.Ambiguous == choice );
			
			return vignetteIntroString + 
				GenerateFuzzyFeedbackString
					( 
						bWasRightChoice,
						vignetteScore,
						questionAsker,
						masterString
					);
		}
	}
	
	private static string GenerateFuzzyFeedbackString(bool bWasRightChoice, VignetteScore vignetteScore, string questionAsker, string masterString)
	{
		string fuzzyFeedbackString = "";
		
		//Now get our intensity.
		FuzzyReasoningCognitiveBias.Intensity intensityAmb = FuzzyReasoningCognitiveBias.GetIntensityForTruth( vignetteScore.AmbigiousBiasScore );
		Debug.Log( "Ambigous Intesity: "+ intensityAmb);
		
		bool unbiasedSearch = false; //If we have a Complete or Intense bias in Ambigous, that means our search was balanced...
		if(intensityAmb == FuzzyReasoningCognitiveBias.Intensity.Complete || intensityAmb == FuzzyReasoningCognitiveBias.Intensity.Intense || intensityAmb == FuzzyReasoningCognitiveBias.Intensity.ExtremelyIntense) {
			unbiasedSearch = true;
		}
		
		//First add the string for whether the player answered correctly or not:
		if(bWasRightChoice) {
			fuzzyFeedbackString += String.Format(TXT_CORRECT_ANSWER, questionAsker);
		} else {
			fuzzyFeedbackString += String.Format(TXT_INCORRECT_ANSWER, questionAsker);
		}
		
		//Did we search around enough?
		if ( vignetteScore.PassedAlphaThreshold == false )
		{
			fuzzyFeedbackString += GetStringForInsufficientData( bWasRightChoice );
		}
		else
		{	
			//Now we add the "However, " or "Additionally, " texts.
			if(bWasRightChoice && unbiasedSearch) 
			{
				fuzzyFeedbackString += TXT_ADDITIONALLY;
			} 
			else if(!bWasRightChoice && !unbiasedSearch)
			{
				fuzzyFeedbackString += TXT_ADDITIONALLY;
			} else {
				fuzzyFeedbackString += TXT_HOWEVER;
			}
			
			fuzzyFeedbackString += TXT_GENERIC_SEARCH;
			
			fuzzyFeedbackString += GetSearchThroughness(intensityAmb);
			Debug.Log("RAW disconfirming: " + vignetteScore.RawDisconfirmingScore);
			Debug.Log("RAW confirming: " + vignetteScore.RawConfirmingScore);
			
			string[] searchAnalysisStrings = GetSearchAnalysisStrings(intensityAmb, vignetteScore.RawDisconfirmingScore, vignetteScore.RawConfirmingScore);
			fuzzyFeedbackString += String.Format(masterString, searchAnalysisStrings);
		}
		
		return fuzzyFeedbackString;
	}
	
	private static string GetSearchThroughness( FuzzyReasoningCognitiveBias.Intensity intensityAmb) 
	{
		
		switch(intensityAmb)
		{
			case FuzzyReasoningCognitiveBias.Intensity.Complete:
			case FuzzyReasoningCognitiveBias.Intensity.ExtremelyIntense:
				return TXT_SEARCH_PERFECT;
			case FuzzyReasoningCognitiveBias.Intensity.Intense:
				return TXT_SEARCH_MILD;
			case FuzzyReasoningCognitiveBias.Intensity.Significant:
				return TXT_SEARCH_SOMEWHAT;
			case FuzzyReasoningCognitiveBias.Intensity.Somewhat:
				return TXT_SEARCH_SIGNFICANT;
			case FuzzyReasoningCognitiveBias.Intensity.Mild:
				return TXT_SEARCH_INTENSE;
			case FuzzyReasoningCognitiveBias.Intensity.None:
			case FuzzyReasoningCognitiveBias.Intensity.AlmostNone:
				return TXT_SEARCH_COMPLETE;
			default:
				Debug.LogError( "Unknown/unsupported intensity encountered!" );
				return "";
		}
		
	}
	
	private static string[] GetSearchAnalysisStrings( FuzzyReasoningCognitiveBias.Intensity intensityAmb, int rawDisconfirming, int rawConfirming ) {
		string[] searchAnalysisStrings = new string[4];
		
		if(rawDisconfirming == rawConfirming)
		{
			searchAnalysisStrings[0] = TXT_EQUAL;
		}
		else if(rawConfirming > rawDisconfirming) 
		{
			searchAnalysisStrings[0] = TXT_CONFIRMING;
		}
		else 
		{
			searchAnalysisStrings[0] = TXT_DISCONFIRMING;
		}
		
		switch( intensityAmb )
		{
		case FuzzyReasoningCognitiveBias.Intensity.Complete:
		case FuzzyReasoningCognitiveBias.Intensity.ExtremelyIntense:
			searchAnalysisStrings[1] = TXT_BIAS_EQUAL_PART1;
			searchAnalysisStrings[2] = TXT_BIAS_EQUAL_PART2;
			break;
		case FuzzyReasoningCognitiveBias.Intensity.Intense:
			searchAnalysisStrings[1] = TXT_BIAS_LEVEL_PART1_MILD;
			searchAnalysisStrings[2] = TXT_BIAS_LEVEL_PART2_MILD;
			break;
		case FuzzyReasoningCognitiveBias.Intensity.Significant:
			searchAnalysisStrings[1] = TXT_BIAS_LEVEL_PART1_SOMEWHAT;
			searchAnalysisStrings[2] = TXT_BIAS_LEVEL_PART2_SOMEWHAT;
			break;
		case FuzzyReasoningCognitiveBias.Intensity.Somewhat:
			searchAnalysisStrings[1] = TXT_BIAS_LEVEL_PART1_SIGNIFICANT;
			searchAnalysisStrings[2] = TXT_BIAS_LEVEL_PART2_SIGNIFICANT;
			break;
		case FuzzyReasoningCognitiveBias.Intensity.Mild:
			searchAnalysisStrings[1] = TXT_BIAS_LEVEL_PART1_SIGNIFICANT;
			searchAnalysisStrings[2] = TXT_BIAS_LEVEL_PART2_SIGNIFICANT;
			break;
		case FuzzyReasoningCognitiveBias.Intensity.AlmostNone:
		case FuzzyReasoningCognitiveBias.Intensity.None:
			searchAnalysisStrings[1] = TXT_BIAS_LEVEL_PART1_COMPLETE;
			searchAnalysisStrings[2] = TXT_BIAS_LEVEL_PART2_COMPLETE;
			break;
		default:
			Debug.LogError( "Unknown/unsupported intensity encountered!" );
			break;
		}			
		
		return searchAnalysisStrings;
	}
	
	private static string GetStringForInsufficientData( bool bWasRightChoice )
	{
		string insufficientData = ""; 
		
		if ( bWasRightChoice )
		{
			insufficientData += TXT_BUT;
		}
		else
		{
			insufficientData += TXT_AND;
		}
		
		insufficientData += TXT_INSUFFICIENT_EVIDENCE;
		
		return insufficientData;
	}	
	
	public static string GetMissingDataDebugTextString() 
	{
		string retStr = "";
		retStr += TXT_NO_CHOICE_RECORDED;
		return retStr;
	}
	
	private static BiasChoice GetCorrectBiasChoice(Vignette.VignetteID vignette, SubjectData currentSubject) 
	{
		
		switch(vignette) 
		{
		case Vignette.VignetteID.E1vHomeOfficeSearch:
			return currentSubject.E1vHomeOfficeSearchAnswer;
		case Vignette.VignetteID.E1vTerrysApartmentSearch:
			return currentSubject.E1VTerrysApartmentSearchAnswer;
		case Vignette.VignetteID.E2vGPCOfficeSearch:
			return currentSubject.E2vGPCOfficeSearchAnswer;
		case Vignette.VignetteID.E3vChrisBriefcaseSearch:
			return currentSubject.E3vChrisBriefcaseSearchAnswer;
		}
		
		Debug.LogError("Non-Search Vignette passed!!");
		return BiasChoice.None;
	}
	
	private static VignetteScore GetVignetteScore(Vignette.VignetteID vignette, SubjectData currentSubject) {
		Debug.Log("Fetching Vignette Score for: " + vignette);
		
		switch(vignette)
		{
		case Vignette.VignetteID.E1vHomeOfficeSearch:
			return currentSubject.e1v3ExplorationScore;
		case Vignette.VignetteID.E1vTerrysApartmentSearch:
			return currentSubject.e1v1ExplorationScore;
		case Vignette.VignetteID.E2vGPCOfficeSearch:
			return currentSubject.e2v7ExplorationScore;
		case Vignette.VignetteID.E3vChrisBriefcaseSearch:
			return currentSubject.e3v10ExplorationScore;
		default:
			Debug.LogError("Incorrect Vignette Passed to Fuzzy Feedback!!");
			break;
		}
		
		return null;
	}
		

}

