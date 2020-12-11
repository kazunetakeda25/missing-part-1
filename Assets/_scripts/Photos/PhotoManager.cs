using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum PhotoColor
{
	Green,
	Red,
	None
}

[AddComponentMenu("Photos/Photo Manager")]
public class PhotoManager : MonoBehaviour
{
    public class PhotoContext
	{
		public PhotoClueObject clue;
		public InteractableWorldObject interactable;
		public float confidence;
		
		public PhotoContext( PhotoClueObject clue
						   , InteractableWorldObject intObj
						   , float confidence
						   )
		{
			this.clue = clue;
			this.interactable = intObj;
			this.confidence = confidence;
		}
	}
	
	public class Photo
    {
		public PhotoContext context;
        public Texture2D image;
		public bool isAttachment;
		
		public Photo( int imageWidth
					, int imageHeight
					, byte[] bytes 
					, PhotoContext context
					)
        {
			this.context = context;
            this.image = new Texture2D( imageWidth
                                   , imageHeight
                                   , TextureFormat.RGB24
                                   , false 
                                   );
            this.image.LoadImage( bytes );
			this.isAttachment = false;
        }
		
    };
    
	private Dictionary<string, int> photoCount = new Dictionary<string, int>();
	private Dictionary<string, int> sentCount = new Dictionary<string, int>();
    private List< Photo > photos = new List<Photo>();
    public int photoWidth = 256;
    public int photoHeight = 256;
	
	// Additional material used when object is in a photo.
	public Material inPhotoMaterial;
	// Additional material used when object is partially in a photo.
	// (No material is used when an object is completely out of a photo.)
	public Material outPhotoMaterial;

	public bool showRaycastDebugInfo = true;
	
	public bool justLeftCameraMode = false;
	
	public LayerMask layerMaskForClueVisibility;
	
	private List< PhotoClueObject > clueObjects = new List< PhotoClueObject >();
	
	private bool attachmentsAllowed = false;
	private int maxAttachments = 0;
    private LevelManager levelManager;
	
    void Start()
    {
    	levelManager = LevelManager.FindLevelManager();
    }
	
	public void ClearPhotos() 
	{
		photos.Clear();
		photos = new List<Photo>();
	}
	
	public void RegisterPhotoClueObject( PhotoClueObject obj )
	{
		if(!clueObjects.Contains(obj)) 
		{
			photoCount.Add(obj.name, 0);
			sentCount.Add(obj.name, 0);
			clueObjects.Add( obj );
		}
	}
	
	public void UnRegisterPhotoClueObject(PhotoClueObject clue)
	{	
		if(clueObjects.Contains(clue))
		{
			clueObjects.Remove(clue);
		}
	}
	
	public bool AreAttachmentsAllowed()
	{
		return attachmentsAllowed;
	}
	
	public void EnableAttachments( int maxAttachments )
	{
		ClearAttachments();
		
		attachmentsAllowed = true;
		this.maxAttachments = maxAttachments;
	}
	
	public int GetMaxAttachmentsAllowed()
	{
		if ( attachmentsAllowed )
		{
			return maxAttachments;
		}
		
		return 0;
	}
	
	public bool AreAttachmentsMaxed() {
		if(GetAttachments().Count >= maxAttachments)
			return true;
		
		return false;
	}
	
	public void DisableAttachments()
	{
		ClearAttachments();
		
		attachmentsAllowed = false;
	}
	
	public List< Photo > GetAttachments()
	{
		List< Photo > attachments = new List< Photo >();
		foreach( Photo photo in photos )
		{
			if(photos != null) {
				if (photo.isAttachment)
				{
					attachments.Add(photo);
				}
			}
		}
		
		return attachments;
	}
	
	public bool AddAsAttachment( Photo photo )
	{
		if ( false == attachmentsAllowed )
		{
			Debug.LogWarning( "Requested to add an attachment, but attachments aren't currently enabled!" );
			return false;
		}
		
		//Ensure we don't have too many attachments as is.
		List< Photo > currAttachments = GetAttachments();
		if ( currAttachments.Count >= maxAttachments )
		{
			return false;	//Nope.
		}
		
		//Set the photo as attached.
		photo.isAttachment = true;
		
		return true;
	}
	

	
	public bool RemoveAttachment( Photo photo )
	{
		if ( false == attachmentsAllowed )
		{
			Debug.LogWarning( "Requested to remove attachment, but attachments aren't currently enabled!" );
			return false;
		}
		
		if (photo.isAttachment)
		{
			photo.isAttachment = false;
			return true;
		}
		else
		{
			Debug.LogWarning( "Requested to remove attachment from a photo that wasn't marked as an attachment!" );
			return false;
		}
	}
	
	private void ClearAttachments()
	{
		foreach( Photo photo in photos )
		{
			if(photos != null) {
				if (photo.isAttachment)
				{
					photo.isAttachment = false;
				}
			}
		}
	}
	
	public int GetTimesPhotographed( string name )
	{
		if(photoCount.ContainsKey(name))
		{
			return photoCount[name];
		}
		else
		{
			return 0;	
		}
	}
	
	public int GetTimesInPhotos( string name )
	{
		int count = 0;
		
		// Iterate through the photos...
		foreach(Photo photo in photos)
		{		
			if(photo != null) 
			{
				// ...and through the clues within them.
				if(photo.context.clue != null) 
				{
					
					if(name == photo.context.clue.name)
					{
						count++;
					}
				}
			}
		}
		
		return count;
	}
	
	public int GetTimesInAttachments( string name )
	{
		if(sentCount.ContainsKey(name))
		{
			return sentCount[name];
		}
		else
		{
			return 0;	
		}
	}
	
	public void RecordAttachments()
	{
		Vignette.VignetteID currentVig = SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID;
		
		List<string> attachedPhotos = new List<string>();
		
		// Iterate through the photos...
		foreach(Photo photo in photos)
		{		
			if(photo != null) {
			
				if(!photo.isAttachment)
				{
					continue;
				}
				
				// ...and through the clues within them.
				if(photo.context.clue != null){
					// Only count ones applicable to the current vignette.
					if(photo.context.interactable.IsApplicableToVignette(currentVig))
					{
						attachedPhotos.Add(photo.context.clue.name);
						sentCount[photo.context.clue.name] = sentCount[photo.context.clue.name] + 1;
					}
				}
			}
		}
		
		ReportEvent.AttachmentsSent(attachedPhotos.ToArray());
	}
	
    public void AddPhoto( Photo photo )
    {
		// Iterate through the clues contained and add them to our counting record if we are in
		// the current vignette and the confidence value was high enough.
		Vignette.VignetteID currentVig = SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID;
		
		if(photo.context.clue != null) {
			if(photo.context.interactable.IsApplicableToVignette(currentVig))
			{
				photoCount[photo.context.clue.name] = photoCount[photo.context.clue.name] + 1;
			}
		}
		
        photos.Add(photo);
        
        //Save it out.
        byte[] bytes = photo.image.EncodeToPNG();
		
		//Find out where the file *should* go, make sure it can.
		string dirPrefix = SessionDataManager.GetSessionPath() + "/";
		string fileName = "tempScreenShot.png";
		//Make sure that directory exists...
		if ( System.IO.Directory.Exists( dirPrefix ) )
		{
			//Find the next available slot.
			int currIdx = 0;
			bool bDone = false;
			while ( !bDone )
			{
				fileName = "CameraPhoneScreenShot_" + currIdx + ".png";
				string tempFullPath = dirPrefix + fileName;
				if ( false == System.IO.File.Exists( tempFullPath ) )
				{
					//Got it!
					bDone = true;
				}
				else
				{
					++currIdx;
				}
			}
		}
		else
		{
			//Put local.
			dirPrefix = "";
		}
		
		
		string finalFileName = dirPrefix + fileName;
        
		//Debug.Log("file: " + finalFileName);
        //System.IO.File.WriteAllBytes( finalFileName, bytes );
		
    }
    
    public int GetNumPhotos()
    {
        return photos.Count;
    }
    
    public Photo GetPhotoByIndex( int idx )
    {
        if(idx >= photos.Count)
			return null;
		
		return photos[ idx ];
    }
    
    public void RemovePhotoByIndex( int idx )
    {
        photos.RemoveAt( idx );
    }
    
    public int GetPhotoWidth()
    {
        return photoWidth;
    }
    
    public int GetPhotoHeight()
    {
        return photoHeight;
    }
	
	public void TakeScreenShot( Camera referenceCam, out Texture2D screenShot )
	{
		
		int imageWidth = photoWidth;
		int imageHeight = photoHeight;
		
		//Create a *new*, temp camera, based off the target one.
        //Using the m_phoneCamera caused MAJOR issues where the GUI would
        //vanish.  In Unity documentation for Camera.Render(), it says:
        //"This is used for taking precise control of render order. To make use of this feature, create a camera and disable it. Then call Render on it. "
        GameObject tempCamObj = (GameObject)Instantiate( Resources.Load( "UI/Photo Camera Prefab" ) );
        Camera tempCam = tempCamObj.GetComponent< Camera >();
        //Assume the parent's stats.
		//backCode preserve the original culling mask and clear flags otherwise we get "junk" in the background
		int cullingMask = tempCam.cullingMask;
		CameraClearFlags clFlags = tempCam.clearFlags;
		
		tempCam.CopyFrom( referenceCam );
		//backCode preserve the original culling mask and clear flags otherwise we get "junk" in the background
		tempCam.cullingMask = cullingMask;
		tempCam.clearFlags = clFlags;
        
        RenderTexture renderTexture = RenderTexture.GetTemporary( imageWidth, imageHeight, 24 );

        //Preserve old state.
        RenderTexture oldActive = RenderTexture.active;
        RenderTexture.active = renderTexture;
        
        //Set new RT and actually do the render!
        tempCam.targetTexture = renderTexture;
        tempCam.Render();
		
		//Generate our "out" variable
        screenShot = new Texture2D( imageWidth, imageHeight, TextureFormat.RGB24, false );
        screenShot.ReadPixels( new Rect( 0.0f, 0.0f, imageWidth, imageHeight ), 0, 0 );
		
        //Restore old settings.
        RenderTexture.active = oldActive;
        //Release our newly-created RT.
        RenderTexture.ReleaseTemporary( renderTexture );

        //Destroy the temp cam.
        Destroy( tempCamObj );
	}
}

