using UnityEngine;

public class RotateAuto : MonoBehaviour
{
    public float speed_X = 0f;
    public float speed_Y = 0f;
    public float speed_Z = 0f;

    void FixedUpdate()
    {
        transform.Rotate(speed_X, speed_Y, speed_Z);
    }
}
