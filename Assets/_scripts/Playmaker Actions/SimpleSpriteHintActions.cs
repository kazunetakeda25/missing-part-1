using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions 
{
	
	[ActionCategory(ActionCategory.GUI)]
	public class ShowSimpleSpriteHint : FsmStateAction 
	{
		public SimpleSpriteHint hint;
		
		public override	void OnEnter() 
		{	
			hint.FadeSpriteIn();
			Finish();
		}
		
	}
	
	[ActionCategory(ActionCategory.GUI)]
	public class HideSimpleSpriteHint : FsmStateAction 
	{
		public SimpleSpriteHint hint;
		
		public override	void OnEnter() 
		{
			if(hint != null) {
				GameObject.Destroy(hint.gameObject);
			}
				
			Finish();
		}
		
	}	
	
}
