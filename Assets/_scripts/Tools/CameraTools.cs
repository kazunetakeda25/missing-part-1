using UnityEngine;
using System.Collections;

public static class CameraTools {
	
	public const string MAIN_CAMERA_TAG = "MainCamera";
	public const string UNTAGGED = "Untagged";
	
	public static void MakeMainCamera(Camera cam) {
		//Make sure whatever is current Main Camera is untagged.
		if(Camera.main != null)
			Camera.main.tag = UNTAGGED;
		
		cam.tag = MAIN_CAMERA_TAG;
		cam.enabled = true;
	}
	
}
