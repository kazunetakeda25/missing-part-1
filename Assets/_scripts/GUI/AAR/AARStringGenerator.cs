using UnityEngine;
using System.Collections;

public class AARStringGenerator {
	
	private const string EVALUATION_GENERIC_STRING1 = 
		"[#555555]Your actions and answers in the game show that you committed {0} {1} the average player.  " +
		"\n\nSince you said you were {2} other players, you {3} [#FFFFFF]seem to " +
		"have a bias blind spot[#555555] when evaluating your {0}.\n\nRemember, to avoid the bias blind spot, " +
		"[#FFFFFF]try to imagine how biased another person would seem[#555555] to you if they answered or acted just as you did.";
	
	private const string CONFIRMATION_BIAS = "confirmation bias";
	private const string FAE_BIAS = "fundamental attribution error";
	private const string MORE_THAN = "[#FFFFFF]more than[#555555]";
	private const string LESS_THAN = "[#FFFFFF]less than[#555555]";
	private const string JUST_AS_MUCH_AS = "[#FFFFFF]just as much as[#555555]";
	private const string JUST_AS = "just as likely to commit bias as";
	private const string MORE = "more likely to commit bias than";
	private const string LESS = "less likely to commit bias than";
	private const string NO_LESS = "no less";
	private const string DO = "[#FFFFFF]do[#555555]";
	private const string DO_NOT = "[#FFFFFF]do not[#555555]";
	
	public static string GenerateBiasString(BiasType biastype, PlayerRating overallPerformance, PlayerRating selfReview) 
	{
		string[] customStrings = new string[4];
		
		customStrings[0] = GetBiasTypeString(biastype);
		customStrings[1] = GetGenericLongPerformanceString(overallPerformance);
		customStrings[2] = GetGenericSelfRatingString(selfReview);
		customStrings[3] = GetGenericBlindspotAssessmentString(overallPerformance, selfReview);
		
		return string.Format(EVALUATION_GENERIC_STRING1, customStrings);
	}
	
	#region Get Generic Strings
	
	private static string GetBiasTypeString(BiasType biasType)
	{
		switch(biasType)
		{
		case BiasType.ConfirmationBias:
			return CONFIRMATION_BIAS;
		case BiasType.FundamentalAttributionError:
			return FAE_BIAS;
		default:
			return CONFIRMATION_BIAS;
		}
	}
	
	private static string GetGenericLongPerformanceString(PlayerRating performanceRating) 
	{
		switch(performanceRating) 
		{
		case PlayerRating.AboveAverage:
			return LESS_THAN;
		case PlayerRating.Average:
			return JUST_AS_MUCH_AS;
		case PlayerRating.BelowAverage:
			return MORE_THAN;
		default:
			Debug.LogError("INCORRECT OVERALL PERFORMANCE VALUE!!");
			return "";
		}
	}
	
	private static string GetGenericShortPerformanceString(PlayerRating performanceRating) 
	{
		switch(performanceRating) 
		{
		case PlayerRating.AboveAverage:
			return MORE;
		case PlayerRating.Average:
			return JUST_AS;
		case PlayerRating.BelowAverage:
			return LESS;
		default:
			Debug.LogError("INCORRECT OVERALL PERFORMANCE VALUE!!");
			return "";
		}
	}
	
	private static string GetGenericSelfRatingString(PlayerRating selfRating)
	{
		Debug.Log("Getting Generic Self Rating: " + selfRating);
		switch(selfRating)
		{
		case PlayerRating.AboveAverage:
			return LESS;
		case PlayerRating.Average:
			return JUST_AS;
		case PlayerRating.BelowAverage:
			return MORE;
		case PlayerRating.NotYetEvaluated:
			Debug.Log("Not Yet Evaluated Passed - Are you debugging?  If not this is a serious bug.");
			break;
		}
		
		return "PLAYER SKIPPED SELF RATING";
	}
	
	private static string GetGenericBlindspotAssessmentString(PlayerRating performanceRating, PlayerRating selfRating)
	{
		Debug.Log("Player Self Rating: " + PlayerRanker.PlayerRatingToInt(selfRating));
		Debug.Log("Player Perfromance Rating: " + PlayerRanker.PlayerRatingToInt(performanceRating));
		if(PlayerRanker.PlayerRatingToInt(selfRating) > PlayerRanker.PlayerRatingToInt(performanceRating))
		{
			//Player has a higher opinon of themselves than they performed; they have a blindspot.
			return DO;
		}
		else
		{
			//Player has a lower or equal opinon of themselves than they performed at; no blind spot.
			return DO_NOT;
		}
	}
	
	#endregion
	
}
