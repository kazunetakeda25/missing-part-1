using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Framework/Interact Manager")]
public class InteractManager : MonoBehaviour
{
    public const float INSPECT_CUTOFF = 2.5f;
	
	//Default material to add for highlight objects.
    public Material defaultHighlightMaterial;
    public bool closeEnoughToInspect = false;
	
    //Keep a list of interactable objects in the scene.
    private List<InteractableWorldObject> interactables = new List< InteractableWorldObject >();
    private InteractableWorldObject currTarget = null;
	private float currDistance = 0f;
	
	
	private void Awake() {
		defaultHighlightMaterial = Resources.Load(ResourcePaths.HIGHLIGHT_MATERIAL) as Material;
	}
	
    private void Start()
    {
        SetTarget( null, 0 );
    }
	
	private void Update() {
		if(Time.timeScale == 0 && currTarget != null) {
			DetargetCurrentTarget();
		}
	}
    
    public void RegisterObject( InteractableWorldObject obj )
    {
        //Ensure it's not already in there.
        if ( interactables.Contains( obj ) )
        {
            Debug.Log( "Attempted to add object '" + obj.name + "', when it already existed in the list!" );
        }
        else
        {
            interactables.Add( obj );
        }
    }
	
	public void UnRegisterObject( InteractableWorldObject targetObj ) 
	{
		foreach(InteractableWorldObject obj in interactables)
		{
			if(obj == targetObj)
			{
				interactables.Remove(obj);
				break;
			}
		}
	}
    
    public InteractableWorldObject GetCurrTarget()
    {
        return currTarget;
    }
    
    public void SetTarget(InteractableWorldObject currObj, float distance)
    {
        //If it was the same as last instance, ignore it.
        if ( currObj == currTarget )
            return;
        
        DetargetCurrentTarget();
        
        currTarget = currObj;
		currDistance = distance;
        
        //And turn on new one.
        if ( currTarget != null ) 
		{
			currTarget.Highlight();
		
			if(currDistance < INSPECT_CUTOFF) 
			{
				ShowClueHint.ShowHint(currTarget.displayName);
			}
			else 
			{
				ShowClueHint.ShowMoveTo(currTarget.displayName);
			}
		}
		
    }
	
	public void DetargetCurrentTarget() {
		if(currTarget != null) {
			currTarget.UnHighlight();
			currTarget = null;
		}
	}
    
    public void InteractWithTarget()
    {
        if ( null != currTarget )
        {
            currTarget.HandleInteraction();
        }
    }
}

