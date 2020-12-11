using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
    public class PauseGame : FsmStateAction
    {	
		
		public const string PAUSE_POP_UP_TAG = "PausePopUp";
		public const string DEBUG_PAUSE_POP_UP_PATH = "gui/Pause Pop-Up (Development)";
		public const string RELEASE_PAUSE_POP_UP_PATH = "gui/Pause Pop-Up (Release)";
		
		public FsmEvent eventToRunOnDone;
		private float oldTimeScale;
		
		List<AudioSource> playingAudio = new List<AudioSource>();
		
        public override void OnEnter()
        {
			ClearAllOldPauseScreens();
			PausePopUp pausePopUp = CreatePausePopUp();
			pausePopUp.SetDoneEvent(FinishHint);
			oldTimeScale = Time.timeScale;
			Time.timeScale = 0;
			FindAllPlayingAudio();
        }
		
		private PausePopUp CreatePausePopUp() {
			GameObject popUpGO;
			if(DebugTools.isDebugMode())
				popUpGO = (GameObject) GameObject.Instantiate(Resources.Load(DEBUG_PAUSE_POP_UP_PATH));
			else
				popUpGO = (GameObject) GameObject.Instantiate(Resources.Load(DEBUG_PAUSE_POP_UP_PATH));
			
			return popUpGO.GetComponent<PausePopUp>();
		}
		
		private void FindAllPlayingAudio() {
			AudioSource[] allAudioSource = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
			
			foreach(AudioSource audioSource in allAudioSource) {
				if(audioSource.isPlaying) {
					audioSource.Pause();
					playingAudio.Add(audioSource);
				}
			}

			MoviePlayer[] allMoviePlayers = GameObject.FindObjectsOfType(typeof(MoviePlayer)) as MoviePlayer[];

			foreach(MoviePlayer movie in allMoviePlayers) {
				if(movie.moviePlaying == true) {
					movie.PauseMovie();
				}
			}
		}
		
		public override void OnUpdate() 
		{
			if(Input.GetKeyUp(KeyCode.Escape)) {
				FinishHint();
			}
		}
		
		private void ClearAllOldPauseScreens() {
			//By Running this we can really spam the ESC key.
			HelpScreen hs = GameObject.FindObjectOfType<HelpScreen>();
			if(hs != null) {
				hs.CleanUp();
			}

			GameObject[] pauseGO = GameObject.FindGameObjectsWithTag(PAUSE_POP_UP_TAG);
			if(pauseGO != null) {
				for (int i = 0; i < pauseGO.Length; i++) {
					GameObject.Destroy(pauseGO[i]);
				}
			}			
		}
		
		public void FinishHint() {	
			ClearAllOldPauseScreens();
			Time.timeScale = oldTimeScale;
			Fsm.Event(eventToRunOnDone);
			ResumeAllPausedAudio();
			ResumeAllPausedMovies();
			Finish();
		}
		
		private void ResumeAllPausedAudio() {
			foreach(AudioSource audioSource in playingAudio) {
				audioSource.Play();
			}
			playingAudio.Clear();
		}

		private void ResumeAllPausedMovies()
		{
			MoviePlayer[] allMoviePlayers = GameObject.FindObjectsOfType(typeof(MoviePlayer)) as MoviePlayer[];

			foreach(MoviePlayer movie in allMoviePlayers) {
				if(movie.moviePlaying == true) {
					movie.UnPauseMovie();
				}
			}
		}
    }
}