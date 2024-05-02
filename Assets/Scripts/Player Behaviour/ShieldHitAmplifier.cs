using UnityEngine;

public class ShieldHitAmplifier : MonoBehaviour
{
    Material shieldMat;
    Blocking blocking;
    PlayerHandler movement;
    private void Start()
    {
        movement = FindObjectOfType<PlayerHandler>();
        blocking = FindObjectOfType<Blocking>();
        shieldMat = GetComponent<Renderer>().material;
    }
}