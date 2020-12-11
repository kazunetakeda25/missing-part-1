using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicFeedback : MonoBehaviour {
	
	private class StandardLogicVignetteStrings {
		public string intro;
		public string correct;
		public string wrong;
	}
	
	private const string PLANT_HUGGER_TITLE = "Episode 1 'Plant Hugger'";
	private const string COPY_PROTECTION_TITLE = "Episode 2 'Copy Protection'";
	private const string DEADLY_TREATMENT_TITLE = "Episode 2 'Deadly Treatment'";
	private const string TO_YOUR_HEALTH_TITLE = "Episode 3 'To Your Health'";
	
	#region PLANT_HUGGER
	
	private const string PLANT_HUGGER_INTRO = 
		"[#555555]When Mike, the super, asked your advice to find the best way to keep the plants alive in Terry's building, " +
		"the puzzle was a test to measure [#FF0000]confirmation bias[#555555].";
	
	private const string PLANT_HUGGER_CORRECT =
		"[#FFFFFF]Congratulations![#555555] Your answer, '{0}' " +
		"shows that you were ready to [#FFFFFF]test the one thing[#555555] that Mike believed would harm the plants. " +
		"In this case, you did not exhibit [#FF0000] confirmation bias[#555555].";
	
	private const string PLANT_HUGGER_WRONG = 
		"[#555555]You chose '{0}'.  In this case, you stuck with the once-a-week watering schedule, which was Mike's theory about how to keep plants healthy. " +
		"By [#FFFFFF]staying with his theory instead of trying to disprove it[#555555], you showed [#FF0000]confirmation bias[#555555].";
	
	#endregion
	
	#region COPY_PROTECTION
	
	private const string COPY_PROTECTION_INTRO = 
		"[#555555]When Maureen, the receptionist, asked how to prove her theory that Rocket Copy prints perfect press packets, " +
		"the puzzle was a test to measure [#FF0000]confirmation bias.";
	
	private const string COPY_PROTECTION_CORRECT =
		"[#FFFFFF]Congratulations![#555555] Your answer, '{0}' " +
		"shows that you were ready to test the hypothesis by [#FFFFFF]changing the suspected cause [#555555](using Rocket Copy), " +
		"[#FFFFFF]while keeping the other variables the same[#555555]." +
		"\n\nIn this case, you did not exhibit [#FF0000] confirmation bias[#555555].";
	
	private const string COPY_PROTECTION_WRONG = 
		"[#555555]You chose '{0}'. In order to avoid [#FF0000]confirmation bias[#555555], " +
		"you need to [#FFFFFF]change the suspected cause while keeping the other variables the same.[#555555]" +
		"\n\nIn this case, you should have changed the use of Rocket Copy (the suspected cause), while continuing to use " +
		"Fast Copy and Warp Speed Copy (the other variables).";
	
	#endregion
	
	#region DEADLY_TREATMENT
	
	private const string DEADLY_TREATMENT_INTRO = 
		"Stephanie, Terry's friend at Global Pharma Corp, asked your advice to test her theory that green pills, " +
		"the defective Exceleron, were being shipped to Africa. The puzzle was a test to measure [#FF0000]confirmation bias[#555555].\n";
	
	private const string DEADLY_TREATMENT_CORRECT_GREEN_DENVER =
		"[#FFFFFF]Congratulations![#555555] Choosing to examine the [#FFFFFF]box of green pills and the box being shipped to Denver[#555555] is the best answer. " +
		"Both choices together provide the knowledge necessary to prove or disprove Stephanie's theory. " +
		"Let's explore the logic behind these two choices and why they are the best selections.";

	private const string DEADLY_TREATMENT_WRONG_SINGLE_CHOICE_DENVER =
		"Your choice to look at the [#FFFFFF]box being shipped to Denver[#555555] is correct since it can provide some knowledge to help " +
		"prove or disprove Stephanie's theory.  However, [#FFFFFF]this single choice is not optimal[#555555] since it only focuses " +
		"on [#FF0000]disconfirming[#555555] Stephanie's theory. In order to fully prove or disprove Stephanie's theory, [#FFFFFF]it " +
		"is also necessary to look for confirming evidence[#555555]";
	
	private const string DEADLY_TREATMENT_WRONG_SINGLE_CHOICE_AFRICA =
		"You decided to look in the[#FFFFFF] box being shipped to Africa[#555555].  " +
		"This is a [#FF0000]confirming test[#555555]; presumably, you were looking to see if the box being shipped to Africa contained green pills.  " +
		"[#FFFFFF]However, this test will not provide you with useful information[#555555].  " +
		"Even if the box has some white Exceleron pills inside, you have gained no more knowledge about whether or not all green " +
		"Exceleron pills are shipped to Africa, which is Stephanie's theory.";
	
	private const string DEADLY_TREATMENT_WRONG_SINGLE_CHOICE_WHITE =
		"You decided to look at the [#FFFFFF]box of white Exceleron pills[#555555].  This is a [#FF0000]disconfirming test[#555555]; " +
		"presumably, you were looking to see if the box of white Exceleron pills was going to Africa.  " +
		"[#FFFFFF]However, this test will not provide you with useful information[#555555].  " +
		"Even if the box of white Exceleron pills is being shipped to Africa, you have gained no more knowledge about whether or not all " +
		"green Exceleron pills are shipped to Africa, which is Stephanie's theory.";
	
	private const string DEADLY_TREATMENT_WRONG_SINGLE_CHOICE_GREEN =
		"Your choice to look at the box of green pills is correct since it can provide some knowledge to help prove or disprove Stephanie's theory.  " +
		"However, [#FFFFFF]this single choice is not optimal since[#555555] it is only focuses on [#FF0000]confirming[#555555] Stephanie's theory.  " +
		"In order to fully prove or disprove Stephanie's theory, [#FFFFFF]it is also necessary to look for disconfirming evidence[#555555].";
	
	private const string DEADLY_TREATMENT_WRONG_TWO_CHOICE_GREEN_AFRICA =
		"Your choices [#FFFFFF]both focused on confirming Stephanie's theory[#555555], displaying [#FF0000]confirmation bias[#555555]. " +
		"In order to fully prove or disprove Stephanies theory, [#FFFFFF]it is also necessary to look for disconfirming evidence[#555555]. " +
		"Furthermore, looking at the box being shipped to Africa cannot provide any useful information to prove or disprove Stephanie's theory.";
	
	private const string DEADLY_TREATMENT_WRONG_TWO_CHOICE_WHITE_AFRICA =
		"Your choices were not optimal since [#FFFFFF]neither could provide any useful information[#555555] to prove or disprove Stephanie's theory.";	
	
	private const string DEADLY_TREATMENT_WRONG_TWO_CHOICE_AFRICA_DENVER =
		"Looking at the [#FF0000]box being shipped to Denver[#555555] has the potential to provide [#FF0000]disconfirming evidence[#555555] " +
		"that would disprove Stephanie's theory. However, looking at the [#FF0000]box being shipped to Africa is not optimal since that " +
		"choice [#FF0000]does not provide any useful information[#555555] to prove or disprove Stephanie's theory. ";
	
	private const string DEADLY_TREATMENT_WRONG_TWO_CHOICE_GREEN_WHITE =
		"Looking at the [#FFFFFF]box of green pills[#555555] has the potential to provide [#FF0000]confirming evidence[#555555] " +
		"to prove Stephanie's theory. However, looking at the [#FFFFFF]box of white pills[#555555] was not " +
		"optimal since that choice [#FFFFFF]does not provide any useful information[#555555] to prove or disprove Stephanie's theory. ";
	
	private const string DEADLY_TREATMENT_WRONG_TWO_CHOICE_DENVER_WHITE =
		"Your choices [#FFFFFF]both focused on disconfirming[#555555] Stephanie's theory.  " +
		"In order to fully prove or disprove Stephanie's theory, [#FFFFFF]it is also necessary to look for confirming evidence[#555555].  " +
		"Furthermore, looking at the box of white pills cannot provide any useful information to prove or disprove Stephanie's theory.";
	
	private const string DEADLY_TREATMENT_WRONG_THREE_CHOICE_WHITE_AFRICA_DENVER =
		"Your choice to look at the [#FFFFFF]box being shipped to Denver[#555555] is correct since it can provide some " +
		"knowledge to help prove or disprove Stephanie's theory. However, looking at the [#FFFFFF]box being shipped to Africa " +
		"and the box of white pills[#555555] was not optimal since those choices [#FFFFFF]do not provide any useful information[#555555] " +
		"to prove or disprove Stephanie's theory.";
	
	private const string DEADLY_TREATMENT_WRONG_THREE_CHOICE_WHITE_GREEN_DENVER =
		"While choosing to examine the box of green pills and the box being shipped to Denver were both useful tests, " +
		"choosing to examine the [#FFFFFF]box with white pills[#555555] was not necessary, since it [#FFFFFF]does not " +
		"provide any useful information[#555555] to prove or disprove Stephanie's theory.";
	
	private const string DEADLY_TREATMENT_WRONG_THREE_CHOICE_GREEN_AFRICA_DENVER =
		"While choosing to examine the box of green pills and the box being shipped to Denver were both useful tests, " +
		"choosing to examine the [#FFFFFF]box marked 'ship to Africa'[#555555] was not necessary, since it [#FFFFFF]does not " +
		"provide any useful information[#555555] to prove or disprove Stephanie's theory.";

	private const string DEADLY_TREATMENT_WRONG_THREE_CHOICE_WHITE_GREEN_AFRICA =
		"Your choice to look at the [#FFFFFF]box of green pills[#555555] is correct since it can provide some knowledge to help prove or " +
		"disprove Stephanie's theory. However, looking at the [#FFFFFF]box being shipped to Africa and the box of white pills[#555555] " +
		"was not optimal since those choices [#FFFFFF]do not provide any useful information[#555555] to prove or disprove Stephanie's theory.";
	
	private const string DEADLY_TREATMENT_WRONG_ALL_CHOICES = 
		"Looking at all of the boxes is [#FFFFFF]not the optimal solution[#555555]. " +
		"Examining the box being shipped to Africa and the box of white pills was not " +
		"necessary since those choices do not provide any useful information to prove or disprove Stephanie's theory.";
	
	private const string DEADLY_TREATMENT_SOLUTION_EXPLANATION =
		"\n\nChoosing to examine the [#FFFFFF]box of green pills and the box being shipped to Denver[#555555] " +
		"is the best answer. Both choices together provide the knowledge necessary to prove or disprove Stephanie's " +
		"theory. Let's explore the logic behind these two choices and why they are the best selections.";
		
	#endregion
	
	#region WHISKEY
	
	private const string WHISKEY_INTRO = 
		"Paige, the waitress at the whiskey bar, asked your advice to test her theory that the Moray single malts have the mushroom flavor notes. " +
		"The puzzle was a test to measure [#FF0000]confirmation bias[#FFFFFF].\n\n";
	
	private const string WHISKEY_WRONG_ONE_CHOICE =
		"You only chose to test one type of whiskey.  Tasting two malts is required to properly test Paige's hypothesis.";
	
	private const string WHISKEY_WRONG_THREE_CHOICES =
		"You chose to test three types of whiskey. However, you only need to test two types of whiskey to properly test Paige's hypothesis.";
	
	private const string WHISKEY_WRONG_FOUR_CHOICES =
		"You chose to test three types of whiskey. However, you only need to test two types of whiskey to properly test Paige's hypothesis.";
	
	private const string WHISKEY_EXPLANTION_ENDING =
		" Choosing to test the [#FFFFFF]whiskey sample from County Moray and a malt that customers describe as 'normal'[#555555] is the best answer. " +
		"These are the options that correspond to testing the conditional statement and the contrapositive. " +
		"Both choices together provide the knowledge necessary to prove or disprove Paige's theory. ";
	
	private const string WHISKEY_TWO_CHOICE_MORAY_ANGUS =
		"Tasting the [#FFFFFF]whiskey sample from County Moray[#555555] has the potential to " +
		"provide [#FF0000]confirming evidence[#555555] to prove Paige's theory. " +
		"However, tasting the whiskey sample from County Angus [#FFFFFF]cannot prove or disprove[#555555] Paige's theory.";
	
	private const string WHISKEY_TWO_CHOICE_MORAY_NORMAL =
		"[#FFFFFF]Congratulations![#555555] Choosing to test the [#FFFFFF]whiskey sample from County Moray " +
		"and a malt customers describe as 'normal' [#555555] is the best answer because it avoids [#FF0000]confirmation bias[#555555]. " +
		"These are the options that correspond to testing the conditional statement and the contrapositive. " +
		"Both choices together provide the knowledge necessary to prove or disprove Paige's theory. ";
	
	private const string WHISKEY_TWO_CHOICE_MORAY_MUSHROOM =
		"Your choices [#FFFFFF]both focused on confirming Paige's theory[#555555], displaying [#FF0000]confirmation bias[#555555]. " +
		"In order to fully prove or disprove Paige's theory, [#FFFFFF]it is also necessary to look for disconfirming evidence[#555555].  " +
		"Furthermore, knowing where the mushroomy malt is from[#FFFFFF] cannot prove or disprove[#555555] Paige's theory. ";
	
	private const string WHISKEY_TWO_CHOICE_ANGUS_NORMAL =
		"You chose a card that could [#FF0000]disconfirm[#555555] Paige's theory, but you also [#FFFFFF]missed an important card " +
		"that could confirm it[#555555]. Furthermore, tasting the whiskey sample from County Angus[#FFFFFF] cannot prove or " +
		"disprove[#555555] Paige's theory.";
	
	private const string WHISKEY_TWO_CHOICE_ANGUS_MUSHROOM =
		"Neither of the [#FFFFFF]Angus[#555555] or [#FFFFFF]Mushroom[#555555] options provide the information you need to prove or disprove Paige's theory.";
	
	private const string WHISKEY_TWO_CHOICE_NORMAL_MUSHROOM =
		"Testing the [#FFFFFF]malt customers describe as 'normal'[#555555] has the potential to provide [#FF0000]disconfirming evidence[#555555] that " +
		"would disprove Paige's theory. However, testing the mushroomy malt [#FFFFFF]cannot prove or disprove[#555555] Paige's theory. ";
	
	#endregion
	
	public static string GetLogicTitle(Vignette.VignetteID vignette)
	{
		switch(vignette) {
		case Vignette.VignetteID.E1vPlantHugger:
			return PLANT_HUGGER_TITLE;
		case Vignette.VignetteID.E2vCopyProtection:
			return COPY_PROTECTION_TITLE;
		case Vignette.VignetteID.E2vDeadlyTreatment:
			return DEADLY_TREATMENT_TITLE;
		case Vignette.VignetteID.E3vToYourHealth:
			return TO_YOUR_HEALTH_TITLE;			
		}
		
		return "NO TITLE SETUP!";
	}
	
	public static string GenerateComplexDynamicLogicResult(Vignette.VignetteID vignette)
	{
		
		SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
		
		switch(vignette)
		{
		case Vignette.VignetteID.E2vDeadlyTreatment:
				return GenerateDeadlyTreatmentString(currentSubject.DeadlyTreatmentChoice);
		case Vignette.VignetteID.E3vToYourHealth:
				return GenerateToYourHealthString(currentSubject.ToYourHealthChoice);
		}
		
		return "NO COMPLEX STRING FOUND!";
	}
	
	private static string GenerateDeadlyTreatmentString(string answer)
	{
		string deadlyTreatment = DEADLY_TREATMENT_INTRO;
		deadlyTreatment += "\n";
		switch(answer)
		{
		case "1000":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_SINGLE_CHOICE_AFRICA;
			break;
		case "0100":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_SINGLE_CHOICE_DENVER;
			break;
		case "0010":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_SINGLE_CHOICE_WHITE;
			break;
		case "0001":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_SINGLE_CHOICE_GREEN;
			break;
		case "1100":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_TWO_CHOICE_AFRICA_DENVER;
			break;
		case "1010":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_TWO_CHOICE_WHITE_AFRICA;
			break;
		case "1001":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_TWO_CHOICE_GREEN_AFRICA;
			break;
		case "0101":
			deadlyTreatment += DEADLY_TREATMENT_CORRECT_GREEN_DENVER;
			break;
		case "0110":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_TWO_CHOICE_DENVER_WHITE;
			break;
		case "0011":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_TWO_CHOICE_GREEN_WHITE;
			break;			
		case "1110":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_THREE_CHOICE_WHITE_AFRICA_DENVER;
			break;
		case "1011":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_THREE_CHOICE_WHITE_GREEN_AFRICA;
			break;
		case "1101":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_THREE_CHOICE_GREEN_AFRICA_DENVER;
			break;
		case "0111":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_THREE_CHOICE_WHITE_GREEN_DENVER;
			break;		
		case "1111":
			deadlyTreatment += DEADLY_TREATMENT_WRONG_ALL_CHOICES;
			break;
		default:
			deadlyTreatment += "\nNO ANSWER RECORDED!";
			break;
		}
		
		if(answer != "0101")
		{
			deadlyTreatment += DEADLY_TREATMENT_SOLUTION_EXPLANATION;
		}
		
		return deadlyTreatment;
	}
	
	private static string GenerateToYourHealthString(string answer)
	{
		string toYourHealth = WHISKEY_INTRO;
		
		toYourHealth += "\n";
		
		switch(answer) 
		{
		case "1000":
		case "0100":
		case "0010":
		case "0001":
			toYourHealth += WHISKEY_WRONG_ONE_CHOICE;
			break;
		case "1100":
			toYourHealth += WHISKEY_TWO_CHOICE_MORAY_ANGUS;
			break;
		case "1010":
			toYourHealth += WHISKEY_TWO_CHOICE_MORAY_NORMAL;
			break;
		case "1001":
			toYourHealth += WHISKEY_TWO_CHOICE_MORAY_MUSHROOM;
			break;
		case "0101":
			toYourHealth += WHISKEY_TWO_CHOICE_ANGUS_MUSHROOM;
			break;
		case "0110":
			toYourHealth += WHISKEY_TWO_CHOICE_ANGUS_NORMAL;
			break;
		case "0011":
			toYourHealth += WHISKEY_TWO_CHOICE_NORMAL_MUSHROOM;
			break;			
		case "1110":
		case "1011":
		case "1101":
		case "0111":
			toYourHealth += WHISKEY_WRONG_THREE_CHOICES;
			break;		
		case "1111":
			toYourHealth += WHISKEY_WRONG_FOUR_CHOICES;
			break;
		}
		
		if(answer != "1010")
			toYourHealth += WHISKEY_EXPLANTION_ENDING;
		
		return toYourHealth;
	}
	
	public static string[] GenerateSimpleDynamicLogicResult(Vignette.VignetteID vignette) 
	{
		SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
		StandardLogicVignetteStrings logicStrings = GetVignetteStrings(vignette);
		
		List<string> generatedStrings = new List<string>();
		
		generatedStrings.Add(logicStrings.intro);
		
		if(LogicAnswerKey.IsAnswerCorrect(vignette, GetPlayerLogicAnswer(vignette)))
			generatedStrings.Add(logicStrings.correct);
		else
			generatedStrings.Add(logicStrings.wrong);
		
		return generatedStrings.ToArray();
	}
	
	private static StandardLogicVignetteStrings GetVignetteStrings(Vignette.VignetteID vignette) 
	{
		Debug.Log("FETCHING ANSWER: " + GetPlayerLogicAnswer(vignette));
		Debug.Log("V: " + vignette);
		StandardLogicVignetteStrings logicStrings = new StandardLogicVignetteStrings();
		switch(vignette)
		{
		case Vignette.VignetteID.E1vPlantHugger:
			logicStrings.intro = PLANT_HUGGER_INTRO;
			logicStrings.correct = string.Format(PLANT_HUGGER_CORRECT, LogicAnswerKey.GetLogicAnswer(vignette, GetPlayerLogicAnswer(vignette))[0]);
			logicStrings.wrong = string.Format(PLANT_HUGGER_WRONG, LogicAnswerKey.GetLogicAnswer(vignette, GetPlayerLogicAnswer(vignette))[0]);
			break;
		case Vignette.VignetteID.E2vCopyProtection:
			logicStrings.intro = COPY_PROTECTION_INTRO;
			logicStrings.correct = string.Format(COPY_PROTECTION_CORRECT, LogicAnswerKey.GetLogicAnswer(vignette, GetPlayerLogicAnswer(vignette))[0]);
			logicStrings.wrong = string.Format(COPY_PROTECTION_WRONG, LogicAnswerKey.GetLogicAnswer(vignette, GetPlayerLogicAnswer(vignette))[0]);
			break;			
		}
		
		return logicStrings;
	}
		
	private static string GetPlayerLogicAnswer(Vignette.VignetteID vignette)
	{
		SubjectData currentSubject = SessionManager.GetSessionManager().currentSubject;
		
		switch(vignette) 
		{
		case Vignette.VignetteID.E1vPlantHugger:
			return currentSubject.PlantHuggerChoice;
		case Vignette.VignetteID.E2vCopyProtection:
			return currentSubject.CopyProtectionChoice;
		case Vignette.VignetteID.E2vDeadlyTreatment:
			return currentSubject.DeadlyTreatmentChoice;
		case Vignette.VignetteID.E3vToYourHealth:
			return currentSubject.ToYourHealthChoice;
		}
		
		Debug.LogError("Unable to retrieve answer!");
		return "xxxx";
	}
	
	private void Update() {
		SubjectData cs = SessionManager.GetSessionManager().currentSubject;
		Debug.Log(cs.PlantHuggerChoice);
	}
	
}
