using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ScytheAttack : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void ResetAnimationStance(){
        anim.SetInteger("CurrentAnim", 2);
        Debug.Log(2);
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            anim.SetInteger("CurrentAnim", 1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            anim.SetInteger("CurrentAnim", 2);
        }
        if(EnemyStagger.StaggerInstance.staggered && anim.GetInteger("CurrentAnim") != 2){
            ResetAnimationStance();
            Debug.Log("reset from stagger");
        }
    }

    // Update is called once per frame
}
