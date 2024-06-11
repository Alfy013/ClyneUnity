using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordProjectile : MonoBehaviour
{
	[SerializeField] float multiplier;
	[SerializeField] ForceMode forceMode;
	[SerializeField] float _timeToDisappear;
	Vector3 startSize;
	float timeToDisappear;
	Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * multiplier, forceMode);
	}

	// Update is called once per frame
	void Update()
    {
		if(timeToDisappear > 0f){
			transform.localScale = Vector3.Lerp(startSize, Vector3.zero, 1 - (timeToDisappear/_timeToDisappear));
			timeToDisappear -= Time.deltaTime;
		}

	}
	void OnTriggerEnter(Collider col){

		
		if(col.CompareTag("Enemy")){
			Destroy(gameObject, _timeToDisappear);
			startSize = transform.localScale;
			timeToDisappear = _timeToDisappear;
			GetComponent<ScaleUP>().enabled = false;
			rb.velocity = Vector3.zero;
		}
	}
}
