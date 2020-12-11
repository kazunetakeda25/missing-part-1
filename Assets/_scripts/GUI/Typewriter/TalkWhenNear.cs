/*
 * Copyright 2010 - David Koontz
 * Please submit bug reports and feature requests/suggestions to david@koontzfamily.org
 */

using UnityEngine;
using System.Collections;

public class TalkWhenNear : MonoBehaviour {
	
	public bool orientSpeechBubbleToFaceCamera = true;
	public bool orientIconToFaceCamera = true;
	
	public bool hideAfterEndOfText = true;
	public float hideDelayAfterEndOfText = 1.5f;
	
	public Transform icon;
	public Transform speechBubble;
	Typewriter typewriter;
	bool started = false;
	bool completed = false;
	bool running = false;
	
	public void Start() {
		if(orientIconToFaceCamera) {
			if(icon != null) {
				icon.GetComponent<Billboard>().enabled = true;
			}
		}
		typewriter = GetComponent<Typewriter>();
		HideSpeechBubble();
	}

	public void OnTriggerEnter(Collider other) {
		ShowSpeechBubble();
		HideIcon();
		typewriter.Write();
		running = true;
		if(!started) {
			started = true;
			StartCoroutine(CheckForCompletion());
		}
	}
	
	public void OnTriggerStay() {
		if(orientSpeechBubbleToFaceCamera) {
			if(speechBubble != null) {
				speechBubble.LookAt(Camera.main.transform.position);
			}
		}
	}
	
	public void OnTriggerExit(Collider other) {
		HideSpeechBubble();
		
		if(!completed) {
			ShowIcon();
		}
		typewriter.Stop();
	}
	
	IEnumerator CheckForCompletion() {
		while(running) {
			if(!completed && typewriter.Completed) {
				MarkComplete();
			}
			yield return new WaitForSeconds(0.1f);	
		} 
	}
	
	void MarkComplete() {
		completed = true;
		running = false;
		if(hideAfterEndOfText) {
			StartCoroutine(HideSpeechBubble(hideDelayAfterEndOfText));
		}
		
		HideIcon();
	}
	
	void HideSpeechBubble() {
		if(speechBubble != null) {
			foreach(var t in speechBubble.transform) {
				(t as Transform).gameObject.GetComponent<Renderer>().enabled = false;
			}
		}
	}
	
	IEnumerator HideSpeechBubble(float delay) {
		yield return new WaitForSeconds(delay);
		
		HideSpeechBubble();
	}
	
	void ShowSpeechBubble() {
		if(speechBubble != null) {
			foreach(var t in speechBubble.transform) {
				(t as Transform).gameObject.GetComponent<Renderer>().enabled = true;
			}
		}
	}
	
	void HideIcon() {
		if(icon != null) {
			foreach(var t in icon.transform) {
				(t as Transform).gameObject.active = false;
			}
		}
	}
	
	void ShowIcon() {
		if(icon != null) {
			foreach(var t in icon.transform) {
				(t as Transform).gameObject.active = true;
			}
		}
	}
}