using UnityEngine;
using System.Collections;

public abstract class GraphRepresentation
{
	//Returns the truth value for a given input on the graph.
	public abstract float GetTruthValue( float input );
}

