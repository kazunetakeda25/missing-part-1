using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.ScriptControl)]
		[HutongGames.PlayMaker.Tooltip("Close Container Object")]	
	
	public class CloseContainerObject : FsmStateAction {
		public InteractablePickUpRotateOpen objectToOpen;
		
		public override	void OnEnter() {
			objectToOpen.CloseObject();
			Finish();
		}
		
	}
	
}
