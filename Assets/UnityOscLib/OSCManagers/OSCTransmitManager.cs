using System;
using System.Net;
using System.Collections.Generic;
using UnityEngine;
using UnityOSC;

public class OSCTransmitManager : MonoBehaviour
{
    // #### Unity Inspector Items ####
    // ###############################

    // Struct for Receiver List in Unity Inspector 
    [Serializable]
    public class OscReceiverInfo
    {
        public string name;
        public string host;
        public int port;
    }

    // List object for Unity Inspector
    [SerializeField]
    private List<OscReceiverInfo> oscReceivers = new List<OscReceiverInfo>();

    // #### Class Items ####
    // #####################

    public static OSCTransmitManager Transmitter { get; private set; }
    private readonly Dictionary<string, OSCClient> oscReceiversDict = new Dictionary<string, OSCClient>();

    void Awake()
    {
        // Implement Pseudo-Singleton 
        // Should only need one transmitter per project
        if(Transmitter == null)
        {
            DontDestroyOnLoad(gameObject);
            Transmitter = this;
        }
        else if (Transmitter != this)
        {
            Destroy(gameObject);
        }

        //Initialize Dict from Receivers added in Inspector
        foreach (var rec in oscReceivers)
        {
            oscReceiversDict[rec.name] = new OSCClient(IPAddress.Parse(rec.host), rec.port);
        }
    }

    public void AddReceiver(string name, string host, int port)
    {
        oscReceivers.Add(new OscReceiverInfo { name = name, host = host, port = port });
        oscReceiversDict[name] = new OSCClient(IPAddress.Parse(host), port);
    }

    public void SendOscMessageAll(string addresss,  params object[] values)
    {
        foreach(var entry in oscReceiversDict)
        {
            SendOscMessage(entry.Key,addresss, values);
        }
    }

    public void SendOscMessage(string name,  string address, params object[] values)
    {
        // Validate that address starts with a /
        address = address[0] != '/' ? '/' + address : address;

        OSCMessage message = new OSCMessage(address);
        foreach (var msgvalue in values)
        {
            message.Append(msgvalue);
        }

        Debug.Log(string.Format("Sending OSC to Receiver {0}: {2}", name, address, message));

        oscReceiversDict[name].Send(message);
    }
}
