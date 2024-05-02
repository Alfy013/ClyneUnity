using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlayer : MonoBehaviour
{
    public Transform target;
    public float RotationSpeed;
    private Vector3 _direction;
	private Quaternion _rotation;
    void Update()
	{
		_direction = (target.position - transform.position).normalized;
		_rotation = Quaternion.LookRotation(new Vector3(_direction.x, 0f, _direction.z));
		transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotation, RotationSpeed);
	}
}
