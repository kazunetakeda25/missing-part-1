using UnityEngine;
using System.Collections;

public class ThirdPersonCameraMotor : CameraMotor
{
    public float anchorRange = 0.1f;
    public GameObject anchor;
    public Transform _transform;    
    public SphereCollider camCollider;
    public float defaultRadius = 0.8f;
    public float defaultOffsetRotation = 300;
	public float anchorDistanceTolerance = 0.1f;
	
	public float boundary = 0.2f;
	
	private float lookTargetDefaultHeight;

    private float currentRadius;
    private float currentOffset;
	
	public float lookSpeed;
	public float threshold;
	public float maxCameraTilt;
		
    private GameObject oldAnchor;
	private Quaternion targetRotation;
	
    private void Start()
    {		
        currentRadius = defaultRadius;
        currentOffset = defaultOffsetRotation;
		
        if (anchor == null)
        {
            anchor = GameObject.Find("anchor");
        }
		
        _transform = gameObject.transform;
        camCollider = gameObject.GetComponent<SphereCollider>();		
		
		lookTargetDefaultHeight = lookTarget.transform.position.y;
    }

	public override void Drive()
	{
		if(frozen)
			return;
		Transform pivot = gameObject.transform.parent;
		
		if (Vector3.Distance(pivot.position, anchor.transform.position) > anchorDistanceTolerance)
		{ 
			pivot.Translate((anchor.transform.position - gameObject.transform.position) * Time.deltaTime * 3, Space.World);
		}
		
		//+--- Vertical is driven by the EdgeScrollDriver in the PC.
		if(vertical > 0 + threshold && !frozen )
		{
			lookTarget.Rotate(Vector3.right  * Time.deltaTime * lookSpeed);
		}
		if(vertical < 0 - threshold && !frozen)
		{
			lookTarget.Rotate(Vector3.right * Time.deltaTime * lookSpeed);
		}
		
		targetRotation = Quaternion.Lerp(pivot.rotation, lookTarget.rotation, Time.deltaTime * 4);
		
		pivot.rotation = targetRotation;
		
		float newX = TransformTools.ClampAngle(pivot.localEulerAngles.x, -maxCameraTilt, maxCameraTilt);
		pivot.localEulerAngles = new Vector3(newX, pivot.localEulerAngles.y, pivot.localEulerAngles.z);
		
	}
	
}