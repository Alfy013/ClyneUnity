using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ScytheAttack : MonoBehaviour
{
    [SerializeField] float _timeToStartSpinning;
    [SerializeField] float spinSpeed;
    [SerializeField] BurstSubEmitter[] bse;
    [SerializeField] float _timeBetweenBursts;
    Quaternion startRotation;
    bool attackInitiated;
    int yellow;
    GameObject enemy;
    float timeToStartSpinning;
    float timer;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enemy = FindObjectOfType<EnemyStagger>().gameObject;
    }
    public void ResetAnimationStance(){
        anim.SetInteger("CurrentAnim", 2);
        Debug.Log(2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            anim.SetInteger("CurrentAnim", 1);
            timeToStartSpinning = _timeToStartSpinning;
            attackInitiated = true;
            startRotation = transform.rotation;
        }
        if(attackInitiated){
            if(timeToStartSpinning > 0f) timeToStartSpinning -= Time.deltaTime;
            else if(timeToStartSpinning > -100f){
                enemy.GetComponent<RotateToPlayer>().enabled = false;
                anim.SetInteger("CurrentAnim", 3);
                transform.Rotate(Vector3.up * spinSpeed);
                timer -= Time.deltaTime;
            }
            if(timer < 0){
                if(yellow == 1) yellow = 0;
                else yellow = 1;
                bse[yellow].FireBurst();
                timer = _timeBetweenBursts;
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            transform.rotation = startRotation;
            enemy.GetComponent<RotateToPlayer>().enabled = true;
            anim.SetInteger("CurrentAnim", 2);
            timeToStartSpinning = -100f;
            timer = 0f;
            attackInitiated = false;
        }
        
    }
}
