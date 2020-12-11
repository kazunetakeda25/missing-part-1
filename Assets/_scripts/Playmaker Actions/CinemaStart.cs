using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Camera)]
		[HutongGames.PlayMaker.Tooltip("Have First and Third Person Cameras Track")]
	
	public class CinemaStart : FsmStateAction 
	{
		private const string FIRST_PERSON_CINEMA_CAMERA_PREFAB = "cameras/First Person Cinema Camera";
		private const string THIRD_PERSON_CINEMA_CAMERA_PREFAB = "cameras/Third Person Cinema Camera";
		
		public Transform initialLookPoint;
		public float initialLookSpeed = 0;
		
		public bool freezePlayer;
		
		//Take Control from Player
		//Create Cinema Cameras from Prefab
		
		public override void OnEnter ()
		{
			PC pc = PC.GetPC();
			
			if(freezePlayer)
				pc.FreezePlayer();
			
			pc.inspector.DisableInspection();
			
			GameObject firstPivot = pc.firstPersonCamera.transform.parent.gameObject;
			GameObject thirdPivot = pc.thirdPersonCamera.transform.parent.gameObject;
			
			GameObject firstPersonCameraCinema = (GameObject) GameObject.Instantiate(Resources.Load(FIRST_PERSON_CINEMA_CAMERA_PREFAB));
			GameObject thirdPersonCameraCinema = (GameObject) GameObject.Instantiate(Resources.Load(THIRD_PERSON_CINEMA_CAMERA_PREFAB));
			
			firstPersonCameraCinema.transform.parent = firstPivot.transform;
			thirdPersonCameraCinema.transform.parent = thirdPivot.transform;
			
			if(Settings.IsFirstPerson()) 
			{
				firstPersonCameraCinema.GetComponent<Camera>().enabled = true;
				firstPersonCameraCinema.tag = Tags.MAIN_CAMERA_TAG;
				pc.firstPersonCamera.enabled = false;
				pc.firstPersonCamera.tag = Tags.UNTAGGED;
				TransformTools.TransformPosRot(firstPersonCameraCinema, firstPivot.transform);
			}
			else
			{
				thirdPersonCameraCinema.GetComponent<Camera>().enabled = true;
				thirdPersonCameraCinema.tag = Tags.MAIN_CAMERA_TAG;
				pc.thirdPersonCamera.enabled = false;
				pc.thirdPersonCamera.tag = Tags.UNTAGGED;
				TransformTools.TransformPosRot(thirdPersonCameraCinema, thirdPivot.transform);
			}
			
			if(initialLookPoint != null) {
				iTween.LookTo(firstPivot, initialLookPoint.position, initialLookSpeed);
				iTween.LookTo(thirdPivot, initialLookPoint.position, initialLookSpeed);
			}
			
			Finish();
		}
		
	}
	
}
