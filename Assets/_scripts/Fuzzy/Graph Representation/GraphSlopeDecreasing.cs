using UnityEngine;
using System.Collections;

public class GraphSlopeDecreasing : GraphRepresentation
{
	public GraphSlopeDecreasing()
	{
	}
	
	public override float GetTruthValue( float input )
	{
		//1:1 (negated) correspondance between location in graph and truth.
		float clampVal = Mathf.Clamp01( input );
		return 1.0f - clampVal;
	}
}

