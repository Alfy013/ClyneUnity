using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectile : MonoBehaviour
{
	[SerializeField] float multiplier;
	[SerializeField] ForceMode forceMode;
	Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
    {
		rb.AddForce(transform.forward * multiplier, forceMode);
	}
}
