using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ScreenType 
{
	InspectObject,
	Phone,
	FAQ,
	Map
}

public class ReportEvent : MonoBehaviour {

	private const string NAME_REPORT_PLAYER_INFO = "ReportPlayerInfo";
	private const string NAME_SIMPLE_EPISODE_SCORE = "SimpleEpisodeScore";
	private const string NAME_BLINDSPOT = "ReportBlindspot";
	private const string NAME_EPISODE_FEEDBACK = "EpisodeFeedback";
	private const string NAME_REPORT_CALCULATED_BIAS_SCORE = "ReportCalculatedBiasScore";
	private const string NAME_FAE_VIGNETTE_COMPLETE = "FAEVignetteComplete";
	private const string NAME_CONF_VIGNETTE_COMPLETE = "ConfVignetteComplete";
	private const string NAME_LOGIC_VIGNETTE_COMPELETE = "LogicVignetteComplete";	
	private const string NAME_SEARCH_VIGNETTE_COMPLETE = "SearchVignetteComplete";
	private const string NAME_REPORT_SELF_REVIEW = "ReportSelfReviewScore";
	private const string NAME_EPISODE_STARTED = "EpisodeStarted";
	
	public static void ReportPlayerInfo(Gender subjectGender, string sessionName, string myMugName) 
	{
		//Save TimeStamp
		SessionDataManager.GetSessionDataManager().StartSessionTimer();
		
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("subjectGender", subjectGender.ToString());
		values.Add("sessionName", sessionName);
		values.Add("myMugName", myMugName);
		string timeStr = "UTC: " + System.DateTime.Now.ToUniversalTime().ToString( "o" );
		values.Add("UTCTimestamp", timeStr);
		values.Add("First Person: ", BoolToString(Settings.IsFirstPerson()));
		values.Add("Hints On: ", BoolToString(Settings.HintsOn()));
		values.Add("Long Duration: ", BoolToString(Settings.IsLongDuration()));
		values.Add("Story Archive: ", BoolToString(Settings.StoryArchiveOn()));
		
		WriteSessionXML.WriteToXML(NAME_REPORT_PLAYER_INFO, values);
	}	
	
	public static void EpisodeFeedback(Episode episode, int engagement, int effort, int challenge)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("episode", episode.ToString());
		values.Add("engagement", engagement.ToString());
		values.Add("effort", effort.ToString());
		values.Add("eagerForNextEpisode", challenge.ToString());
		
		WriteSessionXML.WriteToXML(NAME_EPISODE_FEEDBACK, values);
	}
	
	public static void ReportCalculatedBiasScore(Episode episode, BiasType biasType, PlayerRating playerRating)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("episode", episode.ToString());
		values.Add("biasType", biasType.ToString());
		values.Add("playerRating", playerRating.ToString());
		
		WriteSessionXML.WriteToXML(NAME_REPORT_CALCULATED_BIAS_SCORE, values);
	}
	
	public static void ReportBlindspot(Episode episode, BiasType biasType, bool biased)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("episode", episode.ToString());
		values.Add("biasType", biasType.ToString());
		values.Add("biased", BoolToString(biased));
		
		WriteSessionXML.WriteToXML(NAME_REPORT_CALCULATED_BIAS_SCORE, values);
	}	
	
	public static void ConfVignetteComplete(Vignette.VignetteID vignette, BiasChoice answerIdentity)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("vignette", vignette.ToString());
		values.Add("answerIdentity", answerIdentity.ToString());
		
		WriteSessionXML.WriteToXML(NAME_CONF_VIGNETTE_COMPLETE, values);
	}
	
	public static void FAEVignetteComplete(Vignette.VignetteID vignette, BiasChoice answerIdentity)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("vignette", vignette.ToString());
		values.Add("answerIdentity", answerIdentity.ToString());
		
		WriteSessionXML.WriteToXML(NAME_FAE_VIGNETTE_COMPLETE, values);
	}
	
	public static void LogicVignetteComplete(Vignette.VignetteID vignette, string answer, bool correct)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("vignette", vignette.ToString());
		values.Add("answer", answer.ToString());
		values.Add("correct", BoolToString(correct));
		
		WriteSessionXML.WriteToXML(NAME_LOGIC_VIGNETTE_COMPELETE, values);
	}
	
	public static void SearchVignetteComplete
		(
		Vignette.VignetteID vignette, 
		float confirmingBiasScore,
		float disconfirmingBiasScore,
		float ambigousBiasScore,
		float highestMembership,
		float finalPsychometricScore,
		int rawConfirmingScore,
		int maxConfirmingScore,
		int rawDisconfirmingScore,
		int maxDisconfirmingScore,
		int rawAmbiguousScore,
		int maxAmbiguousScore
		)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("vignette", vignette.ToString());
		values.Add("ambigousBiasScore", ambigousBiasScore.ToString());
		values.Add("highestMembership", highestMembership.ToString());
		values.Add("finalPsychometricScore", finalPsychometricScore.ToString());
		values.Add("rawConfirmingScore", rawConfirmingScore.ToString());
		values.Add("maxConfirmingScore", maxConfirmingScore.ToString());
		values.Add("rawDisconfirmingScore", rawDisconfirmingScore.ToString());
		values.Add("maxDisconfirmingScore", maxDisconfirmingScore.ToString());
		values.Add("rawAmbiguousScore", rawAmbiguousScore.ToString());
		values.Add("maxAmbiguousScore", maxAmbiguousScore.ToString());
		
		WriteSessionXML.WriteToXML(NAME_SEARCH_VIGNETTE_COMPLETE, values);
	}
	
	public static void ReportSelfReviewScore
		(
			Episode episode, 
			int selfConfReviewA1,
			int selfConfReviewA2,
			int selfFAEReviewA1,
			int selfFAEReviewA2
		)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("episode", episode.ToString());
		values.Add("episodeSelfReviewConfAnswer1", selfConfReviewA1.ToString());
		values.Add("episodeSelfReviewConfAnswer2", selfConfReviewA2.ToString());
		values.Add("episodeSelfReviewFAEAnswer1", selfFAEReviewA1.ToString());
		values.Add("episodeSelfReviewFAEAnswer2", selfFAEReviewA2.ToString());
		
		WriteSessionXML.WriteToXML(NAME_REPORT_SELF_REVIEW, values);
	}
	
	public static void SimpleEpisodeScore
		(
			Episode episode, 
			int faeScore, 
			int faeMaxBias, 
			int confScore, 
			int confMaxScore, 
			float searchBiasScore, 
			bool faeBlindSpot, 
			bool confBlindspot
		)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("episode", episode.ToString());
		values.Add("faeScore", faeScore.ToString());
		values.Add("faeMaxBias", faeMaxBias.ToString());
		values.Add("confScore", confScore.ToString());
		values.Add("confMaxScore", confMaxScore.ToString());
		values.Add("searchBiasScore", searchBiasScore.ToString());
		values.Add("faeBlindspot", BoolToString(faeBlindSpot));
		values.Add("confBlindSpot", BoolToString(confBlindspot));
		
		WriteSessionXML.WriteToXML(NAME_SIMPLE_EPISODE_SCORE, values);
	}	
	
	public static void QuizResults(Quiz quiz, string answerChosen, int answerChosenIndex, bool correct, int numberOfTries)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("quiz", quiz.ToString());
		values.Add("answerChosen", answerChosen);
		values.Add("answerChosenIndex", answerChosenIndex.ToString());
		values.Add("answerCorrect", BoolToString(correct));
		values.Add("numberOfTriesForAnswer", numberOfTries.ToString());
		
		WriteSessionXML.WriteToXML("ReportSelfQuizResults", values);
	}
	
	public static void ReportSMS(SMS.SMSCharacter messager, string message)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("Sender", messager.ToString());
		values.Add("Message", message);
		
		WriteSessionXML.WriteToXML("ReportSMS", values);	
	}
	
	public static void TakePicture(string objectName)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("Object", objectName);
		
		WriteSessionXML.WriteToXML("TakePicture", values);	
	}
	
	public static void AttachmentsSent(string[] objectNames)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		for (int i = 0; i < objectNames.Length; i++) 
		{
			string objectNameX = objectNames[i] + i.ToString();
			values.Add(objectNameX, objectNames[i]);
		}
		
		WriteSessionXML.WriteToXML("AttachmentsSent", values);	
	}
	
	public static void InspectObject(string objectName)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("Object", objectName);
		
		WriteSessionXML.WriteToXML("InspectObject", values);	
	}
	
	public static void StartVignette(Vignette.VignetteID vignette)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("Vignette", vignette.ToString());
		
		WriteSessionXML.WriteToXML("VignetteStarted", values);	
	}
	
	public static void VignetteComplete(Vignette.VignetteID vignette)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("Vignette", vignette.ToString());
		
		WriteSessionXML.WriteToXML("VignetteComplete", values);	
	}
	
	public static void ScreenActivated(ScreenType screen)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("ScreenType", screen.ToString());
		
		WriteSessionXML.WriteToXML("ScreenActivated", values);	
	}
	
	public static void ScreenDeactivated(ScreenType screen)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("ScreenType", screen.ToString());
		
		WriteSessionXML.WriteToXML("ScreenDeactivated", values);	
	}	

	public static void PhoneActivated(SmartPhone.Mode mode)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("Mode", mode.ToString());
		
		WriteSessionXML.WriteToXML("PhoneActivated", values);	
	}
	
	public static void PhoneDeactivated(SmartPhone.Mode mode)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("Mode", mode.ToString());
		
		WriteSessionXML.WriteToXML("PhoneDeactivated", values);	
	}
	
	public static void HintSent(SimpleHint.PopUpType type, string body)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("Type", type.ToString());
		values.Add("Body", body	);
		
		WriteSessionXML.WriteToXML("Hint Sent", values);			
	}
	
	public static void MapUsed(InteractiveMap.SupportedRooms room)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("Destination", room.ToString());
		
		WriteSessionXML.WriteToXML("MapUsed", values);	
	}
	
	
	public static void RequestMoveToPoint(Vector3 destination, float speed)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("Destination", destination.ToString());
		values.Add("Speed", speed.ToString());
		
		WriteSessionXML.WriteToXML("RequestMoveToPoint", values);	
	}	
	
	public static void MoveToPointFinished(Vector3 destination)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("NewDestination", destination.ToString());
		
		WriteSessionXML.WriteToXML("MoveToPointFinished", values);	
	}		
	
	public static void CharacterBeginSpeaking(string jsonFile, string line)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("JsonCharacter", jsonFile.ToString());
		values.Add("Line", line.ToString());
		
		WriteSessionXML.WriteToXML("CharacterBeginSpeaking", values);	
	}		
	
	public static void CharacterEndSpeaking(string jsonFile)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("JsonCharacter", jsonFile.ToString());
		
		WriteSessionXML.WriteToXML("CharacterEndSpeaking", values);	
	}

	public static void EpisodeStarted(Episode episode)
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		
		values.Add("Episode", episode.ToString());
		
		WriteSessionXML.WriteToXML(NAME_EPISODE_STARTED, values);	
	}	
	
	private static string BoolToString(bool boolString) {
		if(boolString)
			return "true";
		
		return "false";
	}
}
