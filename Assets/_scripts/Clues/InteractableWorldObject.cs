using UnityEngine;
using System.Collections;

[AddComponentMenu("Interactable/Interactable World Object (base)")]
public abstract class InteractableWorldObject : MonoBehaviour
{
	
	private const float LOOK_DELAY = 0.2f;
	private const float PLAYER_MOVE_TO_SPEED = 2F;
	
	public Material customHighlightMaterial = null;
	
	public string displayName = "";
	public EvidenceGroup evidenceGroup = EvidenceGroup.None;
	public float evidenceValue = 1.0f;
	public Vignette.VignetteID[] applicableVignettes;
	
	private Timer m_lookTimer;
	
	protected bool m_interactedWith = false;
	private bool highlighted = false;
	protected int m_timesInteractedWith = 0;
    protected LevelManager levelManager;
	
    // Use this for initialization
    public virtual void Start()
    {
		levelManager = LevelManager.FindLevelManager();
		Register();
		CreateTimer();
    }
	
	private void Update() {
		if(highlighted)
			HighlightedUpdate();
	}
	
    //Called while this object is highlighted.
    public virtual void HighlightedUpdate() {
		
		if(Input.GetMouseButtonUp(0)) 
		{
			PC pc = PC.GetPC();
			if(Vector3.Distance(pc.gameObject.transform.position, this.transform.position) < InteractManager.INSPECT_CUTOFF)
				InspectObjectScreen.InspectObject(this);
			else
				pc.ForcePlayerMove(this.transform.position, PLAYER_MOVE_TO_SPEED);
		} 
		
		m_lookTimer.Update(Time.deltaTime);
    }
	
	private void Register() {
        levelManager.InteractManager.RegisterObject(this);
		levelManager.EvidenceManager.registerEvidence(this);
	}	
	
	private void Unregister() {
		levelManager.EvidenceManager.UnRegisterEvidence(this);
	}
    
	public void Reset() {
		m_interactedWith = false;	
		m_timesInteractedWith = 0;
	}

	public bool IsApplicableToVignette(Vignette.VignetteID testVignetteID)
	{
		foreach( Vignette.VignetteID id in applicableVignettes ) {
			if ( id == testVignetteID )
				return true;
		}
		
		return false;
	}
	
    public virtual void Highlight()
	{	
		Material highlightMat = GetHighlightMaterial();
		
		Renderer[] renders = GetComponentsInChildren<Renderer>(true);
		//Debug.Log("Render Count: " + renders.Length);
        foreach( Renderer mr in renders ) {
				MaterialUtils.AddMaterialToRenderer( mr, highlightMat );
		}
		
		highlighted = true;
		m_lookTimer.StartTimer(LOOK_DELAY);
    }
	
    public virtual void UnHighlight()
    {
		ShowClueHint.DestroyHints();
		
        Material highlightMat = GetHighlightMaterial();
		
        Renderer[] renders = GetComponentsInChildren<Renderer>(true);
        foreach( Renderer mr in renders ) {
            MaterialUtils.RemoveMaterialFromRenderer( mr, highlightMat );
		}

		highlighted = false;
		m_lookTimer.CancelTimer();
	}
	
	private Material GetHighlightMaterial() {
		Material highlightMat = customHighlightMaterial;
		
		if (highlightMat == null)
            highlightMat = levelManager.InteractManager.defaultHighlightMaterial;
		
		return highlightMat;
	}
	
	private void CreateTimer() {
		m_lookTimer = new Timer(RegisterPhotoClue);
	}
    
	private void RegisterPhotoClue() 
	{
		//BackCODE: If this is a PhotoClue Object - Report it as looked at.
		PhotoClueObject thisPhotoClue = this.gameObject.GetComponent<PhotoClueObject>();
		if(thisPhotoClue != null) 
		{
			//Debug.Log("Object: " + this.gameObject.name + " has been seen!!");
			levelManager.LookManager.RegisterClueLookedAt(this.gameObject.name);
			m_lookTimer.CancelTimer();
		}
	}
    
	protected float getDistanceFromPlayer(Vector3 targetPos)
	{
		GameObject player = PC.GetPC().gameObject;
        
		//We don't care about height differential.
        Vector3 playerPos = new Vector3( player.transform.position.x
									   , 0.0f
									   , player.transform.position.z
									   );
		
		Vector3 targetHeightlessPos = new Vector3( targetPos.x
												   , 0.0f
									   			   , targetPos.z
									              );
        
		return Vector3.Distance( playerPos, targetHeightlessPos );
	}
    
    public abstract void HandleInteraction();
	
	public EvidenceGroup getEvidenceGroup()
	{
		return evidenceGroup;	
	}
	
	public float getEvidenceValue()
	{
		return evidenceValue;	
	}
	
	public bool hasBeenInteractedWith()
	{
		return m_interactedWith;	
	}
	
	public int timesInteractedWith()
	{
		return m_timesInteractedWith;	
	}
	
}

