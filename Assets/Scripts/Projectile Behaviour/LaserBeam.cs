using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    Animator anim;
    [SerializeField] float _timeToStop;
    private float timeToStop;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void OnEnable(){
        timeToStop = _timeToStop;
        anim.SetBool("Stop", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToStop > 0f) timeToStop -= Time.deltaTime;
        else anim.SetBool("Stop", true);
    }
}
