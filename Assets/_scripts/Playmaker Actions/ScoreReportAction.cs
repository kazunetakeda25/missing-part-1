using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions 
{
	
	[ActionCategory(ActionCategory.GameLogic)]
		[HutongGames.PlayMaker.Tooltip("Report Score Events to ScoreKeeper")]
	
	public class ScoreReportAction : FsmStateAction 
	{		
		public BiasChoice answeredBiasChoice;
		
		private LevelManager levelManager;
		
		public override	void OnEnter() 
		{		
			levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
			SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
			switch(SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID) {
			case Vignette.VignetteID.E1vTerrysApartmentSearch:
				currentSubject.E1VTerrysApartmentSearchAnswer = answeredBiasChoice;
				SendConfEvent(Vignette.VignetteID.E1vTerrysApartmentSearch);
				break;
			case Vignette.VignetteID.E1vNervousElevator:
				currentSubject.E1vNervousElevatorAnswer = answeredBiasChoice;
				SendFAEEvent(Vignette.VignetteID.E1vNervousElevator);
				break;
			case Vignette.VignetteID.E1vHomeOfficeSearch:
				currentSubject.E1vHomeOfficeSearchAnswer = answeredBiasChoice;
				SendConfEvent(Vignette.VignetteID.E1vHomeOfficeSearch);
				break;
			case Vignette.VignetteID.E1vStephEvasive:
				currentSubject.E1vStephEvasiveAnswer = answeredBiasChoice;
				SendFAEEvent(Vignette.VignetteID.E1vStephEvasive);
				break;
			case Vignette.VignetteID.E2vSorianoNice:
				currentSubject.E2vSorianoNiceAnswer = answeredBiasChoice;
				SendFAEEvent(Vignette.VignetteID.E2vSorianoNice);
				break;
			case Vignette.VignetteID.E2vPressRelease:
				currentSubject.E2vPressReleaseAnswer = answeredBiasChoice;
				SendFAEEvent(Vignette.VignetteID.E2vPressRelease);
				break;
			case Vignette.VignetteID.E2vGPCOfficeSearch:
				currentSubject.E2vGPCOfficeSearchAnswer = answeredBiasChoice;
				SendConfEvent(Vignette.VignetteID.E2vGPCOfficeSearch);
				break;
			case Vignette.VignetteID.E3vSuspiciousMen:
				currentSubject.E3vSuspiciousMenAnswer = answeredBiasChoice;
				SendFAEEvent(Vignette.VignetteID.E3vSuspiciousMen);
				break;
			case Vignette.VignetteID.E3vCoupleRomance:
				currentSubject.E3vCoupleRomanceAnswer = answeredBiasChoice;
				SendFAEEvent(Vignette.VignetteID.E3vCoupleRomance);
				break;
			case Vignette.VignetteID.E3vChrisBriefcaseSearch:
				currentSubject.E3vChrisBriefcaseSearchAnswer = answeredBiasChoice;
				SendConfEvent(Vignette.VignetteID.E3vChrisBriefcaseSearch);
				break;
			default:
				Debug.LogError("incorrect vignette passed - no question in this vignette: " + SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID);
				break;
			}
			Debug.Log("Saving Score for vignette: " + SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID.ToString());
			Finish();
		}
		
		private void SendFAEEvent(Vignette.VignetteID vignette) {
			ReportEvent.FAEVignetteComplete(vignette, answeredBiasChoice);
		}
		
		private void SendConfEvent(Vignette.VignetteID vignette) {
			ReportEvent.ConfVignetteComplete(vignette, answeredBiasChoice);
		}		
				
	}
	
}
