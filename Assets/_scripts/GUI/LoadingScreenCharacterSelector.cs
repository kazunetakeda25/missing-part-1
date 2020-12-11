using UnityEngine;
using System.Collections;

public class LoadingScreenCharacterSelector : MonoBehaviour {
	
	public enum Character {
		Mike,
		Stephanie,
		Chris,
		None
	}
	
	public GameObject mikeModel;
	public GameObject chrisModel;
	public GameObject stephanieModel;
	
	public bool ActivateCharacterForLevel(string level) {
		bool charPresent = true;
		
		switch(level) {
		case Levels.EPISODE1:
			ActivateCharacter(Character.Mike);
			break;
		case Levels.EPISODE2:
			ActivateCharacter(Character.Stephanie);
			break;
		case Levels.EPISODE3:
			ActivateCharacter(Character.Chris);
			break;
		default:
			charPresent = false;
			break;
		}
		
		return charPresent;
	}
	
	private void ActivateCharacter(Character actor) {
		GameObject goToActivate = null;
		
		switch(actor) {
		case Character.Mike:
			goToActivate = mikeModel;
			break;
		case Character.Stephanie:
			goToActivate = stephanieModel;
			break;
		case Character.Chris:
			goToActivate = chrisModel;
			break;
		}
		
		TurnOnMesh(goToActivate);
	}
	
	private void TurnOnMesh(GameObject actorGO) {
		actorGO.SetActiveRecursively(true);
	}
	
}
