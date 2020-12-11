using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("UI/AAR Screen")]
public class AARScreen : MonoBehaviour
{
	
	public enum Question
	{
		GameplayFeedback1A,
		GameplayFeedback1B,
		GameplayFeedback1C,
		ConfirmationBiasE1Q1,
		ConfirmationBiasE1Q2,
		FundamentalAttributionErrorE1Q1,
		FundamentalAttributionErrorE1Q2,
		GameplayFeedback2A,
		GameplayFeedback2B,
		GameplayFeedback2C,
		GameplayFeedback3A,
		GameplayFeedback3B,
		GameplayFeedback3C,
		None,
		ConfirmationBiasE2Q1,
		ConfirmationBiasE2Q2,
		ConfirmationBiasE3Q1,
		ConfirmationBiasE3Q2,
		FundamentalAttributionErrorE2Q1,
		FundamentalAttributionErrorE2Q2,
		FundamentalAttributionErrorE3Q1,
		FundamentalAttributionErrorE3Q2,
	}
	
	private int[] m_questionAnswerIndices;
		
	public Color m_titleTextColor = Color.white;
	public Color m_bodyTextColor = new Color( 0.5f, 0.5f, 0.5f, 1.0f );
	
	public string m_finalSummaryOverallProgressStr = "Overall Game Progress:";
	public string m_finalSummaryTotalGameplayTimeStr = "Total Gameplay Time:";
	
	private int m_currPaneIndex;
	private LevelManager levelManager;
	
	void Awake()
	{
		//Create our answer indicies.
		int numCategories = System.Enum.GetValues( typeof( Question ) ).Length;
		m_questionAnswerIndices = new int[ numCategories ];
		for ( int ctr = 0; ctr < m_questionAnswerIndices.Length; ++ctr )
		{
			m_questionAnswerIndices[ ctr ] = -1;
		}
	}
	
	void Start()
	{
		levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
	}
	
}

