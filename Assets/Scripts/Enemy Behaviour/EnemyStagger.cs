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
	[SerializeField] ThrustAttack ta;
	[SerializeField] bool thrust;
	public bool staggered;
	private float hitCooldown;
	[HideInInspector]
	public float staggerTimer;

	private void Awake()
	{
		if (StaggerInstance == null) StaggerInstance = this;
		staggerTimer = 0f;
		hitCooldown = 0f;
	}

	private void Update()
	{
		if(staggerTimer > 0f) staggered = false;
		else staggered = true;
		if(Input.GetKeyDown(KeyCode.Alpha1)) staggerTimer = staggerTimerCT;
		if (staggerTimer >= 0f)
		{
			staggerTimer -= Time.deltaTime;
			staggerTime.color = Color.green;
			staggerTime.text = "Time until stagger: " + Convert.ToInt64(staggerTimer);
		}
		if(staggered) Stagger();
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
			staggerTimer -= 2f;
		}
	}
	void Stagger()
	{
		//animator.SetBool("wasHit", true);
		staggerTime.text = "Strike Now!";
		staggerTime.color = Color.red;
		if (thrust)
			ta.enabled = false;
	}
}
