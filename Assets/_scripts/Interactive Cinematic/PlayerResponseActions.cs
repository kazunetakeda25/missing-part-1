using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
	[ActionCategory(ActionCategory.Movement)]
		[HutongGames.PlayMaker.Tooltip("Conversations: Set up the player responses.")]
	
	public class PlayerResponseAction : FsmStateAction {
		
		public PlayerResponses.ResponseID responseID;
		public FsmBool UseLowerGridSpots;
		public FsmBool multiSelect;
		public int multiSelectMaximum;
		private FsmEvent[] events;
		
		public override void OnEnter() {
			GameObject.Destroy(GameObject.FindGameObjectWithTag(Tags.PLAYER_RESPONSE_GRID));
			
			PlayerResponseStore.PlayerResponse playerResponses = PlayerResponses.FetchPlayerResponses(responseID);
			
			if(playerResponses.responses.Length != playerResponses.events.Length) {
				Debug.LogError("Player Reponse: " + responseID + " is not setup correctly!  The number of responses does not line up with the number of Events.");
			}
			
			//Spawn in Grid and populate it.
			GameObject responseGridGO = (GameObject) GameObject.Instantiate(Resources.Load(ResourcePaths.PLAYER_RESPONSE_GRID));
			responseGridGO.GetComponent<PlayerResponseGrid>().SetupResponseGrid(playerResponses, UseLowerGridSpots.Value, multiSelect.Value, multiSelectMaximum);
			Finish();
		}
		
	}
	
	[ActionCategory(ActionCategory.Movement)]
		[HutongGames.PlayMaker.Tooltip("Conversations: Set up the player responses.")]
	
	public class PhoneResponseAction : FsmStateAction {
		
		public const string ResponseGridPath = ResourcePaths.PHONE_RESPONSE_GRID;
		
		public PlayerResponses.ResponseID responseID;
		private FsmEvent[] events;
		
		public override void OnEnter() {
			//Destroy any old Player Responses
			GameObject.Destroy(GameObject.FindGameObjectWithTag(Tags.PLAYER_RESPONSE_GRID));
			
			PlayerResponseStore.PlayerResponse playerResponses = PlayerResponses.FetchPlayerResponses(responseID);
			
			if(CheckResponsesForErrors(playerResponses)) return;
			
			//Spawn in Grid and populate it.
			GameObject responseGridGO = (GameObject) GameObject.Instantiate(Resources.Load(ResponseGridPath));
			responseGridGO.GetComponent<PhoneResponseGrid>().SetupReponseGrid(playerResponses);
			Finish();
		}
		
		private bool CheckResponsesForErrors(PlayerResponseStore.PlayerResponse playerResponses) {
			if(playerResponses.responses.Length != playerResponses.events.Length) {
				Debug.LogError("Player Reponse: " + responseID + " is not setup correctly!  The number of responses does not line up with the number of Events.");
				Finish();
				return true;
			}
			
			if(playerResponses.responses.Length > PhoneResponseGrid.MAX_RESPONSES) {
				Debug.LogError("Too many Player reponses for the phone reponse grid!");
				Finish();
				return true;
			}
			
			return false;
		}
		
	}	
	
}
