using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker.Actions;	

public class HintPopUp : MonoBehaviour {	
	private static HintPopUp instance;
	public static HintPopUp Instance { get { return instance; } }

	public delegate void ActionDelegate();
	
	public SpriteText hintText;
	private ActionDelegate m_action;

	private void Awake()
	{
		instance = this;
	}

	public void SetText(string hint) {
		hintText.Text = hint;
	}
	
	public void SetDoneEvent(ActionDelegate action) {
		m_action = action;
	}
	
	public void DoneButtonPressed() {
		if(m_action != null)
			m_action();
		instance = null;
		Destroy(this.gameObject);
	}
}
