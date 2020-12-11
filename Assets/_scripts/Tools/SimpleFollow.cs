using UnityEngine;
using System.Collections;

public class SimpleFollow : MonoBehaviour 
{
	[SerializeField] bool trackX;
	[SerializeField] bool trackY;
	[SerializeField] bool trackZ;

	public float xOffset;
	public float yOffset;
	public float zOffset;

	public GameObject target;

	private void LateUpdate()
	{
		if(target != null) {
			Vector3 newPos = this.transform.position;
			if (trackX)
				newPos.x = target.transform.position.x + xOffset;

			if(trackY)
				newPos.y = target.transform.position.y + yOffset;

			if(trackZ)
				newPos.z = target.transform.position.z + zOffset;

			this.transform.position = newPos;
		}
	}
}
