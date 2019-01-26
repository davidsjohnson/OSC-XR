using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK.Controllables;

public class OSCSliderReactor : MonoBehaviour
{

    protected VRTK_BaseControllable controllableEvents;
    public  OSCTransmitterObjectBase oscTransmitObject;

    public Image sliderImage;

    private float currentValue = 0;

    protected virtual void OnEnable()
    {
        controllableEvents = GetComponent<VRTK_BaseControllable>();

        ManageListeners(true);
    }

    protected virtual void OnDisable()
    {
        ManageListeners(false);
    }

    protected virtual void ManageListeners(bool state)
    {
        if (state)
        { controllableEvents.ValueChanged += ValueChanged; }
        else
        { controllableEvents.ValueChanged -= ValueChanged; }
    }

    protected virtual void ValueChanged(object sender, ControllableEventArgs e)
    {
        currentValue = e.value;
        oscTransmitObject.SendOSCMessage(string.Format("{0}/value", oscTransmitObject.oscAddress), currentValue);

        // Update Progress Slider
        sliderImage.fillAmount = e.normalizedValue;
    }

    private void FixedUpdate()
    {
        if (oscTransmitObject.sendContinously)
            { oscTransmitObject.SendOSCMessage(string.Format("{0}/value", oscTransmitObject.oscAddress), currentValue); }
    }

}
