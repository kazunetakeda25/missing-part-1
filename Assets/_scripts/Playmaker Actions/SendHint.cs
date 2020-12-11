using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
    public class SendHint : FsmStateAction
    {	
		public string hint;
		public FsmEvent eventToRunOnDone;
		public bool ShowRegardlessOfHintSetting;
		public SimpleHint.PopUpType type;
		
		private LevelManager levelManager;
		
        public override void OnEnter()
        {
			PC.GetPC().inspector.DisableInspection();
			
			if(ShowRegardlessOfHintSetting || Settings.HintsOn())
				ShowHint();
			else
				FinishHint();
        }
		
		private void ShowHint() {
			SimpleHint.CreateSimpleHint(type, hint, FinishHint);
		}
		
		public void FinishHint() {
			PC.GetPC().inspector.EnableInspection();
			Fsm.Event(eventToRunOnDone);
			Finish();
		}
    }
}