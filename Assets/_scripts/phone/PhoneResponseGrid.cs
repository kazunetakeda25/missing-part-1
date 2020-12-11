using UnityEngine;
using System.Collections;

public class PhoneResponseGrid : MonoBehaviour {
	
	public const int MAX_RESPONSES = 4;
	
	public GameObject phoneResponseBubbleLeft;
	public GameObject phoneReponseBubbleRight;
	public Transform[] playerResponsePositions;
	
	private string[] responses;
	private string[] events;
	
	public void SetupReponseGrid(PlayerResponseStore.PlayerResponse playerResponses) {
		responses = playerResponses.responses;
		events = playerResponses.events;
		PopulateReponseGrid();
	}
	
	private void PopulateReponseGrid() {
		for (int i = 0; i < responses.Length; i++) {
			GameObject bubbleGO = (GameObject) GameObject.Instantiate(FetchCorrectResponseBubblePrefab(i), playerResponsePositions[i].position, playerResponsePositions[i].rotation);
			bubbleGO.transform.parent = this.transform;
			bubbleGO.GetComponent<PhoneResponseBubble>().SetupBubble(responses[i], events[i], this);
		}
	}
	
	private GameObject FetchCorrectResponseBubblePrefab(int index) {
		if(index % 2 == 0)
			return phoneResponseBubbleLeft;
		
		return phoneReponseBubbleRight;
	}
	
}
