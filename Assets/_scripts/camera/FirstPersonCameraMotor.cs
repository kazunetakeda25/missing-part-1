using UnityEngine;
using System.Collections;

public class FirstPersonCameraMotor : CameraMotor
{
	public float maxCameraTilt;
	public float cameraTiltSpeed;
	public float threshold;
	
	public override void Drive()
	{
		Transform pivot = gameObject.transform.parent;

		if (MoveCam() == true) {
			pivot.Rotate (Vector3.right * Time.deltaTime * -vertical * cameraTiltSpeed);
		}

		float newX = TransformTools.ClampAngle(pivot.localEulerAngles.x, -maxCameraTilt, maxCameraTilt);

		if(PC.GetPC().IsPlayerFrozen()) {
			pivot.localEulerAngles = new Vector3(newX, pivot.localEulerAngles.y, pivot.localEulerAngles.z);
		} else {
			pivot.localEulerAngles = new Vector3(newX, 0, 0);
		}

	}

	private bool MoveCam()
	{
		if (vertical > 0 + threshold) {
			return true;
		}

		if (vertical < 0 - threshold) {
			return true;
		}

		return false;
	}
		
}