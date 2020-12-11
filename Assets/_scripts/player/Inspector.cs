using UnityEngine;
using System.Collections;

//+--- This class handles interaction with world
//		objects including clues, doors and characters.
public class Inspector : MonoBehaviour
{
    private InteractableWorldObject iwObject;
	private Camera m_camera;		
	
	private float rayDepth = 10f;
	
	private LevelManager levelManager;
	private InteractManager intMgr;
	
	private bool inspectionEnabled;
	
	private int inspectorFreezeStack = 0;
	
	public void Awake()
	{
		levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
		intMgr = levelManager.InteractManager;
	}
	
	public void EnableInspection() {
		inspectorFreezeStack--;
		
		if(inspectorFreezeStack < 0) {
			Debug.LogError("Too many Enable Inspection stack requests!!");
			inspectorFreezeStack = 0;
		}
		
		CheckInspectorStack();
	}
	
	public void DisableInspection() {
		inspectorFreezeStack ++;
		
		CheckInspectorStack();
	}
	
	public void Update()
	{	
		if(HintPopUp.Instance != null) {
			return;
		}

		if(!inspectionEnabled || Time.timeScale == 0)
			return;
		
		GameObject targetObj = OnPerformRayCast();
		if(targetObj != null)
		{
			HandleInspectObject(targetObj);
		} else {
			intMgr.DetargetCurrentTarget();
		}
	}
	
	private void HandleInspectObject(GameObject targetObj) {
		
		InteractableWorldObject intWorld = CheckForApplicableTarget(targetObj);
		
		if(intWorld != null) {
			float distance = Vector3.Distance(this.transform.position, targetObj.transform.position);
			intMgr.SetTarget(intWorld, distance);			
		} else {
			intMgr.DetargetCurrentTarget();
		}	
	}
	
	private InteractableWorldObject CheckForApplicableTarget(GameObject targetObj) {
		
		InteractableWorldObject intWorld = targetObj.GetComponentInChildren<InteractableWorldObject>();
		
		if(intWorld == null)
			return null;
		
		//Debug.Log("Checking: " + intWorld.name);
		
		if(!CheckIfApplicableToThisVignette(intWorld))
			return null;
		
		if(CheckIfContainerObjectIsOpened(intWorld))
			return null;
		
		//This is a valid object!
		return intWorld;
	}
	
	private bool CheckIfContainerObjectIsOpened(InteractableWorldObject intWorld) 
	{
		InteractablePickUpRotateOpen intWorldOpenCheck;
		if(intWorld is InteractablePickUpRotateOpen) 
		{
			intWorldOpenCheck = intWorld as InteractablePickUpRotateOpen;
		} else 
		{
			return false;
		}
		
		if(intWorldOpenCheck.IsObjectOpen())
			return true;
		
		return false;
	}
	
	private bool CheckIfApplicableToThisVignette(InteractableWorldObject intWorld) {
		Vignette.VignetteID currentVignette = SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID;
		
		foreach(Vignette.VignetteID vig in intWorld.applicableVignettes) 
		{
			if(vig == currentVignette)
				return true;
		}
		
		return false;
	}
	
	public GameObject OnPerformRayCast()
	{
		GameObject retVal = null;
		m_camera = Camera.main;
		
		if(m_camera == null)
			return retVal;
		
		Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        Debug.DrawLine(ray.origin, ray.origin + (ray.direction * rayDepth));
		
		//Only cast against Default Layer
		int layerMask = 1 << 0;
		
        if (Physics.Raycast( ray, out hit, rayDepth, layerMask))
                retVal = hit.collider.gameObject;
		
		return retVal;
	}
	
	private void CheckInspectorStack() {
		if(inspectorFreezeStack > 0) {
			inspectionEnabled = false;
			intMgr.DetargetCurrentTarget();
		} else {
			inspectionEnabled = true;
		}
	}
	
}
