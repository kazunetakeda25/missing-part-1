using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;

public class VignetteScoreTools 
{
	
	//BackCODE: This class is for gathering and outputting Vignette Scores.  These are mostly called from SubjectData
	private LevelManager levelManager;
		
	/*
	public static Message GenerateVignetteCompleteMessage(Vignette.VignetteID vignette, VignetteScore vignetteScore)
	{		
		
		return new Message_VignetteComplete(
			vignette, 
			vignetteScore.ConfirmingBiasScore, 
			vignetteScore.DisconfirmingBiasScore, 
			vignetteScore.AmbigiousBiasScore, 
			vignetteScore.HighestMembership, 
			vignetteScore.FinalPsychometricScore, 
			vignetteScore.RawConfirmingScore, 
			vignetteScore.MaxConfirmingScore, 
			vignetteScore.RawDisconfirmingScore, 
			vignetteScore.MaxDisconfirmingScore, 
			vignetteScore.RawAmbigousScore, 
			vignetteScore.MaxAmbigousScore);
	}
	
	*/
	
	public static void DebugDumpVignetteDataToConsole(VignetteScore vignette) 
	{
		Debug.Log("Final Psychometric Score: " + vignette.FinalPsychometricScore);
		Debug.Log("Highest Membership: " + vignette.HighestMembership);
		
		Debug.Log("Ambigous Bias Score: " + vignette.AmbigiousBiasScore);
		Debug.Log("Confirming Bias Score: " + vignette.ConfirmingBiasScore);
		Debug.Log("Diconfirming Bias Score: " + vignette.DisconfirmingBiasScore);
		
		Debug.Log("Raw Ambigous Score: " + vignette.RawAmbigousScore);
		Debug.Log("Max Ambigous Score: " + vignette.MaxAmbigousScore);
		Debug.Log("Raw Confirming Score: " + vignette.RawConfirmingScore);
		Debug.Log("Max Confirming Score: " + vignette.MaxConfirmingScore);
		Debug.Log("Raw Disconfirming Score: " + vignette.RawDisconfirmingScore);
		Debug.Log("Max Disconfirming Score: " + vignette.MaxDisconfirmingScore);
		Debug.Log("Searched Enough: " + vignette.PassedAlphaThreshold);
	}
	
	public static VignetteScore GetVignetteScoreReport(Vignette.VignetteID vignette)
	{
		ScoreReport scoreReport = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>().ScoringManager.GetScoresForMethod(vignette, false, false);
		
		VignetteScore vignetteScore = new VignetteScore();
		var evidenceCats = (EvidenceGroup[]) Enum.GetValues(typeof(EvidenceGroup));

		//Save the Raw and Max Data for each Evidence Group
		foreach(EvidenceGroup evGroup in evidenceCats)
		{
			switch(evGroup) {
			case EvidenceGroup.Ambiguious:
				vignetteScore.RawAmbigousScore = Mathf.RoundToInt(scoreReport.m_scores[EvidenceGroup.Ambiguious]);
				vignetteScore.MaxAmbigousScore = Mathf.RoundToInt(scoreReport.m_maxes[EvidenceGroup.Ambiguious]);
				break;
			case EvidenceGroup.Confirming:
				vignetteScore.RawConfirmingScore = Mathf.RoundToInt(scoreReport.m_scores[EvidenceGroup.Confirming]);
				vignetteScore.MaxConfirmingScore = Mathf.RoundToInt(scoreReport.m_maxes[EvidenceGroup.Confirming]);
				break;
			case EvidenceGroup.Disconfirming:
				vignetteScore.RawDisconfirmingScore = Mathf.RoundToInt(scoreReport.m_scores[EvidenceGroup.Disconfirming]);
				vignetteScore.MaxDisconfirmingScore = Mathf.RoundToInt(scoreReport.m_maxes[EvidenceGroup.Disconfirming]);
				break;
			case EvidenceGroup.None:
				break;
			}
		}
		
		
		//Save the Fuzzified Data
		EvaluationManager evalMgr = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>().EvaluationManager;
		FuzzyReasoningCognitiveBias bias = evalMgr.GetCognitiveBiasReasoning();
		
		FuzzyReasoningCognitiveBias.DefuzzifyResults result;
		
		bool bPassedAlphaThreshold;
		
		bias.GetFinalAssessment( scoreReport			
							   , out result				
							   , out bPassedAlphaThreshold
							   );
		
		vignetteScore.FinalPsychometricScore = result.m_finalDomainVal;
		vignetteScore.HighestMembership = result.m_highestMembership;
		
		vignetteScore.AmbigiousBiasScore = bias.GetCategoryTruthForDomainValue(EvidenceGroup.Ambiguious, result.m_finalDomainVal);
		vignetteScore.ConfirmingBiasScore = bias.GetCategoryTruthForDomainValue(EvidenceGroup.Confirming, result.m_finalDomainVal);
		vignetteScore.DisconfirmingBiasScore = bias.GetCategoryTruthForDomainValue(EvidenceGroup.Disconfirming, result.m_finalDomainVal);
		
		vignetteScore.PassedAlphaThreshold = bPassedAlphaThreshold;
		
		return vignetteScore;
	}
	
	private static float GetFuzzyFloatForEvidence(EvidenceGroup evidenceGroup, float finalDomainValue) {
		FuzzyReasoningCognitiveBias bias = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>().EvaluationManager.GetCognitiveBiasReasoning();
		return  bias.GetCategoryTruthForDomainValue( evidenceGroup, finalDomainValue);
	}
	
}
