using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Camera)]
		[HutongGames.PlayMaker.Tooltip("Have First and Third Person Cameras Track")]
	
	public class CinemaEnd : FsmStateAction 
	{
		
		public bool unfreezePlayer;
		
		public override void OnEnter ()
		{
			PC pc = PC.GetPC();
			
			if(unfreezePlayer)
				pc.UnFreezePlayer();
			
			pc.inspector.EnableInspection();
			
			if(Settings.IsFirstPerson()) 
			{
				pc.firstPersonCamera.enabled = true;
				GameObject go = (GameObject) GameObject.FindGameObjectWithTag(Tags.MAIN_CAMERA_TAG);
				pc.firstPersonCamera.tag = Tags.MAIN_CAMERA_TAG;
				Debug.Log("Destroying: " + go.name);
				GameObject.Destroy(go);
			} else 
			{
				pc.thirdPersonCamera.enabled = true;
				GameObject go = (GameObject) GameObject.FindGameObjectWithTag(Tags.MAIN_CAMERA_TAG);
				pc.thirdPersonCamera.tag = Tags.MAIN_CAMERA_TAG;
				GameObject.Destroy(go);
			}
			
			Finish();
		}
		
	}
	
}
