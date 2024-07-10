using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class CameraModes : MonoBehaviour
{
    [SerializeField] GameObject[] cameras;
    [SerializeField] float rotationSpeed;
    Transform target;
	private Quaternion rotation;
	private Vector3 direction;
    private bool locked = true;
	private int currentCamIndex;
    private bool resetRotation;
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<EnemyStagger>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Lock")) locked = !locked;
        if (!locked)
		{
			transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X"), 0f));
		}
		else
		{
			direction = (target.position - transform.position).normalized;
			rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
		}
		if(Input.GetButtonDown("Camera")){
			cameras[currentCamIndex].SetActive(false);
			if(currentCamIndex < cameras.Length - 1)
				currentCamIndex++;
			else
				currentCamIndex = 0;
			cameras[currentCamIndex].SetActive(true);
		}
    }
}
