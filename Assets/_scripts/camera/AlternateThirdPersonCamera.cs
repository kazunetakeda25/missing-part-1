using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class AlternateThirdPersonCamera : CameraMotor
{
    //Properities
	public Transform target;
    public float targetHeight = 1.7f;
    public float distance = 5.0f;

    public float maxDistance = 20;
    public float minDistance = .6f;

    public float xSpeed = 100;
    public float ySpeed = 60;

    public int yMinLimit = -80;
    public int yMaxLimit = 80;

    public int zoomRate = 40;

    public float rotationDampening = 3.0f;
    public float zoomDampening = 5.0f;
	
	public float x = 0.0f;
	public float y = 0.0f;
    
	//Members
    private float currentDistance;
    private float desiredDistance;
    private float correctedDistance;
    private bool forceHit = false;
	
	public override void Drive()
    {
    }
	
	public void SetYToZero()
	{
		TweenParms parms = new TweenParms();
		parms.Prop("y", 0);
		parms.Ease(EaseType.Linear);
		
		HOTween.To(this, 1.0f, parms);
	}
	
	//Protected Methods
	
    protected override void CheckFreeze()
    {
        if (freezeStack > 0)
            frozen = true;
		
        if (freezeStack <= 0)
        {
            if (!Settings.IsFirstPerson())
            {
                forceHit = true;
                vertical = 1;
            }
            frozen = false;
        }
    }
	
	//Private Methods
	
    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;

        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;

        // Make the rigid body not chnge rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }	

    /**
     * Camera logic on LateUpdate to only update after all character movement logic has been handled.
     */	
	
    private void LateUpdate()
    {
        // Don't do anything if target is not defined
        if (!lookTarget)
        {
            Debug.Log("NO LOOK TARGET");
            return;
        }

		//if(frozen)
			//return;
        
        // If either mouse buttons are down, let the mouse govern camera position
        if /*(horizontal != 0)*/( vertical != 0)
        {
            y += vertical * xSpeed * Time.deltaTime;//0.01f;
            //x += horizontal * ySpeed * Time.deltaTime;//0.01f;
        }
        // otherwise, ease behind the target if any of the directional keys are pressed
	//        if ((horizontal != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || alternateVerticalInput != 0 || alternateHorizontalInput != 0) && !frozen)
	//        {
	//            float targetRotationAngle = lookTarget.eulerAngles.y;
	//            float currentRotationAngle = transform.eulerAngles.y;
	//            x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
	//        }
		
        float targetRotationAngle = lookTarget.eulerAngles.y;
        float currentRotationAngle = transform.eulerAngles.y;
        x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);		
		
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        // set camera rotation
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        // calculate the desired distance
        //desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        correctedDistance = desiredDistance;

        // calculate desired camera position
        Vector3 position = lookTarget.position - (rotation * Vector3.forward * desiredDistance + new Vector3(0, -targetHeight, 0));

        // check for collision using the true target's desired registration point as set by user using height
        RaycastHit collisionHit;
        Vector3 trueTargetPosition = new Vector3(lookTarget.position.x, lookTarget.position.y + targetHeight, lookTarget.position.z);

        // if there was a collision, correct the camera position and calculate the corrected distance
        bool isCorrected = false;
        if (Physics.Linecast(trueTargetPosition, position, out collisionHit))
        {
            position = collisionHit.point;
            correctedDistance = Vector3.Distance(trueTargetPosition, position);
            isCorrected = true;
        }

        // For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance
        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;

        // recalculate position based on the new currentDistance
        position = lookTarget.position - (rotation * Vector3.forward * currentDistance + new Vector3(0, -targetHeight, 0));
        transform.parent.transform.rotation = rotation;
        transform.parent.transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}