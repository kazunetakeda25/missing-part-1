using UnityEngine;
using System.Collections;

public class GraphSampled : GraphRepresentation
{
	private float[] m_samples = null;
	
	//Constructor to initialize to a fixed value.
	public GraphSampled( int numSamples, float initialValue )
	{
		m_samples = new float[ numSamples ];
		
		for( int ctr = 0; ctr < numSamples; ++ctr )
		{
			m_samples[ ctr ] = initialValue;
		}
	}
	
	//Constructor to initialize from existing graph.
	public GraphSampled( int numSamples, GraphRepresentation other )
	{
		m_samples = new float[ numSamples ];
		
		CreateFromOther( other );
	}
	
	//General purpose for converting from another graph.
	public void CreateFromOther( GraphRepresentation other )
	{
		int numSamples = m_samples.Length;
		int maxSampleIdx = numSamples -1;
		float fMaxSampleIdx = (float)maxSampleIdx;
		
		for ( int sampleIdx = 0; sampleIdx <= maxSampleIdx; ++sampleIdx )
		{
			//We do the FP divide instead of adding deltas b/c adding
			//will incur more error.
			float testPt = (float)( sampleIdx ) / fMaxSampleIdx;
			
			float truthVal = other.GetTruthValue( testPt );
			m_samples[ sampleIdx ] = truthVal;
		}
	}
	
	public int GetNumSamples()
	{
		return m_samples.Length;
	}
	
	public float[] GetSamples()
	{
		return m_samples;
	}
	
	public override float GetTruthValue( float input )
	{
		//Find out where in the range we are.
		int finalIdx = (int)( input * (float)m_samples.Length );
		finalIdx = Mathf.Clamp( finalIdx, 0, m_samples.Length - 1 );
		return m_samples[ finalIdx ];
	}
}

