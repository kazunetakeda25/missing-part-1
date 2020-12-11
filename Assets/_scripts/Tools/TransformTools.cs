using UnityEngine;
using System.Collections;

public class TransformTools {
	
	//backCODE: Some static tools to help us fully translate objects to a new pos/rot or scale with one line of code.
	
	public static void TransformPosRot(GameObject subject, Transform targetTransform)
	{
		subject.transform.position = CopyPosition(targetTransform);
		subject.transform.rotation = CopyRotation(targetTransform);
	}
	
	public static void TransformPosRotScale(GameObject subject, Transform targetTransform)
	{
		TransformPosRot(subject, targetTransform);
		subject.transform.localScale = targetTransform.localScale;
	}
	
	public static Vector3 CopyPosition(Transform sourceTransform) {
		return new Vector3(sourceTransform.position.x, sourceTransform.position.y, sourceTransform.position.z);
	}
	
	public static Vector3 CopyLocalPosition(Transform sourceTransform) {
		return new Vector3(sourceTransform.localPosition.x, sourceTransform.localPosition.y, sourceTransform.localPosition.z);
	}
	
	public static Quaternion CopyRotation(Transform sourceTransform) {
		return new Quaternion(sourceTransform.rotation.x, sourceTransform.rotation.y, sourceTransform.rotation.z, sourceTransform.rotation.w);
	}
	
	public static Quaternion CopyLocalRotation(Transform sourceTransform) {
		return new Quaternion(sourceTransform.localRotation.x, sourceTransform.localRotation.y, sourceTransform.localRotation.z, sourceTransform.localRotation.w);
	}
	
	public static Vector3 CopyEulers(Transform sourceTransform) {
		return new Vector3(sourceTransform.eulerAngles.x, sourceTransform.eulerAngles.y, sourceTransform.eulerAngles.z);
	}
	
	public static Vector3 CopyLocalEulers(Transform sourceTransform) {
		return new Vector3(sourceTransform.localEulerAngles.x, sourceTransform.localEulerAngles.y, sourceTransform.localEulerAngles.z);
	}
	
	public static float ClampAngle(float angle, float min, float max) {
		if (angle < -180F)
            angle += 360F;
        if (angle > 180F)
            angle -= 360F;

        return Mathf.Clamp(angle, min, max);
	}
	
}
