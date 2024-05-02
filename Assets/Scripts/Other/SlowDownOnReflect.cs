using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownOnReflect : MonoBehaviour
{
	[SerializeField] float _slowdownCooldown;
	float slowdownCooldown;
	[SerializeField] Blocking blocking;
	private void OnTriggerEnter(Collider other)
	{
		ProjectileHandler proj;
		proj = other.GetComponent<ProjectileHandler>();
		if (other.CompareTag("Projectile") && proj && slowdownCooldown <= 0f)
		{
			if(proj.parriable && !proj.stuck)
			{
				blocking.ReflectSlowDown();
				slowdownCooldown = _slowdownCooldown;
			}
		}
	}
	private void Update()
	{
		if (slowdownCooldown > 0f) slowdownCooldown -= Time.unscaledDeltaTime;
	}
}
