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

    public virtual void OnEnable()
    {
        controllable = GetComponent<VRTK_BaseControllable>();
        ManageListeners(true);

        oscPad = oscTransmitObject as OSCPadController;
    }

    public virtual void OnDisable()
    {
        ManageListeners(false);
    }

    protected virtual void ManageListeners(bool state)
    {

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

        List<object> values = new List<object>();
        if (oscPad.sendTrigger && currValue > oscPad.triggerThreshold)
        {
            if (oscPad.sendVelocity)
                values.Add((currValue - prevValue) / Time.fixedDeltaTime);
            oscTransmitObject.SendOSCMessage(string.Format("{0}/pressed", oscTransmitObject.oscAddress), values.ToArray());
        }
    }

    //Custom Approximately function with modifiable epsilon
    private bool Approximately(float a, float b, float epsilon = .00001f)
    {
        return (a >= b - epsilon && a <= b + epsilon);
    }
}
