using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayPattern : MonoBehaviour
{
	[SerializeField] Transform firePoint;
	[SerializeField] Animator animator;
    [SerializeField] GameObject projectilePrefab;
    public float atkCoolDownCT;
    [SerializeField] float startAngle;
	[SerializeField] float endAngle;
	[SerializeField] float nominator;
	[SerializeField] bool backAndForth;
	private float atkCoolDown;
	private float stunTime;
    private float angleStep = 0f;
    private float angle = 0f;
	

    // Start is called before the first frame update
    void Start()
	{
		angleStep = nominator;
        angle = startAngle + MathF.Abs(startAngle - endAngle) / nominator;
    }

	// Update is called once per frame
	void Update()
	{
        stunTime = EnemyStagger.StaggerInstance.stunDuration;

        if (stunTime > 0f) atkCoolDown = stunTime;

        if (atkCoolDown <= 0f)
        {
            atkCoolDown = atkCoolDownCT;
            Spray();
        }
        else 
			atkCoolDown -= Time.deltaTime;

    }

	private void Spray()
	{
		GameObject bul;
		bul = ProjectilePools.ObjectPoolInstance.GetPooledObject(projectilePrefab);
		if (bul != null)
		{
			if ((angle <= startAngle || angle >= endAngle) && backAndForth)
			{
				angleStep = nominator;
                angleStep *= -1;
            }
            angle += angleStep;
			Vector3 bulDir;
            if (backAndForth) 
				bulDir = Quaternion.AngleAxis(angle, Vector3.up) * firePoint.forward;
			else
                bulDir = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
            bul.GetComponent<Rigidbody>().rotation = Quaternion.Euler(bulDir);
			bul.SetActive(true);
			bul.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        }
	}
}
