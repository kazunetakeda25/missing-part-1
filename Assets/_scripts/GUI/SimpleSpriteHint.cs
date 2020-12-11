using UnityEngine;
using System.Collections;

public class SimpleSpriteHint : MonoBehaviour {

	public PackedSprite spriteToShow;
	public float fadeDuration = 1;
	public float fadeDelay = 0;
	
	private FadeSprite fade;
	
	public void FadeSpriteIn() {
		if(fade != null)
			fade.Clear();		
		
		fade = FadeSprite.Do(
			spriteToShow, 
			EZAnimation.ANIM_MODE.To, 
			ColorTools.SetColorAlpha(spriteToShow.Color, 1), 
			EZAnimation.linear, 
			fadeDuration,
			fadeDelay,
			null,
			null);
	}
	
	public void FadeSpriteOut() {
		if(fade != null)
			fade.Clear();
			
		fade = FadeSprite.Do(
			spriteToShow, 
			EZAnimation.ANIM_MODE.To, 
			ColorTools.SetColorAlpha(spriteToShow.Color, 0), 
			EZAnimation.linear, 
			fadeDuration,
			fadeDelay,
			null,
			null);
	}
	
}
