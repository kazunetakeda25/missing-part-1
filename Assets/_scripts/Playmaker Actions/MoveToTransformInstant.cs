using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {

	[ActionCategory(ActionCategory.Transform)]
		[HutongGames.PlayMaker.Tooltip("Start Particle System")]	
	
	public class MoveToTransformInstant : FsmStateAction {
		
		public GameObject objectToMove;
		public Transform target;
		public bool doRotation;
		
		public override	void OnEnter() 
		{
			UnityEngine.AI.NavMeshAgent objectNMA = objectToMove.GetComponent<UnityEngine.AI.NavMeshAgent>();
			if(objectNMA != null) 
				objectNMA.enabled = false;
			
			objectToMove.transform.position = target.position;
			
			if(doRotation)
				objectToMove.transform.rotation = target.rotation;
			
			if(objectNMA != null)
				objectNMA.enabled = true;
			
			Finish();
		}
		
	}	
	
}
