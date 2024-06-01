using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_test : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            anim.SetInteger("CurrentAnim", 1);
            Debug.Log(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            anim.SetInteger("CurrentAnim", 2);
            Debug.Log(2);
        } 
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            anim.SetInteger("CurrentAnim", 3);
            Debug.Log(3);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            anim.SetInteger("CurrentAnim", 0);
            Debug.Log(0);
        }
    }
}
