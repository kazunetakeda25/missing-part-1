using UnityEngine;
using System.Collections;
using RenderHeads.Media.AVProVideo;

public class MoviePlayer : MonoBehaviour {
	
	public delegate void ActionDelegate();
	
	private const string VIDEOS_FOLDER = "videos/";
	
	private bool startMovieWhenReady = false;
	public bool moviePlaying = false;
	private bool m_fullScreenFadeout;
	private bool m_fullScreenMovie;
	private bool m_playAudio;
	private float movieStartTime = 0;
	private MediaPlayer mediaPlayer;
	
	private float delay;
	private bool isPaused;
	
	private ActionDelegate m_Action;

	public float volume = 1;
	
	private void Awake() {
		this.GetComponent<Renderer>().enabled = false;
		mediaPlayer = GetComponent<MediaPlayer> ();
	}
	
	private void Update() {
		
		//This ensures that the movieTexture is loaded before it starts playing.
		if(startMovieWhenReady && mediaPlayer.Control.CanPlay()) {
			startMovieWhenReady = false;
			StartCoroutine(StartMovie());
			
			//We skip teh first frame to avoid the movieTexture glitch that appears when you first set it to a mesh.
//			StartCoroutine(skipFirstFrame());
		}
		
		//For Debug Skipping
		if(moviePlaying && Input.GetMouseButtonUp(1) && SkipBuffer() && m_fullScreenMovie) {
			if(Application.isEditor || Debug.isDebugBuild) {
				if(m_fullScreenFadeout)
					BeginFadeOut();
				else
					MovieDone();
			}
		}
		
		if(isPaused == true) {
			return;
		}
		
		if(moviePlaying && mediaPlayer.Control.IsFinished()) {
			MovieDone();
		}
	}
	
	private bool SkipBuffer() {
		if(Time.realtimeSinceStartup - movieStartTime < 1f) {
			return false;
		}
		
		return true;
	}
	
	//We send a message to the Fade Out FSM attached to this object.
	private void BeginFadeOut() {
		Debug.Log ("Movie Player Begin Fade Out Called");
		moviePlaying = false;
		PlayMakerFSM fsm = this.gameObject.GetComponent<PlayMakerFSM>();
		fsm.SendEvent(GlobalPlaymakerEvents.MASTER_FADE_OUT);
		StartCoroutine(MovieDoneDelay());
	}
	
	private IEnumerator MovieDoneDelay() {
		yield return new WaitForSeconds(1.0f);
		MovieDone();
	}
	
	private void MovieDone() {
		//renderer.enabled = false;
		moviePlaying = false;
		mediaPlayer.Control.Stop();
		
		this.GetComponent<Renderer>().enabled = false;
		
		if(m_Action != null)
			m_Action();		
	}
	
	private IEnumerator skipFirstFrame() {
		yield return new WaitForSeconds(0.1f + delay);
		this.GetComponent<Renderer>().enabled = true;
	}
	
	public void StopMovie()
	{
		mediaPlayer.Control.Stop();
	}
	
	public void PauseMovie()
	{
		isPaused = true;
		mediaPlayer.Control.Pause();
	}
	
	public void UnPauseMovie()
	{
		isPaused = false;
		mediaPlayer.Control.Play();
	}
	
	public void PlayMovie(string movieToPlay, bool fullScreenFadeOut) {
		PlayMovie(movieToPlay, fullScreenFadeOut, null, 0);
	}
	
	public void PlayMovie(string movieToPlay, bool fullScreenFadeOut, ActionDelegate actionToPerformOnCompletion) {
		PlayMovie(movieToPlay, fullScreenFadeOut, actionToPerformOnCompletion, 0);
	}
	
	public void PlayMovie(string movieToPlay, bool fullScreenFadeOut, float delay) {
		PlayMovie(movieToPlay, fullScreenFadeOut, null, delay);
	}
	
	public void PlayMovie(string movieToPlay, bool fullScreenFadeOut, ActionDelegate actionToPerformOnCompletion, float delay) {
		m_fullScreenFadeout = fullScreenFadeOut;
		m_fullScreenMovie = true;
		m_Action = actionToPerformOnCompletion;
		m_playAudio = true;
		
		this.delay = delay;
		
		SetupMovie(movieToPlay);
		startMovieWhenReady = true;
	}
	
	public void PlayAsBackgroundTexture(string movieToPlay, bool playAudio) {
		m_playAudio = playAudio;
		SetupMovie(movieToPlay);
		startMovieWhenReady = true;
		m_fullScreenFadeout = false;
		mediaPlayer.m_Loop = true;
	}
	
	public void PlayAsBackgroundTextureOnce(string movieToPlay, bool playAudio) {
		m_playAudio = playAudio;
		SetupMovie(movieToPlay);
		startMovieWhenReady = true;
		m_fullScreenFadeout = false;
		mediaPlayer.m_Loop = false;
	}
	
	private void SetupMovie(string movieToPlay) {
		string path = VIDEOS_FOLDER + movieToPlay + ".mov";
		mediaPlayer = GetComponent<MediaPlayer> ();
		mediaPlayer.OpenVideoFromFile (MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, path, true);
		mediaPlayer.Play ();
		mediaPlayer.Control.SetVolume (volume);
		GetComponent<Renderer>().enabled = true;
	}
	
	private IEnumerator StartMovie() {
		yield return new WaitForSeconds(delay);
		
		SetRealTimeStartPoint();
		mediaPlayer.Play();
		moviePlaying = true;
	}
	
	private void SetRealTimeStartPoint() {
		movieStartTime = Time.realtimeSinceStartup;
	}
	
}
