using UnityEngine;
using System;
using System.Collections;

public class AARPGEndOfLevel : AARPanelGenerator {
	
	public const string TITLE = "Episode {0} Complete...";
	public const string GAME_PROGRESS = "[#555555]Overall Game Progress: [#FFFFFF]{0}%";
	public const string GAMEPLAY_TIME = "[#555555]Total Gameplay Time: [#FFFFFF]{0}";
	
	public Episode episode;
	public int percentageComplete;
	
	public override void ActivatePanel ()
	{
		aarMaster.ShowTitle(string.Format(TITLE, EpisodeNumber()));
		base.ActivatePanel ();
	}
	
	private string EpisodeNumber() {
		switch(episode) {
		case Episode.Episode1:
			return "One";
		case Episode.Episode2:
			return "Two";
		case Episode.Episode3:
			return "Three";
		}
		
		return "Episode Fetch Error";
	}
	
	public override void SetupPanel() {
		AARPanel.NextButtonType button = AARPanel.NextButtonType.Done;
		
		if(episode == Episode.Episode3) {
			//button = AARPanel.NextButtonType.Quit;
		}
		
		panel.SetupAARComponents(AARPanel.PanelType.EndOfLevel, button);
	}
	
	public override void CustomizePanel ()
	{
		panel.subText1.Text = "";
		panel.header2.Text = "";
	}
	
	private void Update() {
		if(panel != null) {
			TimeSpan timeSpan = TimeSpan.FromSeconds(SessionDataManager.GetSessionDataManager().GetSessionTime());
			string progressUpdate = string.Format(GAME_PROGRESS, percentageComplete.ToString());
			//progressUpdate += "\n\n";
			//progressUpdate += string.Format(GAMEPLAY_TIME, string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds));
			panel.subText2.Text = progressUpdate;
		}
	}
	
	public override void NextButtonPressed ()
	{
		Debug.Log("Placeholder Saving AAR Data.  Please Hook Up to Instantiated SubjectData");
		base.NextButtonPressed();
	}	
}
