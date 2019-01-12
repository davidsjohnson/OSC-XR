using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCInteractable : OSCTransmitterObjectBase
{
    [Header("OSC Interactable Settings")]

    public bool sendLocalPosition;
    public bool sendWorldPosition;
    public bool sendLocalScale;
    public bool sendWorldScale;
    public bool sendLocalRotation;
    public bool sendWorldRotation;

    private Vector3 prevPositionLocal;
    private Vector3 prevPositionWorld;
    private Vector3 prevScaleLocal;
    private Vector3 prevScaleWorld;
    private Vector3 prevRotationLocal;
    private Vector3 prevRotationWorld;

    private static int counter = 0;

    private bool localPosChanged;
    private bool worldPosChanged;
    private bool localScaleChanged;
    private bool worldScaleChanged;
    private bool localRotationChanged;
    private bool worldRotationChanged;

    private void Start()
    {
        counter++;
        // Set up name and address
        controllerName = string.IsNullOrEmpty(controllerName) ? "interactable" : controllerName;
        oscAddress = string.IsNullOrEmpty(oscAddress) ? string.Format("/{0}/{1}", controllerName, counter) : oscAddress;

        prevPositionLocal = transform.localPosition;
        prevPositionWorld = transform.position;
        prevScaleLocal = transform.localScale;
        prevScaleWorld = transform.lossyScale;
        prevRotationLocal = transform.localEulerAngles;
        prevRotationWorld = transform.eulerAngles;
    }

    private void FixedUpdate()
    {
        // Using Fixed Update for continous data so it is received at predictable intervals
        if (sendContinously)
        {
            sendTransformData();
        }
    }

    private void Update()
    {
        // Updated Changed Flags
        if (transform.hasChanged)
        {
            localPosChanged = prevPositionLocal != transform.localPosition;
            worldPosChanged = prevPositionWorld != transform.position;

            localScaleChanged = prevScaleLocal != transform.localScale;
            worldScaleChanged = prevScaleWorld != transform.lossyScale;

            localRotationChanged = prevRotationLocal != transform.localEulerAngles;
            worldRotationChanged = prevRotationWorld != transform.eulerAngles;

        }

        // Send data for changed values
        sendTransformData();
    }

    private void sendTransformData()
    {
        if (sendLocalPosition && (sendContinously || localPosChanged))
            SendOSCMessage(string.Format("{0}/position/local", oscAddress), 
                transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);

        if (sendWorldPosition && (sendContinously || worldPosChanged) )
            SendOSCMessage(string.Format("{0}/position/world", oscAddress),
                transform.position.x, transform.position.y, transform.position.z);

        if (sendLocalScale && (sendContinously || localScaleChanged))
            SendOSCMessage(string.Format("{0}/scale/local", oscAddress),
                transform.localScale.x, transform.localScale.y, transform.localScale.z);

        if (sendWorldScale && (sendContinously || worldScaleChanged))
            SendOSCMessage(string.Format("{0}/scale/world", oscAddress),
                transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);

        if (sendLocalRotation && (sendContinously || localRotationChanged))
            SendOSCMessage(string.Format("{0}/rotation/local", oscAddress),
                transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);

        if (sendWorldRotation && (sendContinously || worldRotationChanged))
            SendOSCMessage(string.Format("{0}/rotation/world", oscAddress),
                transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
