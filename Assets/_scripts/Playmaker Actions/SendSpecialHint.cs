using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
    public class SendSpecialHint : FsmStateAction
    {	
		public FsmEvent eventToRunOnDone;
		
		private LevelManager levelManager;
		
        public override void OnEnter()
        {
			PC.GetPC().inspector.DisableInspection();
			ShowHint();
        }
		
		private void ShowHint() {
			SpecialHint.Create(FinishHint);
		}
		
		public void FinishHint() {
			PC.GetPC().inspector.EnableInspection();
			Fsm.Event(eventToRunOnDone);
			Finish();
		}
    }
}