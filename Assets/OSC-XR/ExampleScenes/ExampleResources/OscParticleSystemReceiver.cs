using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityOscLib;

public class OscParticleSystemReceiver : MonoBehaviour
{
    ParticleSystem ps;

    private void Start()
    {
        OscReceiverManager.Instance.RegisterOscAddress("/particleslider/value", RateSliderHandler);

        ps = GetComponent<ParticleSystem>();
    }

    private void RateSliderHandler(OSCMessage message)
    {
        float sliderRate = (float)message.Data[1];
        Debug.Log("Rate: " + sliderRate);
        var em = ps.emission;
        em.rateOverTime = sliderRate * 10;
    }
}
