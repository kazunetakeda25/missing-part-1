using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerResponses {
	
	//WARNING: When adding new Responses, always add to the bottom of this list or Unity will slip ALL RESPONSES!
	
	public enum ResponseID {
		#region Episode 1
		Ep1ChrisTerrysFate,
		Ep1MikeWhatHappenedToTerry,
		Ep1MikeFAE1,
		Ep1MikeFAE1Confirmation,
		Ep1MikeConfEvidence,
		Ep1StephIntro,
		Ep1StephYouHaveFlashDrive,
		Ep1MikeFAE2,
		#endregion
		#region Episode 2
		Ep2StephFlashDrive,
		Ep2StephFAE1,
		Ep2StephFAE1Confirmation,
		Ep2StephVoiceMail,
		Ep2StephFAE2,
		Ep2StephFAE2Confirmation,
		Ep2StephGiveMeFlashDrive,
		Ep2StephConf,
		Ep2StephEvidence,
		Ep2StephBlackmail,
		Ep2StephTerryDanger,
		Ep2StephChrisFlashDrive,
		Ep2StephChrisAcquaintance,
		Ep2StephQuestions,
		#endregion
		#region Episode 3
		Ep3WaitressDrink,
		Ep3WaitressSuspiciousGuysWhat,
		Ep3WaitressSuspiciousGuys1,
		Ep3WaitressSuspiciousGuys2,
		Ep3WaitressFAE1,
		Ep3WaitressFAE2,
		Ep3WaitressFAE2Confirmation,
		Ep3StephDeception,
		Ep3StephConf,
		Ep3StephConfEvidence,
		Ep3TerryFlashDrive,
		Ep3TerryChrisGuilt,
		#endregion
		Ep1TerryNervous,
		Ep1ChrisCheckin,
		Ep1ChrisAttachments,
		Ep1PlayerSearchDone,
		Ep1ChrisAttachments3,
		Ep1ChrisAttachments2,
		Ep1ChrisAttachments1,
		Ep1MikePlantCrazy,
		Ep1MikePlantConcern,
		Ep1MikeSolution,
		Ep3ScotchTheory,
		Ep2CopyProtection,
		Ep2DeadlyTreatment
	}
	
	public static PlayerResponseStore.PlayerResponse FetchPlayerResponses(PlayerResponses.ResponseID responseID) {
		if(PlayerResponseStore.playerResponseStorage[responseID] == null) {
			Debug.LogError("Player Response: " + responseID + " is null!");
			return null;
		}
		
		return PlayerResponseStore.playerResponseStorage[responseID];
	}
}
