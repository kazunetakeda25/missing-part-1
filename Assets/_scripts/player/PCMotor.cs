using UnityEngine;
using System.Collections;

//*** PC MOTOR handles the movement of the character controller
//*** and playing the appropriate animations.
public class PCMotor : MonoBehaviour 
{

	private const string VERTICAL_AXIS = "Vertical";
	private const string HORIZONTAL_AXIS = "Horizontal";

	//+--- PUBLIC VARIABLES
    public float walkSpeedForward = 1.25f;
    public float walkSpeedBackward = 1.0f;
    public float turnSpeed = 6f;
    public Transform anchor;
    public float headBob;
    public string idleAnim = "Idle";
    public string walkAnim = "Walk";
    public string shuffleLeftAnim = "ShuffleLeft";
    public string shuffleRightAnim = "ShuffleRight";
    public string turnLeftAnim = "Turn90Left";
    public string turnRightAnim = "Turn90Right";
    public CameraMotor camMotor;

    [HideInInspector]
    public GameObject pawn;
   
    private float vertical;
	public float Vertical 
	{ 
		get 
		{ 
			return vertical; 
		} 
		set 
		{ 
			if (Input.GetAxis (VERTICAL_AXIS) == 0) {
				vertical = value; 
			}
		} 
	}
    private float horizontal;
	public float Horizontal 
	{ 
		get 
		{ 
			return horizontal; 
		} 
		set 
		{ 
			if (Input.GetAxis (HORIZONTAL_AXIS) == 0) {
				horizontal = value; 
			}
		} 
	}

    [HideInInspector]
    public Vector3 direction;
	[HideInInspector]
	public bool beingDriven;
	
	private bool frozen;
    private bool isMoving = false;
    private Animation animComponent;
    private CharacterController controller;
    private Vector3 lastPosition;
    private Transform _transform;
    private bool hasAgent;
   
	//+--- PRIVATE VARIABLES	
    private Camera cam;
    private float angle;
    private Inspector inspector;
	
	//+--- UNITY METHODS	
    void Awake()
    {
        _transform = gameObject.transform;
        inspector = PC.GetPC().inspector;
    }

    void Start()
    {
        lastPosition = _transform.position;
        if (cam == null)
        {
			RefreshMainCam();
        }

        if (animComponent == null)
        {
            animComponent = pawn.GetComponentInChildren<Animation>();
        }
        if (controller == null)
        {
            controller = gameObject.GetComponent<CharacterController>();
        }
        vertical = 1;
    }

    void Update()
    {
        if (_transform.position != lastPosition && isMoving == false)
        {
            isMoving = true;
        }
        else if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
        {
            hasAgent = true;
            if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed > 0)
            {
                isMoving = true;
            }
        }
        else if (_transform.position == lastPosition && isMoving == true)
        {
            hasAgent = false;
            isMoving = false;
        }

        lastPosition = _transform.position;

		ProcessInput(gameObject.GetComponent<CharacterController>());
        CheckAnim();
    }
    private void CheckAnim()
    {
        if (!isMoving)
        {
            if (!animComponent.IsPlaying(idleAnim))
            {
                animComponent.gameObject.GetComponent<Animation>()[idleAnim].wrapMode = WrapMode.Loop;
                animComponent.CrossFade(idleAnim, 0.5f);
            }
        }
        else if ((isMoving && controller.velocity.magnitude > 0) || (isMoving && horizontal != 0))
        {
            if (!animComponent.IsPlaying(walkAnim))
            {
                animComponent.Rewind("walkAnim");
                animComponent.gameObject.GetComponent<Animation>()[walkAnim].wrapMode = WrapMode.Loop;
                animComponent.CrossFade(walkAnim, 0.3f);
            }
        }
        else if ((isMoving && controller.velocity.magnitude < 0) || (isMoving && horizontal != 0))
        {
            if (!animComponent.IsPlaying(walkAnim))
            {
                animComponent.Rewind("walkAnim");
                animComponent.gameObject.GetComponent<Animation>()[walkAnim].wrapMode = WrapMode.Loop;
                animComponent.gameObject.GetComponent<Animation>()[walkAnim].speed = -1;
                animComponent.CrossFade(walkAnim, 0.3f);
            }
        }
        else if (isMoving && hasAgent)
        {
            if (!animComponent.IsPlaying(walkAnim))
            {
                animComponent.Rewind("walkAnim");
                animComponent.gameObject.GetComponent<Animation>()[walkAnim].wrapMode = WrapMode.Loop;
                animComponent.CrossFade(walkAnim, 0.3f);
            }
        }
        
    }
	private void RefreshMainCam() {
        PC pc = PC.GetPC();
		
		if(pc != null) {
			if(Settings.IsFirstPerson())
				cam = pc.firstPersonCamera;
			else
				cam = pc.thirdPersonCamera;
		} else {
			return;
		}
		
        if (cam == null)
        {
            Debug.Log("GameObject " + gameObject.name + " caused this error. MainCamera could not be found. Be sure to initialize it in-editor. \n" + transform.position, gameObject);
        }
	}
	
	//+--- METHODS	
	
	/// <summary>
	/// Gets the input. This handles input received directly from the player, and drives PC's avatar.
	/// </summary>
	/// <param name='pController'>
	/// PlayerController (PC).
	/// </param>

	public void ProcessInput(CharacterController pController)
    {	
		if(cam == null)
			RefreshMainCam();
		
		if(frozen)
			return;

		if(beingDriven == false)
		{
			vertical = Input.GetAxis (VERTICAL_AXIS);
			horizontal = Input.GetAxis(HORIZONTAL_AXIS) * turnSpeed;
		}
		
        if (pController != null)
        {
            if (vertical == 0 && horizontal == 0)
            {
				StopSteps();
            }

			Walk (pController, vertical, horizontal);
        }
    }

	private void Walk(CharacterController cc, float vertical, float horizontal)
	{
		if (vertical > 0)
		{
			cc.Move(cc.transform.TransformDirection(Vector3.forward) * walkSpeedForward * Time.deltaTime);
			StartSteps ();
		}
		else if (vertical < 0)
		{
			cc.Move(cc.transform.TransformDirection(Vector3.back) * walkSpeedBackward * Time.deltaTime);
		}

		if (horizontal > 0)
		{
			animComponent.gameObject.GetComponent<Animation>()[shuffleRightAnim].wrapMode = WrapMode.Loop;
			animComponent.gameObject.GetComponent<Animation>()[shuffleRightAnim].weight = 0.5f;
			animComponent.CrossFade(shuffleRightAnim);
			//cc.transform.RotateAround(Vector3.up * turnSpeed , Time.deltaTime * horizontal);  
		}
		else if (horizontal < 0)
		{
			animComponent.gameObject.GetComponent<Animation>()[shuffleLeftAnim].wrapMode = WrapMode.Loop;
			animComponent.gameObject.GetComponent<Animation>()[shuffleLeftAnim].weight = 0.5f;
			animComponent.CrossFade(shuffleLeftAnim);
		}

		cc.transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * horizontal);   
	}

	private void StopSteps()
	{
		if (AudioController.IsPlaying("footStepsCarpet"))
		{
			AudioController.Stop("footStepsCarpet");
		}
	}

	private void StartSteps()
	{
		if (!AudioController.IsPlaying("footStepsCarpet"))
		{
			AudioController.Play("footStepsCarpet");
		}   
	}
    
	private void LateUpdate()
	{
		//Match FPS Rotation
//		if (Camera.main != null) {
//			this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, this.transform.eulerAngles.z);
//		}

		vertical = 0f;
		//horizontal = 0f;
	}

    void DoWalk()
    {
        transform.localPosition += direction * Time.deltaTime;
        if (cam != null)
        {
            transform.localPosition = Vector3.up * Mathf.Sin(headBob);
        }
        else
        {
            Debug.Log("GameObject " + gameObject.name + " caused this error. Component could not be found. Be sure to initialize it in-editor. \n" + transform.position, gameObject);
        }
    }
	
	public void FreezeMotor() {
		frozen = true;
	}
	
	public void ThawMotor() {
		frozen = false;
	}
}
