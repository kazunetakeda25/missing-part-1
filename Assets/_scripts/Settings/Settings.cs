using UnityEngine;
using System.Collections;

public class Settings {
	
	public enum Template
	{
		C1_H1N1L1,
		C2_H1N1S1,
		C3_H1N0L1,
		C4_H1N0S1,
		C5_H1N1L3,
		C6_H1N1S3,
		C7_H1N0L3,
		C8_H1N0S3
	}
	
	private const string FIRST_PERSON_KEY = "First Person";
	private const string FIRST_PERSON_TRUE = "True";
	private const string FIRST_PERSON_FALSE = "False";
	
	private const string HINTS_KEY = "Hints";
	private const string HINTS_ON = "On";
	private const string HINTS_OFF = "Off";
	
	private const string STORY_ARCHIVE_KEY = "Story Archive";
	private const string STORY_ARCHIVE_TRUE = "True";
	private const string STORY_ARCHIVE_FALSE = "False";
	
	private const string DURATION_KEY = "Duration";
	private const string LONG_DURATION = "Long";
	private const string SHORT_DURATION = "Short";
	
	private const bool defaultIsFirstPerson = true;
	private const bool defaultHintsOn = true;
	private const bool defaultStoryArchive = true;
	private const bool defaultIsLongDuration = true;
	
	public static void NewGameSession() {
		//Delete All Keys
		PlayerPrefs.DeleteAll();
		InitializeKeys();
	}
	
	private static void InitializeKeys() {
		SetFirstPerson(defaultIsFirstPerson);
		SetHints(defaultHintsOn);
		SetStoryArchive(defaultStoryArchive);
		SetDuration(defaultIsLongDuration);
	}
	
	public static bool IsFirstPerson() {
		return true;
		if(PlayerPrefs.GetString(FIRST_PERSON_KEY) == FIRST_PERSON_TRUE)
			return true;
		
		return false;
//		return true;
	}
	
	public static bool HintsOn() {
		return true;

		if(PlayerPrefs.GetString(HINTS_KEY) == HINTS_ON)
			return true;
		
		return false;
	}
	
	public static bool StoryArchiveOn() {
		return true;

		if(PlayerPrefs.GetString(STORY_ARCHIVE_KEY) == STORY_ARCHIVE_TRUE)
			return true;
		
		return false;
	}
	
	public static bool IsLongDuration() {
		return true;

		if(PlayerPrefs.GetString(DURATION_KEY) == LONG_DURATION)
			return true;
		
		return false;
	}
	
	public static void SetFirstPerson(bool isFirstPerson) {
		string val = FIRST_PERSON_FALSE;
		if(isFirstPerson)
			val = FIRST_PERSON_TRUE;
		
		PlayerPrefs.SetString(FIRST_PERSON_KEY, val);
	}
	
	public static void SetHints(bool hintsOn) {
		string val = HINTS_OFF;
		if(hintsOn)
			val = HINTS_ON;
		
		PlayerPrefs.SetString(HINTS_KEY, val);
	}
	
	public static void SetStoryArchive(bool storyArchiveOn) {
		string val = STORY_ARCHIVE_FALSE;
		if(storyArchiveOn)
			val = STORY_ARCHIVE_TRUE;
		
		PlayerPrefs.SetString(STORY_ARCHIVE_KEY, val);
	}
	
	public static void SetDuration(bool longDuration) {
		string val = SHORT_DURATION;
		if(longDuration)
			val = LONG_DURATION;
		
		PlayerPrefs.SetString(DURATION_KEY, val);
	}
	
	public static void SetTemplate(Template template)
	{
		switch(template)
		{
		case Template.C1_H1N1L1:
			SetHints(true);
			SetStoryArchive(true);
			SetDuration(true);
			SetFirstPerson(true);
			break;
		case Template.C2_H1N1S1:
			SetHints(true);
			SetStoryArchive(true);
			SetDuration(false);
			SetFirstPerson(true);
			break;
		case Template.C3_H1N0L1:
			SetHints(true);
			SetStoryArchive(false);
			SetDuration(true);
			SetFirstPerson(true);
			break;
		case Template.C4_H1N0S1:
			SetHints(true);
			SetStoryArchive(false);
			SetDuration(false);
			SetFirstPerson(true);
			break;
		case Template.C5_H1N1L3:
			SetHints(true);
			SetStoryArchive(true);
			SetDuration(true);
			SetFirstPerson(false);
			break;
		case Template.C6_H1N1S3:
			SetHints(true);
			SetStoryArchive(true);
			SetDuration(false);
			SetFirstPerson(false);
			break;
		case Template.C7_H1N0L3:
			SetHints(true);
			SetStoryArchive(false);
			SetDuration(true);
			SetFirstPerson(false);
			break;
		case Template.C8_H1N0S3:
			SetHints(true);
			SetStoryArchive(false);
			SetDuration(false);
			SetFirstPerson(false);
			break;
		}
	}
	
}
