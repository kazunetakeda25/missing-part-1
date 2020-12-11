using UnityEngine;
using System.Collections;

namespace MissingComplete
{
	public class GameLoader 
	{
		private static GameLoader instance;
		private GameLoader() { }
		public static GameLoader Instance 
		{ 
			get 
			{ 
				if(instance == null) {
					instance = new GameLoader();
				}

				return instance; 
			} 
		}

		private bool isGameLoaded = false;
		public bool IsGameLoaded { get { return isGameLoaded; } }
		private int checkpointLoaded = -1;

		private bool faderSet = false;

		public int GetLoadedCheckpoint()
		{
			if(checkpointLoaded == -1) {
				Debug.LogError("No Checkpoint Loaded!!");
			}

			int checkPoint = checkpointLoaded;
			checkpointLoaded = -1;
			isGameLoaded = false;
			return checkPoint;
		}

		public void Load(int checkPoint)
		{
			faderSet = false;

			if(CheckCheckpointValid(checkPoint) == false) {
				Debug.LogError("Invalid Checkpoint Passed! Over max allowed.");
				return;
			}

			Debug.Log("Loading Game...");

			this.checkpointLoaded = checkPoint;

			if(Fader.Instance != null) {
				Fader.Instance.FadeOut();
				Fader.Instance.fadeOutComplete += LoadGame;
				faderSet = true;
				return;
			}

			LoadGame();
		}

		private bool CheckCheckpointValid(int checkPoint)
		{
			if(checkPoint > SaveGameManager.SaveGame.TOTAL_CHECKPOINTS) {
				return false;
			}

			return true;
		}

		private void LoadGame()
		{
			if(faderSet == true) {
				Fader.Instance.fadeOutComplete -= LoadGame;
			}

			isGameLoaded = true;

			switch(checkpointLoaded) {
			case 1:
				Application.LoadLevel("MY_MUG");
				//SessionManager.Instance.SetStartScene(Episode.INTRO_VIDEO);
				break;
			case 3:
				Application.LoadLevel("INTRODUCTION");
				break;
			case 5:
			case 15:
			case 25:
				Application.LoadLevel("EP1_TerrysApartment");
				//SessionManager.Instance.SetStartScene(Episode.TERRYS_APARTMENT);
				break;
			case 30:
				Application.LoadLevel("AAR1");
				break;
			case 40:
			case 43:
			case 46:
			case 49:
			case 52:
				Application.LoadLevel("Ep2_GPCOffice");
				//SessionManager.Instance.SetStartScene(Episode.NEWAAR1);
				break;
			case 55:
				Application.LoadLevel("AAR2");
				break;
			case 65:
			case 68:
			case 71:
			case 75:
				Application.LoadLevel("Ep3_WhiskeyBar");
				//SessionManager.Instance.SetStartScene(Episode.THUNDERJAW);
				break;
			case 80:
				Application.LoadLevel("AAR3");
				break;
			case 90:
				Application.LoadLevel("CONCLUSION");
				break;
			default:
				Debug.LogWarning("Invalid Checkpoint!");
				break;
			}
		}
	}

}
