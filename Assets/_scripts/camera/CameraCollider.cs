using UnityEngine;
using System.Collections;

public class CameraCollider : MonoBehaviour 
{
    CameraMotor cameraMotor;
    PC playerController;
    Vector3 position;   

    private RaycastHit hit;

    public void LateUpdate()
    {
        if(Physics.Linecast(playerController.transform.position, transform.position, out hit))
        {

        }

    }
}
