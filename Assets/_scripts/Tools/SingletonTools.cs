using UnityEngine;
using System.Collections;

public class SingletonTools {

	public static void FailSafe(string tag) {
		
		GameObject inspectObjectGO = GameObject.FindGameObjectWithTag(tag);
		if(inspectObjectGO != null) {
			GameObject.Destroy(inspectObjectGO);
			Debug.LogError("Broken Singleton: " + tag + " GameObject already exists!!!  This should never occur.");
		}
		
	}	
	
}
