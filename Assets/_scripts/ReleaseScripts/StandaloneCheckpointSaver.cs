using UnityEngine;
using System.Collections;

namespace MissingComplete 
{
	public class StandaloneCheckpointSaver : MonoBehaviour 
	{
		public int checkpointToSave;

		private void Start()
		{
			if(SaveGameManager.Instance == null)
				return;
			SaveGameManager.Instance.GetCurrentSaveGame().checkPoint = checkpointToSave;
			SaveGameManager.Instance.SaveCurrentGame();
		}
	}
}

