using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.ScriptControl)]
		[HutongGames.PlayMaker.Tooltip("Have First and Third Person Cameras Track")]
	
	public class EnableAttachemnts : FsmStateAction 
	{	
		
		public int maxAttachmentsAllowed;
		
		public override void OnEnter ()
		{
			LevelManager.FindLevelManager().PhotoManager.EnableAttachments(maxAttachmentsAllowed);
			Finish();
		}
	}
	
}
