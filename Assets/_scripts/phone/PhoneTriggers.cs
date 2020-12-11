using UnityEngine;
using System.Collections;

public class PhoneTriggers : MonoBehaviour {
	
	public InteractiveMap map;
	
	public void OpenSMS() 
	{
		//Debug.Log("Opening SMS Screen");
		SmartPhone.GetSmartPhone().OpenPhone(SmartPhone.Mode.SMS);
	}
	
	public void OpenPhotoAlbum() 
	{
		//Debug.Log("Opening Photo Album");
		SmartPhone.GetSmartPhone().OpenPhone(SmartPhone.Mode.PhotoAlbum);
	}
	
	public void OpenMap() 
	{
		//Debug.Log("Map Button Pressed");
		map.EnableMap();
	}
	
	public void OpenHelp()
	{
		//Debug.Log("Help Button Pressed");
		HelpScreen.ShowHelpScreen();
	}
}
