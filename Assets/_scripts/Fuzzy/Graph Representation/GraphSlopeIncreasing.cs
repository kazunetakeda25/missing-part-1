using UnityEngine;
using System.Collections;

public class GraphSlopeIncreasing : GraphRepresentation
{
	public GraphSlopeIncreasing()
	{
	}
	
	public override float GetTruthValue( float input )
	{
		//1:1 correspondance between location in graph and truth.
		float clampVal = Mathf.Clamp01( input );
		return clampVal;
	}
}

