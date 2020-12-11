using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Camera)]
		[HutongGames.PlayMaker.Tooltip("Player Brain: Freeze Player Movement Completely")]
	
	public class CinemaCameraTrackObject : FsmStateAction
	{
		public Transform objectToTrack;
		public float lookToSpeed;
		public float timeToTrack;
		public FsmEvent eventToTrigger;
		
		private float timer;
		private GameObject firstPersonCameraPivot;
		private GameObject thirdPersonCameraPivot;
		
		public override void OnEnter ()
		{
			//Get Parent Pivot instead of actual cameras so this works with cinema cameras and reg cameras.
			firstPersonCameraPivot = PC.GetPC().firstPersonCamera.transform.parent.gameObject;
			thirdPersonCameraPivot = PC.GetPC().thirdPersonCamera.transform.parent.gameObject;
			
			if(timeToTrack > 0)
				timer = timeToTrack;
		}
		
		public override void OnUpdate()
		{
			timer -= Time.deltaTime;
			
			if(timer < 0) 
				AllDone();
		}
		
		public override void OnLateUpdate ()
		{

			iTween.LookUpdate(firstPersonCameraPivot, objectToTrack.position, lookToSpeed);
			iTween.LookUpdate(thirdPersonCameraPivot, objectToTrack.position, lookToSpeed);
			
		}
		
		private void AllDone() {
			if(eventToTrigger != null)
				this.Fsm.Event(eventToTrigger);
			
			Finish();
		}
		
	}
	
}
