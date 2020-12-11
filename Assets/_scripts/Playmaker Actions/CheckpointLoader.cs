using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
    public class CheckpointLoader : FsmStateAction
    {	
		[System.Serializable]
		public class LoadState
		{
			public int checkpoint;
			public FsmEvent eventToFire;
		}

		public int debugCheckpoint;
		public LoadState[] loadStates;

		public override void OnEnter ()
		{
			if(MissingComplete.SaveGameManager.Instance == null) {
				Fsm.BroadcastEvent(GetEvent(debugCheckpoint));
				return;
			}

			Fsm.BroadcastEvent(GetEvent(MissingComplete.SaveGameManager.Instance.GetCurrentSaveGame().checkPoint));
		}

		private FsmEvent GetEvent(int checkpoint)
		{
			foreach (LoadState ls in loadStates) {
				if (ls.checkpoint == checkpoint) {
					return ls.eventToFire;
				}
			}
				
			return loadStates[0].eventToFire;
		}
    }
}