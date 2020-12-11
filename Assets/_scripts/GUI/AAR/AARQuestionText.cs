using UnityEngine;
using System.Collections;

public class AARQuestionText {

	private const string EP1CBSELFREVIEW1 = "[#FFFFFF]In this episode, you searched for clues about how Terry left her apartment, and decided what kind of problem Terry has. To what extent were your actions and answers influenced by [#FF0000]confirmation bias?[#FFFFFF]";
	private const string EP1CBSELFREVIEW2 = "[#FFFFFF]In this episode, you searched for clues about how Terry left her apartment, and decided what kind of problem Terry has. In your estimation, to what extent did you show [#FF0000]confirmation bias[#FFFFFF] relative to the average player?";
	private const string EP1FAESELFREVIEW1 = "[#FFFFFF]In this episode, you watched a video of Terry in the elevator, and listened to Stephanie on the phone. To what extent were your actions and answers influenced by the [#FF0000]fundamental attribution error[#FFFFFF]? ";
	private const string EP1FAESELFREVIEW2 = "[#FFFFFF]In this episode, you watched a video of Terry in the elevator, and listened to Stephanie on the phone.  In your estimation, to what extent did you commit the [#FF0000]fundamental attribution error[#FFFFFF] relative to the average player? ";
	
	private const string EP2CBSELFREVIEW1 = "[#FFFFFF]In this episode, you searched for clues about Terry’s situation at work, and decided what kind of work problems Terry has. To what extent were your actions and answers influenced by [#FF0000]confirmation bias[#FFFFFF]?";
	private const string EP2CBSELFREVIEW2 = "[#FFFFFF]In this episode, you searched for clues about Terry’s situation at work, and decided what kind of work problems Terry has. In your estimation, to what extent did you show [#FF0000]confirmation bias[#FFFFFF] relative to the average player?";
	private const string EP2FAESELFREVIEW1 = "[#FFFFFF]In this episode, you helped answer some of Stephanie’s questions about Terry. To what extent were your actions and answers influenced by the [#FF0000]fundamental attribution error[#FFFFFF]? ";
	private const string EP2FAESELFREVIEW2 = "[#FFFFFF]IIn this episode, you helped answer some of Stephanie’s questions about Terry. In your estimation, to what extent did you commit the [#FF0000]fundamental attribution error[#FFFFFF] relative to the average player? ";
	
	private const string EP3CBSELFREVIEW1 = "[#FFFFFF]In this episode, you searched for clues about what kind of trouble Chris is in. To what extent were your actions and answers influenced by [#FF0000]confirmation bias[#FFFFFF]?";
	private const string EP3CBSELFREVIEW2 = "[#FFFFFF]In this episode, you searched for clues about what kind of trouble Chris is in. In your estimation, to what extent did you show [#FF0000]confirmation bias[#FFFFFF] relative to the average player?";
	private const string EP3FAESELFREVIEW1 = "[#FFFFFF]In this episode, you gossiped with an inquisitive waitress. To what extent were your actions and answers influenced by the [#FF0000]fundamental attribution error[#FFFFFF]? ";
	private const string EP3FAESELFREVIEW2 = "[#FFFFFF]In this episode, you gossiped with an inquisitive waitress. In your estimation, to what extent did you commit the [#FF0000]fundamental attribution error[#FFFFFF] relative to the average player? "; 
	
	public static string GetSelfReviewQuestionText(AARScreen.Question question) {
		
		switch(question)
		{
		case AARScreen.Question.ConfirmationBiasE1Q1:
			return EP1CBSELFREVIEW1;
		case AARScreen.Question.ConfirmationBiasE1Q2:
			return EP1CBSELFREVIEW2;
		case AARScreen.Question.FundamentalAttributionErrorE1Q1:
			return EP1FAESELFREVIEW1;
		case AARScreen.Question.FundamentalAttributionErrorE1Q2:
			return EP1FAESELFREVIEW2;
		case AARScreen.Question.ConfirmationBiasE2Q1:
			return EP2CBSELFREVIEW1;
		case AARScreen.Question.ConfirmationBiasE2Q2:
			return EP2CBSELFREVIEW2;
		case AARScreen.Question.FundamentalAttributionErrorE2Q1:
			return EP2FAESELFREVIEW1;
		case AARScreen.Question.FundamentalAttributionErrorE2Q2:
			return EP2FAESELFREVIEW2;
		case AARScreen.Question.ConfirmationBiasE3Q1:
			return EP3CBSELFREVIEW1;
		case AARScreen.Question.ConfirmationBiasE3Q2:
			return EP3CBSELFREVIEW2;
		case AARScreen.Question.FundamentalAttributionErrorE3Q1:
			return EP3FAESELFREVIEW1;
		case AARScreen.Question.FundamentalAttributionErrorE3Q2:
			return EP3FAESELFREVIEW2;
		default:
			Debug.LogError("INCORRECT AARSCREEN,QUESTION PASSED");
			return "";
		}
	}
	
}
