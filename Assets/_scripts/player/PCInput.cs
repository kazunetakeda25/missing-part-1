using UnityEngine;
using System.Collections;

//*** PC INPUT sends input to the appropriate
//*** motor attached to the PC. This will generally 
//*** only affect our PCMotor, but it can be decoupled
//*** if needed later on.
namespace HutongGames.PlayMaker
{
    public class PCInput : FsmStateAction
    {
        public FsmOwnerDefault player;
		
        private PCMotor m_motor;
        private CharacterController m_controller;

        public override void OnEnter()
        {
           	m_motor = PC.GetPC().motor;
			GameObject playerGO = Fsm.GetOwnerDefaultTarget(player);
			m_controller = playerGO.GetComponent<CharacterController>();
        }

        public override void  OnUpdate()
        {
            if (m_motor != null)
            {
				m_motor.ProcessInput(m_controller);
            }
        }
    }
}
