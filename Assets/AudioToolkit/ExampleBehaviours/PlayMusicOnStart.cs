using UnityEngine;
using System.Collections;

public class PlayMusicOnStart : MonoBehaviour
{
	public string audioID;

	void Start()
	{
		if( !string.IsNullOrEmpty( audioID ) )
		{
			AudioController.PlayMusic( audioID );
		}		
	}
}
