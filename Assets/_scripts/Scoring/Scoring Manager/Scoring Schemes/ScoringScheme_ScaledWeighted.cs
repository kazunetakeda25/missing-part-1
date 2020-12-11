using UnityEngine;
using System.Collections;

public class ScoringScheme_ScaledWeighted : ScoringScheme
{
	
	public override ScoringMethod GetMethod()
	{
		return ScoringMethod.EvidenceWeightedScaledScoring;
	}
	
	public override string GetMethodDisplayName()
	{
		return "WEIGHTED SCALED SCORING";
	}
	
	//Returns the min and max values *POSSIBLE*
	public override void GetMinMaxValuesForEntry
		( 
			objectReport report
			, out float minVal
			, out float maxVal 
			, bool areAttachmentsAllowed
		)
	{
		minVal = 0.0f * report.m_evidenceStrength;
		
		maxVal = 0;
		
		if(areAttachmentsAllowed)
			maxVal += 8.0f * report.m_evidenceStrength;
		
		maxVal += (1.0f + 2.0f + 4.0f) * report.m_evidenceStrength;
	}
	
	public override float GetScoreForMethod_LookedAt( objectReport report )
	{
		if ( report.m_lookCount > 0 )
		{
			return 1.0f * report.m_evidenceStrength;
		}
		return 0.0f;
	}

	public override float GetScoreForMethod_InteractedWith( objectReport report )
	{
		if ( report.m_interactCount > 0 )
		{
			return 2.0f * report.m_evidenceStrength;
		}
		return 0.0f;
	}
	
	public override float GetScoreForMethod_Photographed( objectReport report )
	{
		if ( report.m_photographedCount > 0 )
		{
			return 4.0f * report.m_evidenceStrength;
		}
		return 0.0f;
	}
	
	public override float GetScoreForMethod_InPicture( objectReport report )
	{
		return 0.0f;
	}
	
	public override float GetScoreForMethod_SubmittedEvidence( objectReport report )
	{
		if ( report.m_attachCount > 0 )
		{
			return 8.0f * report.m_evidenceStrength;
		}
		return 0.0f;
	}
}

