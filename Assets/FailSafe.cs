using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailSafe : MonoBehaviour
{
    float timer;
    void OnEnable(){
        timer = 0.5f;
    }
    // Update is called once per frame
    void Update()
    {
        if(timer > 0f) timer -= Time.deltaTime;
        else gameObject.SetActive(false);
    }
}
