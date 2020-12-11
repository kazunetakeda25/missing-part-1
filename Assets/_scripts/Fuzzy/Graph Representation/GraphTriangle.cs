using UnityEngine;
using System.Collections;

public class GraphTriangle : GraphRepresentation
{
	public GraphTriangle()
	{
	}
	
	public override float GetTruthValue( float input )
	{
		//Clamp the input from 0..1.
		float clamped = Mathf.Clamp01( input );
		if ( clamped <= 0.5f )
		{
			//Truth rising from 0 to 1 (at peak; 0.5)
			return clamped / 0.5f;
		}
		
		//Otherwise, decreasing.
		float offset = clamped - 0.5f;
		return 1.0f - ( offset / 0.5f );
	}
}

