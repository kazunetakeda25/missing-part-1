using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.ScriptControl)]
		[HutongGames.PlayMaker.Tooltip("Force Player to inspect an object")]
	
	public class ForceInspectObject : FsmStateAction {
		public InteractablePickUpRotate clue;
		public FsmEvent eventToRunOnDone;
		
		private InspectObjectScreen inspectObjectScreen;
		
		public override	void OnEnter() {
			inspectObjectScreen = InspectObjectScreen.InspectObject(clue);
			inspectObjectScreen.inspectObjectDoneEvent += new InspectObjectDone(FinishInspect);
		}
		
		public void FinishInspect() {
			inspectObjectScreen.inspectObjectDoneEvent -= new InspectObjectDone(FinishInspect);
			Fsm.Event(eventToRunOnDone);
			Finish();
		}	
		
	}
	
}
