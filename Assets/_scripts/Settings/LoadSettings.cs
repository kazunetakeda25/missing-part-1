using UnityEngine;
using System.Collections;

public class LoadSettings : MonoBehaviour {
	
	public const string SETTINGS_TEMPLATE_PANEL = "Settings Tempalte";
	public UIPanelManager overlayManager;
	public SpriteText currentTemplate;
	public CreateSession createSession;
	
	public void LoadC1()
	{
		SetTemplate(Settings.Template.C1_H1N1L1);
	}
	
	public void LoadC2()
	{
		SetTemplate(Settings.Template.C2_H1N1S1);
	}
	
	public void LoadC3()
	{
		SetTemplate(Settings.Template.C3_H1N0L1);
	}
		
	public void LoadC4()
	{
		SetTemplate(Settings.Template.C4_H1N0S1);
	}
	
	public void LoadC5()
	{
		SetTemplate(Settings.Template.C5_H1N1L3);
	}
	
	public void LoadC6()
	{
		SetTemplate(Settings.Template.C6_H1N1S3);
	}
	
	public void LoadC7()
	{
		SetTemplate(Settings.Template.C7_H1N0L3);
	}
	
	public void LoadC8()
	{
		SetTemplate(Settings.Template.C8_H1N0S3);
	}
	
	private void SetTemplate(Settings.Template template)
	{
		Settings.SetTemplate(template);
		currentTemplate.Text = template.ToString();
		createSession.SetupCreateSessionScreen();
	}
	
	public void ResetToDefault()
	{
		Settings.NewGameSession();
		createSession.SetupCreateSessionScreen();
		currentTemplate.Text = "";
	}
	
	public void Dismiss()
	{
		overlayManager.Dismiss();
	}
	
	public void BringIn()
	{
		overlayManager.BringIn(SETTINGS_TEMPLATE_PANEL);
	}
	
}
