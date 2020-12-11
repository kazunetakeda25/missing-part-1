using UnityEngine;
using System.Collections;

public enum BiasChoice
{
	Confirming,
	Disconfirming,
	Ambiguous,
	None,
};

[AddComponentMenu("Framework/Evaluation Manager")]
public class EvaluationManager : MonoBehaviour
{
	public FuzzyReasoningCognitiveBias m_cognitiveBiasReasoning = new FuzzyReasoningCognitiveBias();
	public BiasChoice m_playerBiasChoice = BiasChoice.None;
	
	public FuzzyReasoningCognitiveBias GetCognitiveBiasReasoning()
	{
		return m_cognitiveBiasReasoning;
	}
	
	public void SetPlayerConfirmationBiasChoice(BiasChoice newChoice)
	{
		m_playerBiasChoice = newChoice;
		
		switch(newChoice)
		{
			case BiasChoice.Confirming:
			{
				Debug.Log("Player chose a Confirming response.");
				break;	
			}
			case BiasChoice.Disconfirming:
			{
				Debug.Log("Player chose a Disconfirming response.");
				break;	
			}
			case BiasChoice.Ambiguous:
			{
				Debug.Log("Player chose an Ambiguous response.");
				break;	
			}
			case BiasChoice.None:
			{
				Debug.Log("Player response cleared.");
				break;	
			}
		}
		
		return;
	}
	
	public BiasChoice GetPlayerconfirmationBiasChoice()
	{
		return m_playerBiasChoice;	
	}		
}

