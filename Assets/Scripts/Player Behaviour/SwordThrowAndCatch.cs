using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SwordThrowAndCatch : AbilityHandler.Ability
{
    [SerializeField] GameObject throwSword;
    [SerializeField] Transform throwPoint;
    [SerializeField] Transform throwTarget;
    [SerializeField] float minThrowDistance;
    [SerializeField] float maxThrowDistance;
    [SerializeField] float scrollSpeedMultiplier = 1;
    [SerializeField] float catchSpeed;
    //[SerializeField] float _timeToCatch;
    [SerializeField] AfterImage afterImage;
    [SerializeField] ParticleSystem chargeParticles;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] Transform playerTop;
    [SerializeField] Transform playerBottom;
    [SerializeField] float chargeDamage;
    [SerializeField] ScaleUP scale;
    [SerializeField] UIBarInterpolator UIBIThrow;

    Vector3 chargeStart;
    Vector3 chargeEnd;
    Vector3 swordPos;
    float throwDistance;
    //float timeToCatch;
    [HideInInspector]
    public bool catchSword;
    void Start(){
        UIBIThrow._virtualMaxValue = _cooldown;
    }
    internal override void AbilitySetup(){
        animator.SetBool("Charging Throw", true);
        scale.SetIndex(0);
    }
	internal override void AbilityEffect(){
        throwSword.transform.SetPositionAndRotation(throwPoint.position, throwPoint.rotation);
        throwSword.SetActive(true);
        scale.SetIndex(1);
    }
	internal override void AbilityReset(){
        if(catchSword){
            catchSword = false;
            GetComponent<MovementHandler>().enabled = true;
            GetComponent<HealthHandler>().enabled = true;
            afterImage.activate = false;
            chargeParticles.Stop();
            chargeEnd = transform.position;
            RaycastHit[] hits = Physics.CapsuleCastAll(playerBottom.position, playerTop.position, 1f, chargeStart - chargeEnd, Vector3.Distance(chargeEnd, chargeStart)); //painful fucking shit
            foreach(RaycastHit hit in hits){
                if(hit.collider.gameObject.CompareTag("Enemy")){
                    explosion.Play();
                    hit.collider.GetComponent<EnemyStagger>().TakeHit(500);
                }   
            }
        }


    }
    // Update is called once per frame
    void Update()
    {
        UIBIThrow.value = cooldown;
        throwDistance += Input.mouseScrollDelta.y * scrollSpeedMultiplier * Time.deltaTime;
        throwDistance = Mathf.Clamp(throwDistance, minThrowDistance, maxThrowDistance);
        throwTarget.localPosition = throwDistance * Vector3.forward;
        if(Input.GetButtonUp(_inputName)){
            animator.SetBool("Charging Throw", false);
        }
        if(throwSword.activeInHierarchy && Input.GetButtonDown(_inputName)){
            catchSword = true;
            GetComponent<MovementHandler>().enabled = false;
            GetComponent<HealthHandler>().enabled = false;
            afterImage.activate = true;
            chargeParticles.Play();
            /*swordPos.x = throwSword.transform.position.x;
            swordPos.y = transform.position.y;
            swordPos.z = throwSword.transform.position.z;
            timeToCatch = _timeToCatch;*/
            chargeStart = transform.position;
        }
        if(catchSword){
            swordPos.x = throwSword.transform.position.x;
            swordPos.y = transform.position.y;
            swordPos.z = throwSword.transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, swordPos, Time.fixedDeltaTime * catchSpeed);
            /*transform.position = Vector3.Lerp(transform.position, swordPos, 1 - timeToCatch/_timeToCatch);
            timeToCatch -= Time.fixedDeltaTime;*/
        }
    }
}
