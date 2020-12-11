using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions 
{	
	
	[ActionCategory(ActionCategory.ScriptControl)]
		[HutongGames.PlayMaker.Tooltip("Complete Vignette.")]	
	
	public class CompleteVignette : FsmStateAction 
	{	
		public override	void OnEnter() 
		{
			SessionManager.GetSessionManager().vignetteManager.VignetteComplete();
			Finish();
		}
	}
	
	[ActionCategory(ActionCategory.ScriptControl)]
		[HutongGames.PlayMaker.Tooltip("Complete Vignette.")]	
	
	public class SetVignette : FsmStateAction 
	{	
		public Vignette.VignetteID vignette;
		
		public override	void OnEnter() 
		{
			SessionManager.GetSessionManager().vignetteManager.SetVignette(vignette);
			Finish();
		}
	}	
	
}
