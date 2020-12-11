using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.GUI)]
	
	public class SpawnTheoryHint : FsmStateAction 
	{
		
		public override void OnEnter ()
		{
			TheoryHint.SpawnTheoryHint();
			Finish();
		}
		
	}
	
}
