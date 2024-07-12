using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class EnemyStagger : MonoBehaviour
{
	public static EnemyStagger StaggerInstance;
	[SerializeField] int maxHP;
	[SerializeField] ParticleSystem hit;
	[SerializeField] GameObject[] rotationPoints;
	[SerializeField] UIBarInterpolator UIBP;
	[SerializeField] SanguisFurantur furantur;
	[SerializeField] GameObject[] clonepoints;
	[HideInInspector]
	public float HP;
	public bool staggered;
	Vector3[] positions = new Vector3[100];
    private void Start(){
		staggered = true;
    }

	private void Awake()
	{
		if (StaggerInstance == null) StaggerInstance = this;
		HP = 0;
		UIBP._virtualMaxValue = maxHP;
	}

	private void Update()
	{
		UIBP.value = HP;
		HP = Mathf.Clamp(HP, 0, maxHP);
		if(HP <= 0 && !staggered){
			staggered = true;
			int i = 0;
			foreach(GameObject obj in clonepoints){
				obj.transform.parent = gameObject.transform;
				obj.transform.localPosition = positions[i];
				i++;
			}
		}
        

		if(Input.GetKeyDown(KeyCode.Alpha1)){
			foreach(GameObject point in rotationPoints){
				point.transform.position = new Vector3(transform.position.x, 5.66f, transform.position.z);
			}
			int i = 0;
			foreach(GameObject obj in clonepoints){
				positions[i] = obj.transform.localPosition;
				i++;
				obj.transform.parent = null;
			}
			HP = maxHP;
			staggered = false;
		}
	}
	public void TakeHit(float damage){
		hit.Play();
		HP -= damage;
		furantur.Heal(damage);
	}
}
