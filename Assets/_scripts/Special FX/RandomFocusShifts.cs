using UnityEngine;
using System.Collections;

//[RequireComponent (typeof(DepthOfField34))]
public class RandomFocusShifts : MonoBehaviour {
	
	public bool effectEnabled;
	
	public float focusShiftDuration = 3f;
	public float focusShiftSpeed = 1f;
	public float focusExtremes = 3.5f;
	
	private bool up;
	
	//private DepthOfField34 effect;
	
	
	private void Awake() {
		//effect = this.gameObject.GetComponent<DepthOfField34>();
		
	}
	
	void Update () {
	
	}
}
