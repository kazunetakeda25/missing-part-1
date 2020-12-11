using UnityEngine;
using System.Collections;

public static class Tags {
	
	public const string UNTAGGED = "Untagged";
	
	public const string MAIN_CAMERA_TAG = "MainCamera";
	public const string PLAYER_TAG = "Player";
	
	public const string UIMANAGER_TAG = "UIManager";
	public const string LOADING_EFFECT_TAG = "Loading Effect";
	public const string LEVEL_MANAGER_TAG = "LevelManager";
	public const string SUBTITLE_MACHINE_TAG = "SubtitleMachine";
	public const string DONE_SEARCHING_TAG = "DoneSearching";
	public const string CLUE_HINT_MACHINE_TAG = "ClueHintMachine";
	public const string INSPECT_OBJECT_SCREEN_TAG = "InspectObjectScreen";
	public const string SMARTPHONE_TAG = "SmartPhone";
	public const string SESSION_MANAGER_TAG = "SessionManager";
	public const string MAP_HOT_SPOT_TAG = "MapHotSpot";
	public const string CHARACTER_SELECTOR = "CharacterSelector";
	
	public const string FIRST_PERSON_CAMERA_TRANSFORM = "First Person Camera Transform";
	public const string FIRST_PERSON_CINEMA_CAMERA = "First Person Cinema Camera";
	public const string THIRD_PERSON_CINEMA_CAMERA = "Third Person Cinema Camera";
	
	public const string GUI_CAMERA = "GUICamera";
	
	public const string PLAYER_RESPONSE_GRID = "ResponseGrid";
	
	public static GameObject FindGameObject(string tag) {
		GameObject go = GameObject.FindGameObjectWithTag(tag);
		CheckNull(go, tag);
		return go;
	}
	
	public static bool CheckNull(GameObject go, string tag) {
		if(go == null) {
			Debug.LogError("The GameObject Tagged " + tag + "does not exist in this scene!");
			return false;
		}
		
		return true;
	}
	
}
