using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker.Actions;	

public class PausePopUp : MonoBehaviour {	
	public delegate void ActionDelegate();
	
	private ActionDelegate m_action;
	
	public void SetDoneEvent(ActionDelegate action) {
		m_action = action;
	}

	public void OnQuit()
	{
		MissingComplete.SaveGameManager.Instance.UnloadSavedGame();
		Application.LoadLevel("MAIN_MENU");
	}
	
	public void DoneButtonPressed() {
		if(m_action != null)
			m_action();
		
		Destroy(this.gameObject);
	}
}
