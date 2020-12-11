using UnityEngine;
using System.Collections;

public class GUIInputBlocker : MonoBehaviour {

	private const float RAY_DEPTH = 50.0f;
	
	public GameObject[] guiElements;
	
	private bool disabled = false;
	private PC pc;
	private Camera guiCamera;
	
	private void Start() 
	{
		pc = PC.GetPC();
		guiCamera = GameObject.FindGameObjectWithTag(Tags.GUI_CAMERA).GetComponent<Camera>();
	}
	
	private void Update() 
	{
		if(CheckRay() && !disabled)
			DisablePlayer();
		
		if(!CheckRay() && disabled)
			EnablePlayer();
	}
	
	private void EnablePlayer() 
	{
		disabled = false;
		pc.inspector.EnableInspection();
		pc.UnFreezePlayer();
	}
	
	private void DisablePlayer() 
	{
		disabled = true;
		pc.inspector.DisableInspection();
		pc.FreezePlayer();
	}
	
	private bool CheckRay() 
	{
		
		Ray ray = guiCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
		
        if (Physics.Raycast( ray, out hit, RAY_DEPTH))
        {	
			if(hit.collider.gameObject == this.gameObject)
				return true;
			
			foreach(GameObject go in guiElements) 
			{
				if(hit.collider.gameObject == go)
					return true;
			}
				
        }		
		
		return false;
		
	}
	
}
