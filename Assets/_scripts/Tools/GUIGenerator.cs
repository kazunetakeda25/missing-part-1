using UnityEngine;
using System.Collections;

public class GUIGenerator : MonoBehaviour {
	
	private const float defaultBoxTopOffset = 10;
	private const float defaultBoxLeftOffset = 10;
	private const float defaultBoxWidth = 500;
	private const float defaultBoxHeight = 25;
	private const float defaultBoxSpacing = 20;
	
	public static void CreateStandardLabel(string label) {
		CreateStandardLabel(label, GUI.contentColor, GUI.backgroundColor);
	}
	
	public static void CreateStandardLabel(string label, Color contentColor, Color backgroundColor) {
		string[] labels = new string[1] {label};
		CreateStandardLabels(labels, contentColor, backgroundColor);
	}	
	
	public static void CreateStandardLabels(string[] labels) {
		CreateStandardLabels(labels, GUI.contentColor, GUI.backgroundColor);
	}
	
	public static void CreateStandardLabels(string[] labels, Color contentColor, Color backgroundColor) {
		//Color defaultBackgroundColor = GUI.backgroundColor;
		//Color defaultContentColor = GUI.contentColor;
		//Apparently this is how you set colors in GUI Unity
		//GUI.backgroundColor = backgroundColor;
		//GUI.contentColor = contentColor;
		
		for (int i = 0; i < labels.Length; i++) {
			float yPos = defaultBoxTopOffset + (i * defaultBoxSpacing);
			GUI.Label(new Rect(defaultBoxLeftOffset, yPos, defaultBoxWidth, defaultBoxHeight), labels[i]);
		}
		
		//Restore GUI Colors to default
		//GUI.backgroundColor = backgroundColor;
		//GUI.contentColor = contentColor;
	}
}
