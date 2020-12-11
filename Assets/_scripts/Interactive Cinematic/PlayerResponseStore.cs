using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerResponseStore {
	
	//backCODE: Denick
	//To make small changes to the player responses, edit the constants below.  
	//If you change the number of responses, you need to update the appropiate dictionary entry below.
	
	#region Episode 1 Player Responses
	
	public const string Ep1ChrisCheckInResponse1 = "Got it.";
	public const string Ep1ChrisCheckInRespEvent1 = "PlayerRespEp1GotIt";
	
	public const string Ep1ChrisTerrysFateResponse1 = "I need more time.";
	public const string Ep1ChrisTerrysFateResponse2 = "Terry was taken from the apartment by force.";
	public const string Ep1ChrisTerrysFateResponse3 = "Terry left the apartment, but was not taken by force.";
	public const string Ep1ChrisTerrysFateResponse4 = "Terry isn't here, but I can't tell where she went.";
	public const string Ep1ChrisTerrysFateRespEvent1 = "PlayerRespEp1Conf1MoreTime";
	public const string Ep1ChrisTerrysFateRespEvent2 = "PlayerRespEp1Conf1Confirming";
	public const string Ep1ChrisTerrysFateRespEvent3 = "PlayerRespEp1Conf1Disconfirming";
	public const string Ep1ChrisTerrysFateRespEvent4 = "PlayerRespEp1Conf1Neutral";

	public const string Ep1ChrisAttachments3Response = "Attach three photographs.";
	public const string Ep1ChrisAttachments3RespEvent = "PHOTO_ALBUM";

	public const string Ep1ChrisAttachments2Response = "Attach two more photographs.";
	public const string Ep1ChrisAttachments2RespEvent = "PHOTO_ALBUM";	
	
	public const string Ep1ChrisAttachments1Response = "Attach one more photograph.";
	public const string Ep1ChrisAttachments1RespEvent = "PHOTO_ALBUM";	
	
	public const string Ep1ChrisAttachmentsResponse1 = "Send the Photographs to Chris.";
	public const string Ep1ChrisAttachmentsRespEvent1 = "PlayerRespEp1AttachmentsSent";
	
	public const string Ep1PlayerSearchDoneResponse1 = "TEXT: OK, I'm done searching.";
	public const string Ep1PlayerSearchDoneRespEvent1 = "PlayerRespEp1DoneSearchingApt";

	public const string Ep1MikeFAE1Response1 = "I'm not sure, I sent Chris some pictures.";
	public const string Ep1MikeFAE1Response2 = "I think some guys grabbed her.";
	public const string Ep1MikeFAE1Response3 = "I think she's just out running an errand.";
	public const string Ep1MikeFAE1RespEvent1 = "PlayerRespEp1MikeSentChrisPics";
	public const string Ep1MikeFAE1RespEvent2 = "PlayerRespEp1MikeGrabbedHer";
	public const string Ep1MikeFAE1RespEvent3 = "PlayerRespEp1MikeRunningErrand";
	
	public const string Ep1MikeFAE1ConfirmationResponse1 = "Yes, I'm sure.";
	public const string Ep1MikeFAE1ConfirmationResponse2 = "I'm not sure.";
	public const string Ep1MikeFAE1ConfirmationRespEvent1 = "PlayerRespEp1MikeFAE1ConfirmationYes";
	public const string Ep1MikeFAE1ConfirmationRespEvent2 = "PlayerRespEp1MikeFAE1ConfirmationNo";
	
	public const string Ep1MikeConfEvidenceResponse1 = "Terry has a financial problem.";
	public const string Ep1MikeConfEvidenceResponse2 = "Terry has a romantic problem.";
	public const string Ep1MikeConfEvidenceResponse3 = "I'm not sure.";
	public const string Ep1MikeConfEvidenceRespEvent1 = "PlayerRespEp1ConfConfirming";
	public const string Ep1MikeConfEvidenceRespEvent2 = "PlayerRespEp1ConfDisconfirming";
	public const string Ep1MikeConfEvidenceRespEvent3 = "PlayerRespEp1ConfNeutral";
	
	public const string Ep1StephIntroResponse1 = "I'm Terry's neighbor. Who is this?";
	public const string Ep1StephIntroRespEvent1 = "PlayerRespEp1StephIntro";
	
	public const string Ep1StephYouHaveFlashDriveResponse1 = "Do you know where Terry is?";
	public const string Ep1StephYouHaveFlashDriveResponse2 = "Do you know what the flash drive is for?";
	public const string Ep1StephYouHaveFlashDriveRespEvent1 = "PlayerRespEp1StephTerry";
	public const string Ep1StephYouHaveFlashDriveRespEvent2 = "PlayerRespEp1StephFlashDrive";
	
	public const string Ep1MikeFAE2Response1 = "I agree, she is an evasive person!";
	public const string Ep1MikeFAE2Response2 = "I don't think she is an evasive person.";
	public const string Ep1MikeFAE2Response3 = "I'm not sure.";
	public const string Ep1MikeFAE2RespEvent1 = "PlayerRespEp1FAE2Confirming";
	public const string Ep1MikeFAE2RespEvent2 = "PlayerRespEp1FAE2Disconfirming";
	public const string Ep1MikeFAE2RespEvent3 = "PlayerRespEp1FAE2Neutral";
	
	public const string Ep1MikeNervousResponse1 = "She is a nervous person.";
	public const string Ep1MikeNervousResponse2 = "No, she's not a nervous person.";
	public const string Ep1MikeNervousResponse3 = "I can't tell.";
	public const string Ep1MikeNervousRespEvent1 = "PlayerRespEp1NervousConfirming";
	public const string Ep1MikeNervousRespEvent2 = "PlayerRespEp1NervouceDisconfirming";
	public const string Ep1MikeNervousRespEvent3 = "PlayerRespEp1NervousNeutral";
	
	public const string Ep1MikePlantCrazyResponse1 = "What does that have to do with Terry's disappearance?";
	public const string Ep1MikePlantCrazyResponse2 = "Maybe I should go?";
	public const string Ep1MikePlantCrazyRespEvent1 = "PlayerRespMikePlantTerry";
	public const string Ep1MikePlantCrazyRespEvent2 = "PlayerRespMikePlantGo";

	public const string Ep1MikePlantConcernResponse1 = "Why are you so concerned about this?";
	public const string Ep1MikePlantConcernResponse2 = "Why do you think the plants are dying?";
	public const string Ep1MikePlantConcernRespEvent1 = "PlayerRespMikePlantConcerned";
	public const string Ep1MikePlantConcernRespEvent2 = "PlayerRespMikePlantDying";
	
	public const string Ep1MikeSolutionResponse1 = "Water them once a week, keep them in a south-facing room with open windows";
	public const string Ep1MikeSolutionResponse2 = "Water every day, keep the plants in a south-facing room with the windows open";
	public const string Ep1MikeSolutionResponse3 = "Water the plants once a week, move the plants to the north side with the windows closed";
	public const string Ep1MikeSolutionResponse4 = "Can you run that by me again?";
	public const string Ep1MikeSolutionRespEvent1 = "PlayerRespMikePlantSolution1";
	public const string Ep1MikeSolutionRespEvent2 = "PlayerRespMikePlantSolution2";
	public const string Ep1MikeSolutionRespEvent3 = "PlayerRespMikePlantSolution3";
	public const string Ep1MikeSolutionRespEvent4 = "RepeatPlantHugger";
	
	#endregion
	
	#region Episode 2 Player Responses
	
	public const string Ep2StephFlashDriveResponse1 = "I have it right here.";
	public const string Ep2StephFlashDriveResponse2 = "What did Terry's voicemail say about the drive?";
	public const string Ep2StephFlashDriveRespEvent1 = "PlayerRespEp2StephFlashDrive1";
	public const string Ep2StephFlashDriveRespEvent2 = "PlayerRespEp2StephFlashDrive2";

	public const string Ep2StephFAE1Response1 = "He does seem like a nice guy.";
	public const string Ep2StephFAE1Response2 = "He doesn't look too nice to me.";
	public const string Ep2StephFAE1Response3 = "I'm not sure.";
	public const string Ep2StephFAE1RespEvent1 = "PlayerRespEp2StephFAE1Confirming";
	public const string Ep2StephFAE1RespEvent2 = "PlayerRespEp2StephFAE1Disconfirming";
	public const string Ep2StephFAE1RespEvent3 = "PlayerRespEp2StephFAE1Neutral";
	
	public const string Ep2StephFAE1ConfirmationResponse1 = "I'm confident.";
	public const string Ep2StephFAE1ConfirmationResponse2 = "I'm not confident.";
	public const string Ep2StephFAE1ConfirmationResponse3 = "I'm really not sure.";
	public const string Ep2StephFAE1ConfirmationRespEvent1 = "PlayerRespEp2StephFAE1ConfirmConfident";
	public const string Ep2StephFAE1ConfirmationRespEvent2 = "PlayerRespEp2StephFAE1ConfirmNotConfident";
	public const string Ep2StephFAE1ConfirmationRespEvent3 = "PlayerRespEp2StephFAE1ConfirmUnsure";
	
	public const string Ep2StephVoiceMailResponse1 = "Can you play that again?";
	public const string Ep2StephVoiceMailResponse2 = "What is Exceleron?";
	public const string Ep2StephVoiceMailResponse3 = "Do you know what's on the flash drive?";
	public const string Ep2StephVoiceMailRespEvent1 = "PlayerRespEp2StephPlayAgain";
	public const string Ep2StephVoiceMailRespEvent2 = "PlayerRespEp2StephWhatExceleron";
	public const string Ep2StephVoiceMailRespEvent3 = "PlayerRespEp2StephOnFlashDrive";
	
	public const string Ep2StephFAE2Response1 = "Terry doesn't believe the company was responsible.";
	public const string Ep2StephFAE2Response2 = "I think Terry blames the company for the Exceleron scandal.";
	public const string Ep2StephFAE2Response3 = "I can't tell from this letter.";
	public const string Ep2StephFAE2RespEvent1 = "PlayerRespEp2StephFAE2Confirming";
	public const string Ep2StephFAE2RespEvent2 = "PlayerRespEp2StephFAE2Disconfirming";
	public const string Ep2StephFAE2RespEvent3 = "PlayerRespEp2StephFAE2Neutral";
	
	public const string Ep2StephFAE2ConfirmationResponse1 = "I'm confident.";
	public const string Ep2StephFAE2ConfirmationResponse2 = "I'm not confident.";
	public const string Ep2StephFAE2ConfirmationResponse3 = "I'm really not sure.";
	public const string Ep2StephFAE2ConfirmationRespEvent1 = "PlayerRespEp2StephFAE2ConfirmConfident";
	public const string Ep2StephFAE2ConfirmationRespEvent2 = "PlayerRespEp2StephFAE2ConfirmNotConfident";
	public const string Ep2StephFAE2ConfirmationRespEvent3 = "PlayerRespEp2StephFAE2ConfirmUnsure";
	
	public const string Ep2StephGiveMeFlashDriveResponse1 = "I'm not sure you should have it.";
	public const string Ep2StephGiveMeFlashDriveResponse2 = "Here, take the flash drive.";
	public const string Ep2StephGiveMeFlashDriveRespEvent1 = "PlayerRespEp2StephNoTrust";
	public const string Ep2StephGiveMeFlashDriveRespEvent2 = "PlayerRespEp2StephTakeIt";
	
	public const string Ep2StephCopyProtectionAnswer1 = "Order from Rocket Copy, and replace Fast Copy and Warp Speed Copy with two new copy companies";
	public const string Ep2StephCopyProtectionAnswer2 = "Order from Fast Copy and Warp Speed Copy only";
	public const string Ep2StephCopyProtectionAnswer3 = "Replace all three: Rocket Copy, Fast Copy, and Warp Speed Copy";
	public const string Ep2StephCopyProtectionAnswer4 = "Can you run that by me again?";
	public const string Ep2StephCopyProtectionRespEvent1 = "PlayerRespEp2CopyProtectionAnswer1";
	public const string Ep2StephCopyProtectionRespEvent2 = "PlayerRespEp2CopyProtectionAnswer2";
	public const string Ep2StephCopyProtectionRespEvent3 = "PlayerRespEp2CopyProtectionAnswer3";
	public const string Ep2StephCopyProtectionRespEvent4 = "PlayerRespEp2CopyProtectionAnswerRepeat";
	
	public const string Ep2StephConf1Response1 = "Terry is sharing competitive information.";
	public const string Ep2StephConf1Response2 = "Terry violated company policy on expense reports.";
	public const string Ep2StephConf1Response3 = "I can't tell based on this evidence.";
	public const string Ep2StephConf1RespEvent1 = "PlayerRespEp2StephConf1Confirming";
	public const string Ep2StephConf1RespEvent2 = "PlayerRespEp2StephConf1Disconfirming";
	public const string Ep2StephConf1RespEvent3 = "PlayerRespEp2StephConf1Neutral";
	
	public const string Ep2StephDeadlyTreatmentAnswer1 = "1) Look at color of pills inside box marked 'ship to Africa'";
	public const string Ep2StephDeadlyTreatmentAnswer2 = "2) Look at color of pills inside box marked 'ship to Denver'";
	public const string Ep2StephDeadlyTreatmentAnswer3 = "3) Look at shipping label on the box containing white Exceleron pills";
	public const string Ep2StephDeadlyTreatmentAnswer4 = "4) Look at shipping label on the box containing green Exceleron pills";
	public const string Ep2StephDeadlyTreatmentAnswer6 = "I'm done.";
	public const string Ep2StephDeadlyTreatmentAnswer5 = "Can you run that by me again?";
	public const string Ep2StephDeadlyTreatmentRespEvent1 = "PlayerRespEp2DeadlyTreatmentAnswer1";
	public const string Ep2StephDeadlyTreatmentRespEvent2 = "PlayerRespEp2DeadlyTreatmentAnswer2";
	public const string Ep2StephDeadlyTreatmentRespEvent3 = "PlayerRespEp2DeadlyTreatmentAnswer3";
	public const string Ep2StephDeadlyTreatmentRespEvent4 = "PlayerRespEp2DeadlyTreatmentAnswer4";
	public const string Ep2StephDeadlyTreatmentRespEvent6 = "PlayerRespEp2DeadlyTreatmentAnswerDone";
	public const string Ep2StephDeadlyTreatmentRespEvent5 = "PlayerRespEp2DeadlyTreatmentAnswerRepeat";
	
	public const string Ep2StephEvidenceResponse1 = "Take a look at this photo of Terry and her mother...";
	public const string Ep2StephEvidenceResponse2 = "Look at this photo of the kids who took Exceleron...";
	public const string Ep2StephEvidenceResponse3 = "What do you think, Stephanie?";
	public const string Ep2StephEvidenceRespEvent1 = "PlayerRespEp2StephTerryMother";
	public const string Ep2StephEvidenceRespEvent2 = "PlayerRespEp2StephKidsExceleron";
	public const string Ep2StephEvidenceRespEvent3 = "PlayerRespEp2StephWhatDoYouThink";
	
	public const string Ep2StephBlackmailResponse1 = "Do you think the blackmail note is fake?";
	public const string Ep2StephBlackmailResponse2 = "Where did Terry get the information for blackmail?";
	public const string Ep2StephBlackmailRespEvent1 = "PlayerRespEp2StephFakeNote";
	public const string Ep2StephBlackmailRespEvent2 = "PlayerRespEp2StephBlackmailSource";
	
	public const string Ep2StephTerryDangerResponse1 = "Who do you think is involved in this?";
	public const string Ep2StephTerryDangerResponse2 = "Do you think Terry is in danger?";
	public const string Ep2StephTerryDangerRespEvent1 = "PlayerRespEp2StephOthersInvolved";
	public const string Ep2StephTerryDangerRespEvent2 = "PlayerRespEp2StephDanger";
	
	public const string Ep2StephChrisFlashDriveResponse1 = "How does Chris know about Terry's flash drive?";
	public const string Ep2StephChrisFlashDriveRespEvent1 = "PlayerRespEp2StephChrisKnows";
	
	public const string Ep2StephChrisAcquaintanceResponse1 = "I know Chris very well.";
	public const string Ep2StephChrisAcquaintanceResponse2 = "I don't know Chris very well.";
	public const string Ep2StephChrisAcquaintanceRespEvent1 = "PlayerRespEp2StephChrisWell";
	public const string Ep2StephChrisAcquaintanceRespEvent2 = "PlayerRespEp2StephChrisNotWell";
	
	public const string Ep2StephQuestionsResponse1 = "Who is Soriano?";
	public const string Ep2StephQuestionsResponse2 = "What does Terry do for GPC?";
	public const string Ep2StephQuestionsResponse3 = "What does Chris do for GPC?";
	public const string Ep2StephQuestionsResponse4 = "What do you do for GPC?";
	public const string Ep2StephQuestionsResponse5 = "Tell me more about Exceleron.";
	public const string Ep2StephQuestionsResponse6 = "Tell me more about the FPD Inhaler.";
	public const string Ep2StephQuestionsResponse7 = "What are you celebrating out there?";
	public const string Ep2StephQuestionsResponse8 = "Where is Terry?";
	public const string Ep2StephQuestionsResponse9 = "That's all I need; I'm off to meet Chris.";
	public const string Ep2StephQuestionsRespEvent1 = "PlayerRespEp2StephSoriano";
	public const string Ep2StephQuestionsRespEvent2 = "PlayerRespEp2StephTerryGPC";
	public const string Ep2StephQuestionsRespEvent3 = "PlayerRespEp2StephChrisGPC";
	public const string Ep2StephQuestionsRespEvent4 = "PlayerRespEp2StephStephGPC";
	public const string Ep2StephQuestionsRespEvent5 = "PlayerRespEp2StephExceleron";
	public const string Ep2StephQuestionsRespEvent6 = "PlayerRespEp2StephFPDInhaler";
	public const string Ep2StephQuestionsRespEvent7 = "PlayerRespEp2StephCelebration";
	public const string Ep2StephQuestionsRespEvent8 = "PlayerRespEp2StephWhereTerry";
	public const string Ep2StephQuestionsRespEvent9 = "PlayerRespEp2StephEndAct2";
	
	#endregion
	
	#region Episode 3 Player responses
	
	public const string Ep3WaitressDrinkResponse1 = "The sampler sounds good.";
	public const string Ep3WaitressDrinkResponse2 = "What's the favor?";
	public const string Ep3WaitressRespEvent1 = "PlayerRespSampler";
	public const string Ep3WaitressRespEvent2 = "PlayerRespFavor";
	
	public const string Ep3WaitressToYourHealthAnswer1 = "A sample from County Moray, and a drink customers describe as \"normal.\"";
	public const string Ep3WaitressToYourHealthAnswer2 = "A sample from County Angus, and a drink customers describe as \"normal.\"";
	public const string Ep3WaitressToYourHealthAnswer3 = "A sample from County Moray, and a drink customers describe as \"mushroomy.\"";
	public const string Ep3WaitressToYourHealthAnswer4 = "A sample from County Angus, and a drink customers describe as \"mushroomy.\"";
	public const string Ep3WaitressToYourHealthAnswer5 = "A drink customers describe as \"normal,\" and a drink customers describe as \"mushroomy.\"";
	public const string Ep3WaitressToYourHealthAnswer6 = "A sample from County Moray, and a sample from County Angus.";
	public const string Ep3WaitressToYourHealthAnswer7 = "Can you run that by me again?";
	public const string Ep3WaitressRespLogicAnswer1 = "PlayerRespAnswer1";
	public const string Ep3WaitressRespLogicAnswer2 = "PlayerRespAnswer2";
	public const string Ep3WaitressRespLogicAnswer3 = "PlayerRespAnswer3";
	public const string Ep3WaitressRespLogicAnswer4 = "PlayerRespAnswer4";
	public const string Ep3WaitressRespLogicAnswer5 = "PlayerRespAnswer5";
	public const string Ep3WaitressRespLogicAnswer6 = "PlayerRespAnswer6";
	public const string Ep3WaitressRespLogicAnswer7 = "PlayerRespAnswer7";
	
	public const string Ep3WaitressSuspiciousGuysWhatResponse1 = "What else did they say?";
	public const string Ep3WaitressSuspiciousGuysWhatResponse2 = "What else did they do?";
	public const string Ep3WaitressSuspiciousGuysWhatRespEvent1 = "PlayerRespWhatTheyDid";
	public const string Ep3WaitressSuspiciousGuysWhatRespEvent2 = "PlayerRespWhatTheySaid";
	
	public const string Ep3WaitressSuspiciousGuys1Response1 = "Were they threatening when you talked to them?";
	public const string Ep3WaitressSuspiciousGuys1Response2 = "Does it seem like they are here for something work-related?";
	public const string Ep3WaitressSuspiciousGuys1Response3 = "Have you seen these guys before?";
	public const string Ep3WaitressSuspiciousGuys1Response4 = "Did they ask for someone?";
	public const string Ep3WaitressSuspiciousGuys1Response5 = "Could they have just been talking business?";
	public const string Ep3WaitressSuspiciousGuys1Response6 = "Did you see any weapons?";
	public const string Ep3WaitressSuspiciousGuys1Response7 = "Ask something else.";
	public const string Ep3WaitressSuspiciousGuys1Response8 = "I've heard enough.";
	public const string Ep3WaitressSuspiciousGuys1RespEvent1 = "PlayerRespWaitressThreatening";
	public const string Ep3WaitressSuspiciousGuys1RespEvent2 = "PlayerRespWaitressJobs";
	public const string Ep3WaitressSuspiciousGuys1RespEvent3 = "PlayerRespWaitressSeenGuys";
	public const string Ep3WaitressSuspiciousGuys1RespEvent4 = "PlayerRespWaitressLookingForSomeone";
	public const string Ep3WaitressSuspiciousGuys1RespEvent5 = "PlayerRespWaitressTalkingBusiness";
	public const string Ep3WaitressSuspiciousGuys1RespEvent6 = "PlayerRespWaitressWeapons";
	public const string Ep3WaitressSuspiciousGuys1RespEvent7 = "PlayerRespWaitressSomethingElse1";
	public const string Ep3WaitressSuspiciousGuys1RespEvent8 = "PlayerRespWaitressDone";
	
	public const string Ep3WaitressSuspiciousGuys2Response1 = "Is it possible they're planning to open a bar in the neighborhood?";
	public const string Ep3WaitressSuspiciousGuys2Response2 = "Did it seem like they were casing the place, like hold-up guys?";
	public const string Ep3WaitressSuspiciousGuys2Response3 = "What drinks did they order?";
	public const string Ep3WaitressSuspiciousGuys2Response4 = "Ask something else.";
	public const string Ep3WaitressSuspiciousGuys2RespEvent1 = "PlayerRespWaitressNeighborhood";
	public const string Ep3WaitressSuspiciousGuys2RespEvent2 = "PlayerRespWaitressHoldUp";
	public const string Ep3WaitressSuspiciousGuys2RespEvent3 = "PlayerRespWaitressOrder";
	public const string Ep3WaitressSuspiciousGuys2RespEvent4 = "PlayerRespWaitressSomethingElse2";
	
	public const string Ep3WaitressFAE1Response1 = "I think they are just planning some kind of business venture.";
	public const string Ep3WaitressFAE1Response2 = "I think the two men have dangerous intentions.";
	public const string Ep3WaitressFAE1Response3 = "I don't know.";
	public const string Ep3WaitressFAE1RespEvent1 = "PlayerRespWaitressBusinessVenture";
	public const string Ep3WaitressFAE1RespEvent2 = "PlayerRespWaitressSerious";
	public const string Ep3WaitressFAE1RespEvent3 = "PlayerRespWaitressNoIdea";
	
	public const string Ep3WaitressFAE1ConfirmationResponse1 = "Yes.";
	public const string Ep3WaitressFAE1ConfirmationResponse2 = "No.";
	public const string Ep3WaitressFAE1ConfirmationResponse3 = "I'm not sure.";
	public const string Ep3WaitressFAE1ConfirmationRespEvent1 = "PlayerRespWaitressFAE1ConfirmYes";
	public const string Ep3WaitressFAE1ConfirmationRespEvent2 = "PlayerRespWaitressFAE1ConfirmNo";
	public const string Ep3WaitressFAE1ConfirmationRespEvent3 = "PlayerRespWaitressFAE1ConfirmDontKnow";

	public const string Ep3WaitressFAE2Response1 = "I think he likes her.";
	public const string Ep3WaitressFAE2Response2 = "No, he doesn't like her.";
	public const string Ep3WaitressFAE2Response3 = "I'm not sure...";
	public const string Ep3WaitressFAE2RespEvent1 = "PlayerRespWaitressHeLikesHer";
	public const string Ep3WaitressFAE2RespEvent2 = "PlayerRespWaitressHeNotLikeHer";
	public const string Ep3WaitressFAE2RespEvent3 = "PlayerRespWaitressRomanceNotSure";
	
	public const string Ep3WaitressFAE2ConfirmationResponse1 = "Yes.";
	public const string Ep3WaitressFAE2ConfirmationResponse2 = "No.";
	public const string Ep3WaitressFAE2ConfirmationRespEvent1 = "PlayerRespWaitressFAE2ConfirmYes";
	public const string Ep3WaitressFAE2ConfirmationRespEvent2 = "PlayerRespWaitressFAE2ConfirmNo";

	public const string Ep3StephDeceptionResponse1 = "What are you talking about, Stephanie?";
	public const string Ep3StephDeceptionResponse2 = "Uh... yeah, that's right. Sorry Chris, haven't found the drive yet.";
	public const string Ep3StephDeceptionRespEvent1 = "PlayerRespStephAngel";
	public const string Ep3StephDeceptionRespEvent2 = "PlayerRespStephDevil";
	
	public const string Ep3StephConfResponse1 = "Chris has a substance abuse problem.";
	public const string Ep3StephConfResponse2 = "Chris has a gambling problem.";
	public const string Ep3StephConfResponse3 = "I can't tell.";
	public const string Ep3StephConfRespEvent1 = "PlayerRespStephSubstanceAbuse";
	public const string Ep3StephConfRespEvent2 = "PlayerRespStephGambling";
	public const string Ep3StephConfRespEvent3 = "PlayerRespStephChrisUnknown";
	
	public const string Ep3StephConfEvidenceResponse1 = "Check out these ATM receipts.";
	public const string Ep3StephConfEvidenceResponse2 = "Take a look at this envelope marked 'Africa Trip.'";
	public const string Ep3StephConfEvidenceResponse3 = "I'm not sure.";
	public const string Ep3StephConfEvidenceRespEvent1 = "PlayerRespStephATMReciepts";
	public const string Ep3StephConfEvidenceRespEvent2 = "PlayerRespStephAfricaTripEnvelope";
	public const string Ep3StephConfEvidenceRespEvent3 = "PlayerRespStephUnsureEvidence";
	
	public const string Ep3TerryFlashDriveResponse1 = "Here, take the flash drive, Terry.";
	public const string Ep3TerryFlashDriveResponse2 = "I have the flash drive right here, Terry.";
	public const string Ep3TerryFlashDriveRespEvent1 = "PlayerRespTerryGiveFlash";
	public const string Ep3TerryFlashDriveRespEvent2 = "PlayerRespTerryShowFlash";
	
	public const string Ep3TerryChrisGuiltResponse1 = "Chris is only trying to protect his sister!";
	public const string Ep3TerryChrisGuiltResponse2 = "Chris must have been involved in the Exceleron scandal.";
	public const string Ep3TerryChrisGuiltRespEvent1 = "PlayerRespChrisInnocent";
	public const string Ep3TerryChrisGuiltRespEvent2 = "PlayerRespChrisGuilty";
	
	#endregion
	
	public class PlayerResponse {
		public string[] responses;
		public string[] events;
	}
	
	//backCODE: This Dictionary stores all of the game's data.  You need to update this initialization if you change the number of responses.
	
	public static Dictionary<PlayerResponses.ResponseID, PlayerResponse> playerResponseStorage = new Dictionary<PlayerResponses.ResponseID, PlayerResponse>()
	{
		#region Episode 1			
		{
			PlayerResponses.ResponseID.Ep1ChrisCheckin, new PlayerResponse
			{
				responses = new string[] {
					Ep1ChrisCheckInResponse1
				},
				
				events = new string[] {
					Ep1ChrisCheckInRespEvent1
				}
			}				
		},
		
		{
			PlayerResponses.ResponseID.Ep1ChrisTerrysFate, new PlayerResponse
			{
				responses = new string[] {
					Ep1ChrisTerrysFateResponse1,
					Ep1ChrisTerrysFateResponse2,
					Ep1ChrisTerrysFateResponse3,
					Ep1ChrisTerrysFateResponse4
				},
				
				events = new string[] {
					Ep1ChrisTerrysFateRespEvent1,
					Ep1ChrisTerrysFateRespEvent2,
					Ep1ChrisTerrysFateRespEvent3,
					Ep1ChrisTerrysFateRespEvent4
				}
			}				
		},
		
		{
			PlayerResponses.ResponseID.Ep1MikeFAE1, new PlayerResponse
			{
				responses = new string[] {
					Ep1MikeFAE1Response1,
					Ep1MikeFAE1Response2,
					Ep1MikeFAE1Response3
				},
				
				events = new string[] {
					Ep1MikeFAE1RespEvent1,
					Ep1MikeFAE1RespEvent2,
					Ep1MikeFAE1RespEvent3
				}
			}				
		},
		
		{
			PlayerResponses.ResponseID.Ep1ChrisAttachments, new PlayerResponse
			{
				responses = new string[] {
					Ep1ChrisAttachmentsResponse1
				},
				
				events = new string[] {
					Ep1ChrisAttachmentsRespEvent1,
				}
			}				
		},
		
		{
			PlayerResponses.ResponseID.Ep1ChrisAttachments3, new PlayerResponse
			{
				responses = new string[] {
					Ep1ChrisAttachments3Response
				},
				
				events = new string[] {
					Ep1ChrisAttachments3RespEvent,
				}
			}				
		},	
		
		{
			PlayerResponses.ResponseID.Ep1ChrisAttachments2, new PlayerResponse
			{
				responses = new string[] {
					Ep1ChrisAttachments2Response
				},
				
				events = new string[] {
					Ep1ChrisAttachments2RespEvent,
				}
			}				
		},
		
		{
			PlayerResponses.ResponseID.Ep1ChrisAttachments1, new PlayerResponse
			{
				responses = new string[] {
					Ep1ChrisAttachments1Response
				},
				
				events = new string[] {
					Ep1ChrisAttachments1RespEvent,
				}
			}				
		},			
		
		{
			PlayerResponses.ResponseID.Ep1PlayerSearchDone, new PlayerResponse
			{
				responses = new string[] {
					Ep1PlayerSearchDoneResponse1
				},
				
				events = new string[] {
					Ep1PlayerSearchDoneRespEvent1
				}
			}				
		},		
		
		{
			PlayerResponses.ResponseID.Ep1MikeFAE1Confirmation, new PlayerResponse
			{
				responses = new string[] {
					Ep1MikeFAE1ConfirmationResponse1,
					Ep1MikeFAE1ConfirmationResponse2
				},
				
				events = new string[] {
					Ep1MikeFAE1ConfirmationRespEvent1,
					Ep1MikeFAE1ConfirmationRespEvent2
				}
			}				
		},
		
		{
			PlayerResponses.ResponseID.Ep1MikeConfEvidence, new PlayerResponse
			{
				responses = new string[] {
					Ep1MikeConfEvidenceResponse1,
					Ep1MikeConfEvidenceResponse2,
					Ep1MikeConfEvidenceResponse3
				},
				
				events = new string[] {
					Ep1MikeConfEvidenceRespEvent1,
					Ep1MikeConfEvidenceRespEvent2,
					Ep1MikeConfEvidenceRespEvent3
				}
			}				
		},
		
		{
			PlayerResponses.ResponseID.Ep1StephIntro, new PlayerResponse
			{
				responses = new string[] {
					Ep1StephIntroResponse1
				},
				
				events = new string[] {
					Ep1StephIntroRespEvent1
				}
			}				
		},
		
		{
			PlayerResponses.ResponseID.Ep1StephYouHaveFlashDrive, new PlayerResponse
			{
				responses = new string[] {
					Ep1StephYouHaveFlashDriveResponse1,
					Ep1StephYouHaveFlashDriveResponse2
				},
				
				events = new string[] {
					Ep1StephYouHaveFlashDriveRespEvent1,
					Ep1StephYouHaveFlashDriveRespEvent2
				}
			}				
		},
		
		{
			PlayerResponses.ResponseID.Ep1MikeFAE2, new PlayerResponse
			{
				responses = new string[] {
					Ep1MikeFAE2Response1,
					Ep1MikeFAE2Response2,
					Ep1MikeFAE2Response3
				},
				
				events = new string[] {
					Ep1MikeFAE2RespEvent1,
					Ep1MikeFAE2RespEvent2,
					Ep1MikeFAE2RespEvent3
				}
			}
		},		

		{
			PlayerResponses.ResponseID.Ep1TerryNervous, new PlayerResponse
			{
				responses = new string[] {
					Ep1MikeNervousResponse1,
					Ep1MikeNervousResponse2,
					Ep1MikeNervousResponse3
				},
				
				events = new string[] {
					Ep1MikeNervousRespEvent1,
					Ep1MikeNervousRespEvent2,
					Ep1MikeNervousRespEvent3
				}
			}
		},			
		
		{
			PlayerResponses.ResponseID.Ep1MikePlantCrazy, new PlayerResponse
			{
				responses = new string[] {
					Ep1MikePlantCrazyResponse1,
					Ep1MikePlantCrazyResponse2,
				},
				
				events = new string[] {
					Ep1MikePlantCrazyRespEvent1,
					Ep1MikePlantCrazyRespEvent2,
				}
			}
		},
		
		{
			PlayerResponses.ResponseID.Ep1MikePlantConcern, new PlayerResponse
			{
				responses = new string[] {
					Ep1MikePlantConcernResponse1,
					Ep1MikePlantConcernResponse2,
				},
				
				events = new string[] {
					Ep1MikePlantConcernRespEvent1,
					Ep1MikePlantConcernRespEvent2,
				}
			}
		},	
		
		{
			PlayerResponses.ResponseID.Ep1MikeSolution, new PlayerResponse
			{
				responses = new string[] {
					Ep1MikeSolutionResponse1,
					Ep1MikeSolutionResponse2,
					Ep1MikeSolutionResponse3,
					Ep1MikeSolutionResponse4
				},
				
				events = new string[] {
					Ep1MikeSolutionRespEvent1,
					Ep1MikeSolutionRespEvent2,
					Ep1MikeSolutionRespEvent3,
					Ep1MikeSolutionRespEvent4
				}
			}
		},			
		
#endregion
		
		#region Episode 2
		
		{
			PlayerResponses.ResponseID.Ep2StephFlashDrive, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephFlashDriveResponse1,
					Ep2StephFlashDriveResponse2
				},
				
				events = new string[] {
					Ep2StephFlashDriveRespEvent1,
					Ep2StephFlashDriveRespEvent2
				}
			}
		},
		
		{
			PlayerResponses.ResponseID.Ep2StephFAE1, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephFAE1Response1,
					Ep2StephFAE1Response2,
					Ep2StephFAE1Response3
				},
				
				events = new string[] {
					Ep2StephFAE1RespEvent1,
					Ep2StephFAE1RespEvent2,
					Ep2StephFAE1RespEvent3
				}
			}
		},
		
		{
			PlayerResponses.ResponseID.Ep2StephFAE1Confirmation, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephFAE1ConfirmationResponse1,
					Ep2StephFAE1ConfirmationResponse2,
					Ep2StephFAE1ConfirmationResponse3
				},
				
				events = new string[] {
					Ep2StephFAE1ConfirmationRespEvent1,
					Ep2StephFAE1ConfirmationRespEvent2,
					Ep2StephFAE1ConfirmationRespEvent3
				}
			}
		},
		
		{
			PlayerResponses.ResponseID.Ep2StephVoiceMail, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephVoiceMailResponse1,
					Ep2StephVoiceMailResponse2,
					Ep2StephVoiceMailResponse3
				},
				
				events = new string[] {
					Ep2StephVoiceMailRespEvent1,
					Ep2StephVoiceMailRespEvent2,
					Ep2StephVoiceMailRespEvent3
				}
			}
		},
		
		{
			PlayerResponses.ResponseID.Ep2StephFAE2, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephFAE2Response1,
					Ep2StephFAE2Response2,
					Ep2StephFAE2Response3
				},
				
				events = new string[] {
					Ep2StephFAE2RespEvent1,
					Ep2StephFAE2RespEvent2,
					Ep2StephFAE2RespEvent3
				}
			}
		},
		
		{
			PlayerResponses.ResponseID.Ep2StephFAE2Confirmation, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephFAE2ConfirmationResponse1,
					Ep2StephFAE2ConfirmationResponse2,
					Ep2StephFAE2ConfirmationResponse3
				},
				
				events = new string[] {
					Ep2StephFAE2ConfirmationRespEvent1,
					Ep2StephFAE2ConfirmationRespEvent2,
					Ep2StephFAE2ConfirmationRespEvent3
				}
			}
		},
		
		{
			PlayerResponses.ResponseID.Ep2StephGiveMeFlashDrive, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephGiveMeFlashDriveResponse1,
					Ep2StephGiveMeFlashDriveResponse2
				},
				
				events = new string[] {
					Ep2StephGiveMeFlashDriveRespEvent1,
					Ep2StephGiveMeFlashDriveRespEvent2
				}
			}
		},
		
		{
			PlayerResponses.ResponseID.Ep2CopyProtection, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephCopyProtectionAnswer1,
					Ep2StephCopyProtectionAnswer2,
					Ep2StephCopyProtectionAnswer3,
					Ep2StephCopyProtectionAnswer4
				},
				
				events = new string[] {
					Ep2StephCopyProtectionRespEvent1,
					Ep2StephCopyProtectionRespEvent2,
					Ep2StephCopyProtectionRespEvent3,
					Ep2StephCopyProtectionRespEvent4
				}
			}
		},		

		{
			PlayerResponses.ResponseID.Ep2StephConf, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephConf1Response1,
					Ep2StephConf1Response2,
					Ep2StephConf1Response3
				},
				
				events = new string[] {
					Ep2StephConf1RespEvent1,
					Ep2StephConf1RespEvent2,
					Ep2StephConf1RespEvent3
				}
			}
		},
		
		{
			PlayerResponses.ResponseID.Ep2DeadlyTreatment, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephDeadlyTreatmentAnswer1,
					Ep2StephDeadlyTreatmentAnswer2,
					Ep2StephDeadlyTreatmentAnswer3,
					Ep2StephDeadlyTreatmentAnswer4,
					Ep2StephDeadlyTreatmentAnswer5,
					Ep2StephDeadlyTreatmentAnswer6
				},
				
				events = new string[] {
					Ep2StephDeadlyTreatmentRespEvent1,
					Ep2StephDeadlyTreatmentRespEvent2,
					Ep2StephDeadlyTreatmentRespEvent3,
					Ep2StephDeadlyTreatmentRespEvent4,
					Ep2StephDeadlyTreatmentRespEvent5,
					Ep2StephDeadlyTreatmentRespEvent6
				}
			}
		},		

		{
			PlayerResponses.ResponseID.Ep2StephEvidence, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephEvidenceResponse1,
					Ep2StephEvidenceResponse2,
					Ep2StephEvidenceResponse3
				},
				
				events = new string[] {
					Ep2StephEvidenceRespEvent1,
					Ep2StephEvidenceRespEvent2,
					Ep2StephEvidenceRespEvent3
				}
			}
		},

		{
			PlayerResponses.ResponseID.Ep2StephBlackmail, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephBlackmailResponse1,
					Ep2StephBlackmailResponse2
				},
				
				events = new string[] {
					Ep2StephBlackmailRespEvent1,
					Ep2StephBlackmailRespEvent2
				}
			}
		},

		{
			PlayerResponses.ResponseID.Ep2StephTerryDanger, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephTerryDangerResponse1,
					Ep2StephTerryDangerResponse2
				},
				
				events = new string[] {
					Ep2StephTerryDangerRespEvent1,
					Ep2StephTerryDangerRespEvent2
				}
			}
		},
		
		{
			PlayerResponses.ResponseID.Ep2StephChrisFlashDrive, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephChrisFlashDriveResponse1
				},
				
				events = new string[] {
					Ep2StephChrisFlashDriveRespEvent1
				}
			}
		},
		
		{
			PlayerResponses.ResponseID.Ep2StephChrisAcquaintance, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephChrisAcquaintanceResponse1,
					Ep2StephChrisAcquaintanceResponse2
				},
				
				events = new string[] {
					Ep2StephChrisAcquaintanceRespEvent1,
					Ep2StephChrisAcquaintanceRespEvent2
				}
			}
		},

		{
			PlayerResponses.ResponseID.Ep2StephQuestions, new PlayerResponse
			{
				responses = new string[] {
					Ep2StephQuestionsResponse1,
					Ep2StephQuestionsResponse2,
					Ep2StephQuestionsResponse3,
					Ep2StephQuestionsResponse4,
					Ep2StephQuestionsResponse5,
					Ep2StephQuestionsResponse6,
					Ep2StephQuestionsResponse7,
					Ep2StephQuestionsResponse8,
					Ep2StephQuestionsResponse9
				},
				
				events = new string[] {
					Ep2StephQuestionsRespEvent1,
					Ep2StephQuestionsRespEvent2,
					Ep2StephQuestionsRespEvent3,
					Ep2StephQuestionsRespEvent4,
					Ep2StephQuestionsRespEvent5,
					Ep2StephQuestionsRespEvent6,
					Ep2StephQuestionsRespEvent7,
					Ep2StephQuestionsRespEvent8,
					Ep2StephQuestionsRespEvent9
				}
			}
		},		
		
		#endregion
		
		#region Episode 3
		
		{
			PlayerResponses.ResponseID.Ep3WaitressDrink, new PlayerResponse 
			{
				responses = new string[] {
					Ep3WaitressDrinkResponse1,
					Ep3WaitressDrinkResponse2
				},
				
				events = new string[] {
					Ep3WaitressRespEvent1,
					Ep3WaitressRespEvent2
				}
			}
		} ,
		
		{
			PlayerResponses.ResponseID.Ep3ScotchTheory, new PlayerResponse 
			{
				responses = new string[] {
					Ep3WaitressToYourHealthAnswer1,
					Ep3WaitressToYourHealthAnswer2,
					Ep3WaitressToYourHealthAnswer3,
					Ep3WaitressToYourHealthAnswer4,
					Ep3WaitressToYourHealthAnswer5,
					Ep3WaitressToYourHealthAnswer6,
					Ep3WaitressToYourHealthAnswer7
				},
				
				events = new string[] {
					Ep3WaitressRespLogicAnswer1,
					Ep3WaitressRespLogicAnswer2,
					Ep3WaitressRespLogicAnswer3,
					Ep3WaitressRespLogicAnswer4,
					Ep3WaitressRespLogicAnswer5,
					Ep3WaitressRespLogicAnswer6,
					Ep3WaitressRespLogicAnswer7
				}
			}
		} ,		
		
		{
			PlayerResponses.ResponseID.Ep3WaitressSuspiciousGuysWhat, new PlayerResponse 
			{
				responses = new string[] {
					Ep3WaitressSuspiciousGuysWhatResponse1,
					Ep3WaitressSuspiciousGuysWhatResponse2
				},
				
				events = new string[] {
					Ep3WaitressSuspiciousGuysWhatRespEvent1,
					Ep3WaitressSuspiciousGuysWhatRespEvent2
				}
			}
		} ,
		
		{
			PlayerResponses.ResponseID.Ep3WaitressSuspiciousGuys1, new PlayerResponse 
			{
				responses = new string[] {
					Ep3WaitressSuspiciousGuys1Response1,
					Ep3WaitressSuspiciousGuys1Response2,
					Ep3WaitressSuspiciousGuys1Response3,
					Ep3WaitressSuspiciousGuys1Response4,
					Ep3WaitressSuspiciousGuys1Response5,
					Ep3WaitressSuspiciousGuys1Response6,
					Ep3WaitressSuspiciousGuys1Response7,
					Ep3WaitressSuspiciousGuys1Response8
				},
				
				events = new string[] {
					Ep3WaitressSuspiciousGuys1RespEvent1,
					Ep3WaitressSuspiciousGuys1RespEvent2,
					Ep3WaitressSuspiciousGuys1RespEvent3,
					Ep3WaitressSuspiciousGuys1RespEvent4,
					Ep3WaitressSuspiciousGuys1RespEvent5,
					Ep3WaitressSuspiciousGuys1RespEvent6,
					Ep3WaitressSuspiciousGuys1RespEvent7,
					Ep3WaitressSuspiciousGuys1RespEvent8
				}
			}
		} ,
		
		{
			PlayerResponses.ResponseID.Ep3WaitressSuspiciousGuys2, new PlayerResponse 
			{
				responses = new string[] {
					Ep3WaitressSuspiciousGuys2Response1,
					Ep3WaitressSuspiciousGuys2Response2,
					Ep3WaitressSuspiciousGuys2Response3,
					Ep3WaitressSuspiciousGuys2Response4,
					Ep3WaitressSuspiciousGuys1Response8
				},
				
				events = new string[] {
					Ep3WaitressSuspiciousGuys2RespEvent1,
					Ep3WaitressSuspiciousGuys2RespEvent2,
					Ep3WaitressSuspiciousGuys2RespEvent3,
					Ep3WaitressSuspiciousGuys2RespEvent4,
					Ep3WaitressSuspiciousGuys1RespEvent8
				}
			}
		} ,
		
		{
			PlayerResponses.ResponseID.Ep3WaitressFAE1, new PlayerResponse 
			{
				responses = new string[] {
					Ep3WaitressFAE1Response1,
					Ep3WaitressFAE1Response2,
					Ep3WaitressFAE1Response3
				},
				
				events = new string[] {
					Ep3WaitressFAE1RespEvent1,
					Ep3WaitressFAE1RespEvent2,
					Ep3WaitressFAE1RespEvent3
				}
			}
		} ,
		
		{
			PlayerResponses.ResponseID.Ep3WaitressFAE2, new PlayerResponse 
			{
				responses = new string[] {
					Ep3WaitressFAE2Response1,
					Ep3WaitressFAE2Response2,
					Ep3WaitressFAE2Response3
				},
				
				events = new string[] {
					Ep3WaitressFAE2RespEvent1,
					Ep3WaitressFAE2RespEvent2,
					Ep3WaitressFAE2RespEvent3
				}
			}
		} ,

		{
			PlayerResponses.ResponseID.Ep3WaitressFAE2Confirmation, new PlayerResponse 
			{
				responses = new string[] {
					Ep3WaitressFAE2ConfirmationResponse1,
					Ep3WaitressFAE2ConfirmationResponse2
				},
				
				events = new string[] {
					Ep3WaitressFAE2ConfirmationRespEvent1,
					Ep3WaitressFAE2ConfirmationRespEvent2
				}
			}
		} ,
		
		{
			PlayerResponses.ResponseID.Ep3StephDeception, new PlayerResponse 
			{
				responses = new string[] {
					Ep3StephDeceptionResponse1,
					Ep3StephDeceptionResponse2
				},
				
				events = new string[] {
					Ep3StephDeceptionRespEvent1,
					Ep3StephDeceptionRespEvent2
				}
			}
		} ,	
		
		{
			PlayerResponses.ResponseID.Ep3StephConf, new PlayerResponse 
			{
				responses = new string[] {
					Ep3StephConfResponse1,
					Ep3StephConfResponse2,
					Ep3StephConfResponse3
				},
				
				events = new string[] {
					Ep3StephConfRespEvent1,
					Ep3StephConfRespEvent2,
					Ep3StephConfRespEvent3
				}
			}
		} ,

		{
			PlayerResponses.ResponseID.Ep3StephConfEvidence, new PlayerResponse 
			{
				responses = new string[] {
					Ep3StephConfEvidenceResponse1,
					Ep3StephConfEvidenceResponse2,
					Ep3StephConfEvidenceResponse3
				},
				
				events = new string[] {
					Ep3StephConfEvidenceRespEvent1,
					Ep3StephConfEvidenceRespEvent2,
					Ep3StephConfEvidenceRespEvent3
				}
			}
		} ,
		
		{
			PlayerResponses.ResponseID.Ep3TerryFlashDrive, new PlayerResponse 
			{
				responses = new string[] {
					Ep3TerryFlashDriveResponse1,
					Ep3TerryFlashDriveResponse2
				},
				
				events = new string[] {
					Ep3TerryFlashDriveRespEvent1,
					Ep3TerryFlashDriveRespEvent2
				}
			}
		} ,

		{
			PlayerResponses.ResponseID.Ep3TerryChrisGuilt, new PlayerResponse 
			{
				responses = new string[] {
					Ep3TerryChrisGuiltResponse1,
					Ep3TerryChrisGuiltResponse2
				},
				
				events = new string[] {
					Ep3TerryChrisGuiltRespEvent1,
					Ep3TerryChrisGuiltRespEvent2
				}
			}
		}
		
		#endregion
		
	};
	
	
}
