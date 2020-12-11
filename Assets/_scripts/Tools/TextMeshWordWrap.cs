using UnityEngine;
using System.Collections;

[RequireComponent (typeof(TextMesh))]
public class TextMeshWordWrap : MonoBehaviour {
	
	public int lineLimit = 10;
	private TextMesh text;
	private string oldString;
	
	private void Awake() {
		text = this.gameObject.GetComponent<TextMesh>();
		Wrap(text.text);
		oldString = text.text;
	}
	
	private void Update() {
		if(oldString != text.text)
			Wrap(text.text);
		
		oldString = text.text;
	}
	
	public void Wrap(string textToDisplay) {
		if(textToDisplay.Length < lineLimit)
			SetText(textToDisplay);
		else
			SetText(StringTools.WordWrap(textToDisplay, lineLimit));
	}
	
	private void SetText(string textToDisplay) {
		text.text = textToDisplay;
	}
	
}
