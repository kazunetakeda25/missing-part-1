using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("UI/MyMug Login Screen")]
public class MyMugLoginScreen : MonoBehaviour
{
	public List<UIRadioBtn> avatarIcon = new List<UIRadioBtn>();
	public UIButton nextButton;
	public SpriteText avatarText;
	public UIButton background;
	public SpriteText completionText;
	public SimpleSprite logoBackdrop;
	public SpriteText logoText;
	public UITextField usernameTextField;
	public SpriteText usernameText;
	public SpriteText usernameHint;
	public SimpleSprite webBrowserTop;
	
	public MovieTexture m_episodeIntroMovie;
	
	private string m_finalUserName;		//What's the finally settled-upon name?
	
	string MyMugUsernameValidationDelegate(UITextField field, string text, ref int index) 
	{
		text = text.TrimStart( ' ' );
		
		//Affect our button based on how long our text is;
		//if insufficient, we'll disappear.
		if ( text.Length < 2 )
		{
			nextButton.Hide(true);
		}
		else
		{
			nextButton.Hide(false);
		}
		
		if(text.Length == 0)
		{
			usernameHint.Hide(false);
		}
		else
		{
			usernameHint.Hide(true);
		}
		return text;
	}
}

