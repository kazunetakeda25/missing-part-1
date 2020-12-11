using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Movie)]
		[HutongGames.PlayMaker.Tooltip("Play a Movie in Resources Folder on a MoviePlayer (custom script)")]
	
	public class MoviePlayerActionPlayMovie : FsmStateAction
	{
		public string movieToPlay;
		public bool fullScreenFadeout;
		public MoviePlayer moviePlayer;
		
		public override void OnEnter ()
		{
			moviePlayer.PlayMovie(movieToPlay, fullScreenFadeout, AllDone);
		}
		
		public void AllDone() {
			Finish();
		}
		
	}
	
}
