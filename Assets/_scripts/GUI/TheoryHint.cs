using UnityEngine;
using System.Collections;

public class TheoryHint : MonoBehaviour {
	
	private const string THEORY_PLANT_HUGGER = "Mike has a theory that watering plants once a week is the key to keeping them healthy. \nWhat's the best way to test this theory?";
	private const string THEORY_COPY_PROTECTION = "Maureen has a theory that Rocket Copy's work is perfect. \nWhat's the best way to test this theory?";
	private const string THEORY_DEADLY_TREATMENT = "Stephanie has a theory that all green pills get shipped to Africa. \nWhich boxes should you open to most efficiently test this theory?";
	private const string THEORY_TO_YOUR_HEALTH = "Paige has a theory that the Moray single malts have the mushroom flavor notes. \nWhat two whiskeys best test this theory?";
	
	public SpriteText text;
	
	public static void SpawnTheoryHint()
	{
		Vignette.VignetteID vignette = SessionManager.GetSessionManager().vignetteManager.currentVignette.vignetteID;
		GameObject theorySubtitle = (GameObject) GameObject.Instantiate(Resources.Load(ResourcePaths.THEORY_SUBTITLE));
		
		//Parent to Response Grid for destruction later.
		GameObject playerResponseGrid = GameObject.FindGameObjectWithTag(Tags.PLAYER_RESPONSE_GRID);
		theorySubtitle.transform.parent = playerResponseGrid.transform;
		
		TheoryHint hint = theorySubtitle.GetComponent<TheoryHint>();
		hint.SetupSubtitle(vignette);
	}
	
	private void SetupSubtitle(Vignette.VignetteID vignette)
	{
		string theory = "";
		
		switch(vignette)
		{
		case Vignette.VignetteID.E1vPlantHugger:
			theory = THEORY_PLANT_HUGGER;
			break;
		case Vignette.VignetteID.E2vCopyProtection:
			theory = THEORY_COPY_PROTECTION;
			break;
		case Vignette.VignetteID.E2vDeadlyTreatment:
			theory = THEORY_DEADLY_TREATMENT;
			break;
		case Vignette.VignetteID.E3vToYourHealth:
			theory = THEORY_TO_YOUR_HEALTH;
			break;
		}
		
		text.Text = theory;
	}
	
}
