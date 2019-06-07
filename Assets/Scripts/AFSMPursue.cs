using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFSMPursue : StateMachineBehaviour {
    public float stopDistance = 8f;
    private Wander _wander;
    private Seek _pursue;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        AgentBehaviour[] behaviours;
        GameObject gameObject = animator.gameObject;
        behaviours = gameObject.GetComponents<AgentBehaviour>();
        foreach (AgentBehaviour b in behaviours) {
            b.enabled = false;
        }

        _wander = gameObject.GetComponent<Wander>();
        _pursue = gameObject.GetComponent<Seek>();
        if (_wander == null || _pursue == null) {
            return;
        }
        _pursue.enabled = true;
        animator.gameObject.name = "Seeking";
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Vector3 targetPos, agentPos;
        targetPos = _pursue.target.transform.position;
        agentPos = animator.transform.position;
        if (Vector3.Distance(targetPos, agentPos) < stopDistance) {
            return;
        }
        animator.SetTrigger("Wander");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
