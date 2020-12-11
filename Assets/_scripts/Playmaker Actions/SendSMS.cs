using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
    public class SendSMS : FsmStateAction
    {	
		public SMS.SMSCharacter sender;
		public string message;
		
		public override void OnEnter ()
		{
			GameObject phoneObject = Tags.FindGameObject(Tags.SMARTPHONE_TAG);
			SmartPhone phone = phoneObject.GetComponent<SmartPhone>();
			
			SMS sms = new SMS();
			sms.sender = sender;
			sms.message = message;
			
			phone.OpenPhone(SmartPhone.Mode.SMS, sms);
			
			ReportEvent.ReportSMS(sender, sms.message);
			Finish();
		}
    }
	
	public class SendRandomSMS : FsmStateAction
	{
		public SMS.SMSCharacter sender;
		public string[] messages;
		private PC pc;
		
		public override void OnEnter ()
		{
			pc = PC.GetPC();
		}
		
		public override void OnUpdate ()
		{
			if(!pc.IsPlayerFrozen())
				SendSMS();
		}
		
		private void SendSMS() {
			GameObject phoneObject = GameObject.FindGameObjectWithTag(Tags.SMARTPHONE_TAG);
			SmartPhone phone = phoneObject.GetComponent<SmartPhone>();
			
			SMS sms = new SMS();
			sms.sender = sender;
			sms.message = messages[Random.Range(0, messages.Length)];
			
			phone.OpenPhone(SmartPhone.Mode.SMS, sms);
			ReportEvent.ReportSMS(sender, sms.message);
			Finish();
		}
	}
}