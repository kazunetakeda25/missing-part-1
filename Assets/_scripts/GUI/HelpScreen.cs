using UnityEngine;
using System.Collections;

public class HelpScreen : MonoBehaviour {
	
	private static float oldTimeScale;
	
	public static void ShowHelpScreen()
	{
		if(Time.timeScale == 0)
			return;

		ReportEvent.ScreenActivated(ScreenType.FAQ);
		GameObject.Instantiate(Resources.Load(ResourcePaths.HELP_SCREEN_OBJECT));
		oldTimeScale = Time.timeScale;
		Time.timeScale = 0;
	}
	
	public void CleanUp() 
	{
		Debug.Log ("Cleanup");
		ReportEvent.ScreenDeactivated(ScreenType.FAQ);
		Destroy(this.gameObject);
		Time.timeScale = oldTimeScale;
	}
	
}
