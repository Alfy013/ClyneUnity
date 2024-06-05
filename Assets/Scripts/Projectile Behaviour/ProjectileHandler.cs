using System;
using Unity.Mathematics;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
	protected Rigidbody rb;
	[SerializeField] AnimationCurve velocityCurve;
	public float lifeTimeCT;
	[SerializeField] private int damageAmount;
	[SerializeField] public bool parriable;
	[SerializeField] private bool destroyedOnParry = false;
	[SerializeField] private bool isBeam;
	[SerializeField] private bool sticks;
	[SerializeField] private float _gracePeriod = 0f;

	Blocking blocking;
	[HideInInspector] public bool stuck;
	private Vector3 originalScale;
	private int projTypeIndex;
	private bool startEvaporate;
	private bool isEvaporating;
	private float moveSpeed;
	public float lifeTime { get; private set; }
	private float interpolateTimer = 0f;
	private float gracePeriod;
	private BoxCollider boxCollider;
	AfterImage aim;

	private Material mat;
	private Color originalColorEM;
	private Color originalColorMAT;
	private Color invertedColorEM;
	private Color invertedColorMAT;

	bool halved = false;
	protected float currentVelocity;

	private void Start()
	{
		aim = GetComponentInChildren<AfterImage>();
		stuck = false;
		if (TryGetComponent(out MeshRenderer mRend))
		{
			mat = mRend.material;
			originalColorEM = mat.GetColor("_EmissionColor");
			originalColorMAT = mat.GetColor("_Color");
			invertedColorEM = new Color(1.0f - mat.GetColor("_EmissionColor").r, 1.0f - mat.GetColor("_EmissionColor").g, 1.0f - mat.GetColor("_EmissionColor").b);
			invertedColorMAT = new Color(1.0f - mat.GetColor("_Color").r, 1.0f - mat.GetColor("_Color").g, 1.0f - mat.GetColor("_Color").b);
		}
			
	}
	private void Awake()
	{
		boxCollider = GetComponent<BoxCollider>();
		rb = GetComponent<Rigidbody>();
		originalScale = transform.localScale;
		blocking = FindObjectOfType<Blocking>();
	}
	private void OnEnable()
	{
		halved = false;
		gracePeriod = _gracePeriod;
		if (_gracePeriod > 0f)
			boxCollider.enabled = false;
		if (!isBeam && _gracePeriod <= 0f)
		{
			boxCollider.enabled = true;
		}
		if(!isBeam) transform.localScale = originalScale;
		lifeTime = lifeTimeCT;
		transform.parent = null;
	}
	private void OnDisable()
	{
		if(mat != null)
		{
			mat.SetColor("_EmissionColor", originalColorEM);
			mat.SetColor("_Color", originalColorMAT);
		}
		stuck = false;
		interpolateTimer = 0f;
		transform.gameObject.layer = 6;
		transform.gameObject.tag = "Projectile";
		isEvaporating = false;
		startEvaporate = false;

	}

	void FixedUpdate()
	{
		if(aim != null){
			if(rb.velocity == Vector3.zero) aim.activate = false;
			else aim.activate = true;
		} 
		if (EnemyStagger.StaggerInstance != null)
		{
			if (EnemyStagger.StaggerInstance.stunDuration > 0f && interpolateTimer <= 0f) Evaporate();
			if (!startEvaporate && !stuck && lifeTime > 0f && EnemyStagger.StaggerInstance.stunDuration < 0f && !isEvaporating){
				SetVelocity(velocityCurve.Evaluate(lifeTimeCT - lifeTime));
			}
			else SetVelocity(0f);

			if (EnemyStagger.StaggerInstance.stunDuration > 0f && !halved)
			{
				lifeTime /= 3;
				halved = true;
			}
		}
		if (lifeTime > 0f)
			lifeTime -= Time.deltaTime + (Convert.ToInt64(isEvaporating) * 4f * Time.deltaTime);
		else Evaporate();

		if (startEvaporate) Evaporate();
		if (_gracePeriod > 0f && gracePeriod > 0f)
			gracePeriod -= Time.deltaTime;

		if(_gracePeriod > 0f && gracePeriod <= 0f)
			boxCollider.enabled = true;
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !isBeam)
		{
			other.GetComponent<HealthHandler>().TakeHit(damageAmount);
			if (sticks)
			{
				transform.parent = other.transform;
				stuck = true;
			}

		}
		if (parriable && other.CompareTag("Shield") && !stuck)
		{
			if(mat != null){
				mat.SetColor("_Color", invertedColorMAT);
				mat.SetColor("_EmissionColor", invertedColorEM);
			}
			if (destroyedOnParry) Evaporate();
			//SetMoveDirection(new Vector3(other.transform.forward.x, 0f, -moveDir.z));
			rb.rotation = Quaternion.LookRotation(new Vector3(other.transform.forward.x, 0f, other.transform.forward.z));
			transform.gameObject.layer = 11;
			transform.gameObject.tag = "FProjectile";
			if(blocking.slowdownCooldown <= 0f && !destroyedOnParry)
			{
				blocking.ReflectSlowDown();
			}

		}
		if (other.CompareTag("Enemy") && sticks)
		{
			stuck = true;
			transform.parent = other.transform;
		}
		if (other.CompareTag("Ground") && sticks)
			stuck = true;
	}
	public void SetIndex(int index)
	{
		projTypeIndex = index;
	}
	public void Evaporate()
	{
		if (!isBeam)
		{
			gameObject.GetComponent<Collider>().enabled = false;
			isEvaporating = true;
			interpolateTimer += Time.deltaTime;
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0f, 0f, 1f), interpolateTimer);
			if (interpolateTimer >= 1f) Destroyed();
		}
		else Destroyed();
			
	}
	public void Destroyed()
	{
		if (ProjectilePools.ObjectPoolInstance != null)
			ProjectilePools.ObjectPoolInstance.ReturnPooledObject(gameObject, projTypeIndex);
		else print("destruction returned null");
	}
	public void SetVelocity(float newSpeed)
	{
		moveSpeed = newSpeed;
		rb.velocity = transform.forward * moveSpeed;
	}
}

