using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAuto : MonoBehaviour
{
    public float speed_X = 0f;
    public float speed_Y = 0f;
    public float speed_Z = 0f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed_X, speed_Y, speed_Z);
    }
}
