using UnityEngine;

public class ShieldHitAmplifier : MonoBehaviour
{
    Material shieldMat;
    Blocking blocking;
    MovementHandler movement;
    private void Start()
    {
        movement = FindObjectOfType<MovementHandler>();
        blocking = FindObjectOfType<Blocking>();
        shieldMat = GetComponent<Renderer>().material;
    }
}