using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Sends an Event when the specified Mouse Button is pressed. Optionally store the button state in a bool variable.")]
	public class GetMouseButtonDownDebug : FsmStateAction
	{
		[RequiredField]
		public MouseButton button;
		public FsmEvent sendEvent;
		[UIHint(UIHint.Variable)]
		public FsmBool storeResult;
		
		
		public override void Reset()
		{
			button = MouseButton.Left;
			sendEvent = null;
			storeResult = null;
		}

		public override void OnUpdate()
		{
			bool buttonDown = Input.GetMouseButtonDown((int)button);
			
			if (buttonDown && Debug.isDebugBuild)
				Fsm.Event(sendEvent);
			
			
			storeResult.Value = buttonDown;
		}
	}
}
