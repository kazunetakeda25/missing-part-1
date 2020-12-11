using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class SoundFXRNG : MonoBehaviour {
	//This class:
	//1.) Randomly places this object in the world (among the transforms given)
	//2.) Randomly starts the a sound effect based on the interval given.
	//3.) Randomly selects a sound effect to play based on the clips given.
	
	public AudioClip[] clips;
	public Transform[] possiblePositions;
	public float minInterval = 5;
	public float maxInterval = 20;
	private float currentInterval;
	
	private void Start() 
	{
		DetermineNextClip();
		currentInterval = DetermineIntervalLength();
	}
	
	void Update () 
	{
		if(currentInterval == float.NaN) {
			if(!this.GetComponent<AudioSource>().isPlaying)
				currentInterval = DetermineIntervalLength();
		} else {
			currentInterval -= Time.deltaTime;
			if(currentInterval <= 0) {
				PlaySFX();
			}
		}
	}
	
	private void DetermineNextClip() {
		this.GetComponent<AudioSource>().clip = clips[Random.Range(0, clips.Length)];
		minInterval += this.GetComponent<AudioSource>().clip.length;
		maxInterval += this.GetComponent<AudioSource>().clip.length;
	}
	
	private void PlaySFX() {
		minInterval -= this.GetComponent<AudioSource>().clip.length;
		maxInterval -= this.GetComponent<AudioSource>().clip.length;
		
		this.transform.position = possiblePositions[Random.Range(0, possiblePositions.Length)].position;
		this.GetComponent<AudioSource>().Play();
		currentInterval = float.NaN;
		
		DetermineNextClip();
	}
	
	private float DetermineIntervalLength() {
		return Random.Range(minInterval, maxInterval);
	}
}
