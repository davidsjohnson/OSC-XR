using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityOscLib;

public class OscTests : MonoBehaviour
{

    private readonly List<string> rcvdMessages = new List<string>();

    private void Start()
    {
        OscReceiverManager.Instance.RegisterOscAddress("/scripting", OscMessageHandler);    // Register Address through Scripting

        OscTransmitManager.Instance.SendOscMessageAll("/scripting", 3, 4.0, "scripting");   // Test address registration with scripting
        OscTransmitManager.Instance.SendOscMessageAll("/inspector", 2, 2.5, "inspector");   // Test address registration through the inspector
        OscTransmitManager.Instance.SendOscMessageAll("/default", 1, 0.5, "default");       // Test default address

        OscTransmitManager.Instance.AddReceiver("Unity2", "127.0.0.1", 10101);              // Register receiver through scripting

        OscTransmitManager.Instance.SendOscMessage("Unity", "/unity", 1, 2.5, "Unity");     // Test sending by name
        OscTransmitManager.Instance.SendOscMessage("Unity2", "/unity2", 1, 2.5, "Unity2");  // Test sending by name

        // Testing Control Rate
        OscTransmitManager.Instance.OnSendOsc += ControlRateTestTransmitter;
        OscReceiverManager.Instance.RegisterOscAddress("/controlrate/test", ControlRateTestHandler);
    }

    public void OscMessageHandler(OSCMessage message)
    {
        UnityEngine.Debug.Log("Message Recieved: " + message);
        rcvdMessages.Add(message.Address);
    }

    private Stopwatch watch = Stopwatch.StartNew();
    public void ControlRateTestHandler(OSCMessage message)
    {
        UnityEngine.Debug.Log("Control Rate Message Received. Time since last message " + watch.ElapsedMilliseconds);
        watch.Restart();
        expectedCount++;
    }

    public void ControlRateTestTransmitter()
    {
        OscTransmitManager.Instance.SendOscMessage("Unity", "/controlrate/test");
    }

    private int expectedCount = 7;
    private void OnDestroy()
    {
        // Check for issues (TODO: Look into Unity Unit Testing)
        bool error = false;
        if (rcvdMessages.Count != expectedCount)
        {
            UnityEngine.Debug.LogError(string.Format("Test Fails: Not all messages accounted for. Received {0} expeced {1}", rcvdMessages.Count, expectedCount));
            error = true;
        }
        if (!rcvdMessages.Contains("/scripting"))
        {
            UnityEngine.Debug.LogError("Test Fails: Scripting registration");
            error = true;
        }
        if (!rcvdMessages.Contains("/inspector"))
        {
            UnityEngine.Debug.LogError("Test Fails: Inspector registration");
            error = true;
        }
        if (!rcvdMessages.Contains("/unity"))
        {
            UnityEngine.Debug.LogError("Test Fails: Send by name");
            error = true;
        }
        if (!rcvdMessages.Contains("/unity2"))
        {
            UnityEngine.Debug.LogError("Test Fails: Send by name with scripting registration");
            error = true;
        }

        // Check if all passed
        if (!error)
        {
            UnityEngine.Debug.Log("Tests ALL Pass");
        }
    }
}
