using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Lights)]
		[HutongGames.PlayMaker.Tooltip("Open Container Object")]	
	
	public class ActivatePulseLight : FsmStateAction {
		public PulsingLight pulseLight;
		public enum SwitchType {
			On,
			Off,
			Toggle
		}
		
		public SwitchType action;
		
		public override	void OnEnter() {
			if(action == SwitchType.On)
				pulseLight.PulseOn();
			
			if(action == SwitchType.Off)
				pulseLight.PulseOff();
			
			if(action == SwitchType.Toggle)
				pulseLight.TogglePulse();
			
			Finish();
		}
		
	}
	
}
