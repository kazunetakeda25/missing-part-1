using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissingTextIntro : MonoBehaviour {
	
	//Enums
	
	public enum ButtonType { NEXT, REPLAY_VIDEO, PLAY_GAME, NONE }
	
	//Constants
	
	private const string INTRO_TEXT1 = 
		"In this game, you will be taking the part of Terry's neighbor. You live in a loft conversion apartment in New York City - East Village adjacent. " +
		"The rents are high, but since you're young, it's worth it. You are squarely in the middle of Manhattan's excitement. " +
		"\n\nLike most New York apartment dwellers, your neighbors are barely acquaintances. You know very little about Terry's work or family. " +
		"You met her brother Chris at a large gathering in Terry's apartment within the last month. " +
		"Chris reached out to you in the voicemail that you heard at the end of the opening video. " +
		"Terry seems to have an active social life, with friends and parties in keeping with her age. " +
		"Your paths have never crossed before outside of the apartment house.";
	
	private const string INTRO_TEXT2 = 
		"You work in a multi-national commodities transport and logistics company headquartered in New York City.  " +
		"You are generally well-liked and respected - viewed as having significant potential for advancement. " +
		"Because of your company's global reach and your work in operations, you are often engaged in problem-solving well into the night. " +
		"This is something of a dilemma: you know that commitment to your work is the basis of advancement... " +
		"but you could use a bit more excitement in your life." +
		"\n\nIt is 7:18 PM, Eastern Standard Time. " +
		"At Chris's request, you have just entered Terry's apartment to check up on her.";
	
	private const string NEXT_BUTTON_TEXT = "Next";
	private const string PLAYGAME_BUTTON_TEXT = "Play Game";
	private const string REPLAYVIDEO_BUTTON_TEXT = "Replay Intro";
	
	//Public Variables
	
	public UIButton leftButton;
	public UIButton rightButton;
	
	public AudioSource music;
	public AudioSource typing;
	
	public MainMenu mainMenu;
	
	//Members
	
	private List<string> strings;
	
	private MissingIntroTypewriter currentScreen;
	private bool listenForTypewriterEnd;
	
	private int screenIndex;
	private ButtonType leftButtonType;
	private ButtonType rightButtonType;
	
	//Public Methods
	
	public void StartIntro() 
	{
		this.GetComponent<AudioSource>().Play();
		music.Play();
		screenIndex = 0;
		ContinueIntro();
	}
	
	public void NextScreen()
	{
		CleanUp();
		screenIndex++;
		ContinueIntro();
	}
	
	public void ReplayIntroVideo() 
	{
		CleanUp();
		music.Stop();
		this.GetComponent<AudioSource>().Stop();
		typing.Stop();
		screenIndex = 0;
		mainMenu.StartMyMugMovie();
	}
	
	public void StartGame() 
	{
		CleanUp();
		mainMenu.LoadFirstEpisode();
	}	
	
	//Private Methods
	
	private void Start() 
	{
		InitList();
	}
	
	private void Update() 
	{	
		if(listenForTypewriterEnd)
		{
			CheckTypewriter();
			
			if(Input.GetMouseButtonUp(1) && Debug.isDebugBuild)
			{
				currentScreen.typewriter.charactersPerSecond = 2000;
				FadeInButtons();
			}
		}
	}
	
	private void CheckTypewriter() 
	{
		if(currentScreen.typewriter.Completed)
		{
			typing.Stop();
			listenForTypewriterEnd = false;
			FadeInButtons();
		}
	}
	
	private void FadeInButtons() 
	{
		if(leftButtonType != ButtonType.NONE)
		{
			leftButton.Hide(false);
			FadeInButton(leftButton);
		}
			
		
		if(rightButtonType != ButtonType.NONE)
		{
			rightButton.Hide(false);
			FadeInButton(rightButton);
		}
	}
	
	private void FadeInButton(UIButton button)
	{
		float fadeTime = 3.0f;
		float delayTime = 0.0f;
		
		FadeSprite.Do
			(
				button, 
				EZAnimation.ANIM_MODE.To, 
				ColorTools.SetColorAlpha(button.color, 1), 
				EZAnimation.linear, 
				fadeTime,
				delayTime,
				null,
				null
			);
		
		FadeText.Do
			(
				button.spriteText,
				EZAnimation.ANIM_MODE.To,
				ColorTools.SetColorAlpha(button.spriteText.Color, 1),
				EZAnimation.linear,
				fadeTime,
				delayTime,
				null,
				null
			);
	}
	
	private void InitList() 
	{
		strings = new List<string>();
		strings.Add(INTRO_TEXT1);
		strings.Add(INTRO_TEXT2);
	}
	
	private void ContinueIntro() 
	{
		switch(screenIndex)
		{
		case 0:
			StartScreen(ButtonType.NONE, ButtonType.NEXT);
			break;
		case 1:
			StartScreen(ButtonType.REPLAY_VIDEO, ButtonType.PLAY_GAME);
			break;
		default:
			Debug.LogError("Incorrect Index Passed to Text Intro");
			break;
		}
	}
		
	private void StartScreen(ButtonType leftButtonType, ButtonType rightButtonType) 
	{
		this.leftButtonType = leftButtonType;
		this.rightButtonType = rightButtonType;
		
		GameObject goTypewriter = (GameObject) GameObject.Instantiate(Resources.Load(ResourcePaths.INTRO_TEXT_OBJECT));
		currentScreen = goTypewriter.GetComponent<MissingIntroTypewriter>();
		goTypewriter.transform.parent = this.transform;
		
		typing.Play();
		
		SetupButton(leftButton, leftButtonType);
		SetupButton(rightButton, rightButtonType);
		
		currentScreen.typewriter.text = strings[screenIndex];
		currentScreen.typewriter.Start();
		currentScreen.typewriter.Write();
		
		listenForTypewriterEnd = true;
	}
	
	private void SetupButton(UIButton button, ButtonType buttonType) 
	{
		
		switch(buttonType)
		{
		case ButtonType.NEXT:
			button.Text = NEXT_BUTTON_TEXT;
			button.methodToInvoke = "NextScreen";
			break;
		case ButtonType.PLAY_GAME:
			button.Text = PLAYGAME_BUTTON_TEXT;
			button.methodToInvoke = "StartGame";
			break;
		case ButtonType.REPLAY_VIDEO:
			button.Text = REPLAYVIDEO_BUTTON_TEXT;
			button.methodToInvoke = "ReplayIntroVideo";
			break;
		case ButtonType.NONE:
			button.Hide(true);
			break;
		}
		
	}
	
	private void CleanUp() 
	{
		CleanUpButton(leftButton);
		CleanUpButton(rightButton);
		listenForTypewriterEnd = false;
		Destroy(currentScreen.gameObject);
	}
	
	private void CleanUpButton(UIButton button)
	{
		button.SetColor(ColorTools.SetColorAlpha(button.color, 0));
		button.spriteText.SetColor(ColorTools.SetColorAlpha(button.spriteText.Color, 0));
		button.Hide(true);
	}
	
}
