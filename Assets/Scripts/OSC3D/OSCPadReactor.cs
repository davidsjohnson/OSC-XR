using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Controllables;

public class OSCPadReactor : MonoBehaviour
{
    protected VRTK_BaseControllable controllable;
    public OSCTransmitterObjectBase oscTransmitObject;

    private float prevValue = 0f;
    private float currValue = 0f;

    private OSCPadController oscPad;

    private bool padPressed;

    public virtual void OnEnable()
    {
        padPressed = false;
        controllable = GetComponent<VRTK_BaseControllable>();
        oscPad = oscTransmitObject as OSCPadController;
    }

    private void FixedUpdate()
    {
        prevValue = currValue;
        currValue = controllable.GetNormalizedValue();
        currValue = Approximately(0, currValue) ? 0f : // if close to 0 set to zero
                    Approximately(1, currValue) ? 1f : // if close to 1 set to one
                    Mathf.Round(currValue*1000)/1000f; // otherwise round value

        // Send continously simply sends 
        if (oscTransmitObject.sendContinously)
            oscTransmitObject.SendOSCMessage(string.Format("{0}/pos", oscTransmitObject.oscAddress), currValue);
    }

    private void Update()
    {
        // Simple Press Release check (should update to VRTK event based mechanism)
        if (!padPressed && currValue > oscPad.triggerThreshold)
        {
            if (oscPad.sendPressedReleased)
            {
                List<object> values = new List<object>();
                if (oscPad.sendVelocity)
                    values.Add((currValue - prevValue) / Time.fixedDeltaTime);
                oscTransmitObject.SendOSCMessage(string.Format("{0}/pressed", oscTransmitObject.oscAddress), values.ToArray());
                
            }
            padPressed = true;
        }
        else if (padPressed && currValue < oscPad.triggerThreshold)
        {
            if (oscPad.sendPressedReleased)
                oscTransmitObject.SendOSCMessage(string.Format("{0}/released", oscTransmitObject.oscAddress));
            padPressed = false;
        }
    }

    //Custom Approximately function with modifiable epsilon
    private bool Approximately(float a, float b, float epsilon = .00001f)
    {
        return (a >= b - epsilon && a <= b + epsilon);
    }
}
