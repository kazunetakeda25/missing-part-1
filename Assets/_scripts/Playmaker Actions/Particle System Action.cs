using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {

	[ActionCategory(ActionCategory.Effects)]
		[HutongGames.PlayMaker.Tooltip("Start Particle System")]	
	
	public class StartParticleSystem : FsmStateAction {
		
		public ParticleSystem particle;
		
		public override	void OnEnter() 
		{
			particle.Play();
			Finish();
		}
		
	}	
	
}
