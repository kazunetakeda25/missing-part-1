using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {

	[ActionCategory(ActionCategory.Effects)]
		[HutongGames.PlayMaker.Tooltip("Start Particle System")]	
	
	public class StartParticleEmitter : FsmStateAction {
		
		//public ParticleEmitter particleEmitter;
		
		public override	void OnEnter() 
		{
			//particleEmitter.emit = true;
			Finish();
		}
		
	}		
	
	public class StartParticleSystem2 : FsmStateAction {
		
		public ParticleSystem particle;
		
		public override	void OnEnter() 
		{
			particle.Play();
			Finish();
		}
		
	}	
	
}
