using UnityEngine;
using System.Collections;

public class PhotoAlbum : MonoBehaviour {
	
	private const string PANEL_CLOSEUP = "Photo Inspect";
	public const string NO_PHOTOS_STRING = "No photos saved.";
	
	public UIPanelManager smartPhoneUIPanelManager;
	public PhotoAlbumInspect photoAlbumInspect;
	
	public UIButton nextButton;
	public UIButton prevButton;
	
	public UIButton[] albumButtons;
	public GameObject[] albumGrid;
	public PackedSprite[] attachmentIcons;
	
	public SpriteText photoNumbers;
	
	private int albumIndex = 1;
	
	private PhotoManager photoManager;
	private int numOfPhotos;
	private int numOfPages;
	
	public void RefreshAlbumScreen() {
		RefreshAlbumScreen(albumIndex);
	}
	
	public void RefreshAlbumScreen(int albumPage) {
		albumIndex = albumPage;
		SetupPhotoAlbum();
	}
	
	private void SetupPhotoAlbum() {
		photoManager = LevelManager.FindLevelManager().PhotoManager;
		numOfPhotos = photoManager.GetNumPhotos();
		numOfPages = Mathf.CeilToInt((float) numOfPhotos / (float) albumGrid.Length);
		SetupPhotoAlbumPage(albumIndex);
	}
	
	private void SetupPhotoAlbumPage(int pageToSetup) {
		for (int i = 0; i < albumGrid.Length; i++) {
			PhotoManager.Photo photo = photoManager.GetPhotoByIndex(i + ((pageToSetup * albumGrid.Length) - albumGrid.Length));
			if(photo != null) {
				PopulateAlbumIcon(i, photo);
				albumButtons[i].controlIsEnabled = true;
			} else {
				HideAlbumIcon(i);
				albumButtons[i].controlIsEnabled = false;
			}
		}
		
		photoNumbers.Text = ConstructPhotoNumbersString();
		RefreshNextPrevButtons();
	}
	
	private string ConstructPhotoNumbersString() {
		
		if(numOfPhotos == 0)
			return NO_PHOTOS_STRING;
		
		string photoNumberString = numOfPhotos.ToString();
		if(numOfPhotos > 1)
			photoNumberString += " photos saved.";
		else
			photoNumberString += " photo saved.";
		
		return photoNumberString;
	}
	
	private void PopulateAlbumIcon(int spot, PhotoManager.Photo photo) {
		TextureTools.SetMainTexture(albumGrid[spot], photo.image);
		albumGrid[spot].GetComponent<Renderer>().enabled = true;
		
		if(photo.isAttachment)
			attachmentIcons[spot].Hide(false);
		else
			attachmentIcons[spot].Hide(true);		
	}
	
	private void HideAlbumIcon(int spot) {
		albumGrid[spot].GetComponent<Renderer>().enabled = false;
		attachmentIcons[spot].Hide(true);
	}
	
	private void RefreshNextPrevButtons() {
		if(albumIndex < numOfPages)
			nextButton.Hide(false);
		else
			nextButton.Hide(true);
		
		if(albumIndex > 1)
			prevButton.Hide(false);
		else
			prevButton.Hide(true);
	}
	
	public void OnPrevButtonPressed() {
		Debug.Log("Previous");
		albumIndex--;
		SetupPhotoAlbumPage(albumIndex);
	}
	
	public void OnNextButtonPressed() {
		Debug.Log("Next");
		albumIndex++;
		SetupPhotoAlbumPage(albumIndex);
	}
	
	private void ProcessAlbumPress(int slot) {
		smartPhoneUIPanelManager.BringIn(PANEL_CLOSEUP);
		
		if(albumGrid[slot - 1].GetComponent<Renderer>().enabled = true)
			photoAlbumInspect.Setup( ( ( slot + ( albumIndex * albumGrid.Length ) - albumGrid.Length ) ) - 1 );
	}
	
	public void PhotoAlbum1Pressed() { ProcessAlbumPress(1); }
	public void PhotoAlbum2Pressed() { ProcessAlbumPress(2); }
	public void PhotoAlbum3Pressed() { ProcessAlbumPress(3); }
	public void PhotoAlbum4Pressed() { ProcessAlbumPress(4); }
	public void PhotoAlbum5Pressed() { ProcessAlbumPress(5); }
	public void PhotoAlbum6Pressed() { ProcessAlbumPress(6); }
	public void PhotoAlbum7Pressed() { ProcessAlbumPress(7); }
	public void PhotoAlbum8Pressed() { ProcessAlbumPress(8); }
	public void PhotoAlbum9Pressed() { ProcessAlbumPress(9); }
	
}
