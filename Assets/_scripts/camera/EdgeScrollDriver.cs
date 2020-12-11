using UnityEngine;
using System.Collections;

//+--- This class is in charge of giving input to the appropriate
//		controllers when the mouse hovers over the 4 edges of the
//		screen. It only feed data into the motors, it doesn't drive them.
public class EdgeScrollDriver : MonoBehaviour 
{
	public float boundary = 0.2f;
	public float speed = 1f;

    public PCMotor xTarget;
    public CameraMotor yTarget;
	
	public PCMotor firstPersonXTarget;
	public CameraMotor firstPersonYTarget;
	
	private int screenWidth;
	private int screenHeight;
	private float x;
	private float y;
	private float xBoundary;
	private float yBoundary;
	private float leftBoundary;
	private float rightBoundary;
	private float topBoundary;
	private float bottomBoundary;
	private float time;
	private PC _pc;
	
	private float xSpeedMultiplier = 6.0f;
	private float ySpeedMultiplier = 2.0f;
	
	private void Start () 
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		
		leftBoundary = xBoundary = screenWidth * boundary;
		bottomBoundary  = yBoundary = screenHeight * boundary;
		rightBoundary = screenWidth - xBoundary;
		topBoundary = screenHeight - yBoundary;
		_pc = PC.GetPC();
	}
	
	private void Update () 
	{
		//print(Input.mousePosition);
		x = Input.mousePosition.x;
		y = Input.mousePosition.y;
		CheckIfMouseOffscreen();
		
		if(Settings.IsFirstPerson())
			UpdateFPTarget();
		else
			UpdateThirdTarget();
		
	}
	
	private void UpdateFPTarget() {
		if(firstPersonXTarget != null)
			UpdateFirstPersonXTarget();
		
		if(firstPersonYTarget != null && firstPersonXTarget.beingDriven == false)
			UpdateFirstPersonYTarget();
	}
	
	private void CheckIfMouseOffscreen() {
		if(x < 0|| x > screenWidth || y < 0 || y> screenHeight)
		{
			x = screenWidth * 0.5f;
			y = screenHeight * 0.5f;
		}
	}
	
	private void UpdateFirstPersonXTarget() {	
		
		if(x > rightBoundary && !_pc.IsPlayerFrozen()) 
		{
			firstPersonXTarget.beingDriven = true;
			firstPersonXTarget.Horizontal = 1 * ((x - rightBoundary)/xBoundary) * xSpeedMultiplier;
		} else if(x < leftBoundary && !_pc.IsPlayerFrozen())
		{
			firstPersonXTarget.beingDriven = true;
			firstPersonXTarget.Horizontal = -1 * (1 - (x / xBoundary)) * xSpeedMultiplier;
		}
		else
		{
			firstPersonXTarget.beingDriven = false;
		}
	}
	
	private void UpdateFirstPersonYTarget() {
		if(y > topBoundary && !_pc.IsPlayerFrozen()) 
		{
			firstPersonYTarget.Vertical = (1 * ((y - topBoundary)/ yBoundary) * ySpeedMultiplier);
		} 
		else if(y < bottomBoundary && !_pc.IsPlayerFrozen()) 
		{
			firstPersonYTarget.Vertical = (-1 * (1 - (y / yBoundary)) * ySpeedMultiplier);
		} 
		else 
		{
			firstPersonYTarget.Vertical = (0);
		}		
	}
	
	private void UpdateThirdTarget() {
		if(xTarget != null)
		{
			if(x > rightBoundary && !_pc.IsPlayerFrozen())
			{
                xTarget.beingDriven = true;
				xTarget.Horizontal = 1 * ((x - rightBoundary)/xBoundary);
				yTarget.Horizontal = 1 * ((x - rightBoundary)/xBoundary);
			}
			
			else if(x < leftBoundary && !_pc.IsPlayerFrozen())
			{
                xTarget.beingDriven = true;
				xTarget.Horizontal = -1 * (1 - (x / xBoundary));
				yTarget.Horizontal = -1 * (1 - (x / xBoundary));
			}
			else
			{
                xTarget.beingDriven = false;
				xTarget.Horizontal = 0;
				yTarget.Horizontal = 0;
			}
			
		}
		if(yTarget != null)
		{
			if(y > topBoundary && !_pc.IsPlayerFrozen())
			{
				yTarget.Vertical = -1 * ((y - topBoundary)/ yBoundary);
			}
			else if(y < bottomBoundary && !_pc.IsPlayerFrozen())
			{
				yTarget.Vertical = 1 * (1 - (y / yBoundary));
			}
			else
			{
				yTarget.Vertical = 0;
			}
		}

	}
}
