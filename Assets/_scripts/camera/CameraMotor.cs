using UnityEngine;
using System.Collections;

public abstract class CameraMotor : MonoBehaviour
{
	
	[HideInInspector]
	protected float vertical;
	public float Vertical { get { return vertical; } set { vertical = value; } }
	protected float horizontal;
	public float Horizontal { get { return horizontal; } set { horizontal = value; } }
	protected float alternateVerticalInput;
    private float alternateHorizontalInput;

    public Transform lookTarget;
    public EdgeScrollDriver driver;
    public abstract void Drive();
	public Texture2D cursor;
	
	protected bool frozen;
	protected int freezeStack = 0;

	public void Freeze() {
		freezeStack ++;
		CheckFreeze();
	}
	
	public void UnFreeze() {
		freezeStack --;
		CheckFreeze();
	}

    public void ForceUnfreeze()
    {
        freezeStack = 0;
        CheckFreeze();
    }
	
	protected virtual void CheckFreeze() {
		if(freezeStack > 0)
			frozen = true;

        if (freezeStack <= 0)
        {
            frozen = false;
        }
	}

	private bool locked = false;
	
	private void Update() {
//		if (frozen == true && locked == true) {
//			Debug.Log ("Frozen");
//			locked = false;
//			Screen.lockCursor = false;
//			Screen.showCursor = true;
//		} else if(frozen == false && locked == false) {
//			if (cursor != null) 
//				Cursor.SetCursor (cursor, Vector2.zero, CursorMode.ForceSoftware);
//				Screen.showCursor = false;
//				Screen.lockCursor = true;
//			}
//		}
//

//		if (this is FirstPersonCameraMotor) {
//			if (frozen && Screen.showCursor == false) {
//				Screen.showCursor = true;
//			} else if (!frozen && Screen.showCursor == true) {
//				Screen.showCursor = false;
//			}
//		}
//
//		if (frozen && Screen.lockCursor == true) {
//			Screen.lockCursor = false;
//		} else if(!frozen && Screen.lockCursor == false) {
//			Screen.lockCursor = true;
//		}

		if(!frozen)
			Drive();
	}
	
}