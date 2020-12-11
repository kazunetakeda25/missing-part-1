using UnityEngine;
using System.Collections;

public class PlaymakerEventTrigger : MonoBehaviour {
	
	public string eventToRun;
	public bool selfDestruct;
	
	private void OnTriggerEnter(Collider collider) {
		if(collider.gameObject.tag == "Player") {
			PlayMakerFSM.BroadcastEvent(eventToRun);
			
			if(selfDestruct)
				Destroy(this.gameObject);
		}
	}
	
}
