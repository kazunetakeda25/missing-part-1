using UnityEngine;
using System.Collections;

public class MuteButton : MonoBehaviour {
	
	public KeyCode muteKey;
	public bool debugOnly;
	
	private bool muted = false;
	
	private void Start()
	{
		if(debugOnly && !Debug.isDebugBuild)
			Destroy(this.gameObject);
	}
	
	void Update () 
	{
		if(Input.GetKeyUp(muteKey))
			OnMuteKeyPressed();
	}
	
	private void OnMuteKeyPressed()
	{
		if(muted)
		{
			muted = false;
			AudioListener.volume = 1;
		}
		else
		{
			muted = true;
			AudioListener.volume = 0;
		}
	}
}
