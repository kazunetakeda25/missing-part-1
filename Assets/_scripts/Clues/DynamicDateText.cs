using UnityEngine;
using System;

[RequireComponent(typeof(TextMesh))]
public class DynamicDateText : MonoBehaviour {
	
	public int daysOffset = 0;
	public int monthsOffset = 0;
	public string formatSring = "MM/dd/yy";
	public bool avoidWeekends = true;
	
	private TextMesh text;
	private DateTime time;
	
	// Use this for initialization
	void Start ()
	{		
		text = gameObject.GetComponent<TextMesh>();
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
		
		text.text = time.ToString(formatSring);
	}
}
