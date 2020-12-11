using UnityEngine;
using System.Collections;

public class FuzzyOutputSet : FuzzySet
{
	bool m_isEmpty;
	
	public FuzzyOutputSet( string name
						 , FuzzyVariable outputVar
						 , int numSamples
						 )
		: base( name									//Name
			  , new GraphSampled( numSamples, 0.0f )	//Graph rep
			  , outputVar.GetDomainBegin()				//Domain begin
			  , outputVar.GetDomainEnd()				//Domain end
			  , numSamples
			  )
	{
		m_isEmpty = true;
	}
	
	public void SetIsEmpty( bool isEmpty )
	{
		m_isEmpty = isEmpty;
	}
	
	public bool IsEmpty()
	{
		return m_isEmpty;
	}
}

