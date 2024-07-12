using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailSafe : MonoBehaviour
{
    float timer;
    HealthHandler hh;
    AbilityHandler ah;
    void Awake(){
        hh = FindObjectOfType<HealthHandler>();
        ah = FindObjectOfType<AbilityHandler>();
    }
    void OnEnable(){
        timer = 0.5f;
    }
    // Update is called once per frame
    void Update()
    {
        if(timer > 0f) timer -= Time.deltaTime;
        else gameObject.SetActive(false);
    }
    public void Parry(float healhToReplenish = 20f, float staminaToReplenish = 3f){
        hh.HP += healhToReplenish;
        ah.stamina += staminaToReplenish;
    }

}
