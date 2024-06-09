using System;
using UnityEngine;

public class ShotgunPattern : MonoBehaviour
{
	//Bullet parameters
	[SerializeField] Transform firePoint;
	[SerializeField] Animator animator;
	[SerializeField] GameObject projectilePrefab;
	[SerializeField] float atkCoolDownCT;
	[SerializeField] float startAngle;
	[SerializeField] float endAngle;
	[SerializeField] int bulletsShot;
	[SerializeField] bool switchShots;
	private bool switched;
	float atkCoolDown;

	private void Start()
	{
		atkCoolDown = atkCoolDownCT;
	}

	private void Shotgun()
	{
		float angleStep = (endAngle - startAngle) / (bulletsShot - 1);
		float angle = startAngle;
		if (switched) angle += angleStep / 1.5f;

		for (int i = 0; i < bulletsShot; i++)
		{
			GameObject bul;
			bul = ProjectilePools.ObjectPoolInstance.GetPooledObject(projectilePrefab);

			if (bul != null)
			{
				Vector3 bulDir = Quaternion.AngleAxis(angle, Vector3.up) * firePoint.forward;
				bul.GetComponent<Rigidbody>().rotation = Quaternion.Euler(bulDir);
				bul.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
				bul.SetActive(true);
				angle += angleStep;
			}
			else print("bul returned null");
		}
		if (switchShots)
		{
			if (bulletsShot % 2 == 0)
			{
				bulletsShot++;
				switched = false;
			}
			else
			{
				bulletsShot--;
				switched = true;
			}
		}
	}

	void Update()
	{
		animator.SetFloat("CD", atkCoolDown);

		if (EnemyStagger.StaggerInstance.staggered) atkCoolDown = 999;

		if(atkCoolDown <= 0f)
		{
			atkCoolDown = atkCoolDownCT;
			Shotgun();
		}
		else
		{
			atkCoolDown -= Time.deltaTime;
		}
	}


}
