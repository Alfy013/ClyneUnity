using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class CurvedProjectile : MonoBehaviour
{
    [SerializeField] AnimationCurve directionOverTime;
    [SerializeField] float _baseModifier;
    [SerializeField] float _orbitAfterSeconds = -1;
    [HideInInspector]
    public Transform parentPoint;
    float countUpToOrbit;
    Rigidbody rb;
    private void Awake(){
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable(){
        countUpToOrbit = 0;
    }
    void FixedUpdate()
    {
        countUpToOrbit += Time.deltaTime;
        if(countUpToOrbit < _orbitAfterSeconds || _orbitAfterSeconds <= -1){
            Vector3 angleVelocity = new(0f, _baseModifier * directionOverTime.Evaluate(countUpToOrbit), 0f);
            Quaternion deltaRotation = Quaternion.Euler(angleVelocity * Time.deltaTime);
            rb.MoveRotation(deltaRotation * rb.rotation);
        } else {
            transform.SetParent(parentPoint);
        }
    }
}
