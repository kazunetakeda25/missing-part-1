using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions 
{
	
	[ActionCategory(ActionCategory.GUI)]
	public class RecordAttachments : FsmStateAction 
	{
		
		public override	void OnEnter() 
		{	
			PhotoManager photoManager = LevelManager.FindLevelManager().PhotoManager;
			photoManager.RecordAttachments();
			Finish();
		}
		
	}
	
}
