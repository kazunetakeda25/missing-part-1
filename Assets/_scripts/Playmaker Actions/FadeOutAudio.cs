using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Audio)]
		[HutongGames.PlayMaker.Tooltip("Fade Out Audio Source over time")]	
	
	public class AudioTo : FsmStateAction {
		public AudioSource audio;
		public FsmFloat targetVolume = 0;
		public FsmFloat targetPitch = 1;
		public FsmFloat timeToTarget = 5;
		public FsmFloat delay = 0;
		public iTween.EaseType easeType = iTween.EaseType.linear;
		
		private bool iTweenCreated = false;
		
		public override	void OnEnter() {
			iTween.AudioTo(audio.gameObject, iTween.Hash(
				"audiosource", audio,
				"volume", targetVolume.Value,
				"pitch", targetPitch.Value,
				"time", timeToTarget.Value,
				"delay", delay.Value,
				"easetype", easeType));
		}
		
		public override void OnUpdate() {
			if(audio.gameObject.GetComponent<iTween>() != null) {
				iTweenCreated = true;
			}
			
			if(iTweenCreated && audio.gameObject.GetComponent<iTween>() == null) {
				Finish();
			}
		}
		
	}
	
}
