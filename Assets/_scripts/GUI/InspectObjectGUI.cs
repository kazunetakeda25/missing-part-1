using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This Class handles displaying and providing references for our InspectObject Screen GUI Elements.

public class InspectObjectGUI : MonoBehaviour {

	public UIButton exitScreenButton;
	public UIButton rotateButtonDown;
    public UIButton rotateButtonLeft;
	public UIButton rotateButtonRight;
	public UIButton rotateButtonUp;
	public UIButton takePhotoButton;
	public UIButton zoomDecreaseButton;
	public UIButton zoomIncreaseButton;
	public UIButton openObject;
	public UIButton photoAlbum;
	public SpriteText zoomText;
	public SpriteText hintText;
	
	private List<UIButton> activeButtons = new List<UIButton>();
	private List<SpriteText> activeText = new List<SpriteText>();	
	
	private void Awake() {
		//HideAllGUIElements();
	}
	
	public void HideAllGUIElements() {
		exitScreenButton.Hide(true);
		rotateButtonDown.Hide(true);
		rotateButtonLeft.Hide(true);
		rotateButtonRight.Hide(true);
		rotateButtonUp.Hide(true);
		takePhotoButton.Hide(true);
		zoomDecreaseButton.Hide(true);
		zoomIncreaseButton.Hide(true);
		openObject.Hide(true);
		photoAlbum.Hide(true);
		zoomText.Hide(true);
	}
	
	public bool isZoomPlusDown() {
		if(zoomIncreaseButton.controlState == UIButton.CONTROL_STATE.ACTIVE)
			return true;
		
		return false;
	}
	
	public bool isZoomMinusDown() {
		
		if(zoomDecreaseButton.controlState == UIButton.CONTROL_STATE.ACTIVE)
			return true;
			
		return false;
	}
	
	public void SetupGUI(InteractablePickUpRotate inspectObject) {
		
		InteractablePickUpRotate.InteractionProperties props = inspectObject.interactionProperties;
		
		if (props.canRotate && !props.isAnchored)
		{
			ShowRotationButtons();
		}
		
		//backCODE: Setting correct Buttons for 2D Objects (No Left or right buttons)
		if(props.twoDimensional && !props.twoDimensionalNoScrollBars) {
			ShowScrollButtons();
		}
		
		if (props.canPhotograph) {
			ShowPhotoButton();	
		}
		
		if(inspectObject is InteractablePickUpRotateOpen) {
			ShowOpenObjectButton();
		}
		
		//ARE WE ALLOWED TO ZOOM?
		if ( props.canManuallyZoom )
		{
			ShowZoomGUI();
		}
		
		if(SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID == Vignette.VignetteID.E1vTerrysApartmentSearch)
			activeButtons.Add(photoAlbum);		
		
		//Global Buttons
		activeButtons.Add(exitScreenButton);
		
		ShowActiveButtons();
	}
	
	private void ShowActiveButtons() {
		foreach(UIButton button in activeButtons)
			button.Hide(false);
		
		foreach(SpriteText text in activeText)
			text.Hide(false);
	}
	
	public void ClearList() {
		activeButtons.Clear();
		activeText.Clear();
	}
	
	public void ShowPhotoButton() {
		activeButtons.Add(takePhotoButton);
	}
	
	public void ShowRotationButtons() {
		activeButtons.Add(rotateButtonDown);
		activeButtons.Add(rotateButtonLeft);
		activeButtons.Add(rotateButtonRight);
		activeButtons.Add(rotateButtonUp);
	}
	
	public void ShowScrollButtons() {
		activeButtons.Add(rotateButtonDown);
		activeButtons.Add(rotateButtonUp);
	}
	
	public void ShowOpenObjectButton() {
		activeButtons.Add(openObject);
	}
	
	public void ShowZoomGUI() {
		activeButtons.Add(zoomDecreaseButton);
		activeButtons.Add(zoomIncreaseButton);
		activeText.Add(zoomText);
	}
	
}
