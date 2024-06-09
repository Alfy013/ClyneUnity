using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternHandler : MonoBehaviour
{
	[SerializeField] public int phaseNumber = 0;
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
		[SerializeField] Transform parentPoint;
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
					bul.GetComponent<Rigidbody>().rotation = Quaternion.Euler(bulDir);
					if(bul.TryGetComponent(out CurvedProjectile curve) || parentPoint != null){
						curve.parentPoint = parentPoint;
					}
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
		[SerializeField] internal float _fireRatePerMinute;
		[SerializeField] internal float startAngle;
		[SerializeField] internal float endAngle;
		[SerializeField] internal bool random;
		[SerializeField] internal int shots;
		[SerializeField] internal float nominator = 1;
		[SerializeField] internal int _sprayCount;
		[SerializeField] internal float _timeToStart;
		[SerializeField] internal float _timeToLoop;
		[SerializeField] bool backAndForth;
		[SerializeField] internal bool loopPattern;
		internal float _timeBetweenSprays;
		internal float timeBetweenSprays;
		internal float angleStep = 0f;
		internal float currentAngle = 0f;
		internal float timeTostart;
		internal int sprayCount;
		internal float timeToLoop;
		Transform enemyTransform;

		internal void Spray()
		{
			if(!enemyTransform)
				enemyTransform = FindObjectOfType<EnemyStagger>().transform;
			GameObject bul;
			bul = ProjectilePools.ObjectPoolInstance.GetPooledObject(projectilePrefab);
			if (bul != null)
			{
				if(!random){
					if (((currentAngle <= startAngle  && angleStep < 0) || (currentAngle >= endAngle && angleStep > 0)) && backAndForth)
					{
						angleStep *= -1;
					}
					currentAngle += angleStep;
				} else currentAngle = UnityEngine.Random.Range(startAngle, endAngle);	
				
				Vector3 bulDir;	
				if (backAndForth)
					bulDir = Quaternion.AngleAxis(currentAngle, Vector3.up) * startPos.forward;
				else
					bulDir = Quaternion.AngleAxis(currentAngle, Vector3.up) * enemyTransform.forward;
				bul.GetComponent<Rigidbody>().rotation = Quaternion.Euler(bulDir);
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
		if(phaseNumber > phases.Count - 1)
			phaseNumber = 0;
	}
	void FixedUpdate(){
		if (Input.GetKeyDown(KeyCode.H)) phaseNumber++;

		foreach (BurstPattern bp in phases[phaseNumber].shotgunPatterns)
		{
			if (EnemyStagger.StaggerInstance.staggered)
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
			if (EnemyStagger.StaggerInstance.staggered)
			{
				if(!sp.random){
					if(sp.shots > 0) sp.nominator = (MathF.Abs(sp.endAngle) + MathF.Abs(sp.startAngle)) / sp.shots;
					sp.currentAngle = sp.startAngle;
					sp.angleStep = sp.nominator;
				}
				sp.timeBetweenSprays = sp._timeBetweenSprays;
				sp.timeTostart = sp._timeToStart;
				sp.sprayCount = sp._sprayCount;
				sp._timeBetweenSprays = 60f / sp._fireRatePerMinute;
			} else{
				//Decrementing the timers
				if (sp.timeTostart > 0f) sp.timeTostart -= Time.deltaTime;
				else if(sp.timeToLoop > 0f) sp.timeToLoop -= Time.deltaTime;
				else if (sp.timeBetweenSprays > 0f) sp.timeBetweenSprays -= Time.deltaTime; 

				if (sp.timeBetweenSprays <= 0f && sp.timeToLoop <= 0f && sp.sprayCount > 0 && sp.timeTostart <= 0f) //When all timers are off and there's a projectile to be fired
				{
					sp.timeBetweenSprays = sp._timeBetweenSprays; //Reset the time, fire, and decrement the projectile amount
					sp.Spray();
					sp.sprayCount--;
				}
				else if (sp.sprayCount <= 0)
				{
					if(!sp.random){
						sp.currentAngle = sp.startAngle;
						sp.angleStep = sp.nominator;
					}
					if(sp.loopPattern){
						sp.sprayCount = sp._sprayCount;
						sp.timeToLoop = sp._timeToLoop;
					}
				}
			}
		}
	}
}

