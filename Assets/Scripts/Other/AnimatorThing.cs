using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorThing : MonoBehaviour
{
    [SerializeField] Dashing dashing;
    [SerializeField] Blocking blocking;
    [SerializeField] Slashing slashing;
	void PlaySlash()
	{
		slashing.PlaySlash();
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
