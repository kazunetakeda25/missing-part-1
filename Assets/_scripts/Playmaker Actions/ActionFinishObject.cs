using UnityEngine;
using System.Collections;

public class ActionFinishObject : MonoBehaviour {
	
	public delegate void ActionDelegate();
	
	private ActionDelegate m_Action;
	
	public void setAction(ActionDelegate action) {
		m_Action = action;
	}
	
	public void Finish() {
		m_Action();
		Destroy(this.gameObject);
	}
		
}
