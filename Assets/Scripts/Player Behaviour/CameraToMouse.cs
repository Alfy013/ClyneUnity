using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToMouse : MonoBehaviour
{
    Camera cam;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        player = FindObjectOfType<MovementHandler>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition - cam.WorldToScreenPoint(player.position);
	}
}
