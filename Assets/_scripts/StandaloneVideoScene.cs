using UnityEngine;
using System.Collections;

public class StandaloneVideoScene : MonoBehaviour 
{

	[SerializeField] MoviePlayer moviePlayer;
	[SerializeField] string movie;
	[SerializeField] string nextLevel;

	private void Start()
	{
		moviePlayer.PlayMovie ("MyMug", true, OnComplete);
	}

	private void OnComplete()
	{
		Application.LoadLevel(nextLevel);
	}
}
