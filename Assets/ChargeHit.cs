using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeHit : MonoBehaviour
{
    [HideInInspector]
    public float damage;
    [SerializeField] ParticleSystem particle;
    void OnTriggerEnter(Collider col){
        if(col.CompareTag("Enemy")){
            col.GetComponent<EnemyStagger>().TakeHit(damage);
            particle.Play();
        }
    }
}
