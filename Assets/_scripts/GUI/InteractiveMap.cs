using UnityEngine;
using System.Collections;

public class InteractiveMap : MonoBehaviour {
	
	private const float RAY_DEPTH = 10f;
	private const float PLAYER_SPEED = 2f;
	
	public enum SupportedRooms {
		Kitchen,
		LivingRoom,
		Bathroom,
		Bedroom,
		CancelButton
	}
	
	public Transform kitchenSpot;
	public Transform bedroomSpot;
	public Transform livingroomSpot;
	public Transform bathroomSpot;
	
	public Camera mapCamera;
	public Camera orthoCam;
	
	public GameObject mapObject;
	
	private InteractiveMapHotspot currentHotSpot;
	
	public void EnableMap() {
		ReportEvent.ScreenActivated(ScreenType.Map);
		
		FreezePlayer();
		PlayMakerFSM.BroadcastEvent(GlobalPlaymakerEvents.HIDE_GUI);
		
		SetupCameras(true);
		mapObject.GetComponent<Renderer>().enabled = true;
	}
	
	public void DisableMap() {
		ReportEvent.ScreenDeactivated(ScreenType.Map);
		
		ShowClueHint.DestroyHints();
		currentHotSpot = null;
		
		SetupCameras(false);
		mapObject.GetComponent<Renderer>().enabled = false;
	}
	
	private void SetupCameras(bool on) {
		mapCamera.enabled = on;
		orthoCam.enabled = on;
		PC.GetPC().GetActiveCamera().enabled = !on;
	}
	
	private void Update() {
		if(Time.timeScale == 0) {
			HandlePaused();
			return;
		}
		
		if(Input.GetMouseButtonUp(0) && currentHotSpot != null) {
			if(currentHotSpot.room == SupportedRooms.CancelButton) {
				OnCancelButtonPressed();
				return;
			}
			
			MovePlayer();
		}
		
		if(mapCamera.enabled) //We don't want the map to work while paused
			HandleMapInteraction();
	}
	
	private void HandlePaused() {
		if(currentHotSpot != null) {
			ShowClueHint.DestroyHints();
			currentHotSpot = null;
		}
	}
	
	private void HandleMapInteraction() {
		GameObject hotSpotGO = PerformRayCast();
		if(hotSpotGO != null && hotSpotGO.tag == Tags.MAP_HOT_SPOT_TAG) {
			InteractiveMapHotspot newHotSpot = hotSpotGO.GetComponent<InteractiveMapHotspot>();
			
			if(newHotSpot == currentHotSpot)
				return;
			
			currentHotSpot = newHotSpot;
			
			if(currentHotSpot.room == SupportedRooms.CancelButton){
				ShowClueHint.DestroyHints();
				return;
			}
			
			ShowClueHint.ShowMoveTo(GetRoomDisplayName(currentHotSpot.room));
		} else {
			ShowClueHint.DestroyHints();
			currentHotSpot = null;
		}		
	}
	
	private void MovePlayer() {
		ReportEvent.MapUsed(currentHotSpot.room);
		
		UnFreezePlayer();
		PC.GetPC().ForcePlayerMove(GetRoomPosition(currentHotSpot.room), PLAYER_SPEED);
		
		DisableMap();		
	}
	
	private void OnCancelButtonPressed() {
		Debug.Log("Cancel Pressed");
		PlayMakerFSM.BroadcastEvent(GlobalPlaymakerEvents.SHOW_GUI);
		DisableMap();
		UnFreezePlayer();
	}
	
	private void FreezePlayer() {
		PC pc = PC.GetPC();
		pc.FreezePlayer();
		pc.inspector.DisableInspection();
	}
	
	private void UnFreezePlayer() {
		PC pc = PC.GetPC();
		pc.UnFreezePlayer();
		pc.inspector.EnableInspection();
	}
	
	private string GetRoomDisplayName(SupportedRooms room) {
		switch(room) {
		case SupportedRooms.Bathroom:
			return "Bathroom";
		case SupportedRooms.Bedroom:
			return "Bedroom";
		case SupportedRooms.Kitchen:
			return "Kitchen";
		case SupportedRooms.LivingRoom:
			return "Living Room";
		}
		
		Debug.LogError("Invalid Room!");
		return "Bad ROOM!";
	}
				
	private Vector3 GetRoomPosition(SupportedRooms room) {
		switch(room) {
		case SupportedRooms.Bathroom:
			return bathroomSpot.position;
		case SupportedRooms.Bedroom:
			return bedroomSpot.position;
		case SupportedRooms.Kitchen:
			return kitchenSpot.position;
		case SupportedRooms.LivingRoom:
			return livingroomSpot.position;
		}
		
		Debug.LogError("Invalid Room!");
		return Vector3.zero;
	}
	
	private GameObject PerformRayCast()
	{
		GameObject retVal = null;
		
		Ray ray = orthoCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        
        Debug.DrawLine(ray.origin, ray.origin + (ray.direction * RAY_DEPTH));
		
		int layerMask = 1 << 6;
		
        if (Physics.Raycast( ray, out hit, RAY_DEPTH))
            retVal = hit.collider.gameObject;
		
		return retVal;
	}
	
}
