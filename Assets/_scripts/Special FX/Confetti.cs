using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class Confetti : MonoBehaviour {
	
	public ParticleSystem[] confettiParticles;
	public Transform northeastCorner;
	public Transform northwestCorner;
	public Transform southwestCorner;
	public Transform southeastCorner;
	public float minInterval = 5;
	public float maxInterval = 10;
	private float currentInterval;
	
	private void Awake() 
	{
		currentInterval = DetermineIntervalLength();
	}
	
	private void Update() 
	{
		currentInterval -= Time.deltaTime;
		if(currentInterval <= 0) {
			FireConfetti();
		}
	}
	
	private void FireConfetti() 
	{
		this.transform.position = new Vector3
			(
				Random.Range(northwestCorner.position.x, northeastCorner.position.x),
				this.transform.position.y,
				Random.Range(southwestCorner.position.z, northwestCorner.position.z)
			);
		
		foreach(ParticleSystem confetti in confettiParticles)
		{
			confetti.Play();
		}
		
		this.GetComponent<AudioSource>().Play();
		currentInterval = DetermineIntervalLength();
	}
	
	private float DetermineIntervalLength() 
	{
		return Random.Range(minInterval, maxInterval);
	}
}
