using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorThing : MonoBehaviour
{
    [SerializeField] Blocking blocking;
    [SerializeField] Slashing slashing;
    [SerializeField] HealthHandler health;
    void Unknocked(){
        health.Unknocked();
    }
	void PlaySlash()
	{
		slashing.PlaySlash();
	}
    void FireSlash(){
        
    }
    void PlayStab(){
        slashing.PlayStab();
    }
	void StartBlock()
    {
        blocking.StartBlock();
    }
    void EndBlock()
    {
        blocking.StopBlock();
    }
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
