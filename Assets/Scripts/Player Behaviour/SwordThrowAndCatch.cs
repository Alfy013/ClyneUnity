using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordThrowAndCatch : AbilityHandler.Ability
{
    [SerializeField] GameObject throwSword;
    [SerializeField] Transform throwPoint;
    [SerializeField] float catchSpeed;
    [SerializeField] AfterImage afterImage;
    [SerializeField] ParticleSystem chargeParticles;
    bool catchSword;
    internal override void AbilitySetup(){
        animator.SetBool("Charging Throw", true);
    }
	internal override void AbilityEffect(){
        throwSword.transform.SetPositionAndRotation(throwPoint.position, throwPoint.rotation);
        throwSword.SetActive(true);
    }
	internal override void AbilityReset(){
        catchSword = false;
        GetComponent<MovementHandler>().enabled = true;
        afterImage.activate = false;
        chargeParticles.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonUp(_inputName)){
            animator.SetBool("Charging Throw", false);
        }
        if(throwSword.activeInHierarchy && Input.GetButtonDown(_inputName) && throwSword.GetComponent<ThrownSword>().canCharge){
            catchSword = true;
            Debug.Log("catching");
            GetComponent<MovementHandler>().enabled = false;
            afterImage.activate = true;
            chargeParticles.Play();
        }
        if(catchSword){
            Vector3 swordPosMinusY;
            swordPosMinusY.x = throwSword.transform.position.x;
            swordPosMinusY.y = transform.position.y;
            swordPosMinusY.z = throwSword.transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, swordPosMinusY, Time.fixedDeltaTime * catchSpeed);
        }
    }
}
