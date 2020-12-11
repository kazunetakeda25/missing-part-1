using UnityEngine;
using System.Collections;

//+--- The level manager is in charge of keeping 
//		references to important components that various
//		systems need to access. The component should be
//		attached to an empty game object tagged "LevelManager"
//		for easy acces via GameObject.FindWithTag();

public class LevelManager : MonoBehaviour 
{	
	//+--- FRAMEWORK-RELATED COMPONENTS.
	private LookManager lookManager;
	private InteractManager interactManager;
	private EvidenceManager evidenceManager;
	private PhotoManager photoManager;
	private SubjectData subjectData;
	private VignetteManager vignetteManager;
	private Settings settings;
	private ScoringManager scoringManager;
	private EvaluationManager evaluationManager;
	
	private void Start() {
		interactManager = InteractManager;
		lookManager = LookManager;
		evidenceManager = EvidenceManager;
		photoManager = PhotoManager;
		
	}
	
	public static LevelManager FindLevelManager() {
		LevelManager lm = null;

		GameObject lmGO = GameObject.FindGameObjectWithTag (Tags.LEVEL_MANAGER_TAG);

		if (lmGO != null) {
			lm = lmGO.GetComponent<LevelManager> ();
		} else {
			//Create Debug Level Manager
			lmGO = new GameObject("Debug Level Manager");
			lmGO.tag = Tags.LEVEL_MANAGER_TAG;
			lm = lmGO.AddComponent(typeof(LevelManager)) as LevelManager;
		}
		return lm;
	}
	
	public void Update()
	{
		if(Input.GetKey(KeyCode.G))
		{
			this.EvidenceManager.getEvidenceReport(SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID, true);
		}
	}
	
	//+--- SETTERS AND GETTERS
	
	public InteractManager InteractManager
	{
		get
		{ 
			if(interactManager == null)
			{
				interactManager = gameObject.AddComponent<InteractManager>();
			}
			return interactManager; 
		}
		set
		{
			interactManager = value; 
		}
	}
	
	public LookManager LookManager
	{
		get
		{
			if(lookManager == null)
			{
				lookManager = gameObject.AddComponent<LookManager>();
			}
			return lookManager;
		}
		set
		{
			lookManager = value;	
		}
	}
	
	public EvidenceManager EvidenceManager 
	{
		get
		{
			if(evidenceManager == null)
			{
				evidenceManager = gameObject.AddComponent<EvidenceManager>();
			}
			return evidenceManager;
		}
		set
		{
			evidenceManager = value;
		}
	}
	
	public PhotoManager PhotoManager 
	{
		get
		{
			if(photoManager == null)
			{
				photoManager = gameObject.AddComponent<PhotoManager>();
			}
			return photoManager;
		}
		set
		{
			photoManager = value;
		}
	}

    public LevelManager levelManager()
    {
        return null;
    }
	
	public ScoringManager ScoringManager 
	{
		get
		{
			if(scoringManager == null)
			{
				scoringManager = gameObject.AddComponent<ScoringManager>();
			}
			return scoringManager;
		}
		set
		{
			scoringManager = value;
		}
	}
	
	public EvaluationManager EvaluationManager 
	{
		get
		{
			if(evaluationManager == null)
			{
				evaluationManager = gameObject.AddComponent<EvaluationManager>();
			}
			return evaluationManager;
		}
		set
		{
			evaluationManager = value;
		}
	}
}