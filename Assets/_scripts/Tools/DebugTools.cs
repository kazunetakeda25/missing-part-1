using UnityEngine;
using System.Collections;

public class DebugTools {
	
	public static void PrintMainCamera() {
		Debug.Log("Main Camera Object is:" + Camera.main);
		
		if(GameObject.FindGameObjectsWithTag(Tags.MAIN_CAMERA_TAG).Length > 1)
			Debug.LogWarning("More than one Main Camera exists in this scene!");
	}
	
	public static void PrintTransform(Transform subject) {
		Debug.Log(subject.name + " Transform Position: " + subject.transform.position);
		Debug.Log(subject.name + " Transform LocalPosition: " + subject.transform.localPosition);
		Debug.Log(subject.name + " Transform EulerAngles: " + subject.transform.eulerAngles);
		Debug.Log(subject.name + " Transform LocalEulerAngles: " + subject.transform.localEulerAngles);
		Debug.Log(subject.name + " Transform Scale: " + subject.transform.localScale);
	}
	
	public static void DiffTransforms(Transform subject1, Transform subject2) {
		bool perfect = true;
		
		if(subject1.position != subject2.position) {
			Debug.Log("Positons are not the same!");
			Debug.Log(subject1.name + " Position: " + subject1.position);
			Debug.Log(subject2.name + " Position: " + subject2.position);
			perfect = false;
		}
		
		if(subject1.rotation != subject2.rotation) {
			Debug.Log("Rotations are not the same!");
			Debug.Log(subject1.name + " Rotation: " + subject1.rotation);
			Debug.Log(subject2.name + " Position: " + subject2.rotation);
			perfect = false;
		}

		if(subject1.localScale != subject2.localScale) {
			Debug.Log("Scales are not the same!");
			Debug.Log(subject1.name + " Scale: " + subject1.localScale);
			Debug.Log(subject2.name + " Scale: " + subject2.localScale);
			perfect = false;
		}		
		
		if(perfect)
			Debug.Log(subject1.name + " and " + subject2.name + " have identical transforms!");
	}
	
	public static bool isDebugMode() {
		if(Application.isEditor || Debug.isDebugBuild)
			return true;
		
		return false;
	}
	
}
