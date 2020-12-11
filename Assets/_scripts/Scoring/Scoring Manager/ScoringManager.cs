using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public enum ScoringMethod
{
	SimpleScoring,
	ScaledScoring,
	EvidenceWeightedSimpleScoring,
	EvidenceWeightedScaledScoring,
}

public class ScoreReport
{
	// Dictionary of evidence groups to minimums.
	public Dictionary<EvidenceGroup, float> m_mins = new Dictionary<EvidenceGroup, float>();
	// Dictionary of evidence groups to scores.
	public Dictionary<EvidenceGroup, float> m_scores = new Dictionary<EvidenceGroup, float>();
	// Dictionary of evidence groups to maximums.
	public Dictionary<EvidenceGroup, float> m_maxes = new Dictionary<EvidenceGroup, float>();
	
	public ScoreReport()
	{
		// Set all scores to 0, based around all evidence group types used.
		var array = (EvidenceGroup[]) Enum.GetValues(typeof(EvidenceGroup));
		foreach(EvidenceGroup eGroup in array)
		{
			if( m_scores.ContainsKey(eGroup) )
			{
				m_mins[eGroup] = 0.0f;
				m_scores[eGroup] = 0.0f;
				m_maxes[eGroup] = 0.0f;
			}
			else //Go ahead and add it.
			{
				m_mins.Add(eGroup, 0.0f);
				m_scores.Add(eGroup, 0.0f);
				m_maxes.Add(eGroup, 0.0f);
			}
		}
	}
	
	public void PrintGroupResults()
	{
		var array = (EvidenceGroup[]) Enum.GetValues(typeof(EvidenceGroup));
		// Debug log all the group scores.
		
		foreach(EvidenceGroup eGroup in array)
			if( m_scores.ContainsKey(eGroup) )
				Debug.Log("Group " + eGroup.ToString() + " has " + m_scores[eGroup] + " points, from within a range of " + m_mins[eGroup] + " to " + m_maxes[eGroup] + ".");
	}
	
	public float GetPlayerPercentForGroup( EvidenceGroup evGroup )
	{
		//Gets the percentage the player was within min/max.
		float minVal = m_mins[ evGroup ];
		float maxVal = m_maxes[ evGroup ];
		float playerScore = m_scores[ evGroup ];
		
		float totalDelta = maxVal - minVal;
		if ( totalDelta > 0.0f )
		{
			float pct = ( playerScore - minVal ) / totalDelta;
			return pct;
		}
		
		return 0.0f;
	}		
}

[AddComponentMenu("Framework/Scoring Manager")]
public class ScoringManager : MonoBehaviour
{
	public enum ScoreComponent
	{
		LookedAt,
		InteractedWith,
		Photographed,
		InPicture,
		SubmittedEvidence,
	}
	
	private ScoringScheme m_currScheme = new ScoringScheme_ScaledWeighted();
	private LevelManager levelManager;
	
	public ScoringScheme GetScoringScheme()
	{
		return m_currScheme;
	}
	
	public ScoreReport GetScoresForMethod
		( 
			Vignette.VignetteID vignetteID, 
			bool bPrintIndividualScores, 
			bool bPrintGroupScores
		)
	{
		if(levelManager == null)
			levelManager = GameObject.FindGameObjectWithTag(Tags.LEVEL_MANAGER_TAG).GetComponent<LevelManager>();
		
		ScoreReport results = new ScoreReport();
		
		EvidenceReport evidenceFacts = levelManager.EvidenceManager.getEvidenceReport( vignetteID, bPrintIndividualScores );
		
		//All enum types.
		ScoreComponent[] scoreComponents = (ScoreComponent[])Enum.GetValues( typeof( ScoreComponent ) );
		
		//Iterate through each piece of evidence.
		foreach( KeyValuePair< string, objectReport > entry in evidenceFacts.m_facts )
		{
			objectReport report = entry.Value;
			
			//Is this object related to the vignette ID specified at all?

			float minAdjustment, maxAdjustment;
			m_currScheme.GetMinMaxValuesForEntry
				( 
					report
					, out minAdjustment
					, out maxAdjustment 
					, AreAttachmentsAllowed(vignetteID)
					);
			results.m_mins[ report.m_group ] += minAdjustment;
			results.m_maxes[ report.m_group ] += maxAdjustment;
			
			float currScore = 0.0f;
			foreach( ScoreComponent component in scoreComponents )
			{
				currScore += m_currScheme.GetScoreForComponent( component, report );
			}
			
			results.m_scores[ report.m_group ] += currScore;
			
			if ( bPrintIndividualScores )
				Debug.Log( entry.Key + " contributed " + currScore + " points to its group: "  + entry.Value.m_group);
		}
		
		/*
		 * JWC:  Leaving around for reference!
		switch(method)
		{		
			case ScoringMethod.SimpleScoring:
			{		
				foreach(KeyValuePair<string, objectReport> entry in evidenceFacts.m_facts)
				{		
					// Now use the facts and scoring method to determine how many points this adds.
					float minAdjustment = 0.0f;	
					float scoreAdjustment = 0.0f;
					float maxAdjustment = 4.0f;			
			
					// One point if it was ever looked at.
					if(entry.Value.m_lookCount > 0)
					{
						scoreAdjustment += 1.0f;
					}				
					// One point if it was ever interacted with.
					if(entry.Value.m_interactCount > 0)
					{
						scoreAdjustment += 1.0f;
					}
					// One point if it was ever photographed.
					if(entry.Value.m_photographedCount > 0)
					{
						scoreAdjustment += 1.0f;
					}
					// No distinction between photographed and in a picture currently.
					if(entry.Value.m_inPictureCount > 0)
					{						
					}
					// One point if it was ever sent as an attachment.
					if(entry.Value.m_attachCount > 0)
					{
						scoreAdjustment += 1.0f;
					}				
					
					// Add the score adjustment to the appropriate group.
					results.m_mins[entry.Value.m_group] += minAdjustment;
					results.m_scores[entry.Value.m_group] += scoreAdjustment;
					results.m_maxes[entry.Value.m_group] += maxAdjustment;
					Debug.Log(entry.Key + " contributed " + scoreAdjustment + " points to its group.");
				}
			
				break;
			}
			
			case ScoringMethod.ScaledScoring:
			{
				foreach(KeyValuePair<string, objectReport> entry in evidenceFacts.m_facts)
				{		
					// Now use the facts and scoring method to determine how many points this adds.
					float minAdjustment = 0.0f;	
					float scoreAdjustment = 0.0f;
					float maxAdjustment =  15.0f;			
			
					// One point if it was ever looked at.
					if(entry.Value.m_lookCount > 0)
					{
						scoreAdjustment += 1.0f;
					}	
					// Two points if it was ever interacted with.
					if(entry.Value.m_interactCount > 0)
					{
						scoreAdjustment += 2.0f;
					}
					// Four points if it was ever photographed.
					if(entry.Value.m_photographedCount > 0)
					{
						scoreAdjustment += 4.0f;
					}
					// No distinction between photographed and in a picture currently.
					if(entry.Value.m_inPictureCount > 0)
					{						
					}
					// Eight points if it was ever sent as an attachment.
					if(entry.Value.m_attachCount > 0)
					{
						scoreAdjustment += 8.0f;
					}	
					
					// Add the score adjustment to the appropriate group.
					results.m_mins[entry.Value.m_group] += minAdjustment;
					results.m_scores[entry.Value.m_group] += scoreAdjustment;
					results.m_maxes[entry.Value.m_group] += maxAdjustment;
					Debug.Log(entry.Key + " contributed " + scoreAdjustment + " points to its group.");
				}
			
				break;
			}
			
			case ScoringMethod.EvidenceWeightedSimpleScoring:
			{		
				foreach(KeyValuePair<string, objectReport> entry in evidenceFacts.m_facts)
				{		
					// Now use the facts and scoring method to determine how many points this adds.
					float minAdjustment = 0.0f;	
					float scoreAdjustment = 0.0f;
					float maxAdjustment = 4.0f;			
			
					// One point if it was ever looked at.
					if(entry.Value.m_lookCount > 0)
					{
						scoreAdjustment += 1.0f;
					}				
					// One point if it was ever interacted with.
					if(entry.Value.m_interactCount > 0)
					{
						scoreAdjustment += 1.0f;
					}
					// One point if it was ever photographed.
					if(entry.Value.m_photographedCount > 0)
					{
						scoreAdjustment += 1.0f;
					}
					// No distinction between photographed and in a picture currently.
					if(entry.Value.m_inPictureCount > 0)
					{						
					}
					// One point if it was ever sent as an attachment.
					if(entry.Value.m_attachCount > 0)
					{
						scoreAdjustment += 1.0f;
					}		
				
					// Multiple the values by the evidence strength.
					minAdjustment = minAdjustment * entry.Value.m_evidenceStrength;
					scoreAdjustment = scoreAdjustment * entry.Value.m_evidenceStrength;
					maxAdjustment = maxAdjustment * entry.Value.m_evidenceStrength;
					
					// Add the score adjustment to the appropriate group.
					results.m_mins[entry.Value.m_group] += minAdjustment;
					results.m_scores[entry.Value.m_group] += scoreAdjustment;
					results.m_maxes[entry.Value.m_group] += maxAdjustment;
					Debug.Log(entry.Key + " contributed " + scoreAdjustment + " points to its group.");
				}
			
				break;
			}
			
			case ScoringMethod.EvidenceWeightedScaledScoring:
			{
				foreach(KeyValuePair<string, objectReport> entry in evidenceFacts.m_facts)
				{		
					// Now use the facts and scoring method to determine how many points this adds.
					float minAdjustment = 0.0f;	
					float scoreAdjustment = 0.0f;
					float maxAdjustment =  15.0f;			
			
					// One point if it was ever looked at.
					if(entry.Value.m_lookCount > 0)
					{
						scoreAdjustment += 1.0f;
					}	
					// Two points if it was ever interacted with.
					if(entry.Value.m_interactCount > 0)
					{
						scoreAdjustment += 2.0f;
					}
					// Four points if it was ever photographed.
					if(entry.Value.m_photographedCount > 0)
					{
						scoreAdjustment += 4.0f;
					}
					// No distinction between photographed and in a picture currently.
					if(entry.Value.m_inPictureCount > 0)
					{						
					}
					// Eight points if it was ever sent as an attachment.
					if(entry.Value.m_attachCount > 0)
					{
						scoreAdjustment += 8.0f;
					}
				
					// Multiple the values by the evidence strength.
					minAdjustment = minAdjustment * entry.Value.m_evidenceStrength;
					scoreAdjustment = scoreAdjustment * entry.Value.m_evidenceStrength;
					maxAdjustment = maxAdjustment * entry.Value.m_evidenceStrength;
					
					// Add the score adjustment to the appropriate group.
					results.m_mins[entry.Value.m_group] += minAdjustment;
					results.m_scores[entry.Value.m_group] += scoreAdjustment;
					results.m_maxes[entry.Value.m_group] += maxAdjustment;
					Debug.Log(entry.Key + " contributed " + scoreAdjustment + " points to its group.");
				}
			
				break;
			}
		}
		*/
		
		if ( bPrintGroupScores )
			results.PrintGroupResults();
			
		return results;
	}
	
	private bool AreAttachmentsAllowed(Vignette.VignetteID vignette)
	{
		if(vignette == Vignette.VignetteID.E1vTerrysApartmentSearch)
			return true;
		
		return false;
	}
	
	private void Start()
	{
		levelManager = GameObject.FindGameObjectWithTag(Tags.LEVEL_MANAGER_TAG).GetComponent<LevelManager>();
	}
}