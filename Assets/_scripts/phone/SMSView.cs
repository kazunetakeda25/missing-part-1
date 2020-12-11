using UnityEngine;
using System.Collections;

public class SMSView : MonoBehaviour {

	public UIScrollList smsScroll;
	public GameObject PlayerTextMessageObj;
	public GameObject CharTextMessageObj;
	
	private void Start() {
		//Test();
	}
	
	public void PostSMS(SMS sms) {
		if(sms.sender == SMS.SMSCharacter.Player)
			SendText(sms, PlayerTextMessageObj);
		else
			SendText(sms, CharTextMessageObj);
		
		//Play SMS Sound
        if (AudioController.DoesInstanceExist())
        {
            AudioController.Play("SMS IN");
        }
	}
	
	private void SendText(SMS sms, GameObject prefab) {
		GameObject smsGUIGO = (GameObject) GameObject.Instantiate(prefab);
		SMSGUI smsGui = smsGUIGO.gameObject.GetComponentInChildren<SMSGUI>();
		if(sms.sender == SMS.SMSCharacter.Player) {
			//TODO Grab Player Name string from Subject Data
			smsGui.senderText.Text = "PLAYER";
		} else {
			smsGui.senderText.Text = "CHRIS";
		}
		
		smsGui.contentText.Text = sms.message;
		
		UIListItemContainer messageContainer = (UIListItemContainer) smsScroll.CreateItem(smsGUIGO);
		smsScroll.ScrollToItem(messageContainer, 1);
	}
	
	public void Test() {
		SMS message1 = new SMS();
		message1.sender = SMS.SMSCharacter.Chris;
		message1.message = "Wild Bill Hickok and those two guys that walked past you downstairs save the squarehead kid; tell Ned to stick around so they see what the kid has to say about him. ";
		
		SMS message2 = new SMS();
		message2.sender = SMS.SMSCharacter.Player;
		message2.message = "Wild Bill Hickok?";
		
		SMS message3 = new SMS();
		message3.sender = SMS.SMSCharacter.Chris;
		message3.message = "And Ned throws down... ";
		
		SMS message4 = new SMS();
		message4.sender = SMS.SMSCharacter.Player;
		message4.message = "Against Wild Bill Hickok?";
		
		SMS message5 = new SMS();
		message5.sender = SMS.SMSCharacter.Chris;
		message5.message = "Against Hickok and this other cocksucker who draws almost as fast, so it's a toss-up who blew Ned's head off. ";
		
		PostSMS(message1);
		PostSMS(message2);
		PostSMS(message3);
		PostSMS(message4);
		PostSMS(message5);
	}	
	
}
