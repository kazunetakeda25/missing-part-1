/*
 * Copyright 2010 - David Koontz
 * Please submit bug reports and feature requests/suggestions to david@koontzfamily.org
 */

using UnityEngine;
using System.Collections;

public class AccelerateTyping : MonoBehaviour {
	
	public KeyCode[] keysToAccelerateTyping;
	public float acceleratedCharactersPerSecond = 40;
	
	Typewriter typewriter;
	float originalCharactersPerSecond;
	bool accelerated = false;
	
	public void Start() {
		typewriter = GetComponent<Typewriter>()	?? GetComponentInChildren<Typewriter>();
		originalCharactersPerSecond = typewriter.charactersPerSecond;
	}
	
	public void Update () {
		if(typewriter.Active) {
			var accelerate = false;
			foreach(var key in keysToAccelerateTyping) {
				if(Input.GetKey(key)) {
					accelerate = true;
				}
			}
			
			if(!accelerated && accelerate) {
				accelerated = true;
				originalCharactersPerSecond = typewriter.charactersPerSecond;
				typewriter.charactersPerSecond = acceleratedCharactersPerSecond;
			}
			else if(accelerated && !accelerate){
				accelerated = false;
				typewriter.charactersPerSecond = originalCharactersPerSecond;
			}
		}
	}
}