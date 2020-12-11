using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.ScriptControl)]
		[HutongGames.PlayMaker.Tooltip("Listen to see how many photo attachments the players has flagged.")]	
	public class ListenForAttachments : FsmStateAction
	{
		//NOTE: If we cared about performance, we'd set this up as a listener, not as an Update.  But,
		//I don't want to touch PhotoManager since it's working well.
		
		public int NumberOfAttachmentsToTrigger;
		public FsmEvent eventToTrigger;
		
		private PhotoManager photoManager;
		private bool started;
		
		public override void OnEnter () {
			photoManager = LevelManager.FindLevelManager().PhotoManager;
			started = true;
		}
		
		public override void OnUpdate () {
			if(photoManager.GetAttachments().Count >= NumberOfAttachmentsToTrigger && started) {
				AllDone();
			}
			
			if(Input.GetKeyUp(KeyCode.Semicolon)) {
				if(Debug.isDebugBuild || Application.isEditor)
					AllDone();
			}
		}
		
		private void AllDone() {
			Fsm.Event(eventToTrigger);
			Finish();
		}
		
	}
	
	[ActionCategory(ActionCategory.ScriptControl)]
		[HutongGames.PlayMaker.Tooltip("Listen for photographs unattached.")]	
	public class ListenForUnattach : FsmStateAction
	{
		
		public FsmEvent eventToTrigger;
		
		private PhotoManager photoManager;
		private int startingAttachments;
		
		public override void OnEnter() {
			photoManager = LevelManager.FindLevelManager().PhotoManager;
			setStartingAttachments(photoManager.GetAttachments().Count);
		}
		
		private void setStartingAttachments(int count) {
			startingAttachments = count;
		}
		
		public override void OnUpdate () {
			int attachmentCount = photoManager.GetAttachments().Count;
			//Debug.Log("C: " + attachmentCount);
			if(attachmentCount < startingAttachments) {
				AllDone();
			}
		}
		
		private void AllDone() {
			GameObject.Destroy(GameObject.FindGameObjectWithTag(Tags.PLAYER_RESPONSE_GRID));
			Fsm.Event(eventToTrigger);
			Finish();
		}
		
	}	
	
}
