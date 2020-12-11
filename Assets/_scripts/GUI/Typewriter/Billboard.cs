/*
 * Copyright 2010 - David Koontz
 * Please submit bug reports and feature requests/suggestions to david@koontzfamily.org
 */

using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {
    public Camera cameraToFace;
	
	void Start() {
		if(null == cameraToFace) {
			cameraToFace = Camera.main;
		}
	}
	
    void Update() {
		transform.LookAt(cameraToFace.transform.position);
    }
}