using UnityEngine;
using System.Collections;

public class PlayerEyeline : MonoBehaviour {

	private void Start() {
		GameObject playerGO = GameObject.FindGameObjectWithTag(Tags.FIRST_PERSON_CAMERA_TRANSFORM);
		this.transform.parent = playerGO.transform;
		TransformTools.TransformPosRot(this.gameObject, playerGO.transform);
	}
	
}
