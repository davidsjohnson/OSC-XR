using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCTransmitterObject : MonoBehaviour
{

    // #### Unity Inspector Items ####
    // ###############################

    [Tooltip("Names of OSC Receivers to send messages to. Leave empty to send to all receivers in Transmitter DB")]
    public List<string> clientNames = new List<string>();

    public bool sendWorldPos = false;
    public bool sendLocalPos = false;

    protected void SendOSCMessage(string address, params object[] values)
    {
        if (clientNames.Count > 0)
        {
            foreach (var cname in clientNames)
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