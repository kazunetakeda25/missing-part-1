using UnityEngine;
using System.Collections;

public class GameInitializer : MonoBehaviour {
	
	private void Awake() {
		Settings.NewGameSession();
		LevelLoader.StaticLoad(Levels.MAIN_MENU);
	}
}
