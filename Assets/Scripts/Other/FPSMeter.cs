using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSMeter : MonoBehaviour
{
    TMP_Text FPS;
    private float deltaTime;
    private float fps;
    // Start is called before the first frame update
    void Start()
    {
        FPS = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float fps = 1.0f / deltaTime;
        FPS.text = "FPS: " + Mathf.Ceil(fps);
        if(Input.GetKeyDown(KeyCode.Backspace)) SceneManager.LoadScene(0);
    }
}
