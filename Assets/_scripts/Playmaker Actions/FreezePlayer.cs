using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {

	[ActionCategory(ActionCategory.ScriptControl)]
		[HutongGames.PlayMaker.Tooltip("Freeze PC")]
	
	public class FreezePlayer : FsmStateAction 
	{
		
		public override void OnEnter ()
		{
			PC.GetPC().FreezePlayer();
			Finish();
		}
		
	}
	
}
