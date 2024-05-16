using System;
using UnityEngine;

public class ThrownSword : MonoBehaviour
{
    [SerializeField] float returnDelay;
    [SerializeField] float _timeToReturn;
    [SerializeField] float speedMultiplier;
    GameObject player;
    float timeToArrive;
    Transform playerPos;
    Vector3 startingPos;
    bool started = false;
    GameObject swordGameObject;
    Rigidbody rb;
    void OnEnable()
    {
        swordGameObject = FindObjectOfType<Sword>().gameObject;
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerHandler>().gameObject;
        playerPos = player.transform;
        rb.AddForce(transform.forward * speedMultiplier, ForceMode.Impulse);
        timeToArrive = _timeToReturn;
        swordGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(returnDelay > 0f) returnDelay -= Time.deltaTime;
        else{
            if(!started){
                started = true;
                startingPos = transform.position;
                GetComponent<BoxCollider>().enabled = true;
            }
            transform.position = Vector3.Lerp(startingPos, playerPos.position, 1 - (timeToArrive / _timeToReturn));
            timeToArrive -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
    private void OnDestroy(){
        player.GetComponent<Slashing>().ReturnSword();
        swordGameObject.SetActive(true);
    }
}
