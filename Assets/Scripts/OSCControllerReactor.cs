using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Controllables;

public class OSCControllerReactor : MonoBehaviour
{

    protected VRTK_BaseControllable controllableEvents;
    public  OSCTransmitterObjectBase oscTransmitter;

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
        oscTransmitter.SendOSCMessage(oscTransmitter.oscAddress, currentValue);
       
    }

    private void FixedUpdate()
    {
        if (!oscTransmitter.sendContinously)
            { oscTransmitter.SendOSCMessage(oscTransmitter.oscAddress, currentValue); }
    }

}
