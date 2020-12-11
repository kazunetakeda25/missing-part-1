using UnityEngine;
using System.Collections;

public class BackgroundMovieTexture : MonoBehaviour {

	public MoviePlayer moviePlayer;
	public string movieToPlay;
	public bool playAudio;
		
	public void Start() {
		moviePlayer.PlayAsBackgroundTexture(movieToPlay, playAudio);
	}
	
}
