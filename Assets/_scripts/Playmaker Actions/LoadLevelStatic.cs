using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Level)]
	public class LoadLevelStatic : FsmStateAction 
	{
		public string levelToLoad;
		
		public override void OnEnter ()
		{
			LevelLoader.StaticLoad(levelToLoad);
			Finish();
		}
	}
	
}
