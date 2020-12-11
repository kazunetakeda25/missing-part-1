using UnityEngine;
using System.Collections;

public class FollowRotation : MonoBehaviour {
	
	public enum Axis {
		x,
		y,
		z
	}
	
	public Transform target;
	public Axis axis;
	
	private void LateUpdate() 
	{
		Vector3 newRotation = Vector3.zero;
		
		switch(axis) {
		case Axis.x:
			newRotation = new Vector3(target.transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
			break;
		case Axis.y:
			newRotation = new Vector3(this.transform.eulerAngles.x, target.transform.eulerAngles.y, this.transform.eulerAngles.z);
			break;
		case Axis.z:
			newRotation = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, target.transform.eulerAngles.z);
			break;			
		}
		
		this.transform.eulerAngles = newRotation;
	}
	
}
