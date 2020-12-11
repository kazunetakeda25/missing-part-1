using UnityEngine;
using System.Collections;

public class DoneSearchingBroadcaster : MonoBehaviour {
	
	public const string DONE_SEARCHING_EVENT = "DoneSearching";
	public UIButton DoneButton;
	public SpriteText DoneButtonTxt;
	public GUIInputBlocker blocker;
	
	public void DoneSearching() {
		PlayMakerFSM.BroadcastEvent(DONE_SEARCHING_EVENT);
		Destroy(this.transform.parent.gameObject);
	}
	
	public void HideDoneButton() {
		DoneButton.Hide(true);
		DoneButtonTxt.GetComponent<Renderer>().enabled = false;
		blocker.GetComponent<Renderer>().enabled = false;
	}
	
	public void ShowDoneButton() {
		DoneButton.Hide(false);
		DoneButtonTxt.GetComponent<Renderer>().enabled = true;
		blocker.GetComponent<Renderer>().enabled = true;
	}
}
