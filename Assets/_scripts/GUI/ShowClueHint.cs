using UnityEngine;
using System.Collections;

public class ShowClueHint : MonoBehaviour {
	
	public const string HINT_PREFIX = "Inspect ";
	public const string MOVE_TO_PREFIX = "Move to ";
	
	public static void ShowHint(string objectName) {
		DestroyHints();
		InstantiateHint(HINT_PREFIX + objectName);
	}
	
	public static void ShowMoveTo(string objectName) {
		DestroyHints();
		InstantiateHint(MOVE_TO_PREFIX + objectName);
	}
	
	private static void InstantiateHint(string textToShow) {
		GameObject clueHintObj = (GameObject) GameObject.Instantiate(Resources.Load(ResourcePaths.CLUE_HINT_MACHINE), Vector3.zero, Quaternion.identity);
		clueHintObj.GetComponent<ClueHintMachine>().hintText.text = textToShow;		
	}
	
	public static void DestroyHints() {
		GameObject[] clues = GameObject.FindGameObjectsWithTag(Tags.CLUE_HINT_MACHINE_TAG);
		for (int i = 0; i < clues.Length; i++) {
			Destroy(clues[i]);
		}
	}
	
	
	
}
