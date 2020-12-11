using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {

	[ActionCategory(ActionCategory.ScriptControl)]
		[HutongGames.PlayMaker.Tooltip("Unfreeze PC")]
	
	public class UnFreezePlayer : FsmStateAction 
	{
		
		public override void OnEnter()
		{
			PC.GetPC().UnFreezePlayer();
			Finish();
		}
		
	}
	
}
