using UnityEngine;
using System.Collections;

public class PlayerResponseBubbleMultiSelect : MonoBehaviour {	
	
	public const string SubtitleMachineTag = "SubtitleMachine";
	
	public SpriteText bubbleText;
	public bool selfDestructing;
	public UIButton myButton;
	private string fsmEventString = "";
	private PlayerResponseGrid parentGrid;
	
	private bool registered = false;
	private Color defaultColor;
	
	//TODO: Add Fade In on Start
	
	public void SetupResponseBubble(string text, string eventToSend, PlayerResponseGrid parentGrid) {
		bubbleText.text = text;
		this.fsmEventString = eventToSend;
		this.parentGrid = parentGrid;
	}
	
	public void BubblePressed() {
		if(selfDestructing)
			SelfDestruct();
		else
			RegisterHit();
	}
	
	private void Start()
	{
		defaultColor = myButton.Color;
	}
	
	private void RegisterHit() {
		if(!registered)
		{
			PlayMakerFSM.BroadcastEvent(fsmEventString);
			myButton.SetColor(new Color(0, 1, 0, 1));
			parentGrid.MultiChoiceSelected();
			registered = true;
		} else
		{
			PlayMakerFSM.BroadcastEvent(fsmEventString);
			myButton.SetColor(defaultColor);
			parentGrid.MultiChoiceUnselected();
			registered = false;
		}
	}
	
	private void SelfDestruct()
	{
		PlayerResponseMemory.Instance().RegisterResponse(bubbleText.text);
		PlayMakerFSM.BroadcastEvent(fsmEventString);
		GameObjectTools.DestroyAllObjectsWithTag(SubtitleMachineTag);
		Destroy(parentGrid.gameObject);		
	}		
	
}
