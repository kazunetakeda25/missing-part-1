using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions 
{
	
	[ActionCategory(ActionCategory.GameLogic)]
		[HutongGames.PlayMaker.Tooltip("Report Score Events to ScoreKeeper")]
	
	public class ReportLogicAnswerComplete : FsmStateAction 
	{		
		
		public override	void OnEnter() 
		{
			SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
			Vignette.VignetteID currentVignette = SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID;
			
			switch(currentVignette) 
			{
			
			case Vignette.VignetteID.E1vPlantHugger:
				ReportEvent.LogicVignetteComplete(currentVignette, currentSubject.PlantHuggerChoice, LogicAnswerKey.IsAnswerCorrect(currentVignette, currentSubject.PlantHuggerChoice));
				break;
			
			case Vignette.VignetteID.E2vCopyProtection:
				ReportEvent.LogicVignetteComplete(currentVignette, currentSubject.CopyProtectionChoice, LogicAnswerKey.IsAnswerCorrect(currentVignette, currentSubject.CopyProtectionChoice));
				break;
				
			case Vignette.VignetteID.E2vDeadlyTreatment:
				ReportEvent.LogicVignetteComplete(currentVignette, currentSubject.DeadlyTreatmentChoice, LogicAnswerKey.IsAnswerCorrect(currentVignette, currentSubject.DeadlyTreatmentChoice));
				break;
				
			case Vignette.VignetteID.E3vToYourHealth:
				ReportEvent.LogicVignetteComplete(currentVignette, currentSubject.ToYourHealthChoice, LogicAnswerKey.IsAnswerCorrect(currentVignette, currentSubject.ToYourHealthChoice));
				break;
			
			default:
				Debug.LogWarning("Incorrect vignette passed - no question in this vignette: " + SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID);
				break;
			}
						
			Finish();
		}
				
	}
	
}
