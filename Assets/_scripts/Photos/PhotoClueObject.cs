using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Photos/Photo Clue Object")]
public class PhotoClueObject : MonoBehaviour
{
	private bool show = false;
	private Rect rect = new Rect();
	
	public float confidenceFallOffStart = 6.0f;
	public float confidenceFallOffEnd = 8.0f;
	public float confidenceForAcceptance = 0.7f;
	
	// The current color add (if any) for this photo.	
	private PhotoColor currentColor = PhotoColor.None;
	private LevelManager levelManager;
	
	public void Start()
	{
		levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
		levelManager.PhotoManager.RegisterPhotoClueObject( this );		
		levelManager.LookManager.RegisterInteractable( this );
	}

}

