using UnityEngine;
using System;
using System.Collections;

public class Password : MonoBehaviour {
	
	private string pass = "sec0ndL1fe";
	
	public UITextField textField;
	public string levelToLoad;
	
	public void OnClearButton()
	{
		textField.Text = "";
	}
	
	private void Awake()
	{
		UIManager.instance.FocusObject = textField;
	}
	
	private void Update()
	{
		if(String.Equals(textField.text, pass))
		{
			Debug.Log("LAUNCH!!");
			Application.LoadLevel(levelToLoad);
		}
	}
	
}
