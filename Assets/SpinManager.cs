using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpinManager : StateMachineBehaviour
{
    [SerializeField] int phaseNumber;
    MoveToPosition mtp;
    RotateAuto ra;
    BurstSubEmitter bse;
    void Awake(){
        bse = FindObjectOfType<EnemyStagger>().gameObject.GetComponent<BurstSubEmitter>();
        mtp = FindObjectOfType<MoveToPosition>();
        ra = FindObjectOfType<EnemyAnimatorEvents>().gameObject.GetComponent<RotateAuto>();

    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bse.FireBurst();
        mtp.CallMovement(phaseNumber);            
        ra.enabled = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ra.enabled = false;
        //rtp.enabled = true;
        bse.FireBurst();
    }

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
