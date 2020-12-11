using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MissingComplete;

public class AAR : MonoBehaviour {
	
	public UIPanelManager aarPanelManager;
	
	public AARPanelGenerator[] panelGenerators;
	
	private List<AARPanelGenerator> activePanelGenerators = new List<AARPanelGenerator>();
	private List<UIPanel> m_Panels = new List<UIPanel>();
	public List<UIPanel> panels {
		get
		{
			return m_Panels;
		}
		set
		{
			//Protected
		}
	}
	
	public SmoothTextDisplay sectionTitle;
	
	private int index = 0;
	
	private void Awake() {
		InitializePanelGenerators();
	}
	
	private void InitializePanelGenerators() {
		for (int i = 0; i < panelGenerators.Length; i++) {
			if(panelGenerators[i] == null) 
			{
				//Skip
			} else if(panelGenerators[i].storySlide && !Settings.StoryArchiveOn()) 
			{
				//Story slide, and story archive is off, skip.
			} else if(panelGenerators[i].shortSlide && !Settings.IsLongDuration())
			{
				//Short Slide skip
			}
			else 
			{
				panelGenerators[i].Init();
				activePanelGenerators.Add(panelGenerators[i]);
			}
				
		}
	}

	SaveGameManager sgm;
	
	private void Start() {
		sgm = SaveGameManager.Instance;

		if(sgm != null) {
			index = sgm.GetCurrentSaveGame().aarCheckpoint;
		}

		BringInPanel(UIPanelManager.MENU_DIRECTION.Forwards);
		CenterAudioListener();
	}
	
	private void CenterAudioListener()
	{
		Object listener = FindObjectOfType(typeof(AudioListener));
		Destroy(listener);
		this.gameObject.AddComponent<AudioListener>();
	}
	
	public void SkipAAR() {
		EndAAR();
	}
	
	public void ShowTitle(string text) {
		if(sectionTitle.GetSpriteText().text == text)
			return;
		
		sectionTitle.SetText(text);
	}
	
	public void HideTitle() {
		sectionTitle.GetSpriteText().SetColor(ColorTools.SetColorAlpha(sectionTitle.GetSpriteText().Color, 0));
	}
	
	public void NextButtonPressed() {
		index++;
		BringInPanel(UIPanelManager.MENU_DIRECTION.Forwards);
	}
	
	public void PrevButtonPressed() {
		index--;
		BringInPanel(UIPanelManager.MENU_DIRECTION.Backwards);
	}
	
	private void BringInPanel(UIPanelManager.MENU_DIRECTION dir) {
		if(index + 1 > m_Panels.Count) {
			if(sgm != null) {
				sgm.GetCurrentSaveGame().aarCheckpoint = 0;
				sgm.SaveCurrentGame();
			}

			EndAAR();
		} else {
			if(sgm != null) {
				sgm.GetCurrentSaveGame().aarCheckpoint = index;
				sgm.SaveCurrentGame();
			}

			aarPanelManager.BringIn(m_Panels[index], dir);
			activePanelGenerators[index].ActivatePanel();
		}
	}
	
	private void EndAAR() {
		PlayMakerFSM fsm = this.gameObject.GetComponent<PlayMakerFSM>();
		
		//This occurs in AAR Prefab
		fsm.SendEvent(GlobalPlaymakerEvents.MASTER_FADE_OUT);
	}
	
}
