using UnityEngine;
using System.Collections;

public class DontDestroyThisOnLoad : MonoBehaviour {

	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}
	
}
