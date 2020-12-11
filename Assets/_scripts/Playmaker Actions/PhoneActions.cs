using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.GUI)]
	public class ForcePhoneClose : FsmStateAction
	{
		
		public override void OnEnter ()
		{
			GameObject phoneObject = Tags.FindGameObject(Tags.SMARTPHONE_TAG);
			SmartPhone phone = phoneObject.GetComponent<SmartPhone>();
			phone.HidePhone();
			Finish();
		}
		
	}
	
	[ActionCategory(ActionCategory.GUI)]
	public class LockPhoneControls : FsmStateAction
	{
		
		public override void OnEnter ()
		{
			GameObject phoneObject = Tags.FindGameObject(Tags.SMARTPHONE_TAG);
			SmartPhone phone = phoneObject.GetComponent<SmartPhone>();
			phone.DisableAllButtons();
			Finish();
		}
		
	}
	
	[ActionCategory(ActionCategory.GUI)]
	public class UnlockPhoneControls : FsmStateAction
	{

		public override void OnEnter ()
		{
			GameObject phoneObject = Tags.FindGameObject(Tags.SMARTPHONE_TAG);
			SmartPhone phone = phoneObject.GetComponent<SmartPhone>();
			
			phone.EnableAllButtons();
			Finish();
		}
		
	}
	
	[ActionCategory(ActionCategory.GUI)]
	public class ListenForPhoneClose : FsmStateAction
	{
		public FsmEvent finishEvent;
		
		public override void OnEnter ()
		{
			GameObject phoneObject = Tags.FindGameObject(Tags.SMARTPHONE_TAG);
			SmartPhone phone = phoneObject.GetComponent<SmartPhone>();
			
			phone.SetPhoneClosedDelegate(PhoneClosed);
		}
		
		private void PhoneClosed() {
			Fsm.Event(finishEvent);
			Finish();
		}
		
	}
	
	[ActionCategory(ActionCategory.GUI)]
	public class ListenForSMSOpen : FsmStateAction
	{
		public FsmEvent finishEvent;
		
		public override void OnEnter ()
		{
			GameObject phoneObject = Tags.FindGameObject(Tags.SMARTPHONE_TAG);
			SmartPhone phone = phoneObject.GetComponent<SmartPhone>();
			
			phone.SetSMSOpenedDelegate(SMSOpened);
		}
		
		private void SMSOpened() {
			Fsm.Event(finishEvent);
			Finish();
		}
	}
	
	[ActionCategory(ActionCategory.GUI)]
	public class ListenForAlbumOpen : FsmStateAction
	{
		public FsmEvent finishEvent;
		
		public override void OnEnter ()
		{
			GameObject phoneObject = Tags.FindGameObject(Tags.SMARTPHONE_TAG);
			SmartPhone phone = phoneObject.GetComponent<SmartPhone>();
			
			phone.SetAlbumOpenedDelegate(AlbumOpened);
		}
		
		private void AlbumOpened() {
			Fsm.Event(finishEvent);
			Finish();
		}
	}	
	
	[ActionCategory(ActionCategory.GUI)]
	public class BringInPhotoAlbum : FsmStateAction
	{
		
		public override void OnEnter ()
		{
			GameObject phoneObject = Tags.FindGameObject(Tags.SMARTPHONE_TAG);
			SmartPhone phone = phoneObject.GetComponent<SmartPhone>();
			phone.OpenPhone(SmartPhone.Mode.PhotoAlbum);
			Finish();
		}
	}	

	[ActionCategory(ActionCategory.GUI)]
	public class BringInSMS : FsmStateAction
	{
		
		public override void OnEnter ()
		{
			GameObject phoneObject = Tags.FindGameObject(Tags.SMARTPHONE_TAG);
			SmartPhone phone = phoneObject.GetComponent<SmartPhone>();
			phone.OpenPhone(SmartPhone.Mode.SMS);
			Finish();
		}
	}		
	
}
