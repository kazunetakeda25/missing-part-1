using UnityEngine;
using System.Collections;

public class PlayerResponseGrid : MonoBehaviour {
	
	//This Monobehavior is designed to be hooked up to a GameObject that is saved as a prefab and instantiated at the start.
	//This prefab should also have a camera that only shows the PlayerGrid's layer to allow for easier position-free instantiation.
	
	public Transform[] responsePositions9;
	public Transform[] responsePositions9Lower;
	public Transform[] responsePositions8;
	public Transform[] responsePositions7;
	public Transform[] responsePositions6;
	public Transform[] responsePositions5;
	public Transform[] responsePositions4Lower;
	public Transform[] responsePositions4;
	public Transform[] responsePositions3Lower;
	public Transform[] responsePositions3;
	public Transform[] responsePositions2Lower;
	public Transform[] responsePositions2;
	public Transform[] responsePositions1Lower;
	public Transform[] responsePositions1;
	
	public GameObject PlayerResponseBubblePrefab;
	public GameObject PlayerMultiResponseBubblePrefab;
	
	private string[] responseTexts;
	private string[] events;
	private Transform[] responseTransformToUse;
	
	private int multiSelectMaximum;
	private int selectedCount = 0;
	private string eventToFire;
	private GameObject imDoneBubble;
	
	public void SetupResponseGrid(PlayerResponseStore.PlayerResponse playerResponse, bool lower)
	{ 
		SetupResponseGrid(playerResponse, lower, false, 0); 
	}
	
	public void SetupResponseGrid(PlayerResponseStore.PlayerResponse playerResponse, bool lower, bool multi, int multiSelectMaximum) 
	{
		selectedCount = 0;
		this.multiSelectMaximum = multiSelectMaximum;
		
		responseTexts = playerResponse.responses;
		events = playerResponse.events;
		SetupTransformGrid(responseTexts.Length, lower);		
		
		if(multi)
			PopulateMultiResponseGrid();
		else
			PopulateResponseGrid();
	}
	
	public void MultiChoiceSelected()
	{
		selectedCount++;
		
		if(selectedCount >= multiSelectMaximum)
			SelfDestruct();
		
		CheckLastBubble();
	}
	
	public void MultiChoiceUnselected()
	{
		selectedCount--;
		CheckLastBubble();
	}
	
	//This functionality is for adding/destroying the last bubble when players select an answer.
	private void CheckLastBubble()
	{
		
		if(selectedCount > 0 && imDoneBubble == null)
		{	
			SetupImDoneBubble();
		}
		
		if(selectedCount <= 0)
		{
			if(imDoneBubble != null)
				Destroy(imDoneBubble);
			
			selectedCount = 0;
		}
		
	}
	
	private void SetupImDoneBubble()
	{
		int index = responseTexts.Length - 1;
		
		GameObject playerResponseBubbleGO = InstantiateBubble(PlayerMultiResponseBubblePrefab, index);
		
		PlayerResponseBubbleMultiSelect bubble = playerResponseBubbleGO.GetComponent<PlayerResponseBubbleMultiSelect>();
		//Im Done Bubble is at -2 Index
		bubble.SetupResponseBubble(responseTexts[index], events[index], this);
		bubble.selfDestructing = true;
			
		imDoneBubble = bubble.gameObject;
	}
	
	private void PopulateResponseGrid() 
	{
        PlayerResponseMemory.Instance().ClearResponses();
		for (int i = 0; i < responseTexts.Length; i++) 
		{
			//Instantiate a new Player Response Bubble for each Response Entry we have.
			GameObject playerResponseBubbleGO = InstantiateBubble(PlayerResponseBubblePrefab, i);
			
			//Setup the Response Bubble.
			playerResponseBubbleGO.GetComponent<PlayerResponseBubble>().SetupResponseBubble(responseTexts[i], events[i], this);
			
			CheckBubbleMemory(responseTexts[i], playerResponseBubbleGO);
		}
	}
	
	private void PopulateMultiResponseGrid() {
		PlayerResponseMemory.Instance().ClearResponses();
		for (int i = 0; i < responseTexts.Length; i++) 
		{
			//I'm Done Button is at -1, we don't show this initially becuase we want to force the palyer to hit at least one choice.
			if(i == (responseTexts.Length - 1))
			{
				eventToFire = events[responseTexts.Length - 1];
				return;
			}
			
			//Instantiate a new Player Response Bubble for each Response Entry we have.
			GameObject playerResponseBubbleGO = InstantiateBubble(PlayerMultiResponseBubblePrefab, i);
			
			PlayerResponseBubbleMultiSelect bubble = playerResponseBubbleGO.GetComponent<PlayerResponseBubbleMultiSelect>();
			bubble.SetupResponseBubble(responseTexts[i], events[i], this);
			
			//If this is the repeat button, we want to self-destruct
			if(i == (responseTexts.Length -2))
				bubble.selfDestructing = true;
		}
	}
	
	private GameObject InstantiateBubble(GameObject prefabToInstantiate, int index)
	{
		GameObject bubbleGO = (GameObject) GameObject.Instantiate(prefabToInstantiate, responseTransformToUse[index].position, Quaternion.identity);
		bubbleGO.transform.localScale = responseTransformToUse[index].localScale;
		
		//Setup New Object as a child of this grid, to make it easier to delete later.
		bubbleGO.transform.parent = this.gameObject.transform;
		
		return bubbleGO;
	}
	
	private void CheckBubbleMemory(string responseText, GameObject bubbleGO)
	{
		if(PlayerResponseMemory.Instance().CheckResponseExist(responseText))
		{
			//Response Exists, darken shader.
			bubbleGO.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.25f, 0.25f, 0.25f, 1.0f));
		}
	}
	
	private void SetupTransformGrid(int numberOfResponses, bool lower) {
		switch(numberOfResponses) {
		case 9:
			if(lower)
				responseTransformToUse = responsePositions9Lower;
			else
				responseTransformToUse = responsePositions9;
			break;
		case 8:
			responseTransformToUse = responsePositions8;
			break;
		case 7:
			responseTransformToUse = responsePositions7;
			break;
		case 6:
			responseTransformToUse = responsePositions6;
			break;
		case 5:
			responseTransformToUse = responsePositions5;
			break;
		case 4:
			if(lower)
				responseTransformToUse = responsePositions4Lower;
			else
				responseTransformToUse = responsePositions4;
			break;
		case 3:
			if(lower)
				responseTransformToUse = responsePositions3Lower;
			else
				responseTransformToUse = responsePositions3;
			break;
		case 2:
			if(lower)
				responseTransformToUse = responsePositions2Lower;
			else
				responseTransformToUse = responsePositions2;
			break;
		case 1:
			if(lower)
				responseTransformToUse = responsePositions1Lower;
			else
				responseTransformToUse = responsePositions1;
			break;
		default:
			Debug.LogError("Invalid number of Responses sent to Grid.  Please use one of the valid responses set#s or setup a new one.");
			break;
		}		
	}
	
	private void SelfDestruct()
	{
		PlayMakerFSM.BroadcastEvent(eventToFire);
		GameObject.Destroy(this.gameObject);
	}
	
}
