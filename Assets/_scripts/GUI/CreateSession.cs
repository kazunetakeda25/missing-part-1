using UnityEngine;
using System.Collections;

public class CreateSession : MonoBehaviour {
	
	public const string SEX_ERROR_MESSAGE = "Please select a subject gender before starting!";
	
	public UIRadioBtn HintsOn;
	public UIRadioBtn HintsOff;
	
	public UIRadioBtn DurationLong;
	public UIRadioBtn DurationShort;
	
	public UIRadioBtn FirstPerson;
	public UIRadioBtn ThirdPerson;
	
	public UIRadioBtn StoryArchiveOn;
	public UIRadioBtn StoryArchiveOff;
	
	public UIRadioBtn Male;
	public UIRadioBtn Female;
	
	public SpriteText lockedIVsWarningMessage;
	
	public MainMenu mainMenuController;
	
	public bool lockIVs;
	
	private void Start() {
		SetupCreateSessionScreen();
		
		if(lockIVs)
			StartCoroutine(LockIVs());
	}
	
	public void SetupCreateSessionScreen() {
		InitializeDuration();
		InitializeCommunicationStyle();
		InitializePerspective();
		InitializeNarrative();
	}
	
	private void InitializeDuration() {
		if(Settings.IsLongDuration())
			DurationLong.Value = true;
		else
			DurationShort.Value = true;
	}
	
	private void InitializeCommunicationStyle() {
		if(Settings.HintsOn())
			HintsOn.Value = true;
		else
			HintsOff.Value = true;
	}
	
	private void InitializePerspective() {
		if(Settings.IsFirstPerson())
			FirstPerson.Value = true;
		else
			ThirdPerson.Value = true;
	}
	
	private void InitializeNarrative() {
		if(Settings.StoryArchiveOn())
			StoryArchiveOn.Value = true;
		else
			StoryArchiveOff.Value = true;
	}
	
	public void StoryArchiveOnChecked() {
		if(!lockIVs)
			Settings.SetStoryArchive(true);
	}
	
	public void StoryArchiveOffChecked() {
		if(!lockIVs)
			Settings.SetStoryArchive(false);
	}
	
	public void HintsOnChecked() {
		if(!lockIVs)
			Settings.SetHints(true);
	}
	
	public void HintsOffChecked() {
		if(!lockIVs)
			Settings.SetHints(false);
	}
	
	public void FirstPersonChecked() {
		if(!lockIVs)
			Settings.SetFirstPerson(true);
	}
	
	public void ThirdPersonChecked() {
		if(!lockIVs)
			Settings.SetFirstPerson(false);
		SetupCreateSessionScreen();
	}
	
	public void LongDurationChecked() {
		if(!lockIVs)
			Settings.SetDuration(true);
	}
	
	public void ShortDurationChecked() {
		if(!lockIVs)
			Settings.SetDuration(false);
	}
	
	public void LaunchSession() {
		if(!Male.Value && !Female.Value) {
			DisplaySexErrorMessage();
			return;
		}
		
		mainMenuController.LaunchSession();
	}
	
	public void DisplaySexErrorMessage() {
		SimpleHint.CreateSimpleHint(SimpleHint.PopUpType.Error, SEX_ERROR_MESSAGE);
	}
	
	private IEnumerator LockIVs() {
		//We have to wait until EZGUI creates the Box Colliders which happens in Start
		yield return new WaitForSeconds(1.0f);
		FirstPerson.GetComponent<Collider>().enabled = false;
		ThirdPerson.GetComponent<Collider>().enabled = false;
		StoryArchiveOn.GetComponent<Collider>().enabled = false;
		StoryArchiveOff.GetComponent<Collider>().enabled = false;
		HintsOn.GetComponent<Collider>().enabled = false;
		HintsOff.GetComponent<Collider>().enabled = false;
		DurationLong.GetComponent<Collider>().enabled = false;
		DurationShort.GetComponent<Collider>().enabled = false;
		lockedIVsWarningMessage.GetComponent<Renderer>().enabled = true;
	}
}
