using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorThing : MonoBehaviour
{
    [SerializeField] Blocking blocking;
    [SerializeField] Slashing slashing;
	void PlaySlash()
	{
		slashing.PlaySlash();
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
