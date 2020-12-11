using UnityEngine;
using System.Collections;

public class GameObjectTools {

	public static void DestroyAllObjectsWithTag(string tag) {
		GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
		
		for (int i = 0; i < gos.Length; i++) {
			GameObject.Destroy(gos[i]);
		}
	}
	
	
}
