using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCSpaceController : OSCTransmitterObjectBase
{

    [Header("On Trigger Enter Messages")]
    public bool sendTriggerEnter = true;
    public bool includeEnterLocation= false;

    [Header("On Trigger Stay Messages")]
    public bool sendTriggerStay = true;
    public bool includeLocalPosition = true;

    [Header("On Trigger Exit Messages")]
    public bool sendTriggerExit = true;
    public bool includeExitLocation = false;

    [Header("Colliders to Ignore")]
    public List<Collider> ignoreColliders;

    private void Start()
    {
        oscAddress = string.IsNullOrEmpty(oscAddress) ? "/space" : oscAddress;
    }
}
