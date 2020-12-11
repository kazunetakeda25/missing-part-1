using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions 
{
	
	[ActionCategory(ActionCategory.GameLogic)]
		[HutongGames.PlayMaker.Tooltip("Report Score Events to ScoreKeeper")]
	
	public class ReportLogicAnswer : FsmStateAction 
	{		
		public int answerNo;
		public bool resetTo0;
		
		public override	void OnEnter() 
		{
			SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
			Vignette.VignetteID currentVignette = SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID;
			
			switch(currentVignette) 
			{
			
			case Vignette.VignetteID.E1vPlantHugger:
				currentSubject.PlantHuggerChoice = SetChar(currentSubject.PlantHuggerChoice);
				//ReportEvent.LogicVignetteComplete(currentVignette, currentSubject.PlantHuggerChoice, LogicAnswerKey.IsAnswerCorrect(currentVignette, currentSubject.PlantHuggerChoice));
				break;
			
			case Vignette.VignetteID.E2vCopyProtection:
				currentSubject.CopyProtectionChoice = SetChar(currentSubject.CopyProtectionChoice);
				//ReportEvent.LogicVignetteComplete(currentVignette, currentSubject.CopyProtectionChoice, LogicAnswerKey.IsAnswerCorrect(currentVignette, currentSubject.CopyProtectionChoice));
				break;
				
			case Vignette.VignetteID.E2vDeadlyTreatment:
				currentSubject.DeadlyTreatmentChoice = SetChar(currentSubject.DeadlyTreatmentChoice);
				Debug.Log("Saving Deadly Treatment: " + answerNo);
				Debug.Log("New Answer Key: " + currentSubject.DeadlyTreatmentChoice);
				//ReportEvent.LogicVignetteComplete(currentVignette, currentSubject.DeadlyTreatmentChoice, LogicAnswerKey.IsAnswerCorrect(currentVignette, currentSubject.DeadlyTreatmentChoice));
				break;
				
			case Vignette.VignetteID.E3vToYourHealth:
				currentSubject.ToYourHealthChoice = SetChar(currentSubject.ToYourHealthChoice);
				Debug.Log("Saving To Your Health : " + answerNo);
				Debug.Log("New Answer Key: " + currentSubject.ToYourHealthChoice);
				//ReportEvent.LogicVignetteComplete(currentVignette, currentSubject.ToYourHealthChoice, LogicAnswerKey.IsAnswerCorrect(currentVignette, currentSubject.ToYourHealthChoice));
				break;
			
			default:
				Debug.LogWarning("Incorrect vignette passed - no question in this vignette: " + SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID);
				break;
			}
						
			Finish();
		}
		
		private string SetChar(string savedAnswer) 
		{
			char[] choiceChars = savedAnswer.ToCharArray();
			if(choiceChars[answerNo - 1] == '0' && !resetTo0)
			 	choiceChars[answerNo - 1] = '1';	
			else
				choiceChars[answerNo - 1] = '0';
			
			return new string(choiceChars);
		}
				
	}
	
}
