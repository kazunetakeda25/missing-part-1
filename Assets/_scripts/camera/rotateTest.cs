using UnityEngine;
using System.Collections;

public class rotateTest : MonoBehaviour 
{
    public Transform position;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.RotateAround(position.position, Vector3.up, 30 * Time.deltaTime);    	
	}
}
