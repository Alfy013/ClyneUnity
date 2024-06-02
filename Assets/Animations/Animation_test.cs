using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_test : MonoBehaviour
{
    bool usetimer;
    float timer;
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

    // Update is called once per frame
    void Update()
    {
        if(usetimer && timer <= 0f){
            anim.SetInteger("CurrentAnim", 3);
            timer = 3f;
        }
        if(timer > 0f) timer -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            anim.SetInteger("CurrentAnim", 1);
            Debug.Log(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            anim.SetInteger("CurrentAnim", 2);
            timer = 3f;
            usetimer = !usetimer;
            Debug.Log(2);
        } 
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            anim.SetInteger("CurrentAnim", 3);
            Debug.Log(3);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            anim.SetInteger("CurrentAnim", 0);
            Debug.Log(0);
        }
    }
}
