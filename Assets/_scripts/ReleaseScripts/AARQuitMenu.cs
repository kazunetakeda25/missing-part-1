using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AARQuitMenu : MonoBehaviour 
{
	private const float FADE_TIME = 0.5f;
	[SerializeField] CanvasGroup canvas;
	[SerializeField] Collider inputBlocker;
	private bool showingMenu;


	public void OnResume()
	{
		HideQuitMenu();
	}

	public void OnQuit()
	{
		if(MissingComplete.SaveGameManager.Instance != null) {
			MissingComplete.SaveGameManager.Instance.UnloadSavedGame();
		}

		MoviePlayer[] allMoviePlayers = GameObject.FindObjectsOfType<MoviePlayer>();
		foreach(MoviePlayer player in allMoviePlayers) {
			if(player.moviePlaying == true) {
				player.StopMovie();
			}
		}

		Application.LoadLevel("MAIN_MENU");
	}

	private void ShowQuitMenu()
	{
		//Time.timeScale = 0.0f;
		showingMenu = true;
		inputBlocker.enabled = true;
		canvas.DOFade(1.0f, FADE_TIME).OnComplete(OnShowFadeComplete);
	}

	private void OnShowFadeComplete()
	{
		canvas.interactable = true;
		Time.timeScale = 0.0f;

		MoviePlayer[] allMoviePlayers = GameObject.FindObjectsOfType<MoviePlayer>();
		foreach(MoviePlayer player in allMoviePlayers) {
			if(player.moviePlaying == true) {
				player.PauseMovie();
			}
		}
	}

	private void HideQuitMenu()
	{
		Time.timeScale = 1.0f;
		showingMenu = false;
		canvas.interactable = false;
		inputBlocker.enabled = false;
		canvas.DOFade(0.0f, FADE_TIME).OnComplete(OnHideFadeComplete);
	}

	private void OnHideFadeComplete()
	{
		MoviePlayer[] allMoviePlayers = GameObject.FindObjectsOfType<MoviePlayer>();	
		foreach(MoviePlayer player in allMoviePlayers) {
			if(player.moviePlaying == true) {
				player.UnPauseMovie();
			}
		}
	}

	private void Update()
	{
		if(Input.GetKeyUp(KeyCode.Escape)) {
			if(showingMenu) {
				HideQuitMenu();
			} else {
				ShowQuitMenu();
			}
		}
	}
}
