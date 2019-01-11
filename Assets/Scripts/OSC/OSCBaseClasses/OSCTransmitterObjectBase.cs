using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCTransmitterObjectBase : MonoBehaviour
{

    // #### Unity Inspector Items ####
    // ###############################

    [Header("OSC Config")]

    public string controllerName;

    [Tooltip("Names of OSC Receivers to send messages to. Leave empty to send to all receivers in Transmitter DB")]
    public List<string> receiverNames = new List<string>();

    [Tooltip("OSC Address for slider value. Leave Blank to use Object Name as Address.")]
    public string oscAddress;

    [Tooltip("If false controller will only send values when the slider is changed; " +
        "if true controller will send a continous stream of values on each fixed update.")]
    public bool sendContinously;

    public void SendOSCMessage(string address, params object[] values)
    {
        if (receiverNames.Count > 0)
        {
            foreach (var cname in receiverNames)
            {
                OSCTransmitManager.Transmitter.SendToReceiver(cname, address, values);
            }
        }
        else
        {
            OSCTransmitManager.Transmitter.SendToReceivers(address, values);
        }
    }
}