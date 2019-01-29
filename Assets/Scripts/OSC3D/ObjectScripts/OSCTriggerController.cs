using UnityEngine;
using VRTK;
public class OSCTriggerController : OSCTransmitterObjectBase
{
    public void Start()
    { 
        oscAddress = string.IsNullOrEmpty(oscAddress) ? "/trigger" : oscAddress;
    }
}
