using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Camera)]
	public class EnableNormalInspection : FsmStateAction 
	{
		public override void OnEnter () {
			PC.GetPC().inspector.EnableInspection();
			Finish();
		}
	}
	
	[ActionCategory(ActionCategory.Camera)]
	public class DisableNormalInspection : FsmStateAction 
	{
		public override void OnEnter () {
			PC.GetPC().inspector.DisableInspection();
			Finish();
		}
	}
	
	[ActionCategory(ActionCategory.Camera)]
	public class StaticInspectEventStart : FsmStateAction 
	{
		public Camera firstPersonCamera;
		public Camera thirdPersonCamera;
		
		private Camera originalCamera;
		
		public override void OnEnter ()
		{
			if(Settings.IsFirstPerson())
				CameraTools.MakeMainCamera(firstPersonCamera);
			else
				CameraTools.MakeMainCamera(thirdPersonCamera);
			
			//PC.GetPC().inspector.EnableInspection();
			
			Finish();
		}
		
	}	
	
	[ActionCategory(ActionCategory.Camera)]
	public class StaticInspectEventEnd : FsmStateAction 
	{
		
		public override void OnEnter ()
		{	
			//PC.GetPC().inspector.DisableInspection();
			
			Camera cam = Camera.main;
			if(cam != null) 
			{
				cam.tag = Tags.UNTAGGED;
				cam.enabled = false;
			}
			Finish();
		}
		
	}
	
}
