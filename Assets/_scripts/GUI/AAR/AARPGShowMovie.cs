using UnityEngine;
using System.Collections;

public class AARPGShowMovie : AARPanelGenerator {
	
	public string movie;
	public string subtitleToShow;
	
	public override void ActivatePanel ()
	{
		aarMaster.HideTitle();
		panel.moviePlayer.PlayMovie(movie, false, MovieComplete, 0.5f);
		base.ActivatePanel ();
	}
	
	public override void SetupPanel() {
		if(subtitleToShow == "") {
			panel.SetupAARComponents(AARPanel.PanelType.MovieWithSubtext, AARPanel.NextButtonType.None);
		} else {
			panel.SetupAARComponents(AARPanel.PanelType.Movie, AARPanel.NextButtonType.None);
		}
	}
	
	public override void CustomizePanel ()
	{
		panel.movieSubtitle.Text = subtitleToShow;
	}
	
	private void MovieComplete() {
		NextButtonPressed();
	}
	
}
