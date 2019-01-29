using UnityEngine;
using VRTK;

public class OSCTriggerController : OSCTransmitterObjectBase
{

    [Header("Trigger Pointers")]
    [Tooltip("Pointer to Trigger the Object (one of right hand or left hand must be populated)")]
    public VRTK_DestinationMarker leftHandPointer;
    [Tooltip("Pointer to Trigger the Object (one of right hand or left hand must be populated)")]
    public VRTK_DestinationMarker rightHandPointer;

    public void Start()
    {
        oscAddress = string.IsNullOrEmpty(oscAddress) ? "/trigger" : oscAddress;
    }
}
