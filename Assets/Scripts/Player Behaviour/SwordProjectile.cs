using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class SwordProjectile : MonoBehaviour
{
	[SerializeField] float multiplier;
	[SerializeField] ForceMode forceMode;
	[SerializeField] float _timeToDisappear;
	[SerializeField] float lifeTime;
	[SerializeField] float damage = 10;
	[SerializeField] bool destroyedOnParry = true;
	[SerializeField] VisualEffect aim;
	Vector3 startSize;

	float timeToDisappear = 0f;
	Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * multiplier, forceMode);
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		lifeTime -= Time.deltaTime;
		if(aim != null){
			aim.SetFloat("Lifetime", lifeTime + _timeToDisappear - timeToDisappear);
		}
		if(lifeTime < 0f && timeToDisappear <= 0) StartDisappearance();
		if(timeToDisappear > 0f){
			transform.localScale = Vector3.Lerp(startSize, Vector3.zero, 1 - (timeToDisappear/_timeToDisappear));
			timeToDisappear -= Time.deltaTime;
		}
	}
	void StartDisappearance(){
		Destroy(gameObject, _timeToDisappear);
		timeToDisappear = _timeToDisappear;
		GetComponent<ScaleUP>().enabled = false;
		rb.velocity = Vector3.zero;
		startSize = transform.localScale;
	}
	void OnTriggerEnter(Collider col){			
		if(col.CompareTag("Enemy")){
			if(destroyedOnParry)
				StartDisappearance();
			col.GetComponent<EnemyStagger>().TakeHit(damage);
			FindObjectOfType<AbilityHandler>().stamina += 1;
		}
	}

}
