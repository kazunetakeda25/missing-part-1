using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace CTIActions.Actions {
	
    [ActionCategory(ActionCategory.ScriptControl)]
		[HutongGames.PlayMaker.Tooltip("Force Unfreeze PC")]

    public class ForceUnfreeze : FsmStateAction
    {

        public override void OnEnter()
        {
            PC.GetPC().ForceUnfreeze();
            Finish();
        }

    }
	
}
