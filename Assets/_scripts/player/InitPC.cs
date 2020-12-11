using UnityEngine;
using System.Collections;

//*** INIT PC establishes all the releveant references
//*** to components on the PC and saves them to global
//*** FSM variables. This runs before any other action
//*** on the player.
namespace HutongGames.PlayMaker
{
    public class InitPC : FsmStateAction
    {
        public FsmOwnerDefault player;
        public FsmObject motor;
        public FsmObject controller;

        private PCMotor m_motor;
        private GameObject go;
    
        public override void OnEnter()
        {
            InitComponents();
        }

        private void InitComponents()
        {
            go = Fsm.GetOwnerDefaultTarget(player);
            if (go == null)
            {
                return;
            }

            if (motor != null)
            {
                m_motor = go.GetComponent<PCMotor>();
                if (m_motor == null)
                {
                    Debug.Log("GameObject " + go.name + " caused this error. Component could not be found. Be sure to initialize it in-editor. \n FSM: " + Fsm.Name + ", State: " + Fsm.ActiveStateName + " @ " + go.transform.position, go);
                }
                else
                {
                    motor.Value = m_motor;
                }
            }

            if (controller != null)
            {
                controller.Value = go.GetComponent<CharacterController>();
                if (controller == null)
                {
                    Debug.Log("GameObject " + go.name + " caused this error. Component could not be found. Be sure to initialize it in-editor. \n FSM: " + Fsm.Name + ", State: " + Fsm.ActiveStateName + " @ " + go.transform.position, go);
                }
            }
            Finish();
        }
    }
}