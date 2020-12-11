using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Renderer)]
	
	public class EnableRenderer : FsmStateAction 
	{
		public Renderer renderer;
		
		public override void OnEnter ()
		{
			renderer.enabled = true;
			Finish();
		}
		
	}
	
}
