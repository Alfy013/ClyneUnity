using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
	public bool isMoving;
	private ProjectileHandler handler;
	[SerializeField] GameObject indicator;
	[SerializeField] GameObject beam;
	[SerializeField] GameObject beamHitbox;
	void OnEnable(){
		indicator.transform.localScale = Vector3.zero;
	}
	private void Start()
	{
		handler = GetComponent<ProjectileHandler>();
	}
	public void On()
	{
		isMoving = true;
		indicator.SetActive(true);
		beamHitbox.SetActive(false);
		beam.SetActive(false);
	}
	public void StopMoving()
	{
		handler.SetVelocity(0f);
		isMoving = false;
	}
	public void Off()
	{
		beamHitbox.GetComponent<BeamDetecion>().canStun = true;
		indicator.SetActive(false);
		beamHitbox.SetActive(true);
		beam.SetActive(true);
		//ShakeHandler.Instance.ShakeCamera(2f, 1f);
	}
	public void StopStun()
	{
		beamHitbox.GetComponent<BeamDetecion>().canStun = false;
	}
}
