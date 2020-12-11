using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.ScriptControl)]
		[HutongGames.PlayMaker.Tooltip("Move Player with Nav Mesh to a point")]	
	
	public class MovePlayerToPoint : FsmStateAction {
		
		public Transform newPosition;
		public float speedToMove = 2;
		public FsmEvent eventToFireOnComplete;
		
		private Vector3 deltaPosition;
        private float closeValue;
		private PC pc;

		public override void OnEnter() {
			pc = PC.GetPC();
            pc.ForcePlayerMove(newPosition.position, speedToMove);
		}
		
		public override void OnUpdate() {
            if(pc != null)
			{
				if(!pc.IsForcePlayerMoveActive())
					AllDone();
			}
		}
		
		private void AllDone() {
			Fsm.Event(eventToFireOnComplete);
			Finish();
		}
		
	}
	
}
