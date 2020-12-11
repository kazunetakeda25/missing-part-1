using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour {

	public float speed;
	public enum Direction {
		X,
		Y,
		Z
	}
	
	public Direction DirectionToRotate;
	
	private void Start() {
		iTween.Init(this.gameObject);
	}
	
	void Update () {
		rotate(Time.deltaTime);
	}
	
	private void rotate(float deltaTime) {
		float moveAmt = speed * deltaTime;
		switch(DirectionToRotate) {
		case Direction.X:
			this.transform.Rotate(moveAmt, 0, 0);
			break;
		case Direction.Y:
			this.transform.Rotate(0, moveAmt, 0);
			break;
		case Direction.Z:
			this.transform.Rotate(0, 0, moveAmt);
			break;
		}
	}
}
