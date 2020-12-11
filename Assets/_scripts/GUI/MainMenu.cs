using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class MainMenu : MonoBehaviour {
	
	//Constants
	
	private const string CREATE_SESSION_PANEL_NAME = "Create Session";
	private const string MAIN_MENU_PANEL_NAME = "Main Menu Screen";
	private const string ABOUT_PANEL_NAME = "About App Screen";
	private const string MY_MUG_PANEL_NAME = "MyMug Login Screen";
	private const string MY_MUG_MOVIE_PANEL_NAME = "My Mug Movie";
	private const string GAME_INTRO_TEXT_PANEL_NAME = "Missing Text Intro";
	
	private const string MY_MUG_VIDEO_NAME = "MyMug";
	
	private const string USERNAME_TEMP_TEXT = "<Type Username Here>";
	
	//Public
	
	public MoviePlayer myMugMovie;
	public AudioSource MainMenuMusic;
	//public NoiseAndGrain noiseAndGrain;
	
	public UITextField myMugNameEntry;
	public RadioBtnGroup myMugRadio;
	public UIButton MyMugStartButton;
	
	public UIPanelManager mainMenuPanel;
	//public ParticleEmitter smoke;
	
	public UIRadioBtn maleRadio;
	public UIRadioBtn femaleRadio;
	public UITextField subjectName;
	
	public MissingTextIntro textIntro;
	
	//Members
	
	private bool sessionStarted;
	
	//Public Methods
	
	public void BringInCreateSession() {
		//smoke.emit = false;
		//smoke.ClearParticles();
		mainMenuPanel.BringInImmediate(CREATE_SESSION_PANEL_NAME);
	}
	
	public void BringInAboutMenu() {
		mainMenuPanel.BringInImmediate(ABOUT_PANEL_NAME);
	}
	
	public void LaunchSession() {
		MainMenuMusic.Stop();
		BringInMyMugPanel();
	}
	
	public void BringInMyMugPanel() {
		mainMenuPanel.BringInImmediate(MY_MUG_PANEL_NAME);
	}
	
	public void StartMyMugMovie() {
		mainMenuPanel.BringInImmediate(MY_MUG_MOVIE_PANEL_NAME);
		//Destroy(noiseAndGrain);
		myMugMovie.PlayMovie(MY_MUG_VIDEO_NAME, true, StartTextIntro);
		
		if(!sessionStarted)
			StartSession();
	}
	
	public void BackToMainMenu() {
		//smoke.emit = true;
		mainMenuPanel.BringInImmediate(MAIN_MENU_PANEL_NAME);
	}
	
	public void ClearMyMugTextEntry(UITextField textField) {
		myMugNameEntry.RemoveFocusDelegate(ClearMyMugTextEntry);
		myMugNameEntry.Text = "";
	}
	
	public void CheckMyMugStartButton() {
		if(myMugNameEntry.Text.Length > 0 && myMugNameEntry.Text != USERNAME_TEMP_TEXT) {
			MyMugStartButton.controlIsEnabled = true;
		} else {
			MyMugStartButton.controlIsEnabled = false;
		}
	}
	
	public void StartTextIntro() {
		PlayMakerFSM.BroadcastEvent(GlobalPlaymakerEvents.MASTER_FADE_IN);
		mainMenuPanel.BringInImmediate(GAME_INTRO_TEXT_PANEL_NAME);
		textIntro.StartIntro();
	}
	
	public void LoadFirstEpisode() {
		LevelLoader.StaticLoad(Levels.EPISODE1);
	}
	
	public void OnQuitButtonHit()
	{
		Application.Quit();
	}
	
	//Private Methods
	
	private void Start() {
		myMugNameEntry.SetFocusDelegate(ClearMyMugTextEntry);
	}
	
	private void Update() {
		CheckMyMugStartButton();
	}	
	
	private void StartSession() {
		SessionManager sessionManager = SessionManager.GetSessionManager(subjectName.text);
		
		SubjectData currentSubject = sessionManager.currentSubject;
		
		currentSubject.myMugName = myMugNameEntry.text;
		
		if(maleRadio.Value)
			currentSubject.subjectGender = Gender.male;
		else
			currentSubject.subjectGender = Gender.female;
		
		currentSubject.subjectID = subjectName.text;
		
		ReportEvent.ReportPlayerInfo(currentSubject.subjectGender, currentSubject.subjectID, currentSubject.myMugName);
		sessionStarted = true;
	}
	
}
