using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Camera)]
		[HutongGames.PlayMaker.Tooltip("Have First and Third Person Cameras Track")]
	
	public class CinemaCameraLookObject : FsmStateAction 
	{
		public GameObject objectToLookAt;
		public float duration;
		public float delay;
		
		private float delayTimer;
		
		public override void OnEnter ()
		{
			
			if(delay > 0) {
				StartDelayTimer();
				return;
			}
			
			StartLook();
			
		}
		
		public override void OnUpdate ()
		{
			
			if(delay > 0 && delayTimer > 0) 
			{
				delayTimer -= Time.deltaTime;
				if(delayTimer <= 0)
					StartLook();
			}
			
		}
		
		private void StartDelayTimer() 
		{			
			delayTimer = delay;
		}
		
		private void StartLook() 
		{
			
			GameObject actionDelegateObject = (GameObject) new GameObject("Cinema Camera look: Action Delegate", typeof(ActionFinishObject));
			actionDelegateObject.GetComponent<ActionFinishObject>().setAction(LookComplete);
			
			//Create iTween
			//Use Parent Object so this always works for both cinema cameras and player cameras
			GameObject firstPersonCameraPivot = PC.GetPC().firstPersonCamera.transform.parent.gameObject;
			GameObject thridPersonCameraPivot = PC.GetPC().thirdPersonCamera.transform.parent.gameObject;
			
			Hashtable hash = iTween.Hash(
				"looktarget", objectToLookAt.transform.position,
				"time", duration,
				"oncompletetarget", actionDelegateObject,
				"oncomplete", "Finish");
			
			//Move Parent so we don't have to move the Player's camera as well.
			iTween.LookTo(firstPersonCameraPivot, hash);
			iTween.LookTo(thridPersonCameraPivot, hash);

		}
		
		public void LookComplete() 
		{
			Finish();
		}
		
	}
	
}
