using UnityEngine;
using System.Collections;

//Generic timer behavior 
public class Timer
{
	public delegate void ActionDelegate();
	
	private float m_timer = 0.0f;
	private bool m_active = false;
	private ActionDelegate m_Action;
		
	// Update is called once per frame
	
	public Timer( ActionDelegate act )
	{
		m_Action = act;
	}
	
	public void StartTimer( float time )
	{
		m_timer = time;
		m_active = true;
	}
	
	public void ResetTime( float time )
	{
		m_timer = time;
	}
	
	public void CancelTimer()
	{
		m_active = false;
		m_timer = 0.0f;
	}
	
	public void Update ( float dt)
	{
		if(m_active)
		{
			m_timer -= dt;
			if(m_timer <= 0.0f)
			{
				m_active = false;
				m_Action();
			}
		}
	}
	
	public bool IsActive()
	{
		return m_active;	
	}
}
