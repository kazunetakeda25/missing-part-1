using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.GameObject)]
		[HutongGames.PlayMaker.Tooltip("Instantiates GameObject from Prefab at a set positon.")]	
	
	public class InstantiateGameObjectFromPrefab : FsmStateAction {
		public GameObject prefabToInstantiate;
		public Transform destinationPosition = null;
		
		public override	void OnEnter() {
			if(destinationPosition == null) {
				GameObject.Instantiate(prefabToInstantiate);
			} else {
				GameObject.Instantiate(prefabToInstantiate, destinationPosition.position, destinationPosition.rotation);
			}
			Finish();
		}
		
	}
	
}
