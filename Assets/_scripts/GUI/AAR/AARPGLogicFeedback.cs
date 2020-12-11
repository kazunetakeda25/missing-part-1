using UnityEngine;
using System.Collections;

public class AARPGLogicFeedback : AARPanelGenerator {
	
	public Vignette.VignetteID vignette;
	
	public override void ActivatePanel ()
	{
		aarMaster.ShowTitle(LogicFeedback.GetLogicTitle(vignette));
		base.ActivatePanel ();
	}
	
	public override void SetupPanel() {
		panel.SetupAARComponents(AARPanel.PanelType.BiasFeedback, AARPanel.NextButtonType.Next);
	}
	
	public override void CustomizePanel ()
	{
		string[] generatedStrings = LogicFeedback.GenerateSimpleDynamicLogicResult(vignette);
		
		panel.subText1.Text = "";
		panel.header2.Text = "";
		panel.subText2.Text = generatedStrings[0] + "\n\n" + generatedStrings[1];
	}
	
	public override void NextButtonPressed ()
	{
		base.NextButtonPressed ();
	}
	
}
