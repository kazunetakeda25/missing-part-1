using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.GUI)]
	public class DestroyReponseGrids : FsmStateAction 
	{
		
		public override void OnEnter ()
		{
			GameObject[] gos = GameObject.FindGameObjectsWithTag(Tags.PLAYER_RESPONSE_GRID);
			for (int i = 0; i < gos.Length; i++) {
				GameObject.Destroy(gos[i]);
			}
			Finish();
		}
		
	}
	
}

