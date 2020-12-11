using UnityEngine;
using System.Collections;

public class PhotoAlbumInspect : MonoBehaviour {

	public GameObject bigImage;
	public UIButton deleteButton;
	public UIButton nextButton;
	public UIButton prevButton;
	public PackedSprite attachmentIcon;
	public UIButton attachButton;
	public UIButton unattachButton;
	public SpriteText descriptionText;
	public UIPanelManager phonePanelManager;
	public PhotoAlbum photoAlbum;
	
	private PhotoManager photoManager;
	private int currentPhotoIndex;
	
	public void Setup(int photoIndex) {
		currentPhotoIndex = photoIndex;
		photoManager = LevelManager.FindLevelManager().PhotoManager;
		
		PhotoManager.Photo photo =  photoManager.GetPhotoByIndex(photoIndex);
		TextureTools.SetMainTexture(bigImage, photo.image);
		
		descriptionText.Text = photo.context.interactable.displayName;
		
		if(photoManager.AreAttachmentsAllowed()) {
			if(photo.isAttachment) {
				ShowUnattachButton();
			} else if(!photo.isAttachment && !photoManager.AreAttachmentsMaxed()){
				ShowAttachButton();
			} else {
				Debug.Log("");
				HideAttachmentButtons();
			}
		} else {
			HideAttachmentButtons();
		}
		
		if(currentPhotoIndex + 1 == photoManager.GetNumPhotos())
			nextButton.Hide(true);
		else
			nextButton.Hide(false);
		
		if(currentPhotoIndex == 0)
			prevButton.Hide(true);
		else
			prevButton.Hide(false);
	}
	
	private void ShowAttachButton() {
		attachButton.Hide(false);
		unattachButton.Hide(true);
		attachmentIcon.Hide(true);		
	}
	
	private void ShowUnattachButton() {
		attachButton.Hide(true);
		unattachButton.Hide(false);
		attachmentIcon.Hide(false);		
	}
	
	private void HideAttachmentButtons() {
		attachButton.Hide(true);
		unattachButton.Hide(true);
		attachmentIcon.Hide(true);		
	}
	
	public void OnDeleteButtonHit() {
		photoManager.RemovePhotoByIndex(currentPhotoIndex);
		
		if(currentPhotoIndex > 0)
			Setup(currentPhotoIndex - 1); // There are still more photos so we just subtract 1.
		else if(photoManager.GetNumPhotos() > 0)
			Setup(currentPhotoIndex); //We just destroyed our 0 index photo, so we need to refresh the 0 index again.
		else
			OnBackButtonHit(); // Out of Photos back to Photo Album Screen
	}
	
	public void OnNextButtonHit() {
		Setup(currentPhotoIndex + 1);
	}
	
	public void OnPrevButtonHit() {
		Setup(currentPhotoIndex - 1);
	}
	
	public void OnAttachButtonHit() {
		PhotoManager.Photo photo =  photoManager.GetPhotoByIndex(currentPhotoIndex);
		photo.isAttachment = true;
		attachmentIcon.Hide(false);
		attachButton.Hide(true);
		unattachButton.Hide(false);
		OnBackButtonHit();
	}
	
	public void OnUnAttachButtonHit() {
		PhotoManager.Photo photo =  photoManager.GetPhotoByIndex(currentPhotoIndex);
		photo.isAttachment = false;
		attachmentIcon.Hide(true);
		attachButton.Hide(false);
		unattachButton.Hide(true);
	}
	
	public void OnBackButtonHit() {
		photoAlbum.RefreshAlbumScreen();
		phonePanelManager.BringIn("PhotoAlbum", UIPanelManager.MENU_DIRECTION.Backwards);
	}
	
}
