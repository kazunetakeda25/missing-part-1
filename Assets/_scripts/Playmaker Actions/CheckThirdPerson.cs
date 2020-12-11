using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
    public class CheckThirdPerson : FsmStateAction
    {
        public FsmEvent thirdPersonTrueEvent;
        public FsmEvent thirdPersonFalseEvent;
        public FsmEvent finishEvent;

        public FsmBool isThirdPerson;
		private LevelManager levelManager;

        public override void OnEnter()
        {
			levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
            bool test = false;
            //Settings mgr = levelManager.Settings;

            if (Settings.IsFirstPerson())
            {
                if (thirdPersonFalseEvent != null)
                {
                    Fsm.Event(thirdPersonFalseEvent);
                    test = false;
                }
            }
            else if (!Settings.IsFirstPerson())
            {
                if (thirdPersonTrueEvent != null)
                {
                    Fsm.Event(thirdPersonTrueEvent);
                    test = true;
                }
            }

            if (isThirdPerson != null)
            {
                isThirdPerson.Value = test;
            }

            if (finishEvent != null)
            {
                Fsm.Event(finishEvent);
            }
        }
    }
}