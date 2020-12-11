using UnityEngine;
using System.Collections;

public class PlayerResponseBubble : MonoBehaviour {	
	
	public const string SubtitleMachineTag = "SubtitleMachine";
	
	public SpriteText bubbleText;
	private string fsmEventString = "";
	private PlayerResponseGrid parentGrid;
	
	//TODO: Add Fade In on Start
	
	public void SetupResponseBubble(string text, string eventToSend, PlayerResponseGrid parentGrid) {
		bubbleText.text = text;
		this.fsmEventString = eventToSend;
		this.parentGrid = parentGrid;
	}
	
	public void BubblePressed() {
		//Debug.Log("Broadcasting Event: " + m_fsmEventString);
		PlayerResponseMemory.Instance().RegisterResponse(bubbleText.text);
		PlayMakerFSM.BroadcastEvent(fsmEventString);	
		GameObjectTools.DestroyAllObjectsWithTag(SubtitleMachineTag);
		Destroy(parentGrid.gameObject);
	}
	
}
