using UnityEngine;
using System.Collections;

public class DebugPerspectiveToggle : MonoBehaviour {

	public UIRadioBtn firstButton;
	public UIRadioBtn thirdButton;
	
	private void Awake() {
		if(Settings.IsFirstPerson())
			firstButton.Value = true;
		else
			thirdButton.Value = true;
	}
	
	public void ThirdHit() {
		
		if(Settings.IsFirstPerson()) {
			Settings.SetFirstPerson(false);
		}
		
		PC.GetPC().SetCurrentCamera();
	}
	
	public void FirstHit() {
		Debug.Log("First Hit");
		if(!Settings.IsFirstPerson()) {
			Settings.SetFirstPerson(true);
		}
		
		PC.GetPC().SetCurrentCamera();
	}
	
}
