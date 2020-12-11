using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.ScriptControl)]
		[HutongGames.PlayMaker.Tooltip("Open Container Object")]	
	
	public class OpenContainerObject : FsmStateAction {
		public InteractablePickUpRotateOpen objectToOpen;
		
		public override	void OnEnter() {
			objectToOpen.OpenObject();
			Finish();
		}
		
	}
	
}
