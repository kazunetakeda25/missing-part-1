using UnityEngine;
using System.Collections;

[RequireComponent (typeof(GlowEffect))]
public class AnimateGlowEffect : MonoBehaviour {

	public float minGlowIntensity = 1f;
	public float maxGlowIntensity = 2f;
	public float glowChangeSpeed = 1f;
	private bool glowUp;
	
	private GlowEffect glowEffect;
	
	private void Start() {
		glowEffect = this.gameObject.GetComponent<GlowEffect>();
	}
	
	public void Update() {
		ModifyGlow();
	}
	
	private void ModifyGlow() {
		if(glowUp) {
			glowEffect.glowIntensity += Time.deltaTime * glowChangeSpeed;
		} else {
			glowEffect.glowIntensity -= Time.deltaTime * glowChangeSpeed;
		}
		
		if(glowEffect.glowIntensity > maxGlowIntensity)
			glowUp = false;
		else if(glowEffect.glowIntensity < minGlowIntensity)
			glowUp = true;
	}
	
}
