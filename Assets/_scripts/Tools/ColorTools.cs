using UnityEngine;
using System.Collections;

public class ColorTools {
	
	public enum DefaultColors {
		ZeroAlpha,
		FullWhite,
		FullBlack,
		Red,
		Blue,
		Green
	}
	
	public static Color ChangeColorAlpha(Color colorToChange, float amountToChange) {
		return new Color(colorToChange.r, colorToChange.b, colorToChange.g, colorToChange.a + amountToChange);
	}
	
	public static Color SetColorAlpha(Color colorToSet, float newAlpha) {
		return new Color(colorToSet.r, colorToSet.b, colorToSet.g, newAlpha);
	}
	
	public static Color GetColor(DefaultColors color) {
		switch(color) {
		case DefaultColors.Blue:
			return new Color(0, 0, 1, 1);
		case DefaultColors.FullBlack:
			return new Color(0, 0, 0, 1);
		case DefaultColors.FullWhite:
			return new Color(1, 1, 1, 1);
		case DefaultColors.Green:
			return new Color(0, 1, 0, 1);
		case DefaultColors.Red:
			return new Color(1, 0, 0, 1);
		case DefaultColors.ZeroAlpha:
			return new Color(0, 0, 0, 0);
		}
		
		return new Color(0, 0, 0, 0);
	}

	public static Color GetAlphaColorChange(Color color, float alpha) { return new Color(color.r, color.g, color.b, alpha); }
	
}
