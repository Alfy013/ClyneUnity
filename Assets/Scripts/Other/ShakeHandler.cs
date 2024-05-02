using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShakeHandler : MonoBehaviour
{
    public static ShakeHandler Instance { get; private set; }
    CinemachineVirtualCamera CVCam;
    private float shakeTimer;

    void Start()
    {
        Instance = this;
        CVCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin shakeMod = CVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        shakeMod.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0f) shakeTimer -= Time.deltaTime;
        else
        {
            CinemachineBasicMultiChannelPerlin shakeMod = CVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            shakeMod.m_AmplitudeGain = 0f;
        }
    }
}
