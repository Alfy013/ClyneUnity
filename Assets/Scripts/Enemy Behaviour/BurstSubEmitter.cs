using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstSubEmitter : MonoBehaviour
{
	[SerializeField] GameObject projectilePrefab;
	[SerializeField] float fireRateCT;
	[SerializeField] int projectileCountCT;
	[SerializeField] int burstsFired;
	[SerializeField] bool varyBursts;
	[SerializeField] float startAngle;
	[SerializeField] float endAngle;
	[SerializeField] float delayAfterStartCT;
	[SerializeField] float delayBetweenLoopsCT;
	[SerializeField] bool loopPattern;
	bool switched = false;
	float fireRate;
	float waitAfterStart;
	int burstCount;
	float patternCooldown;

	void Shotgun()
	{
		float angleStep = (endAngle - startAngle) / projectileCountCT;
		float angle = startAngle;
		if (switched) angle += angleStep / 1.5f;

		for (int i = 0; i < projectileCountCT; i++)
		{
			GameObject bul;
			bul = ProjectilePools.ObjectPoolInstance.GetPooledObject(projectilePrefab);

			if (bul != null)
			{
				Vector3 bulDir = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
				bul.GetComponent<ProjectileHandler>().SetMoveDirection(bulDir);
				bul.transform.SetPositionAndRotation(transform.position, transform.rotation * Quaternion.AngleAxis(angle, Vector3.up));
				bul.SetActive(true);
				angle += angleStep;
			}
			else print("bul returned null");
		}
		if (varyBursts)
		{
			if (projectileCountCT % 2 == 0)
			{
				projectileCountCT++;
				switched = false;
			}
			else
			{
				projectileCountCT--;
				switched = true;
			}
		}
	}
	private void OnEnable()
	{
		fireRate = fireRateCT;
		patternCooldown = delayBetweenLoopsCT;
		waitAfterStart = delayAfterStartCT;
		burstCount = burstsFired;
	}
	void Update()
	{
		if (fireRate > 0f) fireRate -= Time.deltaTime; //reduce atkCooldown by 1 every second if positive
		if (patternCooldown > 0f) patternCooldown -= Time.deltaTime; //reduce patternCooldown by 1 every second if positive
		if (waitAfterStart > 0f) waitAfterStart -= Time.deltaTime; //reduce startDelay by 1 every second if positive, else initiate attacking sequence
		if (fireRate <= 0f && patternCooldown <= 0f && burstCount > 0 && waitAfterStart <= 0f)
		{
			fireRate = fireRateCT;
			Shotgun();
			burstCount--;
		}
		else if (burstCount <= 0 && loopPattern)
		{
			burstCount = burstsFired; //reset the burstCount, might cause issues
			patternCooldown = delayBetweenLoopsCT; //set the patternCooldown, this terminates the attack sequence
		}

	}
}
