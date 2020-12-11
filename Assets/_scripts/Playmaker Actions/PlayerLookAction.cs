using UnityEngine;
using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	public class PlayerLookAction : FsmStateAction
	{
		private Rect top;
		private Rect bot;
		private Rect left;
		private Rect right;
		private Rect mid;
		
		public FsmEvent topLeftEvent;
		public FsmEvent topEvent;
		public FsmEvent topRightEvent;
		public FsmEvent rightEvent;
		public FsmEvent bottomRightEvent;
		public FsmEvent bottomEvent;
		public FsmEvent bottomLeftEvent;
		public FsmEvent leftEvent;
		public FsmEvent notScrolling;

		[UIHint(UIHint.Variable)]
		public FsmVector3 direction;
		
		[UIHint(UIHint.Variable)]
		public FsmBool isScrolling;
		
		public FsmFloat margin;
		
		public FsmObject targetCamera;
		
		public override void OnEnter()
		{
			float camWidth = Camera.main.pixelWidth;
			float camHeight = Camera.main.pixelHeight;	
			
#pragma warning disable 0472
			
			if(margin.Value != null)
			{
				bot = new Rect(0, 0, camWidth, camHeight * margin.Value);
				top = new Rect(0, camHeight - camHeight * margin.Value,camWidth, camHeight * margin.Value);
				left = new Rect(0, 0, camWidth * margin.Value, camHeight);
				right = new Rect(camWidth - (camWidth * margin.Value), 0, camWidth * margin.Value, camHeight);
				mid = new Rect(camWidth * margin.Value, camHeight * margin.Value, camWidth - ((camWidth *margin.Value)*2), camHeight - ((camHeight * margin.Value)*2));
			}
		}
		
		public override void Reset()
		{
			
		}
		
		public override void OnUpdate()
		{
			if(left.Contains(Input.mousePosition) && top.Contains(Input.mousePosition))
			{	
				if(topLeftEvent != null)
				{
					Fsm.Event(topLeftEvent);					
				}
			}
			else if (right.Contains(Input.mousePosition) && top.Contains(Input.mousePosition))
			{
				if(topRightEvent != null)
				{
					Fsm.Event(topRightEvent);					
				}
			}
			else if (right.Contains(Input.mousePosition) && bot.Contains(Input.mousePosition))
			{
				if(bottomRightEvent != null)
				{
					Fsm.Event(bottomRightEvent);					
				}
			}
			else if( left.Contains(Input.mousePosition)  && bot.Contains(Input.mousePosition))
			{
				if(bottomLeftEvent != null)
				{
					Fsm.Event(bottomLeftEvent);					
				}
			}
			else if(top.Contains(Input.mousePosition))
			{
				if(topEvent != null)
				{
					Fsm.Event(topEvent);					
				}
			}
			else if(left.Contains(Input.mousePosition))
			{
				if(leftEvent != null)
				{
					Fsm.Event(leftEvent);
				}
			}
			else if(right.Contains(Input.mousePosition))
			{
				if(rightEvent != null)
				{
					Fsm.Event(rightEvent);
				}
			}
			else if(bot.Contains(Input.mousePosition))
			{
				if(bottomEvent != null)
				{
					Fsm.Event(bottomEvent);
				}
			}
			else if(mid.Contains(Input.mousePosition))
			{
				if(notScrolling != null)
					Fsm.Event(notScrolling);
			}
		}
	}
	
}