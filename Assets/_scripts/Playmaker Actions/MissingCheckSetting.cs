using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.ScriptControl)]
	
	public class MissingCheckSetting : FsmStateAction
	{
		public enum SupportedSetting {
			isFirstPerson,
			isLongDuration,
			hintsOn,
			storiesOn
		}
		
		public SupportedSetting setting;
		public FsmEvent eventOnTrue;
		public FsmEvent eventOnFalse;
		
		public override void OnEnter ()
		{
			if(GetBool())
				Fsm.Event(eventOnTrue);
			else
				Fsm.Event(eventOnFalse);
			
			Finish();
		}
		
		private bool GetBool() {
			switch(setting) {
			case SupportedSetting.hintsOn:
				return Settings.HintsOn();
			case SupportedSetting.isFirstPerson:
				return Settings.IsFirstPerson();
			case SupportedSetting.isLongDuration:
				return Settings.IsLongDuration();
			case SupportedSetting.storiesOn:
				return Settings.StoryArchiveOn();
			default:
				return false;
			}
		}
		
	}
	
}
