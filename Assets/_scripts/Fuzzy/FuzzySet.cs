using UnityEngine;
using System.Collections;

public class FuzzySet
{
	protected string m_name;
	protected float m_domainBegin;
	protected float m_domainEnd;
	protected float m_domainDelta;	//Optimization.
	protected int m_numSamples;	//How many samples we *WOULD* use
	protected GraphRepresentation m_graphRepresentation;
	protected FuzzyVariable m_parentVariable;
	
	public FuzzySet( string name
				   , GraphRepresentation graphRepresentation
				   , float domainBegin
				   , float domainEnd
				   , int numSamples
				   )
	{
		m_name = name;
		m_graphRepresentation = graphRepresentation;
		m_numSamples = numSamples;
		SetDomain( domainBegin, domainEnd );
		m_parentVariable = null;	//Initially.
	}
	
	public FuzzyVariable GetParentVariable()
	{
		return m_parentVariable;
	}
	
	public void SetParentVariable( FuzzyVariable fuzzyVar )
	{
		m_parentVariable = fuzzyVar;
	}
	
	public string GetName()
	{
		return m_name;
	}
	
	public void SetDomain( float domainBegin, float domainEnd )
	{
		if ( m_domainEnd < m_domainBegin )
		{
			Debug.LogError( "Attempted to set a Fuzzy Set's domain end to be less than its beginning!" );
			return;
		}
		
		m_domainBegin = domainBegin;
		m_domainEnd = domainEnd;
		m_domainDelta = m_domainEnd - m_domainBegin;
	}
	
	//Creates a NEW sampled set from our representation.
	public GraphSampled CreateSampleSet()
	{
		GraphSampled sampleGraph = new GraphSampled( m_numSamples, m_graphRepresentation );
		
		return sampleGraph;
	}
	
	//Converts THIS graph to samples, getting rid of the old one.
	public void ConvertToSamples()
	{
		//Don't do this if we already ARE one.
		if ( m_graphRepresentation is GraphSampled )
		{
			return;
		}
		
		GraphSampled sampleGraph = CreateSampleSet();
		
		//This is now us!
		m_graphRepresentation = sampleGraph;
	}
	
	public float GetTruthValue( float domainValue )
	{
		//If our domain delta is 0, this means we're crisp.
		if ( 0.0f == m_domainDelta )
		{
			return ( domainValue == m_domainBegin ) ? 1.0f : 0.0f;
		}
		
		//Otherwise, do some lerping to find out where we are on the underlying
		//graph representation.
		float offsetFromBase = domainValue - m_domainBegin;
		float pct = offsetFromBase / m_domainDelta;
		float truth = m_graphRepresentation.GetTruthValue( pct );
		return truth;
	}
	
	public float GetDomainBegin()
	{
		return m_domainBegin;
	}
	
	public float GetDomainEnd()
	{
		return m_domainEnd;
	}
	
	public GraphRepresentation GetGraphRepresentation()
	{
		return m_graphRepresentation;
	}
	
	//Returns the #/samples even if not turned into a sampled graph.
	public int GetNumSamples()
	{
		return m_numSamples;
	}
}

