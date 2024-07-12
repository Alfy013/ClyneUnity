using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpText : MonoBehaviour
{
    [SerializeField] GameObject[] text1;
    [SerializeField] GameObject[] text2;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)){
            foreach(GameObject text in text1){
                text.SetActive(!text.activeInHierarchy);
            }
        }
        if(Input.GetKeyDown(KeyCode.J)){
            foreach(GameObject text in text2){
                text.SetActive(!text.activeInHierarchy);
            }
        }
        Application.targetFrameRate = 120;

    }
}
