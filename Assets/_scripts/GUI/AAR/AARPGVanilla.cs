using UnityEngine;
using System.Collections;

public class AARPGVanilla : AARPanelGenerator {
	
	public string header1;
	public string subText1;
	public string header2;
	public string subText2;
	
	public override void ActivatePanel () {
		aarMaster.ShowTitle(header1);
		base.ActivatePanel ();
	}
	
	public override void SetupPanel() {
		panel.SetupAARComponents(AARPanel.PanelType.Vanilla, AARPanel.NextButtonType.Next);
	}
	
	public override void CustomizePanel () {
		panel.subText1.Text = subText1;
		panel.header2.Text = header2;
		panel.subText2.Text = subText2;
	}
	
}
