using UnityEngine;
using System.Collections;

public class AARPGPerformanceFeedback : AARPanelGenerator {
	
	public const string TITLE_PREFIX = "Feedback on {0}:";
	public const string TITLE_CONF_BIAS = "[#FF0000]Confirmation Bias[#555555]";
	public const string TITLE_FAE = "[#FF0000]Fundamental Attribution Error[#555555]";
	
	public const string TXT_VIGNETTE_SUBTITLE = "BIAS VIGNETTE {0}";
	
	public BiasType biasType;
	public Vignette.VignetteID vignette;
	public string vignetteDescription;
	public string questionText;
	
	public override void ActivatePanel ()
	{
		aarMaster.ShowTitle(GenerateTitle());
		base.ActivatePanel ();
	}
	
	private string GenerateTitle() {
		switch(biasType) {
		case BiasType.ConfirmationBias:
			return string.Format(TITLE_PREFIX, TITLE_CONF_BIAS);
		case BiasType.FundamentalAttributionError:
			return string.Format(TITLE_PREFIX, TITLE_FAE);
		}
		
		return "Error Generating Title";
	}
	
	public override void SetupPanel() {
		panel.SetupAARComponents(AARPanel.PanelType.BiasFeedback, AARPanel.NextButtonType.Next);
	}
	
	public override void CustomizePanel ()
	{
		panel.subText1.Text = string.Format(TXT_VIGNETTE_SUBTITLE, vignetteDescription);
		panel.header2.Text = questionText;
		panel.subText2.Text = GenerateDynamicFeedbackText();
	}
	
	private string GenerateDynamicFeedbackText() 
	{
		Vignette.VignetteType vignetteType = SessionManager.GetSessionManager().vignetteManager.GetVignetteByID(vignette).vignetteType;
		
		if(vignetteType == Vignette.VignetteType.FAE) 
		{
			return ObjectiveFeedback.GetPerformanceText(vignette);
		}
		
		return FuzzyFeedback.GetPerformanceText(vignette);
	}
	
	public override void NextButtonPressed ()
	{
		base.NextButtonPressed ();
	}
	
}
