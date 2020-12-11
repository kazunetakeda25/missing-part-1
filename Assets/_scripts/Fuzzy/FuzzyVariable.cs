using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuzzyVariable
{
	private string m_name;
	private float m_domainBegin;
	private float m_domainEnd;
	private Dictionary< string, FuzzySet > m_fuzzySets = new Dictionary< string, FuzzySet >();

	public FuzzyVariable( string name
						, float domainBegin
						, float domainEnd 
						)
	{
		m_name = name;
		m_domainBegin = domainBegin;
		m_domainEnd = domainEnd;
		
		m_fuzzySets = new Dictionary< string, FuzzySet >();
	}
	
	public string GetName()
	{
		return m_name;
	}
	
	public void AddSet( FuzzySet fuzzySet )
	{
		//Ensure this set doesn't already exist.  Ignore case.
		string setLC = fuzzySet.GetName().ToLower();
		
		if ( m_fuzzySets.ContainsKey( setLC ) )
		{
			Debug.LogError( "Error:  Attempting to add set '" + fuzzySet.GetName() + "' to variable '" + m_name + "', which already had a set by that name!" );
			return;
		}
		
		//Ensure the new set doesn't already have a parent, because we're about to become it!
		if ( null != fuzzySet.GetParentVariable() )
		{
			Debug.LogError( "Error:  Set '" + fuzzySet.GetName() + "' was attempted to be added to variable '" + m_name + "', but the set already had another parent!" );
			return;
		}
		
		//Set the parent.
		fuzzySet.SetParentVariable( this );
		
		//Add to our dictionary.
		m_fuzzySets.Add( setLC, fuzzySet );
	}
	
	public FuzzySet GetSet( string setName )
	{
		string setLC = setName.ToLower();
		
		FuzzySet retVal;
		if ( false == m_fuzzySets.TryGetValue( setLC, out retVal ) )
		{
			return null;	//Get out.
		}
		
		return retVal;
	}
	
	public float GetDomainBegin()
	{
		return m_domainBegin;
	}
	
	public float GetDomainEnd()
	{
		return m_domainEnd;
	}
	
	public void GetDomain( out float begin, out float end )
	{
		begin = m_domainBegin;
		end = m_domainEnd;
	}
}

