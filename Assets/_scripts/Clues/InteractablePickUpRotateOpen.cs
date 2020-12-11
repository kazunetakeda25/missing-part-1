using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractablePickUpRotateOpen : InteractablePickUpRotate {
	//Created by backCODE
	//Objects contained in the Container Object should be placed in the scene as they should appear after the Container Object is opened.
	//A transform of how the container object should appear after it is opened should be provided.
	
	#region Variables
	
	public GameObject[] containerObjects;
	private Transform[] containerObjectOrigins;
	
	public Transform openTransform;
	private GameObject closeTransform;
	private bool opened = false;
	
	#endregion
	
	#region Start
	
	private void Awake() {
		closeTransform = new GameObject();
		closeTransform.transform.position = this.transform.position;
		closeTransform.transform.rotation = this.transform.rotation;
	}
	
	public override void Start() {
		CaptureContainerObjectOriginPoints();
		HideObjects();
		base.Start();
	}
	
	private void HideObjects() {
		DisableContainerObjectMeshes();
		DisableObjects();
	}
		
	//Objects are returned to wherever they start after the object is opened.
	private void CaptureContainerObjectOriginPoints() {
		containerObjectOrigins = new Transform[containerObjects.Length];
		for (int i = 0; i < containerObjects.Length; i++) 
		{
			containerObjectOrigins[i] = containerObjects[i].transform;
		}
	}
	
	#endregion
	
	#region Enable/Disable Container Objects
	private void EnableContainerObjectMeshes() {
		switchContainerObjectMeshes(true);
	}
	
	private void DisableContainerObjectMeshes() {
		switchContainerObjectMeshes(false);
	}
	
	private void switchContainerObjectMeshes(bool enabled)
	{
		foreach(Renderer meshRenderer in FindContainerObjectMeshes())
		{
			meshRenderer.enabled = enabled;
		}
	}
	
	//Find all Mesh Renderes in our Objects.
	private List<Renderer> FindContainerObjectMeshes() {
		
		List<Renderer> allMeshes = new List<Renderer>();
		foreach(GameObject containerObject in containerObjects) {
			Renderer[] allObjectMeshes = containerObject.GetComponentsInChildren<Renderer>();
			foreach(Renderer meshRenderer in allObjectMeshes) {
				allMeshes.Add(meshRenderer);
			}
		}
		
		return allMeshes;
	}
	
	private void DisableObjects() {
		toggleContainerObjectsActive(false);
	}
	
	private void EnableObjects() {
		toggleContainerObjectsActive(true);
	}
	
	private void toggleContainerObjectsActive(bool active)
	{
		foreach(GameObject obj in containerObjects)
		{
			obj.gameObject.active = active;
		}		
	}
	
	
	#endregion
	
	#region Public Methods
	
	//BackCODE: Open/Close Object code.
	
	public void CloseObject() {
		if(opened) {
			opened = false;
			
			TransformTools.TransformPosRot(this.gameObject, closeTransform.transform);
			
			CaptureContainerObjectOriginPoints();
			HideObjects();
		}
	}
	
	public void OpenObject() {
		if(!opened) 
		{
			opened = true;
			//Move each Object to where it starts in the scene
			for (int i = 0; i < containerObjects.Length; i++) 
			{
				TransformTools.TransformPosRotScale(containerObjects[i], containerObjectOrigins[i]);
			}
			
			if(openTransform != null) 
			{
				TransformTools.TransformPosRotScale(this.gameObject, openTransform);
			}
			
			EnableObjects();
			EnableContainerObjectMeshes();
		}
	}
	
	public bool IsObjectOpen() 
	{
		return opened;
	}
	
	public Transform GetOpenObjectTransform()
	{
		if(openTransform != null) {
			return openTransform;
		} 
		
		return null;
	}
	
	#endregion
}
