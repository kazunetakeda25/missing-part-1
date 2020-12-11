using UnityEngine;
using System.Collections;

public class SimpleHint : MonoBehaviour {
	
	public delegate void ActionDelegate();
	
	public enum PopUpType {
		Hint,
		Error,
		Tutorial
	}
	
	public SpriteText titleTxt;
	public SpriteText bodyTxt;
	public SpriteText buttonTxt;
	
	public AudioClip error;
	public AudioClip hint;
	
	public ActionDelegate action;
	
	public void SetupHint(PopUpType type, string body, string button) {
		if(type == PopUpType.Error)
			GetComponent<AudioSource>().PlayOneShot(error);
		
		if(type == PopUpType.Hint)
			GetComponent<AudioSource>().PlayOneShot(hint);
		
		titleTxt.Text = GetTitleString(type);
		bodyTxt.Text = body;
		buttonTxt.Text = button;
	}
	
	private string GetTitleString(PopUpType type) {
		switch(type) {
		case PopUpType.Error:
			return "Error";
		case PopUpType.Hint:
			return "Hint";
		case PopUpType.Tutorial:
			return "Tutorial";
		}
		
		return "Hint";
	}
	
	public void DoneButtonPressed() {
		if(action != null)
			action();
		
		Destroy(this.gameObject);
	}
	
	public static void CreateSimpleHint(PopUpType type, string body) 
	{
		CreateSimpleHint(type, body, "OK", null);
	}

	public static void CreateSimpleHint(PopUpType type, string body, ActionDelegate onOKButtonHit) 
	{
		CreateSimpleHint(type, body, "OK", onOKButtonHit);
	}
	
	public static void CreateSimpleHint(PopUpType type, string body, string button, ActionDelegate onOKButtonHit) 
	{
		ReportEvent.HintSent(type, body);
		
		GameObject simpleHintGO = (GameObject) GameObject.Instantiate(Resources.Load("gui/SimpleHint"));
		SimpleHint simpleHint = simpleHintGO.GetComponent<SimpleHint>();
		simpleHint.SetupHint(type, body, button);
		
		if(onOKButtonHit != null) {
			simpleHint.action = onOKButtonHit;
		}
	}
	
}
