using UnityEngine;
using System.Collections;

public class AnswerScoreTools {

	public static BiasChoice GetCorrectBiasChoice(Vignette.VignetteID vignette, SubjectData currentSubject) {
		
		BiasChoice playerChoice = BiasChoice.None;
		
		switch(vignette) {
		case Vignette.VignetteID.E1vTerrysApartmentSearch:
			playerChoice = currentSubject.E1VTerrysApartmentSearchAnswer;
			break;
		case Vignette.VignetteID.E1vNervousElevator:
			playerChoice = currentSubject.E1vNervousElevatorAnswer;
			break;
		case Vignette.VignetteID.E1vHomeOfficeSearch:
			playerChoice = currentSubject.E1vHomeOfficeSearchAnswer;
			break;
		case Vignette.VignetteID.E1vStephEvasive:
			playerChoice = currentSubject.E1vStephEvasiveAnswer;
			break;
		case Vignette.VignetteID.E2vSorianoNice:
			playerChoice = currentSubject.E2vSorianoNiceAnswer;
			break;
		case Vignette.VignetteID.E2vPressRelease:
			playerChoice = currentSubject.E2vPressReleaseAnswer;
			break;
		case Vignette.VignetteID.E2vGPCOfficeSearch:
			playerChoice = currentSubject.E2vGPCOfficeSearchAnswer;
			break;
		case Vignette.VignetteID.E3vSuspiciousMen:
			playerChoice = currentSubject.E3vSuspiciousMenAnswer;
			break;
		case Vignette.VignetteID.E3vCoupleRomance:
			playerChoice = currentSubject.E3vCoupleRomanceAnswer;
			break;
		case Vignette.VignetteID.E3vChrisBriefcaseSearch:
			playerChoice = currentSubject.E3vChrisBriefcaseSearchAnswer;
			break;
		}
		
		return playerChoice;
	}
	
}
