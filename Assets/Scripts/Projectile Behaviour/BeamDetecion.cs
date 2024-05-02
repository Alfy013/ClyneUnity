using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamDetecion : MonoBehaviour
{
	[HideInInspector]
	public bool canStun = true;
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.CompareTag("Player"))
			collision.GetComponent<HitScript>().TakeHit(10, 2f * Convert.ToInt16(canStun));
	}
}
