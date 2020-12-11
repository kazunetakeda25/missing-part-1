using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class StartMusicRNG : MonoBehaviour {

	public AudioClip[] clips;
	private int songIndex;
	
	private bool clipStarted = false;
	
	
	private void Start() 
	{
		//Pick a clip to start playing, and randomly pick starting point.
		songIndex = Random.Range(0, clips.Length);
		PlayClipStartingAtRandomPoint(songIndex);
	}
	
	private void Update() 
	{
		if(clipStarted)
			CheckToSeeIfSongIsDone();
	}
	
	private void CheckToSeeIfSongIsDone()
	{
		//Debug.Log("Time: " + audio.time);
		//Debug.Log("Length: " + audio.clip.length);
		if(GetComponent<AudioSource>().time >= GetComponent<AudioSource>().clip.length)
			CycleMusic();
	}
	
	private void CycleMusic() 
	{
		if((songIndex + 1) >= clips.Length)
			songIndex = 0;
		else
			songIndex++;
		
		this.GetComponent<AudioSource>().Stop();
		this.GetComponent<AudioSource>().clip = clips[songIndex];
		this.GetComponent<AudioSource>().loop = false;
		this.GetComponent<AudioSource>().time = 0;
		this.GetComponent<AudioSource>().Play();
		clipStarted = true;
	}
	
	private void PlayClipStartingAtRandomPoint(int clip) {
		this.GetComponent<AudioSource>().clip = clips[clip];
		this.GetComponent<AudioSource>().time = Random.Range(0, this.GetComponent<AudioSource>().clip.length);
		this.GetComponent<AudioSource>().Play();
		clipStarted = true;
	}
}
