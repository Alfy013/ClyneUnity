using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpinManager : StateMachineBehaviour
{
    [SerializeField] int phaseNumber;
    ParticleSystem sys;
    MoveToPosition mtp;
    RotateAuto ra;
    BurstSubEmitter bse;
    ParticleSystem run;
    AfterImage aim;
    GameObject scytheHitbox;
    void Awake(){
        bse = FindObjectOfType<EnemyStagger>().gameObject.GetComponent<BurstSubEmitter>();
        mtp = FindObjectOfType<MoveToPosition>();
        ra = FindObjectOfType<EnemyAnimatorEvents>().gameObject.GetComponent<RotateAuto>();
        sys = GameObject.Find("EnemyTrail").GetComponent<ParticleSystem>();
        aim = GameObject.Find("EnemyVessel").GetComponent<AfterImage>();
        run = GameObject.Find("EnemyRunParticles").GetComponent<ParticleSystem>();
        sys.Stop();
        scytheHitbox = GameObject.FindGameObjectWithTag("EnemySwordHitbox");
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
        run.Play();
        bse.FireBurst();
        mtp.CallMovement(phaseNumber);            
        ra.enabled = true;
        sys.Play();
        scytheHitbox.SetActive(true);
        aim.activate = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ra.enabled = false;
        //rtp.enabled = true;
        bse.FireBurst();
        sys.Stop();
        mtp.EndMovement();
        aim.activate = false;
        run.Stop();
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
