using UnityEngine;
using System.Collections;

[System.Serializable]
public class VignetteScore
{
	public bool PassedAlphaThreshold = false;
	public float ConfirmingBiasScore;
	public float DisconfirmingBiasScore;
	public float AmbigiousBiasScore;
	public float HighestMembership;
	public float FinalPsychometricScore;
	public int RawConfirmingScore;
	public int MaxConfirmingScore;
	public int RawDisconfirmingScore;
	public int MaxDisconfirmingScore;
	public int RawAmbigousScore;
	public int MaxAmbigousScore;
}
