using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class EnemyAnimatorEvents : MonoBehaviour
{
	[SerializeField] BurstSubEmitter[] BSEOrbit;
	[SerializeField] float[] delaysForBSEOrbit;
	[SerializeField] BurstSubEmitter singleSlash;
	
	void Slash(){
		singleSlash.FireBurst();
	}

	void FireBurst(){
		for(int i = 0; i < BSEOrbit.Length; i++)
			BSEOrbit[i].FireBurst(delaysForBSEOrbit[i]);
	}
	void AttackEnd(){
		GetComponent<ScytheAttack>().ResetAnimationStance();
	}
}
