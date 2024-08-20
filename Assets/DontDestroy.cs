using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [SerializeField] GameObject[] items;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(GameObject item in items){
            DontDestroyOnLoad(item);
        }
    }
}
