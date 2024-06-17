using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimatorThing : MonoBehaviour
{
    [SerializeField] Blocking blocking;
    [SerializeField] Slashing slashing;
    [SerializeField] HealthHandler health;
    [SerializeField] AbilityHandler ah;
    void FireAbility(){
        ah.FireAbility();
    }
    void ResetAbility(){
        ah.ResetAbility();
    }
    void Unknocked(){
        health.Unknocked();
    }/*
	void StartBlock()
    {
        blocking.StartBlock();
    }
    void EndBlock()
    {
        blocking.StopBlock();
    }*/
    /*void DashStart()
    {
        dashing.StartDash();
    }
    void DashStop()
    {
        dashing.StopDash();
    }
    
    void EndSlash()
    {
        slashing.EndSlash();
    }*/
}
