using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {
	
	private const string TRANSITION_LEVEL = "LevelTransitioner";
	private float delayBeforeLoad = 10.0f;
	
	public string level;
	
	private void Awake() {
		DontDestroyOnLoad(this.gameObject);
		StartCoroutine(LoadLevel());
	}
	
	public IEnumerator LoadLevel() {
		//Get FSM Attached to Level Loader Prefab
		PlayMakerFSM fsm = this.gameObject.GetComponent<PlayMakerFSM>();
		
		//Load Transition Level
		AsyncOperation async = Application.LoadLevelAsync(TRANSITION_LEVEL);
		yield return async;
		
		//Select Character
		GameObject charSelectGo = GameObject.FindGameObjectWithTag(Tags.CHARACTER_SELECTOR);
		LoadingScreenCharacterSelector charSelect = charSelectGo.GetComponent<LoadingScreenCharacterSelector>();
		
		if(!charSelect.ActivateCharacterForLevel(level))
			delayBeforeLoad = 3.0f;
		
		this.GetComponent<Camera>().enabled = false;
		//fsm.SendEvent(GlobalPlaymakerEvents.MASTER_FADE_IN);
		
		//Free Up Memory now
		Resources.UnloadUnusedAssets();
		
		//Add Delay to make transition less jarring.
		yield return new WaitForSeconds(delayBeforeLoad);
		//Now Jam in the new level.  This will freeze our animation.
		async = Application.LoadLevelAsync(level);
		yield return async;
		
		SelfDestruct();
	}
	
	private void SelfDestruct() {
		Destroy(GameObject.FindGameObjectWithTag(Tags.LOADING_EFFECT_TAG));
		Destroy(this.gameObject);
	}
	
	public static void StaticLoad(string levelToLoad) {
		GameObject levelLoaderGO = GameObject.Instantiate(Resources.Load(ResourcePaths.LEVEL_LOADER)) as GameObject;
		LevelLoader levelLoader = levelLoaderGO.GetComponent<LevelLoader>();
		levelLoader.level = levelToLoad;
	}
	
}
