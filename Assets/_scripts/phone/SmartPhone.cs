using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using HutongGames.PlayMaker;

//The Smartphone Prefab (Containing this script) should be placed in the scene manually.  
//It can be placed anywhere as the prefab and the associated scripts are completely self-sufficient.
public class SmartPhone : MonoBehaviour {
	
	public delegate void ActionDelegate();
	
	private const string PANEL_SMS = "SMS";
	private const string PANEL_ALBUM = "PhotoAlbum";
	
	private ActionDelegate phoneOpenDelegate;
	private ActionDelegate phoneClosedDelegate;
	private ActionDelegate smsOpenDelegate;
	private ActionDelegate albumOpenDelegate;
	
	public enum Mode {
		SMS,
		PhotoAlbum
	}
	
	private enum PhoneState {
		PendingSMS,
		Normal,
		OffScreen,
		CleanUp
	}
	
	public Camera phoneCamera;
	public Camera phoneGUICamera;
	public GameObject arms;
	public Transform offScreenPosition;
	public Transform onScreenPosition;
	
	public UIPanelManager smartPhoneUIPanelManager;
	public PhotoAlbum photoAlbum;
	public SMSView smsView;
	
	public UIButton hideButton;
	public UIButton photoAlbumButton;
	public UIButton smsButton;
	
	private float inTime = 0.5f;
	private float outTime = 0.5f;
	
	private PhoneState phoneState = PhoneState.OffScreen;
	private List<SMS> smsQ = new List<SMS>();
	
	private Mode mode;
	
	//Static
	
	public static SmartPhone GetSmartPhone() {
		GameObject smartPhoneGO = GameObject.FindGameObjectWithTag(Tags.SMARTPHONE_TAG);
		
		if(smartPhoneGO == null) {
			Debug.Log("Smartphone Object not instantiated!!");
			return null;
		}
		
		return smartPhoneGO.GetComponent<SmartPhone>();
	}
	
	//Public
	
	public void OnSMSClicked() {
		OpenPhone(Mode.SMS);
	}
	
	public void OnAlbumClicked() {
		OpenPhone(Mode.PhotoAlbum);
	}
	
	public void DisableAllButtons() {
		hideButton.controlIsEnabled = false;
		smsButton.controlIsEnabled = false;
		photoAlbumButton.controlIsEnabled = false;
	}
	
	public void EnableAllButtons() {
		hideButton.controlIsEnabled = true;
		smsButton.controlIsEnabled = true;
		photoAlbumButton.controlIsEnabled = true;
	}	
	
	public void SetPhoneOpenDelegate(ActionDelegate method) {
		phoneOpenDelegate = method;
	}
	
	public void SetPhoneClosedDelegate(ActionDelegate method) {
		phoneClosedDelegate = method;
	}
	
	public void SetSMSOpenedDelegate(ActionDelegate method) {
		smsOpenDelegate = method;
	}
	
	public void SetAlbumOpenedDelegate(ActionDelegate method) {
		albumOpenDelegate = method;
	}	
	
	private void Awake() {
		HOTween.Init();
	}
	
	public void OpenPhone(Mode mode) {
		OpenPhone(mode, null);
	}
	
	public void OpenPhone(Mode mode, SMS message) {
		
		ReportEvent.PhoneActivated(mode);
		ReportEvent.ScreenActivated(ScreenType.Phone);
		
		this.mode = mode;
		
		if(message != null)
			smsQ.Add(message);
		
		if(phoneState == PhoneState.OffScreen) {
			phoneState = PhoneState.PendingSMS;
			ActivatePhoneScreen();
		} else {
			PendingSMS();
		}
	}
	
	//Private
	
	private void SetupCameras(bool on) 
	{
		phoneCamera.enabled = on;
		phoneGUICamera.enabled = on;
		PC.GetPC().GetActiveCamera().enabled = !on;
	}
	
	private void ActivatePhoneScreen() 
	{	
		PC pc = PC.GetPC();
		pc.FreezePlayer();
		pc.inspector.DisableInspection();
		
		PlayMakerFSM.BroadcastEvent(GlobalPlaymakerEvents.HIDE_GUI);
		
		TransformTools.TransformPosRot(phoneCamera.gameObject, PC.GetPC().firstPersonCamera.transform);
		SetupCameras(true);
		
		AnimateArms(onScreenPosition.position, inTime);
		
		if(phoneOpenDelegate != null) 
		{
			phoneOpenDelegate();
			phoneOpenDelegate = null;
		}
		
		BringInCorrectPhoneGUI();
		
	}
	
	public void HidePhone() 
	{
		ReportEvent.ScreenDeactivated(ScreenType.Phone);
		
		AnimateArms(offScreenPosition.position, outTime);
		
		GameObject phoneResponseGO = GameObject.FindGameObjectWithTag(Tags.PLAYER_RESPONSE_GRID);
		if(phoneResponseGO != null)
			Destroy(phoneResponseGO);
		
		phoneState = PhoneState.CleanUp;
	}
	
	private void PendingSMS() {
		
		if(smsQ.Count > 0) {
			ShowSMS();
			return;
		}
		
		BringInCorrectPhoneGUI();
	}
	
	private void BringInCorrectPhoneGUI() {
		switch(mode) {
		case Mode.PhotoAlbum:
			if(albumOpenDelegate != null) 
			{
				albumOpenDelegate();
				albumOpenDelegate = null;
			}
			
			photoAlbum.RefreshAlbumScreen();
			smartPhoneUIPanelManager.BringIn(PANEL_ALBUM);
			break;
		case Mode.SMS:
			if(smsOpenDelegate != null) 
			{
				smsOpenDelegate();
				smsOpenDelegate = null;
			}
			smartPhoneUIPanelManager.BringIn(PANEL_SMS);
			break;
		}		
	}
	
	private void ShowSMS() {
		//Bring in Message Alert Screen
		smsView.PostSMS(smsQ[0]);
		smsQ.RemoveAt(0);
	}
	
	private void CleanUp() 
	{
		
		ReportEvent.PhoneDeactivated(this.mode);
		
		PC.GetPC().UnFreezePlayer();
		PC.GetPC().inspector.EnableInspection();
		PlayMakerFSM.BroadcastEvent(GlobalPlaymakerEvents.SHOW_GUI);
		PlayMakerFSM.BroadcastEvent(GlobalPlaymakerEvents.PHONE_CLOSED);
		
		SetupCameras(false);
		
		phoneState = PhoneState.OffScreen;
		
		if(phoneClosedDelegate != null) {
			phoneClosedDelegate();
			phoneClosedDelegate = null;
		}
	}
		
	private void AnimateArms(Vector3 newPosition, float duration) {
		TweenParms parms = new TweenParms();
		parms.Prop(HTProps.POSITION, newPosition);
		parms.Ease(EaseType.EaseOutQuad);
		parms.OnComplete(PostAnimationCheckIn);
		
		HOTween.To(arms.transform, duration, parms);
	}
	
	private void PostAnimationCheckIn() {
		switch(phoneState) {
		case PhoneState.CleanUp:
			CleanUp();
			break;
		case PhoneState.PendingSMS:
			PendingSMS();
			break;
		}
	}
	
}
