using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorEvents : MonoBehaviour
{
	[SerializeField] ThrustAttack ta;

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
