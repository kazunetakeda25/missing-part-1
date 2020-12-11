using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.GUI)]
		[HutongGames.PlayMaker.Tooltip("Hide EZ GUI Button.")]	
	
	public class EZGUISetButtonActive : FsmStateAction {
		public UIButton[] buttonsToSet;
		public bool active;
		
		public override	void OnEnter() {
			foreach(UIButton buttonToSet in buttonsToSet)
				buttonToSet.controlIsEnabled = active;
			
			Finish();
		}
		
	}
	
}
