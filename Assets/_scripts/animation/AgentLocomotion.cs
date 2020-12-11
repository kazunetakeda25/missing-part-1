using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]


public class AgentLocomotion : MonoBehaviour {
	
	public string walkAnimName = "Walk";
	public string idleAnimName = "Idle";
	
	public float walkAnimationSpeed = 1.5f;
	public float runAnimationSpeed = 5.0f;
	public float speedThreshold = 0.1f;
	public float closeEnoughThreshold = 0.2f;
	
	private enum LocomotionState {
		Standing,
		Moving
	}
	
	private NavMeshAgent agent;
	private Animation anim;
	private LocomotionState locomotionState;
	
	private Vector3 linkStart;
	private Vector3 linkEnd;
	private Quaternion linkRotation;
	
	private void Start() {
		agent = GetComponent<NavMeshAgent>();
		agent.autoTraverseOffMeshLink = false;
		locomotionState = LocomotionState.Standing;
		AnimationSetup();
	}
	
	private void AnimationSetup() {
		anim = GetComponentInChildren<Animation>();
		anim[walkAnimName].layer = 1;
		anim.SyncLayer(1);
		
		anim.CrossFade(idleAnimName, 0.1f, PlayMode.StopAll);
	}
	
	private void Update() {
		CheckStateChange();
		UpdateAnimationBlend();
	}	
	
	private void CheckStateChange() {
		switch(locomotionState) {
		case LocomotionState.Moving:
			if(agent.remainingDistance <= closeEnoughThreshold)
				locomotionState = LocomotionState.Standing;
			break;
		case LocomotionState.Standing:
			if(agent.remainingDistance > closeEnoughThreshold)
				locomotionState = LocomotionState.Moving;			
			break;
		}		
	}
	
	private void UpdateAnimationBlend() {
		Vector3 velocityXZ = new Vector3(agent.velocity.x, 0f, agent.velocity.z);
		float speed = velocityXZ.magnitude;
		
		anim[walkAnimName].speed = speed / walkAnimationSpeed;
		
		if(speed > speedThreshold)
			anim.CrossFade(walkAnimName);
		else
			anim.CrossFade(idleAnimName, 0.1f, PlayMode.StopAll);
	}
}

//#pragma strict
//@script RequireComponent(NavMeshAgent)
//
//private var locoState_ : String = "Locomotion_Stand";
//private var agent_ : NavMeshAgent;
//private var anim_ : Animation;
//private var linkStart_ : Vector3;
//private var linkEnd_ : Vector3;
//private var linkRot_ : Quaternion;
//
//function Start() {
//	agent_ = GetComponent.<NavMeshAgent>();
//	agent_.autoTraverseOffMeshLink = false;
//	AnimationSetup();
//
//	while(Application.isPlaying) {
//		yield StartCoroutine(locoState_);
//	}
//}
//
//function Locomotion_Stand() {
//	do {
//		UpdateAnimationBlend();
//		yield;
//	} while(agent_.remainingDistance == 0);
//
//	locoState_ = "Locomotion_Move";
//	return;
//}
//
//function Locomotion_Move() {
//	do {
//		UpdateAnimationBlend();
//		yield;
//
//		if(agent_.isOnOffMeshLink) {
//			locoState_ = SelectLinkAnimation();
//			return;
//		}
//	} while(agent_.remainingDistance != 0);
//
//	locoState_ = "Locomotion_Stand";
//	return;
//}
//
//function Locomotion_JumpAnimation() {
//	var linkAnimationName : String = "RunJump";
//
//	agent_.Stop(true);
//	anim_.CrossFade(linkAnimationName, 0.1, PlayMode.StopAll);
//	transform.rotation = linkRot_;
//	var posStartAnim : Vector3 = transform.position;
//	do {
//		var tlerp : float = anim_[linkAnimationName].normalizedTime;
//		var newPos : Vector3 = Vector3.Lerp(posStartAnim, linkEnd_, tlerp);
//		newPos.y += 0.4*Mathf.Sin(3.14159*tlerp);
//		transform.position = newPos;
//
//		yield;
//	} while(anim_[linkAnimationName].normalizedTime < 1);
//
//	anim_.Play("Idle");
//	agent_.CompleteOffMeshLink();
//	agent_.Resume();
//	transform.position = linkEnd_;
//	locoState_ = "Locomotion_Stand";
//	return;
//}
//
//function Locomotion_LadderAnimation() {
//	var linkCenter : Vector3 = 0.5*(linkEnd_ + linkStart_);
//	var linkAnimationName : String;
//	if(transform.position.y > linkCenter.y) {
//		linkAnimationName = "Ladder Down";
//	} else {
//		linkAnimationName = "Ladder Up";
//	}
//
// 	agent_.Stop(true);
//
//	var startRot : Quaternion = transform.rotation;
//	var startPos : Vector3 = transform.position;
//	var blendTime : float = 0.2;
//	var tblend : float = 0;
//	do {
//		transform.position = Vector3.Lerp(startPos, linkStart_, tblend/blendTime);
//		transform.rotation = Quaternion.Slerp(startRot, linkRot_, tblend/blendTime);
//		yield;
//		tblend += Time.deltaTime;
//	} while(tblend < blendTime);
//	transform.position = linkStart_;
//
//	anim_.CrossFade(linkAnimationName, 0.1, PlayMode.StopAll);
//	agent_.ActivateCurrentOffMeshLink(false);
//	do {
//		yield;
//	} while(anim_[linkAnimationName].normalizedTime < 1);
//	agent_.ActivateCurrentOffMeshLink(true);
//
//	anim_.Play("Idle");
//	transform.position = linkEnd_;
//	agent_.CompleteOffMeshLink();
//	agent_.Resume();
//
//	locoState_ = "Locomotion_Stand";
//	return;
//}
//
//private function SelectLinkAnimation() : String {
//	var link : OffMeshLinkData = agent_.currentOffMeshLinkData;
//	var distS : float = (transform.position - link.startPos).magnitude;
//	var distE : float = (transform.position - link.endPos).magnitude;
//	if(distS < distE) {
//		linkStart_ = link.startPos;
//		linkEnd_ = link.endPos;
//	} else {
//		linkStart_ = link.endPos;
//		linkEnd_ = link.startPos;
//	}
//
//	var alignDir : Vector3 = linkEnd_ - linkStart_;
//	alignDir.y = 0;
//	linkRot_ = Quaternion.LookRotation(alignDir);
//
//	if(link.linkType == OffMeshLinkType.LinkTypeManual) {
//		return "Locomotion_LadderAnimation";
//	} else {
//		return "Locomotion_JumpAnimation";
//	}
//}
//

