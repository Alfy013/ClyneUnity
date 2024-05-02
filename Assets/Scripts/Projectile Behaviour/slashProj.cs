using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class slashProj : MonoBehaviour
{
    [SerializeField] float slashProjLifeTime;
    // Update is called once per frame
    private void Awake()
    {
        Destroy(gameObject, slashProjLifeTime);
        GetComponent<VisualEffect>().Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.CompareTag("Player") && !collision.collider.CompareTag("Projectile")) Destroy(gameObject);
    }
}
