using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine.AI;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Movement)]
		[HutongGames.PlayMaker.Tooltip("NavMesh Tool: Use Playmaker to set a new destination for a NavMeshAgent.")]
	
	public class WalkWithNavmesh : FsmStateAction {
		
		public const string ACTOR_FSM_NAME = "Actor";
		public const string IDLE_EVENT = "Idle";
		public const string IDLE_LOOP_EVENT = "IdleLoop";
		public const string WALK_EVENT = "StartWalk";	
		
		public NavMeshAgent navMeshAgent;
		public Transform destination;
		public float speed = 1;
		public float acceleration = 2;
		public float angularSpeed = 120;
		public float stoppingDistance = 0.2f;
		public string WalkAnimationOverride = null;
		private bool pathSet = false;
		private bool pathCalculated = false;
		public float closeEnoughValue = 0.55f;
		private float speedCutOff = 0.015f;
		private Vector3 deltaPosition;
		
		public override void OnEnter() {
			deltaPosition = navMeshAgent.transform.position;
			navMeshAgent.SetDestination(destination.position);
			navMeshAgent.speed = speed;
			navMeshAgent.acceleration = acceleration;
			navMeshAgent.stoppingDistance = stoppingDistance;
			pathSet = true;
			pathCalculated = false;
			if(WalkAnimationOverride == "")
				navMeshAgent.gameObject.GetComponentInChildren<PlayMakerFSM>().SendEvent(WALK_EVENT);
			else
				navMeshAgent.gameObject.GetComponentInChildren<Animation>().GetComponent<Animation>().CrossFade(WalkAnimationOverride, 0.1f, PlayMode.StopSameLayer);
		}
		
		public override void OnUpdate() {
			float speed = Vector3.Distance(navMeshAgent.transform.position, deltaPosition);
			deltaPosition = new Vector3(navMeshAgent.transform.position.x, navMeshAgent.transform.position.y, navMeshAgent.transform.position.z);
			
			//Debug.Log("Distance: " + Vector3.Distance(navMeshAgent.transform.position, destination.position));
			//Debug.Log("Speed " + speed);
			
			if(Vector3.Distance(navMeshAgent.transform.position, destination.position) <= closeEnoughValue && speed <= speedCutOff) {
				navMeshAgent.gameObject.GetComponentInChildren<PlayMakerFSM>().SendEvent(IDLE_LOOP_EVENT);
				Finish();
			}
		}
		
	}
	
}
