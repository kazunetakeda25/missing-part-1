using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.GUI)]
		[HutongGames.PlayMaker.Tooltip("Hide EZ GUI Button.")]	
	
	public class HideEZGUIButton : FsmStateAction {
		public UIButton[] buttonsToHide;
		public float fadeDuration;
		
		public override	void OnEnter() 
		{
			
			foreach(UIButton buttonToHide in buttonsToHide) 
			{
				buttonToHide.controlIsEnabled = false;
				FadeSprite.Do(
					buttonToHide, 
					EZAnimation.ANIM_MODE.FromTo, 
					ColorTools.SetColorAlpha(buttonToHide.Color, 0),
					EZAnimation.linear,
					fadeDuration,
					0,
					null,
					CleanUp);
			}
		}
		
		private void CleanUp(EZAnimation ez)
		{
			foreach(UIButton buttonToHide in buttonsToHide) 
			{
				buttonToHide.Hide(true);
			}
			
			Finish();
		}
		
	}
	
	[ActionCategory(ActionCategory.GUI)]
		[HutongGames.PlayMaker.Tooltip("Show EZ GUI Button.")]	
	
	public class ShowEZGUIButton : FsmStateAction {
		public UIButton[] buttonsToShow;
		public float fadeDuration;
		
		public override	void OnEnter() {
			
			foreach(UIButton buttonToShow in buttonsToShow) {
				buttonToShow.Hide(false);
				
				FadeSprite.Do(
					buttonToShow, 
					EZAnimation.ANIM_MODE.FromTo, 
					ColorTools.SetColorAlpha(buttonToShow.Color, 1),
					EZAnimation.linear,
					fadeDuration,
					0,
					null,
					CleanUp);
			}
			
		}
		
		private void CleanUp(EZAnimation ez) {
			foreach(UIButton buttonToShow in buttonsToShow) {
				buttonToShow.controlIsEnabled = true;
			}
			
			Finish();
		}
		
	}	
	
	[ActionCategory(ActionCategory.GUI)]
		[HutongGames.PlayMaker.Tooltip("Bring in EZ GUI Panel.")]	
	
	public class BringInEZGUIPanel : FsmStateAction {
		public UIPanelManager panelManager;
		public string panelName;
		public bool instant;
		
		public override	void OnEnter() {
			
			if(instant) {
				panelManager.BringInImmediate(panelName);
			} else {
				panelManager.BringIn(panelName);
			}
			Finish();
		}
	}
		
	
	[ActionCategory(ActionCategory.GUI)]
		[HutongGames.PlayMaker.Tooltip("Dismiss Currnetly Showing UIPanel")]	
	
	public class DismissCurrentEZGUIPanel : FsmStateAction {
		public UIPanelManager panelManager;
		public bool instant;
		
		public override	void OnEnter() {
			
			if(instant) {
				panelManager.DismissImmediate();
			} else {
				panelManager.Dismiss();
			}
			
			Finish();
		}
		
	}
	
	[ActionCategory(ActionCategory.GUI)]
		[HutongGames.PlayMaker.Tooltip("Dismiss a specific Panel")]	
	
	public class DismissEZGUIPanel : FsmStateAction {
		public UIPanel panelToDismiss;
		
		public override	void OnEnter() {
			
			panelToDismiss.Dismiss();
			Finish();
		}
		
	}
	
}
