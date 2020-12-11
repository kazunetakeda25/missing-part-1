using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
    public class CheckpointSaver : FsmStateAction
    {	
		public int checkpointToSave;

		public override void OnEnter ()
		{
			if(MissingComplete.SaveGameManager.Instance == null) {
				Finish();
				return;
			}

			MissingComplete.SaveGameManager.Instance.GetCurrentSaveGame().checkPoint = checkpointToSave;
			MissingComplete.SaveGameManager.Instance.SaveCurrentGame();
			Finish();
		}
    }
}