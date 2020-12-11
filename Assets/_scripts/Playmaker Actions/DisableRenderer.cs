using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Renderer)]
	
	public class DisableRenderer : FsmStateAction 
	{
		public Renderer renderer;
		
		public override void OnEnter ()
		{
			renderer.enabled = false;
			Finish();
		}
	}
	
}
