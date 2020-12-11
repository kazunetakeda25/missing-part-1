using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicAnswerKey : MonoBehaviour {

	public const string PlantHuggerCorrectAnswer = "0100";
	public const string CopyProtectionAnswer = "0100";
	public const string DeadlyTreatmentCorrectAnswer = "0101";
	public const string ToYourHealthCorrectAnswer = "1010";
	
	public static bool IsAnswerCorrect(Vignette.VignetteID vignette, string answer)
	{
		
		switch(vignette)
		{
		case Vignette.VignetteID.E1vPlantHugger:
			if(answer == PlantHuggerCorrectAnswer)
				return true;
			break;
		case Vignette.VignetteID.E2vCopyProtection:
			if(answer == CopyProtectionAnswer)
				return true;
			break;
		case Vignette.VignetteID.E2vDeadlyTreatment:
			if(answer == DeadlyTreatmentCorrectAnswer)
				return true;
			break;
		case Vignette.VignetteID.E3vToYourHealth:
			if(answer == ToYourHealthCorrectAnswer)
				return true;
			break;
		}
		
		return false;
	}
	
	public static string[] GetLogicAnswer(Vignette.VignetteID vignette, string answer) 
	{
		Debug.Log("Looking for answer: " + answer + " for vignette " + vignette);
		List<string> chosenAnswers = new List<string>();
		
		switch(vignette)
		{
		case Vignette.VignetteID.E1vPlantHugger:
			
			if(answer == "1000")
				chosenAnswers.Add(PlayerResponseStore.Ep1MikeSolutionResponse1);
			
			if(answer == "0100")
				chosenAnswers.Add(PlayerResponseStore.Ep1MikeSolutionResponse2);

			if(answer == "0010")
				chosenAnswers.Add(PlayerResponseStore.Ep1MikeSolutionResponse3);
			
			break;
			
		case Vignette.VignetteID.E2vCopyProtection:
			
			if(answer == "1000")
				chosenAnswers.Add(PlayerResponseStore.Ep2StephCopyProtectionAnswer1);
			
			if(answer == "0100")
				chosenAnswers.Add(PlayerResponseStore.Ep2StephCopyProtectionAnswer2);

			if(answer == "0010")
				chosenAnswers.Add(PlayerResponseStore.Ep2StephCopyProtectionAnswer3);
			
			break;
		case Vignette.VignetteID.E2vDeadlyTreatment:

			if(answer == "1000")
				chosenAnswers.Add(PlayerResponseStore.Ep2StephDeadlyTreatmentAnswer1);
			
			if(answer == "0100")
				chosenAnswers.Add(PlayerResponseStore.Ep2StephDeadlyTreatmentAnswer2);

			if(answer == "0010")
				chosenAnswers.Add(PlayerResponseStore.Ep2StephDeadlyTreatmentAnswer3);
			
			if(answer == "0001")
				chosenAnswers.Add(PlayerResponseStore.Ep2StephDeadlyTreatmentAnswer4);
			
			
			break;
		case Vignette.VignetteID.E3vToYourHealth:
			
			if(answer == "1000")
				chosenAnswers.Add(PlayerResponseStore.Ep3WaitressToYourHealthAnswer1);
			
			if(answer == "0100")
				chosenAnswers.Add(PlayerResponseStore.Ep3WaitressToYourHealthAnswer2);

			if(answer == "0010")
				chosenAnswers.Add(PlayerResponseStore.Ep3WaitressToYourHealthAnswer3);
			
			if(answer == "0001")
				chosenAnswers.Add(PlayerResponseStore.Ep3WaitressToYourHealthAnswer4);					
			
			break;
		}
		
		if(chosenAnswers.Count == 0)
			chosenAnswers.Add("NO ANSWER FOUND!!");
		
		return chosenAnswers.ToArray();
	}
	
}
