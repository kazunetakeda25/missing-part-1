using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]

public class PulsingLight : MonoBehaviour {
	
	public float pulseSpeed = 1;
	public bool pulsing = true;
	private bool pulsingUp;
	
	void Update () {
		if(pulsing) {
		
			if(pulsingUp) {
				if(this.GetComponent<Light>().intensity >= 8) {
					pulsingUp = false;
				} else {
					this.GetComponent<Light>().intensity += Time.deltaTime * pulseSpeed;
				}
			} else {
				if(this.GetComponent<Light>().intensity <= 0) {
					pulsingUp = true;
				} else {
					this.GetComponent<Light>().intensity -= Time.deltaTime * pulseSpeed;
				}
			}
			
		}
	}
	
	public void PulseOn() {
		pulsing = true;
		this.GetComponent<Light>().intensity = 0;
	}
	
	public void PulseOff() {
		pulsing = false;
		this.GetComponent<Light>().intensity = 0;
	}
	
	public void TogglePulse() {
		pulsing =! pulsing;
		this.GetComponent<Light>().intensity = 0;
	}
}
