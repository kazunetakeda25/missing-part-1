using UnityEngine;
using System.Collections;

public class SimpleBounce : MonoBehaviour {
	
	public enum Direction {
		X,
		Y,
		Z
	}
	
	public Direction axis;
	
	public Transform start;
	public Transform end;
	
	public float speed = 1;
	
	private bool movingForward;
	
	private void Update() {
		int direction;
		if(movingForward)
			direction = 1;
		else
			direction = -1;
		
		float movementAmount = speed * Time.deltaTime * direction;
		
		Vector3 newPosition;
		switch(axis) {
		case Direction.X:
			this.transform.position = new Vector3(
				this.transform.position.x + movementAmount,
				this.transform.position.y,
				this.transform.position.z);
			CheckXLimits();
			break;
		case Direction.Y:
			this.transform.position = new Vector3(
				this.transform.position.x,
				this.transform.position.y + movementAmount,
				this.transform.position.z);
			CheckYLimits();
			break;
		case Direction.Z:
			this.transform.position = new Vector3(
				this.transform.position.x,
				this.transform.position.y,
				this.transform.position.z + movementAmount);
			CheckZLimits();
			break;
		}
	}
	
	private void CheckXLimits() {
		if(this.transform.position.x > end.position.x) {
			movingForward = false;
			this.transform.position = new Vector3(
				end.position.x,
				this.transform.position.y,
				this.transform.position.z);
		} else if(this.transform.position.x < start.position.x) {
			movingForward = true;
			this.transform.position = new Vector3(
				start.position.x,
				this.transform.position.y,
				this.transform.position.z);
		}
	}
	
	private void CheckYLimits() {
		if(this.transform.position.y > end.position.y) {
			movingForward = false;
			this.transform.position = new Vector3(
				this.transform.position.x,
				end.position.y,
				this.transform.position.z);
		} else if(this.transform.position.y < start.position.y) {
			movingForward = true;
			this.transform.position = new Vector3(
				this.transform.position.x,
				start.position.y,
				this.transform.position.z);
		}
	}
	
	private void CheckZLimits() {
		if(this.transform.position.z > end.position.z) {
			movingForward = false;
			this.transform.position = new Vector3(
				this.transform.position.x,
				this.transform.position.y,
				end.position.z);
		} else if(this.transform.position.z < start.position.z) {
			movingForward = true;
			this.transform.position = new Vector3(
				this.transform.position.x,
				this.transform.position.y,
				start.position.z);
		}
	}	
	
}
