using System;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.VFX;

public class ThrownSword : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] Transform targetPoint;
    [SerializeField] ParticleSystem trail;
    [SerializeField] GameObject swordGameObject;
    [SerializeField] float _returnDelay;
    [SerializeField] float _timeToReturn;
    [SerializeField] float swordSpeedFraction = 0.5f;
    [SerializeField] float moveFor = 0.5f;
    [SerializeField] float _graceTime = 0.1f;
    [SerializeField] float _timeBetweenHits;
    [SerializeField] VisualEffect recallEffectPlayer;
    [SerializeField] VisualEffect recallEffectSword;
    SwordThrowAndCatch swordThrowAndCatch;
    float timeBetweenHits;
    GameObject player;
    float returnDelay;
    float graceTime;
    Vector3 targetPosStatic;
    BoxCollider swordCollider;
    void Awake(){
        player = FindObjectOfType<MovementHandler>().gameObject;
        swordThrowAndCatch = player.GetComponent<SwordThrowAndCatch>();
        swordCollider = GetComponent<BoxCollider>();
    }
    void OnEnable()
    {
        trail.Play();
        returnDelay = _returnDelay;
        swordGameObject.SetActive(false);
        graceTime = _graceTime;
        swordCollider.enabled = false;
        targetPosStatic = new Vector3(targetPoint.transform.position.x, transform.position.y, targetPoint.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(returnDelay > _returnDelay - moveFor)
            transform.position = Vector3.Lerp(transform.position, targetPosStatic, 1 - Mathf.Pow(swordSpeedFraction, Time.deltaTime * 10));

        if(Input.GetButtonDown("Slash") && returnDelay < _returnDelay - moveFor && !swordThrowAndCatch.catchSword){
            returnDelay = 0.25f;
            swordThrowAndCatch.animator.SetBool("Recall", true);
            recallEffectPlayer.Play();
            recallEffectSword.gameObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
            recallEffectSword.Play();
        }

        if(returnDelay > 0f){
            if(graceTime > 0f) graceTime -= Time.deltaTime;
            else swordCollider.enabled = true;
            returnDelay -= Time.deltaTime;
        } else if(!swordThrowAndCatch.catchSword){
            transform.position = Vector3.Lerp(transform.position, swordGameObject.transform.position, 1 - Mathf.Pow(swordSpeedFraction, Time.deltaTime * 10));
            FindObjectOfType<ShakeHandler>().ShakeCamera(3f, 0.1f);
        }
        if(timeBetweenHits > 0) timeBetweenHits -= Time.deltaTime;
    }


    void OnTriggerEnter(Collider col){
        if(col.gameObject.CompareTag("Player")){
            trail.Stop();
            player.GetComponent<SwordThrowAndCatch>().stopped = true;
            swordThrowAndCatch.animator.SetBool("Recall", false);
            gameObject.SetActive(false);
            swordGameObject.SetActive(true);
        }
    }
    void OnTriggerStay(Collider col){
        if(col.gameObject.CompareTag("Enemy") && timeBetweenHits <= 0f){
            col.GetComponent<EnemyStagger>().TakeHit(damage);
            timeBetweenHits = _timeBetweenHits;
        }
    }
}
