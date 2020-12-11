using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Camera)]
	[Tooltip("switches between two cameras")]
	public class CameraSwitch : FsmStateAction
	{
		public Camera Camera1;
		public Camera Camera2;
		public override void Reset ()
		{
			if(!Camera1)
			{
				Camera1 = Camera.main;
			}
			else if(!Camera2)
			{
				Camera2 = Camera.main;
			}
		}
		public override void OnEnter ()
		{
			if(!Camera1)
			{
				Camera1 = Camera.main;
			}
			else if(!Camera2)
			{
				Camera2 = Camera.main;
			}
			Camera1.enabled = false;
			Camera2.enabled = true;
		}
	}
}