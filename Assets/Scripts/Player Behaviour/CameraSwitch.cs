using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] GameObject cam1;
	[SerializeField] GameObject cam2;
	Transform target;
	public float RotationSpeed;
	private Quaternion _rotation;
	private Vector3 _direction;

	private void Start()
	{
		target = FindObjectOfType<EnemyStagger>().transform;
	}

	// Update is called once per frame
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
			cam1.SetActive(!cam1.activeInHierarchy);
			cam2.SetActive(!cam2.activeInHierarchy);
		}

		if (cam2.activeInHierarchy)
		{
			
		}
	}
}
