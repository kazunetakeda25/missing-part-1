using UnityEngine;
using System.Collections;

public class iTweenBuilder : MonoBehaviour {
		
	#region Rotate By
	public static void RotateBy(GameObject targetObject, Vector3 amount, float duration) {
		RotateBy(targetObject, amount, duration, iTween.LoopType.none, iTween.EaseType.linear, false, "iTween", 0, Space.Self);
	}
	
	public static void RotateBy(GameObject targetObject, Vector3 amount, float duration, Space space) {
		RotateBy(targetObject, amount, duration, iTween.LoopType.none, iTween.EaseType.linear, false, "iTween", 0, space);
	}
	
	public static void RotateBy(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType) {
		RotateBy(targetObject, amount, duration, loopType, easeType, false, "iTween", 0, Space.Self);
	}
	
	public static void RotateBy(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType, bool ignoreTimescale) {
		RotateBy(targetObject, amount, duration, loopType, easeType, ignoreTimescale, "iTween", 0, Space.Self);
	}
	
	public static void RotateBy(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType, bool ignoreTimescale, string name) {
		RotateBy(targetObject, amount, duration, loopType, easeType, ignoreTimescale, name, 0, Space.Self);
	}
	
	public static void RotateBy(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType, bool ignoreTimescale, string name, float delay, Space space) {
		
		iTween.RotateBy(targetObject, iTween.Hash("name", name,
		                                          "amount", amount,
		                                          "time", duration,
		                                          "delay", delay,
		                                          "looptype", loopType,
		                                          "easetype", easeType,
		                                          "ignoretimescale", ignoreTimescale,
												  "space", space));
	}
	
	#endregion
	
	#region RotateFrom
	public static void RotateFrom(GameObject targetObject, Vector3 amount, float duration) {
		RotateFrom(targetObject, amount, duration, iTween.LoopType.none, iTween.EaseType.linear, false, "", 0);
	}
	
	public static void RotateFrom(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType) {
		RotateFrom(targetObject, amount, duration, loopType, easeType, false, "", 0);
	}		
	
	public static void RotateFrom(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType, bool ignoreTimescale) {
		RotateFrom(targetObject, amount, duration, loopType, easeType, ignoreTimescale, "", 0);
	}	
	
	public static void RotateFrom(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType, bool ignoreTimescale, string name) {
		RotateFrom(targetObject, amount, duration, loopType, easeType, ignoreTimescale, name, 0);
	}
	
	public static void RotateFrom(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType, bool ignoreTimescale, string name, float delay) {
		
		iTween.RotateFrom(targetObject, iTween.Hash("name", name,
		                                          "rotation", amount,
		                                          "time", duration,
		                                          "delay", delay,
		                                          "looptype", loopType,
		                                          "easetype", easeType,
		                                          "ignoretimescale", ignoreTimescale));
	}
	
	#endregion
	
	#region RotateTo

	public static void RotateTo(GameObject targetObject, Vector3 amount, float duration) {
		RotateTo(targetObject, amount, duration, iTween.LoopType.none, iTween.EaseType.linear, false, "", 0f);
	}		
	
	public static void RotateTo(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType) {
		RotateTo(targetObject, amount, duration, loopType, easeType, false, "", 0f);
	}	
	
	public static void RotateTo(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType, bool ignoreTimescale) {
		RotateTo(targetObject, amount, duration, loopType, easeType, ignoreTimescale, "", 0f);
	}	
	
	public static void RotateTo(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType, bool ignoreTimescale, string name) {
		RotateTo(targetObject, amount, duration, loopType, easeType, ignoreTimescale, name, 0f);
	}
	
	public static void RotateTo(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType, bool ignoreTimescale, string name, float delay) {
		
		iTween.RotateTo(targetObject, iTween.Hash("name", name,
		                                          "rotation", amount,
		                                          "time", duration,
		                                          "delay", delay,
		                                          "looptype", loopType,
		                                          "easetype", easeType,
		                                          "ignoretimescale", ignoreTimescale));
	}
	
	#endregion
	
	#region RotateAdd
	
	public static void RotateAdd(GameObject targetObject, Vector3 amount, float duration) {
		RotateAdd(targetObject, amount, duration, iTween.LoopType.none, iTween.EaseType.linear, false, "", 0);
	}
	
	public static void RotateAdd(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType) {
		RotateAdd(targetObject, amount, duration, loopType, easeType, false, "", 0);
	}	
	
	public static void RotateAdd(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType, bool ignoreTimescale) {
		RotateAdd(targetObject, amount, duration, loopType, easeType, ignoreTimescale, "", 0);
	}
	
	public static void RotateAdd(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType, bool ignoreTimescale, string name) {
		RotateAdd(targetObject, amount, duration, loopType, easeType, ignoreTimescale, name, 0);
	}
	
	public static void RotateAdd(GameObject targetObject, Vector3 amount, float duration, iTween.LoopType loopType, iTween.EaseType easeType, bool ignoreTimescale, string name, float delay) {
		iTween.RotateAdd(targetObject, iTween.Hash("name", name,
		                                           "amount", amount,
		                                           "time", duration,
		                                           "delay", delay,
		                                           "looptype", loopType,
		                                           "easetype", easeType,
		                                           "ignoretimescale", ignoreTimescale));
	}
	
	#endregion
}
