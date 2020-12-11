using UnityEngine;
using System.Collections;

public class PhoneResponseBubble : MonoBehaviour {
	
	public TextMesh bubbleText;
	public UIButton bubble;
	private string eventToBroadcast;
	private PhoneResponseGrid parentGrid;
	
	public void SetupBubble(string bubbleContent, string eventToBroadcast, PhoneResponseGrid parentGrid) {
		bubbleText.text = bubbleContent;
		this.eventToBroadcast = eventToBroadcast;
		this.parentGrid = parentGrid;
	}
	
	public void OnPressed() {
		PlayMakerFSM.BroadcastEvent(eventToBroadcast);
		Destroy(parentGrid.gameObject);
	}
	
}
