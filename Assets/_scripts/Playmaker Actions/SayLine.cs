using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// For the sake of simplicity and organization. The name of the line
/// (lineName) should correspond to the Json Key and the animation name (case sensitive).
/// The audio clip name can be anything, as long as it's correctly referenced in the json file
/// and exists with that name inside the asset bundle being referenced.
/// 
/// The name of the asset bundle should be indicative of the vingnette or the context
/// of the conversation.
/// 
/// The script find the appropriate audio clip and dialogue line by looking up the key fed to the 
/// lineName variable. 
/// 
/// </summary>

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Character)]
	[Tooltip("Play audio clip and say line")]
	public class SayLine : FsmStateAction
	{
		private const float BASE_CUTOFF_TIME = 1.0f;
		
		public const string IDLE_EVENT = "Idle";
		public const string IDLE_LOOP_EVENT = "IdleLoop";
		public enum Bundle {
			ep1AudioAll,
			ep2AudioAll,
			ep3AudioAll
		}
		
		public enum Dialogue {
			Ep1Mike,
			Ep1Stephanie,
			Ep2Receptionist,
			Ep2Stephanie,
			Ep2Terry,
			Ep3Waitress,
			Ep3Stephanie,
			Ep3Terry,
			Ep3Chris,
			Ep3Cop1,
			Ep3Cop2
		}
		
		[UIHint(UIHint.FsmString)]
		[Tooltip("Name of bundle.")]
		public Bundle bundle;
		
		[UIHint(UIHint.FsmString)]
		[Tooltip("Name of line to find in bundle")]
		public Dialogue jsonFile;		
		
		[UIHint(UIHint.FsmString)]
		[Tooltip("Name of line to find in bundle")]
		public FsmString lineName;
		
		[UIHint(UIHint.FsmString)]
		[Tooltip("Character GameObject speaking the line. This is for animation purposes.")]
		public FsmGameObject character;
		
		[UIHint(UIHint.FsmString)]
		[Tooltip("Should the subtitle stay up after this line is done?")]
		public FsmBool keepSubtitleUp = true;
		
		public FsmEvent eventToFire;
		
		private FsmString path = "";
		private SayLineScript lineScript;		
		private AudioSource lineAudio;		
		private AudioClip clip;
		private string convoLine;
		private GameObject tempGo;
		public bool dontReturnToIdle = false;
		public bool noAnimWithThisLine = false;
		public float voDelay = 0;
		public float animDelay = 0;
		private Timer animDelayTimer;
		public FsmFloat dialogueCuttoff = 0.0f;
		
		private SubtitleMachine subtitleMachine;
		private bool delayedVO = false;
		
		public override void OnEnter()
		{
			//*** We create a temporary game object to hold the SayLineScript and AudioSource components.
			tempGo = new GameObject("tempGo");
			tempGo.AddComponent<SayLineScript>();
			tempGo.AddComponent<AudioSource>();
			lineAudio = tempGo.GetComponent<AudioSource>();
			lineScript = tempGo.GetComponent<SayLineScript>(); //Helper script.
			//Put the tempGO in the same spot as the speaker for positional audio.
			tempGo.transform.position = character.Value.transform.position;
			
			//*** Assign the variables from the FSM gui to the handler script.
			lineScript.episode = bundle;
			lineScript.line = lineName.ToString();
			lineScript.json = jsonFile.ToString();
			lineScript.character = character.Value;
			
			//*** Add event listener for when the handler event returns the values we asked for.
			lineScript.LineEventFinished += new SayLineScript.LineSayEventHandler(FireEvent);
			
			if(voDelay > 0)
				delayedVO = true;
			
		}

		//*** Fired off by the event manager in helper script.
		void FireEvent(AudioClip pClip, string pConvoLine)
		{
			clip = pClip;
			convoLine = pConvoLine;
			
			ReportEvent.CharacterBeginSpeaking(jsonFile.ToString(), convoLine);
			
			if(animDelay > 0) 
			{
				animDelayTimer = new Timer(StartAnimation);
				animDelayTimer.StartTimer(animDelay);
			} else 
			{
				StartAnimation();
			}

		}
		
		private void StartAnimation() 
		{
			if(!noAnimWithThisLine) 
			{
				character.Value.GetComponentInChildren<PlayMakerFSM>().SendEvent(IDLE_EVENT);
				Animation anim = character.Value.GetComponentInChildren<Animation>();
				anim.GetComponent<Animation>().CrossFade(lineName.Value, 0.1f, PlayMode.StopAll);
			}
		}
		
		//*** Replaces Update in FSM Actions.
		public override void OnUpdate ()
		{
			if(Input.GetMouseButtonUp(1) && Debug.isDebugBuild) {
				AllDone();
				this.Fsm.Event("FINISHED");
			}
			
			if(delayedVO) {
				voDelay -= Time.deltaTime;
				if(voDelay <= 0)
					delayedVO = false;
			}
			
			if( animDelayTimer != null)
				animDelayTimer.Update(Time.deltaTime);
			
			if(!delayedVO)
				AudioClipUpdate();
		}
		
		private void AudioClipUpdate() {
			if(lineAudio.clip == null)
			{
				if(clip != null)
					SetupAudioClip();
				
			} 
			else if(CheckForEndOfLine()) 
			{ 
				AllDone();
			}
		}
		
		private void SetupAudioClip() {
			lineAudio.clip = clip;
			lineAudio.Play();
			CreateSubtitleMachine();			
		}
		
		private bool CheckForEndOfLine() {
			
			if(Time.timeScale == 0)
				return false;
			
			if(lineAudio.time >= (lineAudio.clip.length - (dialogueCuttoff.Value))) {
				return true;
			}
			
			if(!lineAudio.isPlaying) {
				return true;
			}
			
			return false;
		}
		
		private void CreateSubtitleMachine() {
			DestroySubtitleMachines();
			
			//Post Subtitle text.
			GameObject subtitleMachineGO = (GameObject) GameObject.Instantiate(Resources.Load("gui/Subtitle Machine"), Vector3.zero, Quaternion.identity);
			subtitleMachine = subtitleMachineGO.GetComponent<SubtitleMachine>();
			subtitleMachine.ShowSubtitle(convoLine, clip.length);			
		}
		
		private void DestroySubtitleMachines() {
			if(GameObject.FindGameObjectsWithTag("SubtitleMachine").Length > 0) 
			{
				foreach(GameObject go in GameObject.FindGameObjectsWithTag("SubtitleMachine")) {
					GameObject.Destroy(go);
				}
			}			
		}
		
		private void AllDone() {
			if(!keepSubtitleUp.Value)
				DestroySubtitleMachines();
			
			ReportEvent.CharacterEndSpeaking(jsonFile.ToString());
			
			if(!dontReturnToIdle && !noAnimWithThisLine) 
			{
				//Have char loop idle animation for blinking
				character.Value.GetComponentInChildren<PlayMakerFSM>().SendEvent(IDLE_LOOP_EVENT);
			}
			
			GameObject.Destroy(tempGo);
			
			if(eventToFire != null)
				Fsm.Event(eventToFire);
			
			Finish();			
		}
	}
}
