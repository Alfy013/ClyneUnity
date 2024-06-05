using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class EnemyAnimatorEvents : MonoBehaviour
{
	[SerializeField] ThrustAttack ta;
	[SerializeField] BurstSubEmitter bse;
	[SerializeField] Animation_test at;
	void FireBurst(){
		bse.FireBurst();
	}
	void AttackEnd(){
		at.ResetAnimationStance();
	}
	void StartThrust()
	{
		ta.StartThrust();
	}
	void StopThrust()
	{
		ta.StopThrust();
	}
	void EndThrust()
	{
		ta.EndThrust();
	}
	void HitBoxOff()
	{
		ta.HitBoxOff();
	}
	void LockOn()
	{
		ta.LockOn();
	}
}
