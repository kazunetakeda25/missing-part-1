using UnityEngine;
using System.Collections;

public class VignetteDebugger : MonoBehaviour {

	private void OnGUI() {
		SessionManager sessionManager = SessionManager.GetSessionManager();
		
		if(sessionManager != null) {
			if(Application.isEditor || Debug.isDebugBuild) {
				string vignette = "";
				
				if(sessionManager.vignetteManager.isVignetteActive())
					vignette = "Current Vignette is: " + sessionManager.vignetteManager.currentVignette.vignetteID.ToString();
				else
					vignette = "Vignette " + sessionManager.vignetteManager.currentVignette.vignetteID.ToString() + " is complete.";
					
				GUI.Label(new Rect(Screen.width / 2, Screen.height - 50, 300, 300), vignette);
			}
		}
	}
}
