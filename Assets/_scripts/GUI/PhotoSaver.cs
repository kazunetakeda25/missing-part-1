using UnityEngine;
using System.Collections;

public class PhotoSaver {
	
	private InspectObjectScreen ios;
	private UIButton takePhotoButton;
	private bool photoDone;
	private PhotoManager photoMgr;
	
	public void Init(InspectObjectScreen ios, UIButton takePhotoButton) {
		this.photoMgr = LevelManager.FindLevelManager().PhotoManager;
		this.ios = ios;
		this.takePhotoButton = takePhotoButton;
	}
	
	public void TakePhoto(InteractablePickUpRotate currentObject) {
		if(photoMgr.GetNumPhotos() >= 32) 
		{
			SimpleHint.CreateSimpleHint(SimpleHint.PopUpType.Error, "You have exceeded the maximum number of pictures allowed for this vignette.");
			return;
		}
		
		ReportEvent.TakePicture(currentObject.name);
		
		//Disable button to disallow quick presses
		takePhotoButton.controlIsEnabled = false;
		
		Texture2D screenShot = GetScreenshot();
        byte[] screenShotByteStr = GetScreenshot().EncodeToPNG();
		
		PhotoManager.PhotoContext context = CreatePhotoContextFromObject(currentObject);
		//Debug.Log("Adding photo of " + context.clue.name + " to photo album");
		
        PhotoManager.Photo photo = new PhotoManager.Photo( screenShot.width
														 , screenShot.height
														 , screenShotByteStr 
														 , context
														 );
		//Free up the memory
		screenShot = null;
		
        photoMgr.AddPhoto( photo );
		
		takePhotoButton.controlIsEnabled = true;
		
	}
	
	private Texture2D GetScreenshot() 
	{
		Texture2D screenShot;
		
		if(ios.IsAnchored())
			photoMgr.TakeScreenShot(ios.anchorCamera, out screenShot);
		else
			photoMgr.TakeScreenShot(ios.inspectObjectCamera, out screenShot);
		
		return screenShot;
	}
	
	private PhotoManager.PhotoContext CreatePhotoContextFromObject(InteractablePickUpRotate inspectObject) {
		PhotoClueObject objectPhotoClue = null;
		
		if(inspectObject.GetComponent<PhotoClueObject>() != null)
		{
			objectPhotoClue = inspectObject.GetComponent<PhotoClueObject>();
		} 
		else 
		{
			Debug.LogError("Object does NOT have a PhotoClueObject script.");
		}
		
		PhotoManager.PhotoContext photoClueContext = new PhotoManager.PhotoContext(objectPhotoClue, inspectObject, objectPhotoClue.confidenceForAcceptance * 2);
		
		return photoClueContext;		
	}
	
}
