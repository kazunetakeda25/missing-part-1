using UnityEngine;
using System;
using System.Collections;

public class SpecialHint : MonoBehaviour {

	public Action onClick;

	public void OnClick()
	{
		if(onClick != null) {
			onClick();
		}

		GameObject.Destroy(this.gameObject);
	}

	public static void Create(Action onOKButtonHit)
	{
		GameObject simpleHintGO = (GameObject) GameObject.Instantiate(Resources.Load("gui/SpecialHint"));
		SpecialHint specialHint = simpleHintGO.GetComponent<SpecialHint>();
		
		if(onOKButtonHit != null) {
			specialHint.onClick = onOKButtonHit;
		}
	}
	
}
