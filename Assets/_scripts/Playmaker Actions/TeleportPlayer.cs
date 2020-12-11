using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Character)]
		[HutongGames.PlayMaker.Tooltip("Teleport Player Instantly")]
	
	public class TeleportPlayer : FsmStateAction
	{
		public Transform newPosition;
		
		public override void OnEnter ()
		{
			GameObject player = PC.GetPC().gameObject;
			UnityEngine.AI.NavMeshAgent nma = player.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
			if(nma != null) {
				Debug.Log("NMA Found" + nma.name);
				nma.enabled = false;
			}
			
			TransformTools.TransformPosRot(player, newPosition);
			
			if(nma != null) {
				nma.enabled = true;
			}
			
			Finish();
		}
		
	}
	
}
