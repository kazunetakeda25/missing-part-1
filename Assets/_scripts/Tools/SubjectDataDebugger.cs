using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubjectDataDebugger : MonoBehaviour 
{	
	public const string TXT_NO_SUBJECT_FOUND = "No Subject Found";
	public const string E1V1_AMB_SCORE = "E1VTerrysApartmentSearch - Ambigious Search Score: ";
	public const string E1V3_AMB_SCORE = "E1VHomeOfficeSearch - Ambigious Search Score: ";
	public const string E2V7_AMB_SCORE = "E2VGPCOfficeSearch - Ambigious Search Score: ";
	public const string E3V10_AMB_SCORE = "E3VChrisBriefcaseSearch - Ambigious Search Score: ";
	public const string PLAYER_BIAS = "Player Bias";
	public const string PLAYER_RATING = "Player Rating";
	public const string LOGIC_PICK = "Logic Vignette";
	public const string PLAYER_MYMUG_ID = "My Mug ID: ";
	public const string PLAYER_MYMUG_AVATAR = "My Mug Avatar: ";
	public const string PLAYER_SUBJECTID = "Subject ID: ";
	public const string PLAYER_SUBJECT_GENDER = "Gender: ";
	public const string PLAYER_GENDER_MALE = "Male";
	public const string PLAYER_GENDER_FEMALE = "Female";
	
	private string[] BIASVIGNETTES = new string[10] 
	{
		"E1VTerrysApartmentSeach", 
		"E1VNervousElevator", 
		"E1VHomeOfficeSearch", 
		"E1VStephEvasive", 
		"E2VSorianoNice", 
		"E2VPressRelease", 
		"E2VGPCOfficeSearch", 
		"E3VSuspiciousMen", 
		"E3VCoupleRomance", 
		"E3VChrisBriefcaseSearch"
	};
	private string[] LOGIC_VIGNETTES = new string[4]
	{
		"E1vPlantHugger", 
		"E1vCopyProtection",
		"E1vDeadlyTreatment",
		"E1vToYourHealth"
	};
		
	private string[] EPISODES = new string[3] {"Episode 1", "Episode 2", "Episode 3"};
	
	private Dictionary<string, float> PlayerSearchScores = new Dictionary<string, float>();
	private Dictionary<string, PlayerRating> PlayerRatings = new Dictionary<string, PlayerRating>();
	private Dictionary<string, BiasChoice> PlayerBiasChoices = new Dictionary<string, BiasChoice>();
	private Dictionary<string, int> PlayerBiasScores = new Dictionary<string, int>();
	private Dictionary<string, string> LogicPicks = new Dictionary<string, string>();
	
	public bool on;
	public KeyCode toggleKey;
	public Color contentColor;
	public Color backgroundColor;
	
	private bool trackingSubject;
	private SubjectData subject;
	private float timer = 0;
	private string[] oldStrings;
	
	private void Awake()
	{
		//GameObject.DontDestroyOnLoad(this.gameObject);
		InitializeDictionaries();
	}
	
	private void InitializeDictionaries() 
	{
		PlayerSearchScores.Add(E1V1_AMB_SCORE, float.NaN);
		PlayerSearchScores.Add(E1V3_AMB_SCORE, float.NaN);
		PlayerSearchScores.Add(E2V7_AMB_SCORE, float.NaN);
		PlayerSearchScores.Add(E3V10_AMB_SCORE, float.NaN);		
		
		for (int i = 0; i < BIASVIGNETTES.Length; i++) 
		{
			PlayerBiasChoices.Add(PlayerBiasKey(i), BiasChoice.None);
		}
		
		for (int i = 0; i < EPISODES.Length; i++) 
		{
			PlayerRatings.Add(PlayerRatingKey(i, BiasType.ConfirmationBias), PlayerRating.NotYetEvaluated);
			PlayerRatings.Add(PlayerRatingKey(i, BiasType.FundamentalAttributionError), PlayerRating.NotYetEvaluated);
		}
		
		for (int i = 0; i < LOGIC_VIGNETTES.Length; i++) 
		{
			LogicPicks.Add(LogicVignetteKey(i), "XXXX");
		}
	}
	
	private string PlayerBiasKey(int vignette) {
		string key = PLAYER_BIAS + " " + BIASVIGNETTES[vignette];
		return key;
	}
	
	private string PlayerRatingKey(int episode, BiasType biasType) 
	{
		string key = PLAYER_RATING + " " + EPISODES[episode] + " " + biasType.ToString() + ": ";
		return key;
	}
	
	private string LogicVignetteKey(int vignette)
	{
		return LOGIC_PICK + " " + LOGIC_VIGNETTES[vignette];
	}
	
	private void OnGUI() 
	{
		if(trackingSubject) {
			GUIGenerator.CreateStandardLabels(GenerateDebugStrings(), contentColor, backgroundColor);
		} else if(on) {
			GUIGenerator.CreateStandardLabel(TXT_NO_SUBJECT_FOUND);
		}
	}
	
	private void Update () {
		timer += Time.deltaTime;
		if(on && !trackingSubject) {
			subject = SessionManager.GetSessionManager().currentSubject;

			if(subject == null) {
				GameObject.Destroy(this.gameObject);
				return;
			}

			if(subject != null)
				trackingSubject = true;
		}
		
		if(Application.isEditor || Debug.isDebugBuild)
			CheckHotKey();
	}
	
	private void CheckHotKey() {
		if(Input.GetKeyUp(toggleKey)) {
			ToggleDisplay();
		}
	}
	
	private void ToggleDisplay() {
		on = !on;
		
		if(on)
			ShowDebug();
		else
			HideDebug();
	}
	
	private string[] GenerateDebugStrings() {	
		if(timer < 1 && oldStrings != null)
			return oldStrings;
		
		FetchSubjectValues();
		List<string> debugStrings = new List<string>();
		
		debugStrings.Add(PLAYER_MYMUG_ID + subject.myMugName);
		
		if(SessionManager.GetSessionManager().vignetteManager.isExplorationVignette()) {
			//Debug.Log("Current Vignette: " + SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID);
			debugStrings.Add("Current Searching Score: " + VignetteScoreTools.GetVignetteScoreReport(SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID).AmbigiousBiasScore.ToString());
		}
			
		foreach(KeyValuePair<string, float> pair in PlayerSearchScores)
			debugStrings.Add(pair.Key.ToString() + pair.Value.ToString());
		
		foreach(KeyValuePair<string, PlayerRating> pair in PlayerRatings) 
			debugStrings.Add(pair.Key.ToString() + ": " + pair.Value.ToString());
		
		foreach(KeyValuePair<string, BiasChoice> pair in PlayerBiasChoices) 
			debugStrings.Add(pair.Key.ToString() + ": " + pair.Value.ToString());
		
		foreach(KeyValuePair<string, string> pair in LogicPicks)
			debugStrings.Add(pair.Key.ToString() + ": " + pair.Value);
		
		oldStrings = debugStrings.ToArray();
		
		return oldStrings;
	}
	
	private void FetchSubjectValues() {
		FetchSavedSearchScores();
		FetchPlayerRatings();
		FetchPlayerChoices();
		FetchLogicChoices();
	}
	
	private void FetchSavedSearchScores() {
		
		if(subject.e1v1ExplorationScore != null) 
		{
			PlayerSearchScores[E1V1_AMB_SCORE] = subject.e1v1ExplorationScore.AmbigiousBiasScore;
		}
		
		if(subject.e1v3ExplorationScore != null) 
		{
			PlayerSearchScores[E1V3_AMB_SCORE] = subject.e1v3ExplorationScore.AmbigiousBiasScore;
		}
		
		if(subject.e2v7ExplorationScore != null) 
		{
		PlayerSearchScores[E2V7_AMB_SCORE] = subject.e2v7ExplorationScore.AmbigiousBiasScore;
		}
		
		if(subject.e3v10ExplorationScore != null) 
		{
			PlayerSearchScores[E3V10_AMB_SCORE] = subject.e3v10ExplorationScore.AmbigiousBiasScore;
		}
	}
	
	private void FetchPlayerRatings() 
	{
		PlayerRatings[PlayerRatingKey(0, BiasType.ConfirmationBias)] = subject.GetPlayerPerformanceRating(Episode.Episode1, BiasType.ConfirmationBias);
		PlayerRatings[PlayerRatingKey(1, BiasType.ConfirmationBias)] = subject.GetPlayerPerformanceRating(Episode.Episode2, BiasType.ConfirmationBias);
		PlayerRatings[PlayerRatingKey(2, BiasType.ConfirmationBias)] = subject.GetPlayerPerformanceRating(Episode.Episode3, BiasType.ConfirmationBias);
		PlayerRatings[PlayerRatingKey(0, BiasType.FundamentalAttributionError)] = subject.GetPlayerPerformanceRating(Episode.Episode1, BiasType.FundamentalAttributionError);
		PlayerRatings[PlayerRatingKey(1, BiasType.FundamentalAttributionError)] = subject.GetPlayerPerformanceRating(Episode.Episode2, BiasType.FundamentalAttributionError);
		PlayerRatings[PlayerRatingKey(2, BiasType.FundamentalAttributionError)] = subject.GetPlayerPerformanceRating(Episode.Episode3, BiasType.FundamentalAttributionError);
	}
	
	private void FetchPlayerChoices() 
	{
		PlayerBiasChoices[PlayerBiasKey(0)] = subject.E1VTerrysApartmentSearchAnswer;
		PlayerBiasChoices[PlayerBiasKey(1)] = subject.E1vNervousElevatorAnswer;
		PlayerBiasChoices[PlayerBiasKey(2)] = subject.E1vHomeOfficeSearchAnswer;
		PlayerBiasChoices[PlayerBiasKey(3)] = subject.E1vStephEvasiveAnswer;
		PlayerBiasChoices[PlayerBiasKey(4)] = subject.E2vSorianoNiceAnswer;
		PlayerBiasChoices[PlayerBiasKey(5)] = subject.E2vPressReleaseAnswer;
		PlayerBiasChoices[PlayerBiasKey(6)] = subject.E2vGPCOfficeSearchAnswer;
		PlayerBiasChoices[PlayerBiasKey(7)] = subject.E3vSuspiciousMenAnswer;
		PlayerBiasChoices[PlayerBiasKey(8)] = subject.E3vCoupleRomanceAnswer;
		PlayerBiasChoices[PlayerBiasKey(9)] = subject.E3vChrisBriefcaseSearchAnswer;
	}
	
	private void FetchLogicChoices()
	{
		LogicPicks[LogicVignetteKey(0)] = subject.PlantHuggerChoice;
		LogicPicks[LogicVignetteKey(1)] = subject.CopyProtectionChoice;
		LogicPicks[LogicVignetteKey(2)] = subject.DeadlyTreatmentChoice;
		LogicPicks[LogicVignetteKey(3)] = subject.ToYourHealthChoice;
	}
	
	private void ShowDebug() {
		
	}
	
	private void HideDebug() {
		trackingSubject = false;
		subject = null;
	}
}
