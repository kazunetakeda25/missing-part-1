using UnityEngine;
using System.Collections;

public abstract class ScoringScheme
{
	public abstract ScoringMethod GetMethod();
	public virtual string GetMethodDisplayName()
	{
		//For the very lazy.
		return GetMethod().ToString();
	}
	
	public float GetScoreForComponent( ScoringManager.ScoreComponent component
									 , objectReport report
									 )
	{
		switch( component )
		{
		case ScoringManager.ScoreComponent.LookedAt:
			return GetScoreForMethod_LookedAt( report );
		case ScoringManager.ScoreComponent.InteractedWith:
			return GetScoreForMethod_InteractedWith( report );
		case ScoringManager.ScoreComponent.Photographed:
			return GetScoreForMethod_Photographed( report );
		case ScoringManager.ScoreComponent.InPicture:
			return GetScoreForMethod_InPicture( report );
		case ScoringManager.ScoreComponent.SubmittedEvidence:
			return GetScoreForMethod_SubmittedEvidence( report );
		default:
			Debug.LogError( "Unknown/unsupported score component encountered: " + component.ToString() + "!" );
			break;
		}
		
		return 0.0f;
	}

	//Returns the min and max values *POSSIBLE*
	public abstract void GetMinMaxValuesForEntry
		( 
			objectReport report
			, out float minVal
			, out float maxVal
			, bool areAttachmentsAllowed
		);
	
	public abstract float GetScoreForMethod_LookedAt( objectReport report );
	public abstract float GetScoreForMethod_InteractedWith( objectReport report );
	public abstract float GetScoreForMethod_Photographed( objectReport report );
	public abstract float GetScoreForMethod_InPicture( objectReport report );
	public abstract float GetScoreForMethod_SubmittedEvidence( objectReport report );
}

