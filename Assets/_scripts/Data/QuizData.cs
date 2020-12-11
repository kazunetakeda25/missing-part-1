using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Quiz {
	FirstAid,
	Omelet,
	Coupons,
	BugRepellant,
	Karaoke
}

public class QuizData : MonoBehaviour {

	private const string FIRST_AID_QUESTION =
		"[#555555]Look at this example:\n" +
		"[#FFFFFF]'I got a bad scratch from a thorn when I was cutting some roses from the bush in front of my house.  " +
		"That night, I used an antibiotic cream, a hand lotion and had a bowl of chicken soup.  " +
		"The next day, my scratch totally healed.  I think it was the soup.'  What's the suspected cause in this puzzle?";
	
	private const string FIRST_AID_ANSWER1 = "A) antibiotic cream";
	private const string FIRST_AID_ANSWER2 = "B) hand lotion";
	private const string FIRST_AID_ANSWER3 = "C) chicken soup";
	
	private const int FIRST_AID_CORRECT_ANSWER_INDEX = 3;
	
	private const string FIRST_AID_CORRECT_RESPONSE = "[#FFFFFF]Correct![#555555] The chicken soup is the [#FF0000]suspected cause[#555555] in this case. \n\n" +
		"Now remember:  to test the hypothesis, [#FFFFFF]you should only change the 'chicken soup' in a future trial[#555555].  \n\n" +
		"To make an unbiased test, you need to hold the other conditions - antibiotic cream and hand lotion - constant:  don't change them.";
	
	private const string FIRST_AID_INCORRECT_RESPONSE = "[#555555]While {0} [#555555]is a plausible reason for a scratch healing quickly, it is not the [#FF0000]suspected cause[#555555].\n\n" +
		"In this example, [#FFFFFF]'chicken soup'[#555555] is the suspected cause because that is what the person thinks healed her scratch so quickly. " +
		"\n\nNow remember:  to test the hypothesis, [#FFFFFF]you should only change the 'chicken soup' in a future trial[#555555].  " +
		"To make an unbiased test, you need to hold the other conditions - antibiotic cream and hand lotion - constant: don't change them.";
	
	private const string OMELET_QUESTION =
		"[#555555]Consider this example:\n" +
		"[#FFFFFF]Yesterday I made an omelet that came out really fluffy.  " +
		"I let the eggs come to room temperature, added milk, and beat them for two minutes.  " +
		"I think adding milk made the difference.  To test my theory, should I: ";
	
	private const string OMELET_ANSWER1 = "A) Use eggs straight from the refrigerator, add milk, and beat for one minute?";
	private const string OMELET_ANSWER2 = "B) Use room temperature eggs, add milk, and beat for two minutes?";
	private const string OMELET_ANSWER3 = "C) Use room temperature eggs, do not add milk, and beat for two minutes?";
	
	private const int OMELET_CORRECT_ANSWER_INDEX = 3;
	
	private const string OMELET_CORRECT_RESPONSE = "[#FFFFFF]Good work!  [#555555]You changed the [#FF0000]suspected cause[#555555] - " +
		"[#FFFFFF]adding milk[#555555] - " +
		"and held the other conditions constant.  That is the key to avoiding [#FF0000]confirmation bias[#555555].";
	
	private const string OMELET_INCORRECT_RESPONSE = 
		"[#555555]When the storyteller said \"I think adding milk made the difference,\" he was [#FFFFFF]suggesting a cause[#555555] for the omelet being fluffy.  " +
		"Adding milk is the omelet-maker's theory for why the omelet was fluffy, just like Mike had a theory that watering plants once a week was best.  " +
		"\n\nIn order to test the initial theory that adding milk was the cause for the difference in taste, hold the other causes constant " +
		"(room temperature eggs and beating the omelet mixture for 2 minutes) and vary the [#FF0000]suggested cause[#555555] (adding milk).  " +
		"In other words, [#FFFFFF]do not[#555555] add milk next time and examine the resulting omelet. That is the key to avoiding [#FF0000]confirmation bias.[#555555]";
	
	private const string KARAOKE_QUESTION =	
		"[#555555]Here's another example:  \n" +
		"[#FFFFFF]Last weekend, I threw a great party. " +
		"I made it a costume party, rented a karaoke machine, and served wine and cheese. " +
		"I think the karaoke machine kicked the party up a notch.  To test my theory, should I: ";
	
	private const string KARAOKE_ANSWER1 = "A) Have a costume party, rent a karaoke machine, and serve beer?";
	private const string KARAOKE_ANSWER2 = "B) Have a costume party, not rent a karaoke machine, and serve wine and cheese?";
	private const string KARAOKE_ANSWER3 = "C) Have a costume party, not rent a karaoke machine, and serve beer?";
	
	private const int KARAOKE_CORRECT_ANSWER_INDEX = 2;
	
	private const string KARAOKE_CORRECT_RESPONSE = "[#FFFFFF]Correct! [#555555]You changed the [#FF0000]suspected cause[#555555] - " +
		"the [#FFFFFF]karaoke machine[#555555] - and held the other conditions constant.  " +
		"That is the key to avoiding [#FF0000]confirmation bias[#555555].";

	private const string KARAOKE_INCORRECT_RESPONSE = "When the storyteller said, 'I think the karaoke machine kicked the party up a notch,' " +
		"she was [#FFFFFF]suggesting a cause[#555555] for the great party. In order to test the hypothesis, you have to take the [#FF0000]suggested cause[#555555] and change it. " +
		"In other words, do not rent a karaoke machine next time and see how the party goes.  " +
		"At the same time, you have to [#FFFFFF]hold the other conditions constant[#555555].  Have another costume party, and serve wine and cheese again.  " +
		"That is the only way to know whether or not the karaoke machine was the factor that made the first party so much fun.";
	
	private const string COUPONS_QUESTION =	
		"[#555555]Try this example question: \n" +
		"[#FFFFFF]You have a set of four coupons.  The front of the coupons indicate whether they are for a clothing store or an electronics store.  " +
		"The back of the coupons indicate whether they are worth 10% off a purchase or 20% off a purchase. " +
		"Which two cards would you turn over to test the following hypothesis?\n" +
		"'If the coupon is for a clothing store, then the coupon is worth 20% off a purchase.'";
	
	private const string COUPONS_ANSWER1 = "A) The coupons marked 'clothing store' and 'electronics store.'";
	private const string COUPONS_ANSWER2 = "B) The coupons marked 'clothing store' and '10% off a purchase.'";
	private const string COUPONS_ANSWER3 = "C) The coupons marked 'clothing store' and '20% off a purchase.'";
	private const string COUPONS_ANSWER4 = "D) The coupons marked 'electronics store' and '10% off a purchase.'";
	private const string COUPONS_ANSWER5 = "E) The coupons marked 'electronics store' and '20% off a purchase.'";
	private const string COUPONS_ANSWER6 = "F) The coupons marked '10% off a purchase' and '20% off a purchase.'";
	
	private const int COUPONS_CORRECT_ANSWER_INDEX = 2;
	
	private const string COUPONS_CORRECT_RESPONSE = 
		"[#FFFFFF]Correct![#555555] The answer is the coupon marked 'clothing store' and the coupon marked '10% off a purchase.'\n\n" +
		"Your answer shows that you successfully avoided [#FF0000]confirmation bias[#555555].  " +
		"You performed both [#FFFFFF]confirming and disconfirming[#555555] tests to establish whether or not all clothing store coupons were worth 20% off a purchase. ";

	private const string COUPONS_INCORRECT_RESPONSE = 
		"[#555555]Not quite.  The contrapositive is of the form, 'If not Y, then not X.' " +
		"So, for the [#FF0000]conditional statement[#555555]: 'If the coupon is for a clothing store, then the coupon is worth 20% off a purchase,'" +
		" the [#FF0000]contrapositive[#555555] would be, 'If the coupon is not worth 20% off a purchase, then the coupon is not for a clothing store.' " +
		"\n\n[#FFFFFF]Remember, you need to test both the conditional statement and its contrapositive to make a complete and unbiased assessment of the hypothesis.[#555555] " +
		"In this example, you need to look at the coupon marked 'clothing store' (a test of the conditional statement) and the coupon marked " +
		"'10% off a purchase' (a test of the contrapositive).";	
	
	private const string STINK_BUG_QUESTION =	
		"[#555555]Try this example: \n" +
		"[#555555]Jerry is an amateur chemist who has developed a stink bug repellant. His repellant has been distributed in a neighborhood, but not everyone tried it." +
		" The cards below represent four houses in the neighborhood. " +
		"One side of the card tells whether or not the owner used the stink bug repellant, and the other side tells whether or not the house became infested with stink bugs. " +
		"Which two cards are needed to definitively test the following hypothesis?\n'[#FFFFFF]If you use the stink bug repellant, then your house will not have a stink bug infestation.'"; 
	
	private const string STINK_BUG_ANSWER1 = "A) The cards marked 'repellant' and 'no repellant.'";
	private const string STINK_BUG_ANSWER2 = "B) The cards marked 'repellant' and 'infestation.'";
	private const string STINK_BUG_ANSWER3 = "C) The cards marked 'repellant' and 'no infestation.'";
	private const string STINK_BUG_ANSWER4 = "D) The cards marked 'no repellant' and 'infestation.'";
	private const string STINK_BUG_ANSWER5 = "E) The cards marked 'no repellant' and 'no infestation.'";
	private const string STINK_BUG_ANSWER6 = "F) The cards marked 'infestation' and 'no infestation.'";
	
	private const int STINK_BUG_CORRECT_ANSWER_INDEX = 2;
	
	private const string STINK_BUG_CORRECT_RESPONSE = 
		"[#FFFFFF]Correct![#555555] The answer is the card marked 'repellant' and the card marked 'infestation.' " +
		"You [#FFFFFF]tested both the conditional statement and the contrapositive[#555555] to establish whether " +
		"or not Jerry's stink bug repellant is effective at preventing stink bug infestations. If both the " +
		"conditional statement and the contrapositive are true, then the theory must be true. By [#FFFFFF]checking " +
		"both cases that could show the hypothesis to be false[#555555], this strategy avoids [#FF0000]confirmation bias[#555555].";

	private const string STINK_BUG_INCORRECT_RESPONSE = 
		"[#555555]Not quite. In this case, the [#FF0000]conditional statement[#555555] is: " +
		"'[#FFFFFF]If you use the stink bug repellant [X], then your house will not have a stink bug infestation [Y].[#555555]' " +
		"The [#FF0000]contrapositive[#555555] of the statement is: " +
		"'[#FFFFFF]If the house has a stink bug infestation [not Y], then you did not use stink bug repellant [not X].[#555555]'\n\n" +
		"[#FFFFFF]Remember, you need to test both the conditional statement and its contrapositive to make a complete and unbiased " +
		"assessment of the hypothesis.[#555555]  " +
		"In this example, you need to look at the card marked 'repellant' (a test of the conditional statement) " +
		"and the card marked 'infestation' (a test of the contrapositive).  If both the conditional statement and the contrapositive are true, then the theory must be true. By testing the hypothesis this way, " +
		"you will [#FFFFFF]successfully avoid[#FF0000] confirmation bias[#555555]. ";
			
	public static string GetQuizQuestionText(Quiz quiz) 
	{
		switch(quiz)
		{
		case Quiz.FirstAid:
			return FIRST_AID_QUESTION;
		case Quiz.BugRepellant:
			return STINK_BUG_QUESTION;
		case Quiz.Coupons:
			return COUPONS_QUESTION;
		case Quiz.Karaoke:
			return KARAOKE_QUESTION;
		case Quiz.Omelet:
			return OMELET_QUESTION;
		}
		
		return "QUIZ QUESTION NOT FOUND!!";
	}
	
	public static string[] GetQuizAnswerText(Quiz quiz) 
	{
		List<string> answers = new List<string>();
		
		switch(quiz)
		{
		case Quiz.FirstAid:
			answers.Add(FIRST_AID_ANSWER1);
			answers.Add(FIRST_AID_ANSWER2);
			answers.Add(FIRST_AID_ANSWER3);
			break;
		case Quiz.BugRepellant:
			answers.Add(STINK_BUG_ANSWER1);
			answers.Add(STINK_BUG_ANSWER2);
			answers.Add(STINK_BUG_ANSWER3);
			answers.Add(STINK_BUG_ANSWER4);
			answers.Add(STINK_BUG_ANSWER5);
			answers.Add(STINK_BUG_ANSWER6);
			break;
		case Quiz.Coupons:
			answers.Add(COUPONS_ANSWER1);
			answers.Add(COUPONS_ANSWER2);
			answers.Add(COUPONS_ANSWER3);
			answers.Add(COUPONS_ANSWER4);
			answers.Add(COUPONS_ANSWER5);
			answers.Add(COUPONS_ANSWER6);
			break;
		case Quiz.Karaoke:
			answers.Add(KARAOKE_ANSWER1);
			answers.Add(KARAOKE_ANSWER2);
			answers.Add(KARAOKE_ANSWER3);
			break;
		case Quiz.Omelet:
			answers.Add(OMELET_ANSWER1);
			answers.Add(OMELET_ANSWER2);
			answers.Add(OMELET_ANSWER3);
			break;
		}
		
		if(answers.Count == 0)
			answers.Add("NO ANSWERS FOUND!!");
		
		return answers.ToArray();
	}
	
	public static int GetCorrectQuizAnswer(Quiz quiz)
	{
		switch(quiz)
		{
		case Quiz.FirstAid:
			return FIRST_AID_CORRECT_ANSWER_INDEX;
		case Quiz.BugRepellant:
			return STINK_BUG_CORRECT_ANSWER_INDEX;
		case Quiz.Coupons:
			return COUPONS_CORRECT_ANSWER_INDEX;
		case Quiz.Karaoke:
			return KARAOKE_CORRECT_ANSWER_INDEX;
		case Quiz.Omelet:
			return OMELET_CORRECT_ANSWER_INDEX;
		}
		
		Debug.LogError("Correct Answer Not Found!");
		return -1;
	}
	
	public static string GetCorrectResponse(Quiz quiz)
	{
		switch(quiz)
		{
		case Quiz.FirstAid:
			return FIRST_AID_CORRECT_RESPONSE;
		case Quiz.BugRepellant:
			return STINK_BUG_CORRECT_RESPONSE;
		case Quiz.Coupons:
			return COUPONS_CORRECT_RESPONSE;
		case Quiz.Karaoke:
			return KARAOKE_CORRECT_RESPONSE;
		case Quiz.Omelet:
			return OMELET_CORRECT_RESPONSE;
		}
		
		return "NO Response Found!!";
	}
	
	public static string GetIncorrectResponse(Quiz quiz)
	{
		switch(quiz)
		{
		case Quiz.FirstAid:
			return FIRST_AID_INCORRECT_RESPONSE;
		case Quiz.BugRepellant:
			return STINK_BUG_INCORRECT_RESPONSE;
		case Quiz.Coupons:
			return COUPONS_INCORRECT_RESPONSE;
		case Quiz.Karaoke:
			return KARAOKE_INCORRECT_RESPONSE;
		case Quiz.Omelet:
			return OMELET_INCORRECT_RESPONSE;
		}
		
		return "NO Response Found!!";
	}
	
}
