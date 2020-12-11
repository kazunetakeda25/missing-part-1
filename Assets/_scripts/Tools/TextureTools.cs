using UnityEngine;
using System.Collections;

public class TextureTools {
	
	public static void SetMainTexture(GameObject target, Texture2D newTexture) {
		SetTexture("_MainTex", target, newTexture);
	}
	
	public static void SetBumpMapTexture(GameObject target, Texture2D newTexture) {
		SetTexture("_BumpMap", target, newTexture);
	}
	
	public static void SetCubeMapTexture(GameObject target, Texture2D newTexture) {
		SetTexture("_Cube", target, newTexture);
	}
	
	private static void SetTexture(string texType, GameObject target, Texture2D newTexture) {
		if(!TextureNullCheck(target))
			return;
		
		target.GetComponent<Renderer>().material.SetTexture(texType, newTexture);		
	}
	
	private static bool TextureNullCheck(GameObject target) {
		if(target.GetComponent<Renderer>().material == null) {
			Debug.LogError("GameObject does not have a material!!");
			return false;
		}
		
		return true;
	}
}
