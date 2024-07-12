using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class SanguisFurantur : AbilityHandler.Ability
{
    [SerializeField] float _duration;
    [SerializeField] float healMultiplier;
    [SerializeField] UIBarInterpolator UIBIFuntur;
    [SerializeField] ParticleSystem furanturParticles;
    float duration;
    float initialStamina;
    HealthHandler hh;
    internal override void AbilitySetup()
    {
        animator.ResetTrigger("Funtur");
        animator.SetTrigger("Funtur");
    }   
    void Awake(){
        hh = GetComponent<HealthHandler>();
        UIBIFuntur._virtualMaxValue = _cooldown;
    }
    internal override void AbilityEffect()
    {
        initialStamina = abilityHandler.stamina;
        duration = _duration;
        furanturParticles.Play();
    }
    internal override void AbilityReset()
    {

    }
    void Update(){
        UIBIFuntur.value = cooldown;
        if(duration > 0)
            duration -= Time.deltaTime;
        else if(furanturParticles.isPlaying)
            furanturParticles.Stop();
    }
    public void Heal(float heal){
        if(duration > 0f)
            hh.HP += healMultiplier * heal;
    }
}
