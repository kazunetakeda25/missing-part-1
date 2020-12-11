using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;

namespace MissingComplete
{
	public class ConclusionScreen : MonoBehaviour 
	{
		[SerializeField] CanvasGroup canvas;
		[SerializeField] Text timeText;

		private void Start()
		{
			canvas.DOFade(1.0f, 1.0f).OnComplete(FadeComplete);


			SaveGameManager.SaveGame save = SaveGameManager.Instance.GetCurrentSaveGame();

			if(save != null) {
				save.gameComplete = true;
				save.dateCompleted = DateTime.Now;
				SaveGameManager.Instance.SaveCurrentGame();
			}
		}

		private void FadeComplete()
		{
			canvas.interactable = true;
		}

		private void Update()
		{
			if(SaveGameManager.Instance == null) {
				return;
			}

			TimeSpan time = TimeSpan.FromSeconds(SaveGameManager.Instance.GetCurrentSaveGame().playTime);

			string answer = string.Format("{0:D2}:{1:D2}:{2:D2}", 
				time.Hours, 
				time.Minutes, 
				time.Seconds);

			timeText.text = answer;
		}

		public void OnQuitHit()
		{
			canvas.interactable = false;
			canvas.DOFade(0.0f, 1.0f).OnComplete(OnFadeOutComplete);
		}

		private void OnFadeOutComplete()
		{
			Application.LoadLevel("MAIN_MENU");
			SaveGameManager.Instance.UnloadSavedGame();
		}

	}
}

