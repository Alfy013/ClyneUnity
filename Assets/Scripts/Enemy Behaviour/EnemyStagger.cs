using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStagger : MonoBehaviour
{
	public static EnemyStagger StaggerInstance;

	[SerializeField] TMP_Text staggerTime;
	[SerializeField] Animator animator;
	[SerializeField] private float staggerTimerCT;
	[SerializeField] private float _stunDuration;
	[SerializeField] ThrustAttack ta;
	[SerializeField] bool thrust;
	private float hitCooldown;
	[HideInInspector]
	public float staggerTimer;
	PatternHandler ph;
	private bool staggerTimerResetted;
	[HideInInspector]
	public float stunDuration = 0f;
	private bool incrementedPhase;
	private bool first = true;

	private void Awake()
	{
		ph = GetComponent<PatternHandler>();
		stunDuration = _stunDuration;
		if (StaggerInstance == null) StaggerInstance = this;
		staggerTimerResetted = true;
		staggerTimer = 0f;
		hitCooldown = 0f;
	}

	private void Update()
	{
		if (staggerTimer >= 0f && staggerTimerResetted)
		{
			staggerTimer -= Time.deltaTime;
			staggerTime.color = Color.green;
			staggerTime.text = "Time until stagger: " + Convert.ToInt64(staggerTimer);
		}
		else if(staggerTimerResetted)
		{
			Stagger();
		}

		if (stunDuration > 0f)
			stunDuration -= Time.deltaTime;
		else if (!staggerTimerResetted)
		{
			staggerTimer = staggerTimerCT;
			staggerTimerResetted = true;
			if(thrust)
				ta.enabled = true;
		}
		
		if(stunDuration <= 0.5f && !incrementedPhase){
			if(!first){
				ph.phaseNumber++;
			} else first = false;
			incrementedPhase = true;		
		}	
		
		
		if(hitCooldown > 0f)
		{
			hitCooldown -= Time.deltaTime;
			//animator.SetBool("wasHit", true);
		}
		//else animator.SetBool("wasHit", false);


	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag("FProjectile"))
		{
			hitCooldown = 0.4f;
			staggerTimer -= 5f;
		}
	}
	void Stagger()
	{
		if(!first)
			incrementedPhase = false;
		//animator.SetBool("wasHit", true);
		stunDuration = _stunDuration;
		staggerTime.text = "Strike Now!";
		staggerTime.color = Color.red;
		staggerTimerResetted = false;
		if (thrust)
			ta.enabled = false;
	}
}
