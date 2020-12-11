using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class AnimationManager : MonoBehaviour
{
    private Vector3 currentDirection;
    private Vector3 targetDirection;

    private NavMeshAgent agent;

    public string walkAnimName = "walk";
    public string backAnimName = "back";
    public string turnLeftAnimName = "turnLeft";
    public string turnRightAnimName = "turnRight";
    public string yesAnimName = "yes";
    public string noAnimName = "no";
    public string talkAnimName = "talk";

    public float turnAngleThreshold;
    public float walkSpeed;

    private Vector3 lastPos;

    public void FixedUpdate()
    {
        if (transform.position != lastPos)
        {
            //Debug.Log("moved");
        }
        lastPos = transform.position;
    }
}
