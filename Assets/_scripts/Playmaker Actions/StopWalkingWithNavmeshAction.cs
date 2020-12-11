using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.AI;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Movement)]
		[HutongGames.PlayMaker.Tooltip("Tell a NavMeshAgent to stop moving.")]	
	
	public class StopWalkingWithNavMesh : FsmStateAction {
		public NavMeshAgent navMeshAgent;
		public GameObject ActorFSM;
		
		public override	void OnEnter() {
			navMeshAgent.Stop();
			navMeshAgent.gameObject.GetComponentInChildren<PlayMakerFSM>().SendEvent(WalkWithNavmesh.IDLE_LOOP_EVENT);
			Finish();
		}
		
	}
	
}
