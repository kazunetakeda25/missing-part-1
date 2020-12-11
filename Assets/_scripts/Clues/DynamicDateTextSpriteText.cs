using UnityEngine;
using System;

[RequireComponent(typeof(SpriteText))]
public class DynamicDateTextSpriteText : MonoBehaviour {
	
	public int daysOffset = 0;
	public int monthsOffset = 0;
	public string formatSring = "MM/dd/yy";
	public bool avoidWeekends = true;
	public bool allcaps = false;
	
	private SpriteText text;
	private DateTime time;
	
	// Use this for initialization
	void Start ()
	{		
		text = gameObject.GetComponent<SpriteText>();
		time = DateTime.Now.AddDays(daysOffset).AddMonths(monthsOffset);
		
		if(avoidWeekends)
		{
			if(time.DayOfWeek == DayOfWeek.Saturday)
			{
				time = time.AddDays(-1);
			}
			if(time.DayOfWeek == DayOfWeek.Sunday)
			{
				time = time.AddDays(-2);
			}
		}
		
		if(allcaps)
		{
			text.Text = time.ToString(formatSring).ToUpper();
		}
		else
		{
			text.Text = time.ToString(formatSring);
		}
	}
}
