using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectile : MonoBehaviour
{
	Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
    {
		rb.AddForce(transform.forward * 3, ForceMode.Impulse);
	}
}
