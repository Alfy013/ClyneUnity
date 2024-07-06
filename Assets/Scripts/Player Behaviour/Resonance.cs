using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Resonance : AbilityHandler.Ability
{
    [SerializeField] float _staminaCostMultiplier = 2f;
    [SerializeField] float _healMultiplier = 3f;
    [SerializeField] float drainMultiplier;
    [SerializeField] VisualEffect resonanceParticles;
    [SerializeField] UIBarInterpolator UIBIMend;
    
    MovementHandler player;
    UIBarInterpolator UIBIHP;
    HealthHandler health;
    float difference;
    private void Awake(){
        resonanceParticles.Stop();
        health = GetComponent<HealthHandler>();
        UIBIHP = GetComponent<UIBarInterpolator>();
        player = GetComponent<MovementHandler>();
        UIBIMend._virtualMaxValue = _cooldown;
    }
    void Update(){
        UIBIMend.value = cooldown;
    }
    internal override void AbilitySetup()
    {
        if(UIBIHP.currentSlowValue01 - UIBIHP.currentValue01 <= 0f){
            stopped = true;
            return;
        }
        difference = UIBIHP.currentSlowValue01 - UIBIHP.currentValue01;
        resonanceParticles.Play();
        FindObjectOfType<AudioManager>().PlaySound("Resonance");
        animator.SetBool("Resonance", true);
        player.enabled = false; //this will 100% cause issues later
        UIBIHP.drainRate *= drainMultiplier;

    }
    internal override void AbilityEffect()
    {
        _staminaCost = difference * _staminaCostMultiplier * 100;
        health.HP += difference * UIBIHP._virtualMaxValue * _healMultiplier;
        UIBIHP.drainRate /= drainMultiplier;
    }
    internal override void AbilityReset()
    {
        animator.SetBool("Resonance", false);
        player.enabled = true;
        resonanceParticles.Stop();
    }



    // Update is called once per frame

    /*
    if(Input.GetButtonDown("Resonance")){
            health.HP += difference * UIBI._maxValue * _healMultiplier;
            //player.StopAction();
    }*/

}
