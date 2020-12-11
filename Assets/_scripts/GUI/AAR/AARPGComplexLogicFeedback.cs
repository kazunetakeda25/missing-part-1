using UnityEngine;
using System.Collections;

public class AARPGComplexLogicFeedback : AARPanelGenerator {
	
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
		
		panel.subText1.Text = LogicFeedback.GenerateComplexDynamicLogicResult(vignette);
		panel.header2.Text = "";
		panel.subText2.Text = "";
	}
	
	public override void NextButtonPressed ()
	{
		base.NextButtonPressed ();
	}
}
