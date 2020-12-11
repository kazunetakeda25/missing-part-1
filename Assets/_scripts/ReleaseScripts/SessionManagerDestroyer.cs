using UnityEngine;
using System.Collections;

public class SessionManagerDestroyer : MonoBehaviour 
{

	private void Awake()
	{
		GameObject sessionManagerGO = GameObject.FindGameObjectWithTag(Tags.SESSION_MANAGER_TAG);
		if(sessionManagerGO != null) {
			GameObject.Destroy(sessionManagerGO);
		}

		GameObject.Destroy(this.gameObject);
		return;
	}

}
