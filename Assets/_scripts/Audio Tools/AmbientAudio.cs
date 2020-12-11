using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("SFX Generators/Ambient Audio")]
public class AmbientAudio : MonoBehaviour
{
	// Will eventually make this take parameters for what sounds to use so it can
	// work for any episode, but for now, it's HCed for Episode 1. - HP
	
	public AudioSource m_ambientSource;
	public float m_delayMultiplier = 3.0f;
	public float m_baseDelay = 5.0f;
	public float m_volume = 0.1f;
	
	public List<AudioClip> m_soundFiles = null;
	
	private float m_delayUntilNextSound = 2.0f;
	private AudioClip m_clipToPlay;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		m_delayUntilNextSound = m_delayUntilNextSound - Time.deltaTime;
		
		if(m_delayUntilNextSound <= 0.0f)
		{
			// Pick one of our ambient sounds at random.
			int d3 = UnityEngine.Random.Range(0, m_soundFiles.Count - 1);
			
			m_clipToPlay = m_soundFiles[d3];
			
//			switch(d3)
//			{
//				case 1:
//				{
//					m_clipToPlay = Resources.Load("Audio/Episode 1/cricketChirp")as AudioClip;
//					break;
//				}				
//				case 2:
//				{
//					m_clipToPlay = Resources.Load("Audio/Episode 1/policeSirenDriveBy")as AudioClip;
//					break;
//				}
//				case 3:
//				{
//					m_clipToPlay = Resources.Load("Audio/Episode 1/helicopterFlyBy")as AudioClip;
//					break;
//				}
//			}		
			
			// Delay the next sound by at least slightly more than the length of the
			// selected clip.
			m_delayUntilNextSound = m_baseDelay + (m_clipToPlay.length * m_delayMultiplier);
			
			// Play the clip.
			m_ambientSource.volume = m_volume;
			m_ambientSource.PlayOneShot(m_clipToPlay);
		}		
	}
}
