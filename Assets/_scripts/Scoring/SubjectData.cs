using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

	
#region enums

public enum PlayerRating 
{
	AboveAverage,
	Average,
	BelowAverage,
	NotYetEvaluated
}

public enum Gender 
{
	unspecified, 
	male, 
	female
};

public enum GameFeedbackAnswer 
{
	StronglyDisagree,
	Disagree,
	Neutral,
	Agree,
	StronglyAgree
}

public enum QuestionType 
{
	Short, //Short Self Review
	Long, //Long Self Review
	Feedback //Episode Feedback
}

public enum BiasType
{
	ConfirmationBias,
	FundamentalAttributionError
}

public enum SelfReviewQuestion
{
	Conf1,
	Conf2,
	FAE1,
	FAE2
}

#endregion
[System.Serializable]
public class SubjectData
{
	[System.Serializable]
	public class EpisodeFeedback
	{
		public int nextEpisode;
		public int effort;
		public int challenge;
	}

	[System.Serializable]
	public class EpisodeSelfAssessment
	{
		public int confSelfReview1;
		public int confSelfReview2;
		public int faeSelfReview1;
		public int faeSelfReview2;
	}

	[System.Serializable]
	public class EpisodeBiasRating
	{
		public PlayerRating Conf = PlayerRating.NotYetEvaluated;
		public PlayerRating FAE = PlayerRating.NotYetEvaluated;
	}

	[System.Serializable]
	public class EpisodeBlindspot
	{
		public bool confBlindspot;
		public bool faeBlindspot;
	}

	[System.Serializable]
	public class EpisodeData
	{
		public Episode episode;
		public EpisodeBlindspot blindspot;
		public EpisodeSelfAssessment selfAssessment;
		public EpisodeBiasRating biasRating;
		public EpisodeFeedback episodeFeedback;
		public bool sentSelfReviewMessage;
	}
	
	public string subjectID = "";
	public Gender subjectGender = Gender.female;
	public string myMugName = "Debug Session";
	
	public BiasChoice E1VTerrysApartmentSearchAnswer = BiasChoice.None;
	public BiasChoice E1vNervousElevatorAnswer = BiasChoice.None;
	public BiasChoice E1vHomeOfficeSearchAnswer = BiasChoice.None;
	public BiasChoice E1vStephEvasiveAnswer = BiasChoice.None;
	public BiasChoice E2vSorianoNiceAnswer = BiasChoice.None;
	public BiasChoice E2vPressReleaseAnswer = BiasChoice.None;
	public BiasChoice E2vGPCOfficeSearchAnswer = BiasChoice.None;
	public BiasChoice E3vSuspiciousMenAnswer = BiasChoice.None;
	public BiasChoice E3vCoupleRomanceAnswer = BiasChoice.None;
	public BiasChoice E3vChrisBriefcaseSearchAnswer = BiasChoice.None;
	
	public string PlantHuggerChoice = "0000";
	public string CopyProtectionChoice = "0000";
	public string DeadlyTreatmentChoice = "0000";
	public string ToYourHealthChoice = "0000";
	
	public VignetteScore e1v1ExplorationScore = new VignetteScore();
	public VignetteScore e1v3ExplorationScore = new VignetteScore();
	public VignetteScore e2v7ExplorationScore = new VignetteScore();
	public VignetteScore e3v10ExplorationScore = new VignetteScore();
	
	public List<EpisodeData> episodeData = new List<EpisodeData>();
	
	public void Init() {
		InitEpisodeData(Episode.Episode1);
		InitEpisodeData(Episode.Episode2);
		InitEpisodeData(Episode.Episode3);
	}
	
	private void InitEpisodeData(Episode episode) {
		EpisodeData epData = new EpisodeData();
		epData.episode = episode;
		epData.blindspot = new EpisodeBlindspot();
		epData.selfAssessment = new EpisodeSelfAssessment();
		epData.biasRating = new EpisodeBiasRating();
		epData.episodeFeedback = new EpisodeFeedback();
		episodeData.Add(epData);
	}
	
	//These are all our public Score Saving Actions
	public void SaveE1V1Data() 
	{
		e1v1ExplorationScore = VignetteScoreTools.GetVignetteScoreReport(Vignette.VignetteID.E1vTerrysApartmentSearch);
		//VignetteScoreTools.DebugDumpVignetteDataToConsole(e1v1ExplorationScore);
		ReportSearchScore(Vignette.VignetteID.E1vTerrysApartmentSearch, e1v1ExplorationScore);
	}
	
	public void SaveE1V3Data() 
	{
		e1v3ExplorationScore = VignetteScoreTools.GetVignetteScoreReport(Vignette.VignetteID.E1vHomeOfficeSearch);
		//VignetteScoreTools.DebugDumpVignetteDataToConsole(e1v3ExplorationScore);
		ReportSearchScore(Vignette.VignetteID.E1vHomeOfficeSearch, e1v3ExplorationScore);
	}
	
	public void SaveE2V7Data() 
	{
		e2v7ExplorationScore = VignetteScoreTools.GetVignetteScoreReport(Vignette.VignetteID.E2vGPCOfficeSearch);
		//VignetteScoreTools.DebugDumpVignetteDataToConsole(e1v3ExplorationScore);
		ReportSearchScore(Vignette.VignetteID.E2vGPCOfficeSearch, e2v7ExplorationScore);
	}
	
	public void SaveE3V10Data() 
	{
		e3v10ExplorationScore = VignetteScoreTools.GetVignetteScoreReport(Vignette.VignetteID.E3vChrisBriefcaseSearch);
		//VignetteScoreTools.DebugDumpVignetteDataToConsole(e3v10ExplorationScore);
		ReportSearchScore(Vignette.VignetteID.E3vChrisBriefcaseSearch, e3v10ExplorationScore);
	}
	
	public PlayerRating GetPlayerPerformanceRating(Episode episode, BiasType biasType) {
		if(biasType == BiasType.ConfirmationBias)
			return GetEpisodeData(episode).biasRating.Conf;
		else
			return GetEpisodeData(episode).biasRating.FAE;
	}
	
	public EpisodeData GetEpisodeData(Episode episode) {
		for (int i = 0; i < episodeData.Count; i++) {
			if(episodeData[i].episode == episode)
				return episodeData[i];
		}
		
		Debug.LogError("Requested Episode Data not found.");
		return episodeData[0];
	}	
	
	public void ReportSimpleEpisode1Score() {
		Debug.Log("Sending Final Episode 1 Score");
		
		int faeQuestionScore = CalculateBiasInt(E1vNervousElevatorAnswer) + CalculateBiasInt(E1vStephEvasiveAnswer);
		int maxFAEQuestionScore = 2;
		int confirmationQuestionScore = CalculateBiasInt(E1vHomeOfficeSearchAnswer) + CalculateBiasInt(E1VTerrysApartmentSearchAnswer);
		int maxConfirmationQuestionScore = 2;
		
		float confirmationSearchingScore = (e1v3ExplorationScore.AmbigiousBiasScore + e1v1ExplorationScore.AmbigiousBiasScore) / 2;
		
		EpisodeData epData = GetEpisodeData(Episode.Episode1);
		
		ReportEvent.SimpleEpisodeScore
			(
				Episode.Episode1,
				faeQuestionScore,
				maxFAEQuestionScore,
				confirmationQuestionScore,
				maxConfirmationQuestionScore,
				confirmationSearchingScore,
				epData.blindspot.faeBlindspot,
				epData.blindspot.confBlindspot
			);
	}
	
	public void ReportSimpleEpisode2Score() {
		Debug.Log("Sending Final Episode 2 Score");
		
		int faeQuestionScore = CalculateBiasInt(E2vPressReleaseAnswer) + CalculateBiasInt(E2vSorianoNiceAnswer);
		int maxFAEQuestionScore = 2;
		int confirmationQuestionScore = CalculateBiasInt(E2vGPCOfficeSearchAnswer);
		int maxConfirmationQuestionScore = 1;
		
		float confirmationSearchingScore = (e2v7ExplorationScore.AmbigiousBiasScore);
		
		EpisodeData epData = GetEpisodeData(Episode.Episode2);
		
		ReportEvent.SimpleEpisodeScore
			(
				Episode.Episode2,
				faeQuestionScore,
				maxFAEQuestionScore,
				confirmationQuestionScore,
				maxConfirmationQuestionScore,
				confirmationSearchingScore,
				epData.blindspot.faeBlindspot,
				epData.blindspot.confBlindspot
			);
	}
	
	public void ReportSimpleEpisode3Score() {
		Debug.Log("Sending Final Episode 3 Score");
		
		int faeQuestionScore = CalculateBiasInt(E3vCoupleRomanceAnswer) + CalculateBiasInt(E3vSuspiciousMenAnswer);
		int maxFAEQuestionScore = 2;
		int confirmationQuestionScore = CalculateBiasInt(E3vChrisBriefcaseSearchAnswer);
		int maxConfirmationQuestionScore = 1;
		
		float confirmationSearchingScore = (e3v10ExplorationScore.AmbigiousBiasScore);
		
		EpisodeData epData = GetEpisodeData(Episode.Episode3);
		
		ReportEvent.SimpleEpisodeScore
			(
				Episode.Episode3,
				faeQuestionScore,
				maxFAEQuestionScore,
				confirmationQuestionScore,
				maxConfirmationQuestionScore,
				confirmationSearchingScore,
				epData.blindspot.faeBlindspot,
				epData.blindspot.confBlindspot
			);
	}
	
	public void ReportSelfReviewAnswer(Episode episode, SelfReviewQuestion question, int answer) {
		Debug.Log("Report Episode " + episode + " - Self Review Question: " + question + " - Answer: " + answer );
		switch(question) {
		case SelfReviewQuestion.Conf1:
			GetEpisodeData(episode).selfAssessment.confSelfReview1 = answer;
			break;
		case SelfReviewQuestion.Conf2:
			GetEpisodeData(episode).selfAssessment.confSelfReview2 = answer;
			break;
		case SelfReviewQuestion.FAE1:
			GetEpisodeData(episode).selfAssessment.faeSelfReview1 = answer;
			break;
		case SelfReviewQuestion.FAE2:
			GetEpisodeData(episode).selfAssessment.faeSelfReview2 = answer;
			OutputSelfReviewAnswers(episode);
			break;
		}
	}
	
	private void OutputSelfReviewAnswers(Episode episode) {
		EpisodeData data = GetEpisodeData(episode);
		ReportEvent.ReportSelfReviewScore(episode, data.selfAssessment.confSelfReview1, data.selfAssessment.confSelfReview2, data.selfAssessment.faeSelfReview1, data.selfAssessment.faeSelfReview2);
	}
	
	private void ReportSearchScore(Vignette.VignetteID vignette, VignetteScore score) 
	{
		ReportEvent.SearchVignetteComplete
			(
				vignette, 
				score.ConfirmingBiasScore, 
				score.DisconfirmingBiasScore, 
				score.AmbigiousBiasScore, 
				score.HighestMembership, 
				score.FinalPsychometricScore, 
				score.RawConfirmingScore, 
				score.MaxConfirmingScore, 
				score.RawDisconfirmingScore, 
				score.MaxDisconfirmingScore, 
				score.RawAmbigousScore, 
				score.MaxAmbigousScore
			);
	}
	
	private int CalculateBiasInt(BiasChoice biasChoice) 
	{
		//This is done for scoring/reporting purposes.
		if(biasChoice == BiasChoice.Ambiguous)
			return 0;
		else
			return 1;
	}
	
}
