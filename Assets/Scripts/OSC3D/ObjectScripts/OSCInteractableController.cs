using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class OSCInteractableController : OSCTransmitterObjectBase
{
    [Header("OSC Interactable Settings")]
    public bool sendLocalPosition;
    public bool sendWorldPosition;
    public bool sendLocalScale;
    public bool sendWorldScale;
    public bool sendLocalRotation;
    public bool sendWorldRotation;

    [Header("OSC Interactable Config")]
    public VRTK_InteractableObject interactableObject;

    private Vector3 prevPositionLocal;
    private Vector3 prevPositionWorld;
    private Vector3 prevScaleLocal;
    private Vector3 prevScaleWorld;
    private Vector3 prevRotationLocal;
    private Vector3 prevRotationWorld;

    private static int counter = 0;

    private float equalityFidelity = 0.001f;

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
        oscAddress = string.IsNullOrEmpty(oscAddress) ? "/interactable" : oscAddress;

        prevPositionLocal = interactableObject.transform.localPosition;
        prevPositionWorld = interactableObject.transform.position;
        prevScaleLocal = interactableObject.transform.localScale;
        prevScaleWorld = interactableObject.transform.lossyScale;
        prevRotationLocal = interactableObject.transform.localEulerAngles;
        prevRotationWorld = interactableObject.transform.eulerAngles;
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
        if (!sendContinously)
        {
            // Updated Changed Flags
            localPosChanged = !VRTK_SharedMethods.Vector3ShallowCompare(prevPositionLocal, interactableObject.transform.localPosition, equalityFidelity);
            worldPosChanged = !VRTK_SharedMethods.Vector3ShallowCompare(prevPositionWorld, interactableObject.transform.position, equalityFidelity);

            localScaleChanged = !VRTK_SharedMethods.Vector3ShallowCompare(prevScaleLocal, interactableObject.transform.localScale, equalityFidelity);
            worldScaleChanged = !VRTK_SharedMethods.Vector3ShallowCompare(prevScaleWorld, interactableObject.transform.lossyScale, equalityFidelity);

            localRotationChanged = !VRTK_SharedMethods.Vector3ShallowCompare(prevRotationLocal, interactableObject.transform.localEulerAngles, equalityFidelity);
            worldRotationChanged = !VRTK_SharedMethods.Vector3ShallowCompare(prevRotationWorld, interactableObject.transform.eulerAngles, equalityFidelity);

            // Send data for changed values
            sendTransformData();
        }

        prevPositionLocal = interactableObject.transform.localPosition;
        prevPositionWorld = interactableObject.transform.position;
        prevScaleLocal = interactableObject.transform.localScale;
        prevScaleWorld = interactableObject.transform.lossyScale;
        prevRotationLocal = interactableObject.transform.localEulerAngles;
        prevRotationWorld = interactableObject.transform.eulerAngles;
    }

    private void sendTransformData()
    {
        if (sendLocalPosition && (sendContinously || localPosChanged))
            SendOSCMessage(string.Format("{0}/position/local", oscAddress),
                interactableObject.transform.localPosition.x, interactableObject.transform.localPosition.y, interactableObject.transform.localPosition.z);

        if (sendWorldPosition && (sendContinously || worldPosChanged) )
            SendOSCMessage(string.Format("{0}/position/world", oscAddress),
                interactableObject.transform.position.x, interactableObject.transform.position.y, interactableObject.transform.position.z);

        if (sendLocalScale && (sendContinously || localScaleChanged))
            SendOSCMessage(string.Format("{0}/scale/local", oscAddress),
                interactableObject.transform.localScale.x, interactableObject.transform.localScale.y, interactableObject.transform.localScale.z);

        if (sendWorldScale && (sendContinously || worldScaleChanged))
            SendOSCMessage(string.Format("{0}/scale/world", oscAddress),
                interactableObject.transform.lossyScale.x, interactableObject.transform.lossyScale.y, interactableObject.transform.lossyScale.z);

        if (sendLocalRotation && (sendContinously || localRotationChanged))
            SendOSCMessage(string.Format("{0}/rotation/local", oscAddress),
                interactableObject.transform.localEulerAngles.x, interactableObject.transform.localEulerAngles.y, interactableObject.transform.localEulerAngles.z);

        if (sendWorldRotation && (sendContinously || worldRotationChanged))
            SendOSCMessage(string.Format("{0}/rotation/world", oscAddress),
                interactableObject.transform.eulerAngles.x, interactableObject.transform.eulerAngles.y, interactableObject.transform.eulerAngles.z);
    }
}
