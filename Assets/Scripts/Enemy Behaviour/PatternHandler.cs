using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternHandler : MonoBehaviour
{

	private float stunTime;
	[SerializeField] private int phaseNumber = 0;
	private bool changedPhase;
	[Serializable]
	private class BurstPattern 
	{
		[SerializeField] Transform startPos;
		[SerializeField] GameObject projectilePrefab;
		[SerializeField] internal float fireRateCT;
		[SerializeField] int projectileCountCT;
		[SerializeField] internal int burstsFired;
		[SerializeField] bool varyBursts;
		[SerializeField] float startAngle;
		[SerializeField] float endAngle;
		[SerializeField] internal float timeToStartCT;
		[SerializeField] internal float timeBetweenBursts;
		[SerializeField] internal bool loopPattern;
		[SerializeField] internal bool looksAtPlayer;
		internal bool activated = true;
		internal bool started;
		private bool switched = false;
		internal int burstCount;
		internal float patternCooldown;

		internal IEnumerator Shotgun(float coolDown)
		{
			yield return new WaitForSeconds(coolDown);
			int projectileCount = projectileCountCT;
			
			float angleStep = (endAngle - startAngle) / projectileCount;
			float angle = startAngle;
			if (switched) angle += angleStep / 1.5f;
			for (int i = 0; i < projectileCount; i++)
			{
				GameObject bul;
				bul = ProjectilePools.ObjectPoolInstance.GetPooledObject(projectilePrefab);

				if (bul != null)
				{
					Vector3 bulDir = Quaternion.AngleAxis(angle, Vector3.up) * (looksAtPlayer == true ? startPos.forward : Vector3.forward);
					bul.transform.SetPositionAndRotation(startPos.position, startPos.rotation * Quaternion.AngleAxis(angle, Vector3.up));
					bul.GetComponent<ProjectileHandler>().SetMoveDirection(bulDir);
					bul.SetActive(true);
					angle += angleStep;
				}
				else print("bul returned null");
			}
			if (varyBursts)
			{
				switched = !switched;
			}
		}
		internal IEnumerator RepeatPattern(MonoBehaviour m)
		{
			yield return new WaitForSeconds(timeBetweenBursts + burstsFired * fireRateCT);
			for (int i = burstsFired - 1; i >= 0; i--)
			{
				m.StartCoroutine(Shotgun(fireRateCT * i));
			}
			if (loopPattern && activated) m.StartCoroutine(RepeatPattern(m));
		}
		internal IEnumerator WaitAfterStart(MonoBehaviour m)
		{
			yield return new WaitForSeconds(timeToStartCT);
			for (int i = burstsFired - 1; i >= 0; i--)
			{
				m.StartCoroutine(Shotgun(fireRateCT * i));
			}
			if (loopPattern && activated)
				m.StartCoroutine(RepeatPattern(m));
		}

	}
	[Serializable]
	private class SprayPattern
	{
		[SerializeField] Transform startPos;
		[SerializeField] GameObject projectilePrefab;
		[SerializeField] internal float fireRateCT;
		[SerializeField] internal float startAngle;
		[SerializeField] internal float endAngle;
		[SerializeField] internal float nominator = 1;
		[SerializeField] bool backAndForth;
		[SerializeField] internal int repetitionsUntilEndCT;
		[SerializeField] internal float delayAfterStartCT;
		[SerializeField] internal float delayBetweenLoopsCT;
		[SerializeField] internal bool loopPattern;
		internal float fireRate;
		internal float angleStep = 0f;
		internal float angle = 0f;
		internal float delayAfterStart;
		internal int burstCount;
		internal float delayUntilLoop;

		internal void Spray()
		{
			Transform enemyTransform = FindObjectOfType<EnemyStagger>().transform;
			GameObject bul;
			bul = ProjectilePools.ObjectPoolInstance.GetPooledObject(projectilePrefab);
			if (bul != null)
			{
				if ((startAngle >= angle || angle >= endAngle) && backAndForth)
				{
					angleStep *= -1;
				}
				angle += angleStep;
				Vector3 bulDir;
				if (backAndForth)
					bulDir = Quaternion.AngleAxis(angle, Vector3.up) * startPos.forward;
				else
					bulDir = Quaternion.AngleAxis(angle, Vector3.up) * enemyTransform.forward;
				bul.GetComponent<ProjectileHandler>().SetMoveDirection(bulDir);
				bul.SetActive(true);
				bul.transform.SetPositionAndRotation(startPos.position, startPos.rotation);
			}
		}
	}
	[Serializable]
	private class Phases
	{
		public List<BurstPattern> shotgunPatterns;
		public List<SprayPattern> sprayPatterns;
	}

	[SerializeField] List<Phases> phases;

	void Update()
	{
		stunTime = EnemyStagger.StaggerInstance.stunDuration;
		if (Input.GetKeyDown(KeyCode.H)) phaseNumber++;
		if(stunTime <= 0f && !changedPhase){
			if(phaseNumber < phases.Count - 1) phaseNumber++;
			else phaseNumber = 0;
			changedPhase = true;
		}
		if(stunTime > 0f && changedPhase)
			changedPhase = false;
		foreach (BurstPattern bp in phases[phaseNumber].shotgunPatterns)
		{
			if (stunTime > 0f)
			{
				bp.activated = false;
				bp.burstCount = bp.burstsFired;
				bp.started = false;
				StopAllCoroutines();
			}
			else bp.activated = true;

			if(bp.activated && !bp.started)
			{
				bp.started = true;
				StartCoroutine(bp.WaitAfterStart(this));
			}
		}
		foreach (SprayPattern sp in phases[phaseNumber].sprayPatterns)
		{
			if (stunTime > 0f)
			{
				sp.angle = sp.startAngle + MathF.Abs(sp.startAngle - sp.endAngle) / sp.nominator;
				sp.angleStep = sp.nominator;
				sp.fireRate = stunTime + sp.fireRateCT;
				sp.delayAfterStart = sp.delayAfterStartCT;
				sp.burstCount = sp.repetitionsUntilEndCT;
			}
			if (sp.fireRate > 0f) sp.fireRate -= Time.deltaTime; //reduce atkCooldown by 1 every second if positive
			if (sp.delayUntilLoop > 0f) sp.delayUntilLoop -= Time.deltaTime; //reduce patternCooldown by 1 every second if positive
			if (sp.delayAfterStart > 0f) sp.delayAfterStart -= Time.deltaTime; //reduce startDelay by 1 every second if positive, else initiate attacking sequence
			if (sp.fireRate <= 0f && sp.delayUntilLoop <= 0f && sp.burstCount > 0 && sp.delayAfterStart <= 0f)
			{
				sp.fireRate = sp.fireRateCT;
				sp.Spray();
				sp.burstCount--;
			}
			else if (sp.burstCount <= 0 && sp.loopPattern)
			{
				sp.burstCount = sp.repetitionsUntilEndCT; //reset the burstCount, might cause issues
				sp.delayUntilLoop = sp.delayBetweenLoopsCT; //set the patternCooldown, this terminates the attack sequence
				sp.angle = sp.startAngle + MathF.Abs(sp.startAngle - sp.endAngle) / sp.nominator;
				sp.angleStep = sp.nominator;
			}
		}
	}
}

