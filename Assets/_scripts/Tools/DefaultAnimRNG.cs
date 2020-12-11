using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation))]

public class DefaultAnimRNG : MonoBehaviour {

	//Someday we should add multiple animation support.
	private float delay;
	
	void Awake () {
		this.GetComponent<Animation>()[this.GetComponent<Animation>().clip.name].time = Random.Range(0, this.GetComponent<Animation>().clip.length);
		this.GetComponent<Animation>().Play();
	}
	
	public void Update() {
		delay -= Time.deltaTime;
		if(delay <= 0)
			this.GetComponent<Animation>().Play();
	}

}
