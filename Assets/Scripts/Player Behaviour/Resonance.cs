using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Resonance : AbilityHandler.Ability
{
    [SerializeField] float _staminaCostMultiplier = 2f;
    [SerializeField] float _healMultiplier = 3f;
    [SerializeField] VisualEffect resonanceParticles;
    MovementHandler player;
    UIBarInterpolator UIBI;
    HealthHandler health;
    private void Awake(){
        resonanceParticles.Stop();
        health = GetComponent<HealthHandler>();
        UIBI = GetComponent<UIBarInterpolator>();
        player = GetComponent<MovementHandler>();
    }
    internal override void AbilitySetup()
    {
        if(UIBI.currentSlowValue01 - UIBI.currentValue01 <= 0f){
            aborted = true;
            return;
        }
        FindObjectOfType<AudioManager>().PlaySound("Resonance");
        animator.SetBool("Resonance", true);
        player.enabled = false; //this will 100% cause issues later

    }
    internal override void AbilityEffect()
    {
        resonanceParticles.Play();
        float difference = UIBI.currentSlowValue01 - UIBI.currentValue01;
        _staminaCost = difference * _staminaCostMultiplier * 100;
        health.HP += difference * UIBI._virtualMaxValue * _healMultiplier;
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
