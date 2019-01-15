using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.PhysicsBased;

public class OSCPadController : OSCTransmitterObjectBase
{
    [Tooltip("Send Trigger message when button position passes specified threshold")]
    public bool sendPressedReleased;
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

        Vector3 pos = transform.localPosition;
        pos.z += .5f;
        transform.localPosition = pos;
    }

    // Hack to update VRTK slider maximum length when scaling the OSC Slider object
    private float previousYScale = 1f;
    void OnDrawGizmosSelected()
    {
        if (transform.localScale.y != previousYScale && transform.localScale.y != 0)
        {
            float factor = transform.localScale.y / previousYScale;
            float newmax = button.pressedDistance * factor;
            button.pressedDistance = newmax;

            previousYScale = transform.localScale.y;
        }
    }
}
