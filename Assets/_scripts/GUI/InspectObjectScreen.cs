using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

//BackCODE: For anything that needs to know when the Inspect Object Screen Done button has been pressed.
public delegate void InspectObjectDone();

public class InspectObjectScreen : MonoBehaviour
{	
	public enum ScrollDirection {
		Up,
		Down,
		Left,
		Right
	}
	
	public const int DEFAULT_LAYER = 0;
	public const float DEFAULT_CAMERA_FOV = 60.0f;
	public const float OBJECT_PICK_UP_TIME = 1.0f;
    public const float OBJECT_PUT_DOWN_TIME = 1.0f;
	
	private const string ALBUM_HINT = "Use the Album icon on your phone to review the photos you have taken.";
	private const string INSPECT_HINT = "Inspect the object with the zoom and rotate buttons.  If you think it's important take a picture";
	
	//BackCODE: Event Handler 
	public event InspectObjectDone inspectObjectDoneEvent;
	
	//Big Hooks
	public InspectObjectGUI inspectObjectGUI;
	public InspectObjectInteractiveHandler inspectObjectInteractiveHandler;
	public Camera inspectObjectCamera;
	public Camera backgroundCamera;
	public Camera anchorCamera;
	//public DepthOfFieldScatter dofFX;
    public Transform floatingObjectSpot;
	
	//Properties
	public bool CanManuallyZoom() { return canManuallyZoom; }
	private bool canManuallyZoom;
	
	public bool IsAnchored() { return anchored; }
	private bool anchored;
	
	//members
    private float objectPickUpTime = 1.0f;
    private float objectPutDownTime = 1.0f;
	private float rotationSpeed = 250.0f;
	private float scrollSpeed = 0.1f;
	private float minFOV = 25.0f;
	private float maxFOV = 90.0f;
	private float anchoredTimeFactor = 1f;
	
    private float objectAddlDistanceFromCamera = 1.0f;
	
	private InteractablePickUpRotate inspectObject;
    private Vector3 objectTargetPosition;
    private Quaternion objectTargetOrient;
	
    private Vector3 objectOrigPosition;
    private Vector3 objectOrigOrient;
	private int originLayer;
	
	private bool object2D;
	private bool trackObject = false;
	
	private float origFOVInspectCamera;
	private float currFOVInspectCamera;
	private float origFOVBackgroundCamera;
	private float currFOVBackgroundCamera;
	
	private Quaternion lookAtBeginRotation;
	private Vector3 lookAtBeginZoomPos;
	private Vector3 lookAtEndZoomPos;
	
	private Camera myMainCamera;
	
	private PC player;
	private PhotoSaver photoSaver;
	
	public static InspectObjectScreen InspectObject(InteractableWorldObject objectToInspect) {
		SingletonTools.FailSafe(Tags.INSPECT_OBJECT_SCREEN_TAG);
		GameObject inspectObjectScreenGO = (GameObject) GameObject.Instantiate(Resources.Load(ResourcePaths.INSPECT_OBJECT_SCREEN), Vector3.zero, Quaternion.identity);
		InspectObjectScreen ios = inspectObjectScreenGO.GetComponent<InspectObjectScreen>();
		
		ios.Init(objectToInspect as InteractablePickUpRotate);
		
		ReportEvent.InspectObject(objectToInspect.name);
		
		return ios;
	}
	
	//Public methods
	
	public void Init(InteractablePickUpRotate objectToInspect) {
		player = PC.GetPC();
		InitializePhotoSaver();
		inspectObject = objectToInspect;
		BeginInteraction();
		
		ReportEvent.ScreenActivated(ScreenType.InspectObject);
	}
	
	public void TakePhoto() {
		photoSaver.TakePhoto(inspectObject);
	}
	
	//Private methods
	
	private void InitializePhotoSaver() {
		photoSaver = new PhotoSaver();
		photoSaver.Init(this, inspectObjectGUI.takePhotoButton);
	}
	
	private void Update() {
		//if(trackObject) {
		//	dofFX.focalLength = Vector3.Distance(inspectObject.transform.position, anchorCamera.transform.position);
		//}
	}	
	
	//Called for ALL objects when they are first inspected 
	private void BeginInteraction()
	{
		player.FreezePlayer();
		player.inspector.DisableInspection();
		
		InteractablePickUpRotate.InteractionProperties props = inspectObject.interactionProperties;
		StoreObjectProperties(props);
		
		PrepGUI();
		PrepCameras();
		ReassignInspectedObjectLayer();

		if(anchored)
			InspectAnchoredObject(props);
		else
			InspectFloatingObject(props);
		
		currFOVInspectCamera = inspectObjectCamera.fieldOfView;
	}
	
	private void PrepGUI() {
		PlayMakerFSM.BroadcastEvent(GlobalPlaymakerEvents.HIDE_GUI);
		SetDoneSearchingVisibility(false);
		inspectObjectGUI.SetupGUI(inspectObject);
	}
	
	private void PrepCameras() 
	{
		
		Transform iosCameraStartPosition = player.GetActiveCamera().transform;
		TransformTools.TransformPosRot(inspectObjectCamera.gameObject, iosCameraStartPosition);
		TransformTools.TransformPosRot(backgroundCamera.gameObject, iosCameraStartPosition);
		
		if(anchored)
		{
			inspectObjectCamera.enabled = false;
			backgroundCamera.enabled = false;
		}
		else
		{
			anchorCamera.enabled = false;
		}
		
		ToggleMainCameraOff();
		
	}
	
	private void StoreObjectProperties(InteractablePickUpRotate.InteractionProperties props) {
		object2D = props.twoDimensional;
		canManuallyZoom = props.canManuallyZoom;
		anchored = props.isAnchored;
	}
	
	private void ReassignInspectedObjectLayer() {
		foreach(Transform child in inspectObject.transform)
			child.gameObject.layer = floatingObjectSpot.gameObject.layer;
		
		inspectObject.gameObject.layer = floatingObjectSpot.gameObject.layer;		
	}
	
	//For non-anchored objects
	private void InspectFloatingObject(InteractablePickUpRotate.InteractionProperties props) {
		objectOrigPosition = TransformTools.CopyPosition(inspectObject.transform);
		objectOrigOrient = TransformTools.CopyEulers(inspectObject.transform);
		
		GameObject adjustedFloatingObjectPosition = new GameObject();
		adjustedFloatingObjectPosition.transform.position =	TransformTools.CopyPosition(floatingObjectSpot);
		adjustedFloatingObjectPosition.transform.rotation = TransformTools.CopyRotation(floatingObjectSpot);
		adjustedFloatingObjectPosition.transform.Translate(Vector3.forward * inspectObject.holdDistance, inspectObjectCamera.transform);
			
		iTween.MoveTo(inspectObject.gameObject, adjustedFloatingObjectPosition.transform.position, objectPickUpTime);
		
		Vector3 objectAngle = floatingObjectSpot.eulerAngles;
		
		if(props.usesCustomOrientation)
			objectAngle += props.customOrientation;
		
		if(object2D)
			inspectObjectCamera.transform.Translate(Vector3.up * inspectObject.yStart);
		
		iTween.RotateTo(inspectObject.gameObject, iTween.Hash(
			"rotation", objectAngle,
			"time", objectPickUpTime,
			"islocal", false
			));
	}
	
	//For anchored objects
	private void InspectAnchoredObject(InteractablePickUpRotate.InteractionProperties props) {
		anchored = true;
		backgroundCamera.enabled = false;
		inspectObjectCamera.enabled = false;
		
		float targetFOV;
		
		TransformTools.TransformPosRot(anchorCamera.gameObject, player.GetActiveCamera().transform);
		
		TweenToFOV(anchorCamera, 90f, objectPickUpTime * anchoredTimeFactor);
		
		trackObject = true;
		
		if(props.AnchoredInspectPoint == null){
			Debug.LogError("This Anchored Inspect Object does not have a special anchor transform!!");
			return;
		}
			
		iTween.MoveTo(anchorCamera.gameObject, props.AnchoredInspectPoint.position, objectPickUpTime * anchoredTimeFactor);
		iTween.RotateTo(anchorCamera.gameObject, props.AnchoredInspectPoint.eulerAngles, objectPickUpTime * anchoredTimeFactor);
	}
	
	private void TweenToFOV(Camera camera, float targetFOV, float time) {
		TweenParms parms = new TweenParms();
		
		parms.Prop("fov", targetFOV);
		parms.Ease(EaseType.Linear);
		
		HOTween.To(camera, time, parms);
	}
	
	public void EndInteraction()
	{
		inspectObjectGUI.HideAllGUIElements();
		
		inspectObjectCamera.fieldOfView = DEFAULT_CAMERA_FOV;
		
		if(!anchored)
			HandleFloatingEnd();
		else
			HandleAnchoredEnd();
		
	}
	
	private void HandleFloatingEnd() {
		TransformTools.TransformPosRot(inspectObjectCamera.gameObject, player.GetActiveCamera().transform);
		
		iTween.MoveTo(inspectObject.gameObject, iTween.Hash
			(
			"position", objectOrigPosition,
			"time", objectPutDownTime,
			"oncomplete", "SelfDestruct",
			"oncompletetarget", this.gameObject
			));
		
		iTween.RotateTo(inspectObject.gameObject, iTween.Hash
			(
			"rotation", objectOrigOrient,
			"time", objectPutDownTime
			));		
	}
	
	private void HandleAnchoredEnd() {
		Vector3 endPos;
		Vector3 endRot;
		
		float targetFOV;
		
		Camera activeCamera = player.GetActiveCamera();
		
		endPos = activeCamera.transform.position;
		endRot = activeCamera.transform.eulerAngles;
		targetFOV = activeCamera.fieldOfView;
		
		iTween.MoveTo(anchorCamera.gameObject, iTween.Hash
			(
			"position", endPos,
			"time", objectPutDownTime * anchoredTimeFactor,
			"oncomplete", "SelfDestruct",
			"oncompletetarget", this.gameObject
			));
		
		iTween.RotateTo(anchorCamera.gameObject, iTween.Hash
			(
			"rotation", endRot,
			"time", objectPutDownTime * anchoredTimeFactor
			));		
		
		TweenToFOV(anchorCamera, targetFOV, objectPutDownTime * anchoredTimeFactor - 0.25f);
	}
	
	private void SelfDestruct() {
		ReportEvent.ScreenDeactivated(ScreenType.InspectObject);
		
		ToggleMainCameraOn();
		
		if(inspectObjectDoneEvent != null)
			inspectObjectDoneEvent();
		
		ResetObjectLayer();
		
		SetDoneSearchingVisibility(true);		
		
		player.UnFreezePlayer();
		player.inspector.EnableInspection();
		trackObject = false;
		
		PlayMakerFSM.BroadcastEvent(GlobalPlaymakerEvents.SHOW_GUI);
		Destroy(this.gameObject);
	}
	
	private void ResetObjectLayer() 
	{
		foreach(Transform child in inspectObject.transform)
			child.gameObject.layer = DEFAULT_LAYER;
		inspectObject.gameObject.layer = DEFAULT_LAYER;		
	}
	
	private void SetDoneSearchingVisibility(bool visible) {
		//backCODE Hide our "Done Searching Button"
		GameObject doneSearchingButtonGO = GameObject.FindGameObjectWithTag(Tags.DONE_SEARCHING_TAG);
		if(doneSearchingButtonGO != null)
		{
			DoneSearchingBroadcaster doneSearchingButton = doneSearchingButtonGO.GetComponent<DoneSearchingBroadcaster>();
			if(!visible)
				doneSearchingButton.HideDoneButton();
			else
				doneSearchingButton.ShowDoneButton();
		}
	}	
	
	private void ConstructHintString() {
		//Tailor the instructions based on what we are allowed to do.
		InteractablePickUpRotate.InteractionProperties props = inspectObject.interactionProperties;
		string instructions = "";
		bool isFirst = true;
		if ( props.canRotate )
		{
			if ( false == isFirst )
			{
				instructions += "  ";	//Add spaces.
			}
			isFirst = false;
			
			instructions += "Use the arrow buttons at the screen edges to rotate the object.";
		}
		if ( props.canManuallyZoom )
		{
			if ( false == isFirst )
			{
				instructions += "  ";	//Add spaces.
			}
			isFirst = false;
			
			instructions += "Use the scroll wheel or the Zoom buttons to get a closer look.";
		}
		
	}
	
	private void ToggleMainCameraOn() {
		if(myMainCamera != null) {
			myMainCamera.enabled = true;
			myMainCamera.tag = Tags.MAIN_CAMERA_TAG;
		}
	}
	
	private void ToggleMainCameraOff() {
		if(Camera.main != null) {
			myMainCamera = Camera.main;
			myMainCamera.enabled = false;
		}
	}	
	
	//TODO: This should be broken out into it's own class.
	//"InspectObjectInteraction"
	
	public void OpenObject () 
	{
		if(inspectObject is InteractablePickUpRotateOpen)
		{
			inspectObjectGUI.HideAllGUIElements();
			
			InteractablePickUpRotateOpen objectContainer = (InteractablePickUpRotateOpen) inspectObject;
			objectContainer.OpenObject();
			
			LevelManager levelManager = LevelManager.FindLevelManager();
			levelManager.InteractManager.UnRegisterObject(inspectObject);
			levelManager.EvidenceManager.UnRegisterEvidence(inspectObject);
			
			Transform openTransform = objectContainer.GetOpenObjectTransform();
			if(openTransform != null) {
				objectOrigPosition = openTransform.position;
				objectOrigOrient = openTransform.eulerAngles;
			}
			EndInteraction();
		} else {
			Debug.LogError("Pressed the OPEN Button on an non-Container Object!!");
		}
	}	
	
	public void RotateObject(ScrollDirection dir) {
		if(object2D) {
			switch(dir) {
			case ScrollDirection.Down:
				ScrollObjectVertically(-1.0f);
				break;
			case ScrollDirection.Up:
				ScrollObjectVertically(1.0f);
				break;
			}
		} else {
			switch(dir) {
			case ScrollDirection.Down:
				PitchObject( -1.0f );
				break;
			case ScrollDirection.Left:
				RotateObject( -1.0f );
				break;
			case ScrollDirection.Right:
				RotateObject( 1.0f );
				break;
			case ScrollDirection.Up:
				PitchObject(1.0f);
				break;
			}
		}
	}
	
	public void UpdateZoomByDelta( float zoomAmt ) {
		currFOVInspectCamera = Mathf.Clamp( currFOVInspectCamera + zoomAmt, minFOV, maxFOV );
		inspectObjectCamera.fieldOfView = currFOVInspectCamera;
	}	

	private void RotateObject( float dir )
	{	
		Transform cam = inspectObjectCamera.transform;
		float rotAmt = dir * ((rotationSpeed * Time.deltaTime * -1) * Mathf.Deg2Rad );
		
		//Rotate around camera axes.
        inspectObject.transform.RotateAround( cam.up, rotAmt );
	}
	
	private void PitchObject( float dir )
	{
		Transform cam = inspectObjectCamera.transform;
		float rotAmt = dir * ((rotationSpeed * Time.deltaTime) * Mathf.Deg2Rad );
		
        inspectObject.transform.RotateAround( cam.right, rotAmt );
	}		
	
	private void ScrollObjectVertically( float dir ) {
		inspectObjectCamera.transform.Translate((Vector3.up * dir) * (Time.deltaTime * scrollSpeed), Space.Self);
		//correctYPosition();
	}
	
	private void correctYPosition() {
		float yClamp = Mathf.Clamp(inspectObjectCamera.transform.position.y, inspectObject.yEnd, inspectObject.yStart);
		inspectObjectCamera.transform.localPosition = new Vector3(
			inspectObjectCamera.transform.localPosition.x,
			yClamp,
			inspectObjectCamera.transform.localPosition.z);
	}
	
}

