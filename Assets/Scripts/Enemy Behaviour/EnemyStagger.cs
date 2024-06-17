using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStagger : MonoBehaviour
{
	public static EnemyStagger StaggerInstance;
	[SerializeField] int maxHP;
	[SerializeField] ParticleSystem hit;
	[SerializeField] GameObject[] rotationPoints;
	[SerializeField] UIBarInterpolator UIBP;
	[HideInInspector]
	public float HP;
	public bool staggered;
	private float hitCooldown;

    private void Start(){
		staggered = true;
    }

	private void Awake()
	{
		if (StaggerInstance == null) StaggerInstance = this;
		hitCooldown = 0f;
		HP = 0;
		UIBP._virtualMaxValue = maxHP;
	}

	private void Update()
	{
		UIBP.value = HP;
		HP = Mathf.Clamp(HP, 0, maxHP);
		if(HP <= 0) staggered = true;
        

		if(Input.GetKeyDown(KeyCode.Alpha1)){
			foreach(GameObject point in rotationPoints){
				point.transform.position = new Vector3(transform.position.x, 5.66f, transform.position.z);
			}
			HP = maxHP;
			staggered = false;
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
			hit.Play();
		}
	}
}
