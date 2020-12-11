using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class LevelInitializer : MonoBehaviour
{
	public const string PC_PREFAB_PATH = "framework/PC";
	
	public PlayMakerFSM controlFsmPC;
    public PC player;
    public Transform spawnPoint;

    void Awake()
    {
		player = InitPlayer();
    }

    private PC InitPlayer()
    {
        if (player == null)
        {
            GameObject playerGO = GameObject.Instantiate(Resources.Load(PC_PREFAB_PATH), spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
            playerGO.name = "Player";
            controlFsmPC = InitFsm(playerGO, "Controller");
            controlFsmPC.Fsm.Event("init");
			Debug.Log ("Player: " + playerGO.name);
            return playerGO.GetComponent<PC>();
        }
        else
        {
            TransformTools.TransformPosRot(player.gameObject, spawnPoint.transform);
            return player;
        }
    }
	
    public PlayMakerFSM InitFsm(GameObject pGo, string pFsmName)
    {
        if (pGo != null)
        {
            foreach (PlayMakerFSM fsm in pGo.GetComponents<PlayMakerFSM>())
            {
                if (fsm.FsmName == pFsmName)
                {
                    return fsm;
                }
            }
        }

        return null;
    }
}