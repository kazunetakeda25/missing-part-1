using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Audio)]
		[HutongGames.PlayMaker.Tooltip("This simply runs Play() followed by a quick Finish() on an audiosource.")]	
	
	public class UnPauseAudio : FsmStateAction {
		public AudioSource audioSource;
		public float delay = 0;
		
		public override	void OnEnter() {
			if(delay == 0)
				PlayAndFinish();
		}
		
		public override void OnUpdate() {
			delay -= Time.deltaTime;
			if(delay <= 0)
				PlayAndFinish();
		}
		
		private void PlayAndFinish() {
			audioSource.Play();
			Finish();
		}
		
	}
	
}
