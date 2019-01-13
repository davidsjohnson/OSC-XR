using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.PhysicsBased;

public class OSCPadController : OSCTransmitterObjectBase
{
    [Tooltip("Send Trigger message when button position passes specified threshold")]
    public bool sendTrigger;
    public float triggerThreshold = 0.5f;

    [Tooltip("Send velocity information with Trigger message")]
    public bool sendVelocity;

    [Header("Button Config")]
    public VRTK_PhysicsPusher button;

    private static int counter = 0;

    private void Start()
    {
        counter++;

        // Set up name and address
        controllerName = string.IsNullOrEmpty(controllerName) ? "button" : controllerName;
        oscAddress = string.IsNullOrEmpty(oscAddress) ? string.Format("/{0}/{1}", controllerName, counter) : oscAddress;
    }

}
