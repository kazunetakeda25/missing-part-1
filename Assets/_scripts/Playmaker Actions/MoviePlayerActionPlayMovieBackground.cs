using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Movie)]
		[HutongGames.PlayMaker.Tooltip("Play a Movie in Resources Folder on a MoviePlayer in the scene (custom script)")]
	
	public class MoviePlayerActionPlayMovieBackground : FsmStateAction
	{
		public string movieToPlay;
		public MoviePlayer moviePlayer;
		public bool loop;
		
		public override void OnEnter ()
		{
			if(loop)
				moviePlayer.PlayAsBackgroundTexture(movieToPlay, false);
			else
				moviePlayer.PlayAsBackgroundTextureOnce(movieToPlay,false);
			
			AllDone();
		}
		
		public void AllDone() {
			Finish();
		}
		
	}
	
}
