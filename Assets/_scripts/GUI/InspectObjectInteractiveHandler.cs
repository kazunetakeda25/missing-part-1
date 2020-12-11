using UnityEngine;
using System.Collections;

//This class is designed to Handle Inputs from our various InspectObject GUI Elements.

public class InspectObjectInteractiveHandler : MonoBehaviour {

	public InspectObjectScreen inspectObjectScreen;
	
	private Timer hintTimer;
	private float zoomIncrementScrollWheel = 300.0f;
	private float zoomIncrementButton = 40.0f;
	private float hintTime = 5.0f;
	
	public void OnExitScreenButtonHit() {
		inspectObjectScreen.EndInteraction();
		OnAnyButtonHit();
	}
	
	public void OnOpenObjectHit() {
		inspectObjectScreen.OpenObject();
		OnAnyButtonHit();
	}
	
	public void OnTakePhotoButtonHit() {
		inspectObjectScreen.TakePhoto();
		OnAnyButtonHit();
	}
	
	public void OnRotateUpHit() {
		inspectObjectScreen.RotateObject(InspectObjectScreen.ScrollDirection.Up);
		OnAnyButtonHit();
	}	
	
	public void OnRotateDownHit() {		
		inspectObjectScreen.RotateObject(InspectObjectScreen.ScrollDirection.Down);
		OnAnyButtonHit();
	}
	
	public void OnRotateLeftHit() {
		inspectObjectScreen.RotateObject(InspectObjectScreen.ScrollDirection.Left);
		OnAnyButtonHit();
	}
	
	public void OnRotateRightHit() {
		inspectObjectScreen.RotateObject(InspectObjectScreen.ScrollDirection.Right);
		OnAnyButtonHit();
	}
	
	public void OnPhotoAlbumButtonHit() {
		OnExitScreenButtonHit();
		StartCoroutine(CuePhoneAlbum());
		OnAnyButtonHit();
	}
	
	private IEnumerator CuePhoneAlbum() {
		yield return new WaitForSeconds(InspectObjectScreen.OBJECT_PUT_DOWN_TIME);
		GameObject phoneObject = Tags.FindGameObject(Tags.SMARTPHONE_TAG);
		SmartPhone phone = phoneObject.GetComponent<SmartPhone>();
		phone.OpenPhone(SmartPhone.Mode.PhotoAlbum);
	}
	
	public void OnZoomButtonDecreaseHit() {
		OnAnyButtonHit();
	}
	
	public void OnZoomButtonIncreaseHit() {
		OnAnyButtonHit();
	}
	
	private void Start() {
		hintTimer = new Timer(DisplayInteractHint);
		hintTimer.StartTimer(hintTime);
	}
	
	private void Update() {
		HandleInput();
		hintTimer.Update(Time.deltaTime);
	}
	
	private void OnDestroy() {
		hintTimer.CancelTimer();
	}
	
    private void HandleInput()
    {
		if (inspectObjectScreen.CanManuallyZoom()) {
			
			float scrollWheel = Input.GetAxis( "Mouse ScrollWheel" );
			
			if ( 0.0f != scrollWheel ) {
				float zoomAmt = -scrollWheel * (zoomIncrementScrollWheel * Time.deltaTime);
				inspectObjectScreen.UpdateZoomByDelta(zoomAmt);
			}
			
			//Check increase zoom amt.
			if(inspectObjectScreen.inspectObjectGUI.isZoomPlusDown()) {
				ZoomCamera(-1);
			} else if(inspectObjectScreen.inspectObjectGUI.isZoomMinusDown()) {
				ZoomCamera(1);
			}
			
		}
    }	
	
	private void ZoomCamera(float dir) {
		float zoomAmt = dir * zoomIncrementButton * Time.deltaTime;
		inspectObjectScreen.UpdateZoomByDelta(zoomAmt);
	}
	
	private void OnAnyButtonHit() {
		hintTimer.CancelTimer();	
	}
	
	private void DisplayInteractHint() {
		
	}	
	
}
