using System;
using UnityEngine;

public class ThrownSword : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] Transform targetPoint;
    [SerializeField] ParticleSystem trail;
    [SerializeField] float _returnDelay;
    [SerializeField] float _timeToReturn;
    [SerializeField] float swordSpeedFraction = 0.5f;
    [SerializeField] float moveFor = 0.5f;
    [SerializeField] float _graceTime = 0.1f;
    [SerializeField] float _timeBetweenHits;
    [HideInInspector] public bool canCharge;
    float timeBetweenHits;
    GameObject player;
    float returnDelay;
    float graceTime;
    GameObject swordGameObject;
    BoxCollider swordCollider;
    void Awake(){
        swordGameObject = FindObjectOfType<Sword>().gameObject;
        player = FindObjectOfType<MovementHandler>().gameObject;
        swordCollider = GetComponent<BoxCollider>();
    }
    void OnEnable()
    {
        trail.Play();
        returnDelay = _returnDelay;
        swordGameObject.SetActive(false);
        graceTime = _graceTime;
        swordCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(returnDelay > _returnDelay * moveFor){
            transform.position = Vector3.Lerp(transform.position, targetPoint.position, 1 - Mathf.Pow(swordSpeedFraction, Time.deltaTime * 10));
            canCharge = false;
        } else canCharge = true;
        if(returnDelay > 0f){
            if(graceTime > 0f) graceTime -= Time.deltaTime;
            else swordCollider.enabled = true;
            returnDelay -= Time.deltaTime;
        } else{
            transform.position = Vector3.Lerp(transform.position, swordGameObject.transform.position, 1 - Mathf.Pow(swordSpeedFraction, Time.deltaTime * 10));
        }
        if(timeBetweenHits > 0) timeBetweenHits -= Time.deltaTime;
    }


    void OnTriggerEnter(Collider col){
        if(col.gameObject.CompareTag("Player")){
            gameObject.SetActive(false);
            swordGameObject.SetActive(true);
            trail.Stop();
            player.GetComponent<SwordThrowAndCatch>().stopped = true;
        }
    }
    void OnTriggerStay(Collider col){
        if(col.gameObject.CompareTag("Enemy") && timeBetweenHits <= 0f){
            col.GetComponent<EnemyStagger>().TakeHit(damage);
            timeBetweenHits = _timeBetweenHits;
        }
    }
}
