using UnityEngine;
using System.Collections;

public abstract class AARPanelGenerator : MonoBehaviour {

	private const string AAR_PANEL_PREFAB_NAME = "gui/Generic AAR Panel";
	public UIPanelManager uiPanelManager;
	public AAR aarMaster;
	public bool storySlide;
	public bool shortSlide;
	
	protected AARPanel panel;
	
	//AARPanels must be instantiated in order by AAR Class
	public virtual void Init() {
		CreateNewPanel();
		SetupPanel();
		CustomizePanel();
	}
	
	public virtual void ActivatePanel() {
		//
	}
	
	private void CreateNewPanel() {
		InstantiateFromPrefab();
		HookUpToAAR();
	}
	
	private void InstantiateFromPrefab() {
		GameObject aarPanel = (GameObject) GameObject.Instantiate(Resources.Load(AAR_PANEL_PREFAB_NAME));
		aarPanel.transform.parent = uiPanelManager.transform;
		aarPanel.name = this.name + " PANEL";
		panel = aarPanel.GetComponent<AARPanel>();
	}
	
	private void HookUpToAAR() {
		panel.skipButton.scriptWithMethodToInvoke = aarMaster;
		panel.nextButton.scriptWithMethodToInvoke = this;
		panel.prevButton.scriptWithMethodToInvoke = this;
		
		UIPanel uiPanel = panel.gameObject.GetComponent<UIPanel>();
		//Debug.Log("Adding Panel: " + this.name + "to AAR");
		aarMaster.panels.Add(uiPanel);
		//Debug.Log("New Panel Count: " + aarMaster.panels.Count);
	}
	
	public abstract void SetupPanel();
	
	public abstract void CustomizePanel();
	
	public void SelfDestruct() {
		Destroy(this.gameObject);
	}
	
	public bool CheckToSeeIfAnyRadioIsSelected(UIRadioBtn[] radioGroup) {
		for (int i = 0; i < radioGroup.Length; i++) {
			if(radioGroup[i].Value)
				return true;
		}
		
		return false;
	}
	
	public int GetRadioValue(UIRadioBtn[] radioGroup) {
		for (int i = 0; i < radioGroup.Length; i++) {
			if(radioGroup[i].Value)
				return i;
		}
		
		return -1;
	}
	
	public virtual void NextButtonPressed() {
		aarMaster.NextButtonPressed();
	}
	
	public virtual void PrevButtonPressed() {
		aarMaster.PrevButtonPressed();
	}
	
}
