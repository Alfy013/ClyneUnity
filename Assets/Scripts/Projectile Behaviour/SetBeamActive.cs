using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SetBeamActive : MonoBehaviour
{
	[SerializeField] BeamAttack beamscript;
	[SerializeField] private GameObject beam;
	[SerializeField] private Transform player;
	[SerializeField] private float speed;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			beam.transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
			beam.SetActive(!beam.activeInHierarchy);
		}
		if(beamscript.isMoving == true)
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, transform.position.y, player.position.z), speed);
	}
}
