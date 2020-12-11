using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerRanker {

	//These methods can be used by anyone to calculate more complicated scoring for Player Ratings
	public static PlayerRating AveragePlayerRatings(PlayerRating[] playerRatings) {
		float[] playerRatingFloats = new float[playerRatings.Length];
		for (int i = 0; i < playerRatings.Length; i++) {
			playerRatingFloats[i] = PlayerRatingToInt(playerRatings[i]);
		}
		
		//We Round The score to the floor just to make sure players aren't too cocky.
		int playerRatingAverageInt =  Mathf.FloorToInt(playerRatingFloats.Average());
		return IntToPlayerRating(playerRatingAverageInt);
	}
	
	public static PlayerRating IntToPlayerRating(int playerRatingInt)
	{
		PlayerRating playerRatingConversion = PlayerRating.NotYetEvaluated;
		
		switch(playerRatingInt) {
		case 0:
			playerRatingConversion = PlayerRating.NotYetEvaluated;
			break;
		case 1:
			playerRatingConversion = PlayerRating.BelowAverage;
			break;
		case 2:
			playerRatingConversion = PlayerRating.Average;
			break;
		case 3:
			playerRatingConversion = PlayerRating.AboveAverage;
			break;
		default:
			Debug.LogError("Incorrect Int Assigned to Player Rating");
			break;
		}
		
		return playerRatingConversion;
	}
	
	public static int PlayerRatingToInt(PlayerRating playerRating) 
	{
		int playerRatingInt = 0;
		
		switch(playerRating) {
		case PlayerRating.AboveAverage:
			playerRatingInt = 3;
			break;
		case PlayerRating.Average:
			playerRatingInt = 2;
			break;
		case PlayerRating.BelowAverage:
			playerRatingInt = 1;
			break;
		case PlayerRating.NotYetEvaluated:
			playerRatingInt = 0;
			break;
		default:
			Debug.LogError("Incorrect Player Rating Passed");
			break;
		}
		
		return playerRatingInt;
	}
}
