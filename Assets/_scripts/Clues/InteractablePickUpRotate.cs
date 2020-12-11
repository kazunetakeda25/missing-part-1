using UnityEngine;
using System.Collections;

[AddComponentMenu("Interactable/Pick Up and Rotate")]
public class InteractablePickUpRotate : InteractableWorldObject
{
	public float holdDistance = 1.0f;
	
	public float yStart = 0;
	public float yEnd = 0;
	
	
	[System.Serializable]
	public class InteractionProperties {
		public bool canRotate = true;
		public bool canPhotograph = true;
		public bool isAnchored = false;	//If true, indicates we zoom in
		public bool canManuallyZoom = true;
		public bool usesCustomOrientation = false;	//If true, indicates we use a custom orientation on pickup.
		public Vector3 customOrientation = Vector3.zero;
		public bool twoDimensional = false;
		public bool twoDimensionalNoScrollBars = false;
		public Transform AnchoredInspectPoint;
	};
	
	public InteractionProperties interactionProperties;
	
    public override void Start() {
		if(levelManager == null)
		{
			levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
		}
		
        //If we don't have a display name, use the game object's name.
        if ( "" == displayName )
        {
            displayName = gameObject.name;
        }
        
        base.Start();
    }
    
    public override void HandleInteraction()
    {
     
        //Pick it up.  Automatically pushes the screen.
        //App.Instance().GetMessageManager().Trigger(new Messages_InspectObject(this.m_displayName));
        //InspectObjectScreen screen = App.Instance().GetScreenManager().m_screenInspectObjectScreen;
        //screen.SetObject( this, m_holdDistance, m_yStart);
		m_interactedWith = true;
		
		// Increment times interacted with if we are in the correct vignette.
		
		//if(IsApplicableToVignette(App.Instance().GetVignetteManager().currentVignette.vignetteID))
		
		if(IsApplicableToVignette(SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID))
		{
			m_timesInteractedWith++;
		}
    }
	
}