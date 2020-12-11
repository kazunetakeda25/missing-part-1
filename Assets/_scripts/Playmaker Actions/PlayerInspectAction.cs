using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	public class PlayerInspectAction : FsmStateAction
	{
		
		private InteractableWorldObject iwObject;
		private Camera m_camera;			
		public FsmFloat rayDepth = 10;

		public override void OnEnter()
		{
			m_camera = Camera.main;
		}
		
		public override void OnUpdate()
		{
			InteractableWorldObject newTarget = null;
			GameObject targetObj = OnPerformRayCast();
			if(targetObj != null)
			{
				InteractableWorldObject intWorld = targetObj.GetComponent< InteractableWorldObject >();

				while ( intWorld == null && targetObj.transform.parent != null)
				{
					targetObj = targetObj.transform.parent.gameObject;
					intWorld = targetObj.GetComponent< InteractableWorldObject >();
				}
				
				if ( intWorld != null)
                {
                    newTarget = intWorld;
					
					//backCODE: If this is a Container Object make sure it's not Opened.
					if(intWorld is InteractablePickUpRotateOpen)
					{
						InteractablePickUpRotateOpen intWorldOpenCheck = intWorld.GetComponent<InteractablePickUpRotateOpen>();
						if(intWorldOpenCheck.IsObjectOpen()) {
							return;
						}
					}
					
					OnInteractableSelected( intWorld );
                }
			}
			
			LevelManager levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
			InteractManager intMgr = levelManager.InteractManager;
			intMgr.SetTarget(newTarget, 0);
		}		
		
		public override void OnExit()
		{
			
		}
		
		public GameObject OnPerformRayCast()
		{
			GameObject retVal = null;
			bool bHUDIconsHighlighted = false;
            m_camera = Camera.main;
			return retVal;
		}
		
		public void OnInteractableSelected( InteractableWorldObject obj )
		{
			//Turn the cursor to an interaction one.
//			MouseCursorManager mouseCursorMgr = App.Instance().GetScreenManager().GetMouseCursorManager();
//			mouseCursorMgr.SetMode( MouseCursorManager.Mode.Navigation );
//			mouseCursorMgr.SetNavCursor( MouseCursorManager.NavCursor.Interact );
		}
	}
}