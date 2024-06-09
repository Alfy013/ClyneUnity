using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheatCodes : MonoBehaviour
{
    public KeyCode[] cheatCode;
    public UnityEvent CheatEvent;
    public string cheatMessage;
    int i = 0;
    
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown){
            if(Input.GetKeyDown(cheatCode[i]))
                i++;
            else i = 0;
        }
        if(i == cheatCode.Length){
            i = 0;
            CheatEvent.Invoke();
            print(cheatMessage);
        }
    }
}
