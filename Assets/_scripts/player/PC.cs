using UnityEngine;
using System.Collections;
using UnityEngine.AI;

//+--- THE PC is the Player Controller. This acts as the 
//		"hub" for all other player-associated components.
//		The PC creates the following:
//			-Anchor
//				+Anchor EdgeScrollDriver
public class PC : MonoBehaviour 
{
	private const string FIRST_PERSON_CAMERA_PREFAB_PATH = "cameras/FirstPersonCamera";
	private const string THIRD_PERSON_CAMERA_PREFAB_PATH = "cameras/ThirdPersonCamera";
	private const float CLOSE_ENOUGH_VALUE = 1f;
	//This is an iTween Update value, so the higher the number, the lower the speed.
	private const float MOVE_LOOK_TO_SPEED = 3.0f;
	
	public enum CameraType { fp, tp };
	
	private bool frozen = false;
	private int frozenStack = 0;

    public GameObject pawn;
    public CameraType cameraType;
    public PCMotor motor;
    public Transform lookTarget;
   
    public Camera thirdPersonCamera;
	public Camera firstPersonCamera;
	
    public CameraMotor thirdCameraMotor;
	public CameraMotor firstCameraMotor;
	
	private CameraMotor currentCameraMotor;
	
    public Inspector inspector;
    public Vector3 defaultAnchorOffset = new Vector3(0.7f, 1, -2);
	
	public GameObject manModel;
	public GameObject womanModel;
	
    [HideInInspector]
	public NavMeshAgent agent;
	
	private float yLock;
	private Vector3 moveToTarget = Vector3.zero;
	
	//Static]
	private static PC instance;
	public static PC GetPC() { return instance; }

//	public static PC GetPC() {
//		GameObject pcGO = (GameObject) GameObject.FindGameObjectWithTag(Tags.PLAYER_TAG);
//		
//		PC pc = null;
//		
//		if(pcGO != null)
//				pc = pcGO.GetComponent<PC>();
//		
//		return pc;
//	}
	
	public void FreezePlayer() {
		frozenStack++;
		//Debug.Log("Freezing Player Stack: " + frozenStack);
		CheckFrozenState();
	}
	
	public void UnFreezePlayer() {
		frozenStack--;
		//Debug.Log("Unfreezing Player Stack: " + frozenStack);
		
		if(frozenStack < 0) {
			Debug.LogError("Too many Unfreeze Requests!");
			frozenStack = 0;
		}
		
		CheckFrozenState();
	}
	
	public void SetCurrentCamera() {
		if(Settings.IsFirstPerson()) 
		{
			CameraTools.MakeMainCamera(firstPersonCamera);
			currentCameraMotor = firstCameraMotor;
			thirdPersonCamera.enabled = false;
		}
		else 
		{
			CameraTools.MakeMainCamera(thirdPersonCamera);
			currentCameraMotor = thirdCameraMotor;
			firstPersonCamera.enabled = false;
		}		
	}
	
	public bool IsPlayerFrozen() {
		return frozen;
	}
	
	public void ForcePlayerMove(Vector3 newPosition, float speed) {        
		FreezePlayer();
		
		if(agent == null) 
		{
			agent = this.gameObject.AddComponent<NavMeshAgent>();
			ReportEvent.RequestMoveToPoint(newPosition, speed);
		} else 
		{
			Debug.LogWarning("Move Action already in progress.  Cancelling extra request!");
		}
		
		agent.speed = speed;
		
		agent.SetDestination(newPosition);
		agent.baseOffset = 0.3f;
		agent.updatePosition = true;
		agent.updateRotation = true;
	}
	
	public bool IsForcePlayerMoveActive() {
		if(this.gameObject.GetComponent<NavMeshAgent>())
			return true;
		
		return false;
	}
	
	public Camera GetActiveCamera() {
		if(Settings.IsFirstPerson()) {
			return firstPersonCamera;
		} else {
			return thirdPersonCamera;
		}
	}
	
	//Private
	private void Awake() {
		instance = this;

		SetYLock();
		
		//Gender gender = SessionManager.GetSessionManager().currentSubject.subjectGender;
		bool male = false;

		if (MissingComplete.SaveGameManager.Instance != null) {
			male = MissingComplete.SaveGameManager.Instance.GetCurrentSaveGame().male;
		}

		if(male == false) {
			Destroy(manModel);
			pawn = womanModel;
		} else {
			Destroy(womanModel);
			pawn = manModel;
		}
		
	}
	
	private void SetYLock() {
		yLock = this.transform.position.y;
	}
	
	private void Start () 
    {
		// INITIALIZE THE PC CLASS

		if(this.gameObject.GetComponent<PCMotor>() != null) {
			return;
		}

		motor = gameObject.AddComponent<PCMotor>();
        motor.pawn = pawn;
		
		InitCameras();
		SetCurrentCamera();
		
        if(inspector == null)
        {
            inspector = gameObject.AddComponent<Inspector>();
            if (inspector == null)
            {
                Debug.LogWarning("PC was unable to attach Inspector component. Initialization stopped.");
                return;
            }
        }

        if (motor.anchor == null)
        {
            motor.anchor = new GameObject("anchor").transform;
            motor.anchor.transform.parent = gameObject.transform;

            EdgeScrollDriver edgeDriver = motor.anchor.gameObject.AddComponent<EdgeScrollDriver>();
            edgeDriver.firstPersonXTarget = motor;
            edgeDriver.firstPersonYTarget = currentCameraMotor;
            if (!Settings.IsFirstPerson())
            {
                currentCameraMotor.driver = edgeDriver;
                edgeDriver.xTarget = motor;
                edgeDriver.yTarget = thirdCameraMotor;
                motor.camMotor = currentCameraMotor;
            }

            Vector3 initialPos = new Vector3(gameObject.transform.position.x + (1f * Mathf.Cos(Mathf.Deg2Rad * 290))
                , gameObject.transform.position.y
                , gameObject.transform.position.z + (1f * Mathf.Sin(Mathf.Deg2Rad * 290)));
            motor.anchor.transform.position = initialPos;

            if (motor.anchor == null)
            {
                Debug.Log("Unable to create anchor. Initialization stopped.");
                return;
            }
        }
    }
	
	private void Update() {
		if(agent != null) {
			ProcessMovementFrame();
		}
	}
	
	private void ProcessMovementFrame()
	{
		currentCameraMotor.Vertical = 1;
		
		LookWhileMoving();
		
		if(agent != null && agent.hasPath && agent.remainingDistance <= CLOSE_ENOUGH_VALUE ) 
		{
			Debug.Log("Move Done");
			agent.Stop();
			Destroy(agent);
			UnFreezePlayer();
			PlayMakerFSM.BroadcastEvent(GlobalPlaymakerEvents.SHOW_GUI);
			ReportEvent.MoveToPointFinished(this.transform.position);
		}
	}
	
	private void LookWhileMoving()
	{
		AdjustFPCameraWhileMoving();
		AdjustTPCameraWhileMoving();
	}
	
	private void AdjustFPCameraWhileMoving()
	{
		GameObject firstPersonPivot = firstPersonCamera.transform.parent.gameObject;
		//Keep the camera centered (the parent relationship breaks when we have a navmesh active)
		firstPersonPivot.transform.localPosition = Vector3.zero;
		//Level out the camera.
		Vector3 levelRotation = new Vector3(0, firstPersonPivot.transform.eulerAngles.y, firstPersonPivot.transform.eulerAngles.z);
		iTween.RotateUpdate(firstPersonPivot, levelRotation, MOVE_LOOK_TO_SPEED);
	}
	
	private void AdjustTPCameraWhileMoving()
	{
//		GameObject thirdPersonPivot = thirdPersonCamera.transform.parent.gameObject;
//		//Keep the camera centered (the parent relationship breaks when we have a navmesh active)
//		thirdPersonPivot.transform.localPosition = Vector3.zero;
//		Debug.Log("Test: " + thirdPersonPivot.name);
		//Level out the camera.
//		Vector3 levelRotation = new Vector3(0, thirdPersonCamera.transform.eulerAngles.y, thirdPersonCamera.transform.eulerAngles.z);
//		iTween.RotateUpdate(thirdPersonCamera.gameObject, levelRotation, MOVE_LOOK_TO_SPEED);
		
		AlternateThirdPersonCamera tpcam = thirdCameraMotor as AlternateThirdPersonCamera;
		tpcam.SetYToZero();
	}	
	
			
	private void LateUpdate() {
		this.transform.position = new Vector3(this.transform.position.x, yLock, this.transform.position.z);
	}
	
	private void CheckFrozenState() {
		if(frozenStack == 0)
			Unfreeze();
		
		if(!frozen && frozenStack > 0)
			Freeze();
	}
	
	private void Unfreeze() {
		frozen = false;
		motor.ThawMotor();
		firstCameraMotor.UnFreeze();
		thirdCameraMotor.UnFreeze();
		
		AlignPlayerToCamera();
	}

    public void ForceUnfreeze()
    {
        frozenStack = 0;
        //Debug.Log("Unfreezing Player Stack: " + frozenStack);

        if (frozenStack < 0)
        {
            Debug.LogError("Too many Unfreeze Requests!");
            frozenStack = 0;
        }

        CheckFrozenState();
    }
	
	private void Freeze() {
		frozen = true;
		if(motor == null)
			Start();
		
		motor.FreezeMotor();
		firstCameraMotor.Freeze();
		thirdCameraMotor.Freeze();
	}
	
    private void InitCameras()
    {
        firstPersonCamera = SpawnFirstPersonCamera();
		firstCameraMotor = firstPersonCamera.GetComponent<CameraMotor>();
		firstCameraMotor.lookTarget = lookTarget.transform;
		
		thirdPersonCamera = SpawnThirdPersonCamera();
		thirdCameraMotor = thirdPersonCamera.GetComponent<CameraMotor>();
		thirdCameraMotor.lookTarget = lookTarget.transform;
		
		SetCurrentCamera();
    }
	
	private void AlignPlayerToCamera() {
		GameObject firstPersonPivot = GameObject.FindGameObjectWithTag(Tags.FIRST_PERSON_CAMERA_TRANSFORM);
		firstPersonPivot.transform.parent = null;
		
		this.transform.localEulerAngles = 
			new Vector3(
				0, //Never change the player's X Orientation!
				firstPersonPivot.transform.localEulerAngles.y,
				firstPersonPivot.transform.localEulerAngles.z);
		
		firstPersonPivot.transform.parent = this.transform;
	}	
	
	private Camera SpawnThirdPersonCamera() {
		GameObject thirdPersonCameraPivot = new GameObject();
		thirdPersonCameraPivot.name = "Third Person Camera";
		GameObject thirdPerson = GameObject.Instantiate(Resources.Load(THIRD_PERSON_CAMERA_PREFAB_PATH), this.transform.position, Quaternion.identity) as GameObject;
		TransformTools.TransformPosRot(thirdPersonCameraPivot, thirdPerson.transform);
		thirdPerson.transform.parent = thirdPersonCameraPivot.transform;
		return thirdPerson.GetComponent<Camera>();
	}
	
	private Camera SpawnFirstPersonCamera() {
		Transform firstPersonCameraTransform = GameObject.FindGameObjectWithTag(Tags.FIRST_PERSON_CAMERA_TRANSFORM).transform;
		GameObject firstPerson = GameObject.Instantiate(Resources.Load(FIRST_PERSON_CAMERA_PREFAB_PATH), firstPersonCameraTransform.position, firstPersonCameraTransform.rotation) as GameObject;
		firstPerson.transform.parent = firstPersonCameraTransform;
		//firstPersonCameraTransform.transform.SetParent (null);
		return firstPerson.GetComponent<Camera>();
	}
	
}