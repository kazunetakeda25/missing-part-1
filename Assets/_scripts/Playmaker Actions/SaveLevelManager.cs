using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Level)]
	public class SaveLevelManager : FsmStateAction 
	{
		
		public override void OnEnter ()
		{
			GameObject lmGO = GameObject.FindGameObjectWithTag (Tags.LEVEL_MANAGER_TAG);
			GameObject.DontDestroyOnLoad (lmGO);
			Finish();
		}
	}
	
}
