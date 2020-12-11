using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class FuzzyReasoningCognitiveBias
{
	//Default #/samples per set (note that an output var may use more)
	private static int sk_defaultNumSetSamples = 256;
	
	//"Indicative" fuzzy var, used for predicate of conditional rules
	//for matching player's score in a particular category to its
	//consequent set.  Has several sets within it.
	private FuzzyVariable m_fuzzyVarIndicative;
	private FuzzySet m_fuzzySetIndicativeConfirming;
	private FuzzySet m_fuzzySetIndicativeDisconfirming;
	private FuzzySet m_fuzzySetIndicativeAmbiguous;
	public static string sk_fuzzyVarIndicativeStr = "IndicativeOf";
	public static string sk_fuzzySetIndicativeConfirmingStr = "Confirming";
	public static string sk_fuzzySetIndicativeDisconfirmingStr = "Disconfirming";
	public static string sk_fuzzySetIndicativeAmbiguousStr = "Ambiguous";
	
	//Consequent fuzzy variable & sets.
	private FuzzyVariable m_fuzzyVarFinalScore;
	private FuzzySet m_fuzzySetFinalScoreConfirming;
	private FuzzySet m_fuzzySetFinalScoreDisconfirming;
	private FuzzySet m_fuzzySetFinalScoreAmbiguous;
	public static string sk_fuzzyVarFinalScoreStr = "FinalScore";
	public static string sk_fuzzySetFinalScoreConfirmingStr = "Confirming";
	public static string sk_fuzzySetFinalScoreDisconfirmingStr = "Disconfirming";
	public static string sk_fuzzySetFinalScoreAmbiguousStr = "Ambiguous";
	
	public float m_alphaThreshold = 0.1f;

	public enum Intensity
	{
		None,
		AlmostNone,
		Mild,
		Somewhat,
		Significant,
		Intense,
		ExtremelyIntense,
		Complete,
	};
	
	
	[System.Serializable]
	public class LinguisticCategory
	{
		public float m_truthBeginVal;
		public float m_truthEndVal;
		public string m_categoryString;
	}
	
	public LinguisticCategory[] m_linguisticCategories;
	
	public FuzzyReasoningCognitiveBias()
	{
		//Rez it all up.
		
		//INDICATIVE VAR & SETS
		{
			m_fuzzyVarIndicative = 
				new FuzzyVariable( sk_fuzzyVarIndicativeStr	//Name
													, 0.0f	//Universe of discourse begin
													, 1.0f	//UoD end
													);
			m_fuzzySetIndicativeConfirming = 
				new FuzzySet( sk_fuzzySetIndicativeConfirmingStr	//Name
							, new GraphSlopeIncreasing()			//Graph
							, 0.0f									//Domain begin
							, 1.0f									//Domain end
							, sk_defaultNumSetSamples				//#/samples
							);
			m_fuzzySetIndicativeDisconfirming = 
				new FuzzySet( sk_fuzzySetIndicativeDisconfirmingStr	//Name
							, new GraphSlopeIncreasing()			//Graph
							, 0.0f									//Domain begin
							, 1.0f									//Domain end
							, sk_defaultNumSetSamples				//#/samples
							);
			m_fuzzySetIndicativeAmbiguous = 
				new FuzzySet( sk_fuzzySetIndicativeAmbiguousStr		//Name
							, new GraphSlopeIncreasing()			//Graph
							, 0.0f									//Domain begin
							, 1.0f									//Domain end
							, sk_defaultNumSetSamples				//#/samples
							);
			m_fuzzyVarIndicative.AddSet( m_fuzzySetIndicativeConfirming );
			m_fuzzyVarIndicative.AddSet( m_fuzzySetIndicativeDisconfirming );
			m_fuzzyVarIndicative.AddSet( m_fuzzySetIndicativeAmbiguous );
		}
		
		//CONSEQUENT VAR & SETS
		{
			m_fuzzyVarFinalScore =
				new FuzzyVariable( sk_fuzzyVarFinalScoreStr			//Name
								 , 0.0f								//Universe of discourse begin
								 , 100.0f							//UoD end
								 );
			//Some domains may start or end early, meaning it caps off.
			m_fuzzySetFinalScoreDisconfirming =
				new FuzzySet( sk_fuzzySetFinalScoreDisconfirmingStr	//Name
							, new GraphSlopeDecreasing()			//Graph
							, 25.0f									//Domain begin
							, 50.0f									//Domain end
							, sk_defaultNumSetSamples				//#/samples
							);
			m_fuzzySetFinalScoreAmbiguous =
				new FuzzySet( sk_fuzzySetFinalScoreAmbiguousStr		//Name
							, new GraphTriangle()					//Graph
							, 25.0f									//Domain begin
							, 75.0f									//Domain end
							, sk_defaultNumSetSamples				//#/samples
							);
			m_fuzzySetFinalScoreConfirming =
				new FuzzySet( sk_fuzzySetFinalScoreConfirmingStr	//Name
							, new GraphSlopeIncreasing()			//Graph
							, 50.0f									//Domain begin
							, 75.0f									//Domain end
							, sk_defaultNumSetSamples				//#/samples
							);
			
			m_fuzzyVarFinalScore.AddSet( m_fuzzySetFinalScoreDisconfirming );
			m_fuzzyVarFinalScore.AddSet( m_fuzzySetFinalScoreAmbiguous );
			m_fuzzyVarFinalScore.AddSet( m_fuzzySetFinalScoreConfirming );
		}
	}
	
	public FuzzyVariable GetPredicateVariable()
	{
		return m_fuzzyVarIndicative;
	}
	
	public FuzzyVariable GetConsequentVariable()
	{
		return m_fuzzyVarFinalScore;
	}
	
	public float GetAlphaThreshold()
	{
		return m_alphaThreshold;
	}
	
	public void PerformInference( float scorePctForCategory
								, FuzzySet predicateSet
								, FuzzySet consequentSet
								, FuzzySet outputSet
								)
	{
		//Ensure the output set is a sampled graph.
		GraphSampled outGraph = outputSet.GetGraphRepresentation() as GraphSampled;
		if ( null == outGraph )
		{
			Debug.LogError( "Output set '" + outputSet.GetName() + "' did not have a sampled graph type!" );
			return;
		}
		
		//Find out predicate truth first.
		float predicateTruth = predicateSet.GetTruthValue( scorePctForCategory );
		
		//MIN-MAX IMPLICATION
		float[] outputArray = outGraph.GetSamples();
		
		int numSamples = outputArray.Length;
		int maxSampleIdx = numSamples - 1;
		float fMaxSampleIdx = (float)maxSampleIdx;
		float outputDomainBegin = outputSet.GetDomainBegin();
		float outputDomainEnd = outputSet.GetDomainEnd();
		
		for ( int sampleIdx = 0; sampleIdx <= maxSampleIdx; ++sampleIdx )
		{
			//We do the FP divide instead of adding deltas b/c adding
			//will incur more error.
			float domainPct = (float)( sampleIdx ) / fMaxSampleIdx;
			float domainVal = Mathf.Lerp( outputDomainBegin, outputDomainEnd, domainPct );
			
			//For MIN-MAX, we need to:
			//* Take the MINimum of the truth values of the original set and our predicate truth
			//* Take the MAXimum of the truth values from the above and the value presently in the set.
			
			//How true is it in the consequent set?
			float consequentTruth = consequentSet.GetTruthValue( domainVal );
			float minTruth = Mathf.Min( predicateTruth, consequentTruth );
			float currTruth = outputArray[ sampleIdx ];
			float finalTruth = Mathf.Max( minTruth, currTruth );
			outputArray[ sampleIdx ] = finalTruth;
		}
	}
	
	public class DefuzzifyResults
	{
		public enum Result
		{
			Error,			//Something really bad happened.
			NoData,			//There wasn't any data.  Sum of all truth values = 0.
			Defuzzified,	//Success!
		}
		public readonly Result m_result;
		public readonly float m_finalDomainVal;
		public readonly float m_highestMembership;
		
		public DefuzzifyResults( Result result
							   , float finalDomainVal
							   , float highestMembership
							   )
		{
			m_result = result;
			m_finalDomainVal = finalDomainVal;
			m_highestMembership = highestMembership;
		}
	}
	
	public void Defuzzify( FuzzySet outputSet
						 , out DefuzzifyResults result
						 )
	{
		//Center of gravity:  
		//( sum of all (indices * corresponding truth values )
		//  (div)------------------------------------------
		//              (sum of all truth values)
		
		float topVal = 0.0f;
		float bottomVal = 0.0f;
		
		outputSet.ConvertToSamples();
		GraphRepresentation representation = outputSet.GetGraphRepresentation();
		GraphSampled sampled = representation as GraphSampled;
		if ( null == sampled )
		{
			result = new DefuzzifyResults( DefuzzifyResults.Result.Error
										 , 0.0f							//Final domain val
										 , 0.0f							//Highest membership
										);
			return;
		}
		
		float[] samples = sampled.GetSamples();
		float highestMembership = 0.0f;
		
		for ( int ctr = 0; ctr < samples.Length; ++ctr )
		{
			float truthVal = samples[ ctr ];
			highestMembership = Mathf.Max( highestMembership, truthVal );
			topVal += ( truthVal * (float)ctr );
			bottomVal += truthVal;
		}
		
		//No truth => empty set.
		if ( 0.0f == bottomVal )
		{
			result = new DefuzzifyResults( DefuzzifyResults.Result.NoData
										 , 0.0f							//Final domain val
										 , 0.0f							//Highest membership
										 );
		}
		else
		{
			float finalDomainVal = 0.0f;
			
			float fFinalSamplePos = ( topVal / bottomVal );
			float fMaxSampleIndex = (float)( samples.Length - 1 );
			float fFinalSamplePosPct = fFinalSamplePos / fMaxSampleIndex;
			float finalVal = Mathf.Lerp( outputSet.GetDomainBegin()
									   , outputSet.GetDomainEnd()
									   , fFinalSamplePosPct
									   );
			finalDomainVal = finalVal;
			
			result = new DefuzzifyResults( DefuzzifyResults.Result.Defuzzified
										 , finalDomainVal
										 , highestMembership
										 );
		}
	}
	
	public void GetFinalAssessment( ScoreReport results
								  , out DefuzzifyResults result
								  , out bool bExceededAlphaThreshold
								  )
	{
		float confirmingPct = results.GetPlayerPercentForGroup( EvidenceGroup.Confirming );
		float ambiguousPct = results.GetPlayerPercentForGroup( EvidenceGroup.Ambiguious );
		float disconfirmingPct = results.GetPlayerPercentForGroup( EvidenceGroup.Disconfirming );
		
		FuzzyVariable predicateVar = GetPredicateVariable();
		FuzzySet predicateSetAmb = predicateVar.GetSet( sk_fuzzySetIndicativeAmbiguousStr );
		FuzzySet predicateSetConf = predicateVar.GetSet( sk_fuzzySetIndicativeConfirmingStr );
		FuzzySet predicateSetDisc = predicateVar.GetSet( sk_fuzzySetIndicativeDisconfirmingStr );

		FuzzyVariable consequentVar = GetConsequentVariable();
		FuzzySet consequentSetAmb = consequentVar.GetSet( sk_fuzzySetFinalScoreAmbiguousStr );
		FuzzySet consequentSetConf = consequentVar.GetSet( sk_fuzzySetFinalScoreConfirmingStr );
		FuzzySet consequentSetDisc = consequentVar.GetSet( sk_fuzzySetFinalScoreDisconfirmingStr );
		
		FuzzySet outputSet = new FuzzyOutputSet( "OUTPUT"
											   , consequentVar
											   , 768			//#/samples
											   );
		
		PerformInference( disconfirmingPct
						, predicateSetDisc
						, consequentSetDisc
						, outputSet
						);
		PerformInference( ambiguousPct
						, predicateSetAmb
						, consequentSetAmb
						, outputSet
						);
		PerformInference( confirmingPct
						, predicateSetConf
						, consequentSetConf
						, outputSet
						);
		
		Defuzzify( outputSet
				 , out result
				 );
		
		float membershipAverage = (confirmingPct + ambiguousPct + disconfirmingPct) / 3;
		Debug.Log("Membership Average: " + membershipAverage);
		bExceededAlphaThreshold = ( membershipAverage >= m_alphaThreshold );
		
		//This is a hack/fail safe in case a user isn't doing anything with objects they don't consider important
		if(result.m_highestMembership > 0.5f)
		{
			Debug.Log("Search Failsafe triggered");
			bExceededAlphaThreshold = true;
		}
	}
	
	public float GetCategoryTruthForDomainValue( EvidenceGroup evGroup
											   , float domainVal
											   )
	{
		FuzzySet testSet = null;
		switch( evGroup )
		{
		case EvidenceGroup.Ambiguious:
			testSet = m_fuzzyVarFinalScore.GetSet( sk_fuzzySetFinalScoreAmbiguousStr );
			break;
		case EvidenceGroup.Confirming:
			testSet = m_fuzzyVarFinalScore.GetSet( sk_fuzzySetFinalScoreConfirmingStr );
			break;
		case EvidenceGroup.Disconfirming:
			testSet = m_fuzzyVarFinalScore.GetSet( sk_fuzzySetFinalScoreDisconfirmingStr );
			break;
		default:
			Debug.LogError( "Unknown/unsupported group '" + evGroup.ToString() + "' encountered!" );
			break;
		}
		
		if ( null == testSet )
		{
			return 0.0f;
		}
		
		float truthVal = testSet.GetTruthValue( domainVal );
		return truthVal;
	}
	
	public static Intensity GetIntensityForTruth( float truthVal )
	{
		//Obviously this is backwards and needs to be reversed at some point.
		//When we do fix it, it needs to be fixed in FuzzyFeedback.cs as well.
		Debug.Log("Getting Intensity for TruthVal: " + truthVal);
		
		if(truthVal == 0.0f)
			return Intensity.None;
		
		if(truthVal == 1.0f)
			return Intensity.Complete;
		
		if(truthVal <= 0.1f)
			return Intensity.AlmostNone;
		
		if(truthVal <= 0.25f)
			return Intensity.Mild;
		
		if(truthVal <= 0.5f)
			return Intensity.Somewhat;
		
		if(truthVal <= 0.75f)
			return Intensity.Significant;
		
		if(truthVal <= 0.9f)
			return Intensity.ExtremelyIntense;
		
		if(truthVal < 1.0f)
			return Intensity.Intense;
		
		return Intensity.Complete;
	}
				
	public string GetLinguisticValueForTruth( float truthVal )
	{
		if(m_linguisticCategories != null) 
		{	
			foreach( LinguisticCategory cat in m_linguisticCategories )
			{
				if ( ( truthVal >= cat.m_truthBeginVal ) &&
					 ( truthVal <= cat.m_truthEndVal ) )
				{
					return cat.m_categoryString;
				}
			}
		}
		
		return "UNABLE TO FIND CORRESPONDING TRUTH VALUE CATEGORY!";
	}
}