using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModes : MonoBehaviour
{
    [SerializeField] GameObject lockedCamera;
	[SerializeField] GameObject unlockedCamera;	
    [SerializeField] GameObject playerAsset;
    [SerializeField] float rotationSpeed;
    Transform target;
	private Quaternion rotation;
	private Vector3 direction;
    private bool locked = true;
    private bool resetRotation;
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<EnemyStagger>().transform;
        unlockedCamera.transform.rotation = lockedCamera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) locked = !locked;
        if (!locked)
		{
			transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X"), 0f));
		}
		else if(resetRotation)
		{
			lockedCamera.SetActive(true);
			unlockedCamera.SetActive(false);
			resetRotation = false;
			transform.rotation = rotation;
			playerAsset.transform.localRotation = Quaternion.Euler(Vector3.zero);
			
		}
		if (!locked && !resetRotation)
		{
			unlockedCamera.transform.rotation = lockedCamera.transform.rotation;
			resetRotation = true;
			unlockedCamera.SetActive(true);
			lockedCamera.SetActive(false);
		}
		if (locked)
		{
			direction = (target.position - transform.position).normalized;
			rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
		}
    }
}
