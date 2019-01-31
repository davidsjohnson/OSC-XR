using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityOSC;

public class OscAddressExistsException : System.Exception
{
    public OscAddressExistsException(string message) : base(message) { }
}

public class OscReceiverManager : MonoBehaviour
{
    public int listeningPort = 10101;
    public bool registerDefaultHandler = true;

    public delegate void OscReceivedHandler(OSCMessage message);

    public static OscReceiverManager ReceiverManager { get; private set; }

    private OSCReciever oscReciever = new OSCReciever();
    private Dictionary<string, OscReceivedHandler> registeredAddresses = new Dictionary<string, OscReceivedHandler>();

    void Awake()
    {
        // Implement Pseudo-Singleton 
        // Should only need one receiver per project
        if (ReceiverManager == null)
        {
            DontDestroyOnLoad(gameObject);
            ReceiverManager = this;
        }
        else if (ReceiverManager != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        oscReciever.Open(listeningPort);
        if (registerDefaultHandler) RegisterOscAddress("/*", DefaultHandler);
        StartCoroutine("ProcessMessages");
    }

    private void OnDisable()
    {
        oscReciever.Close();
        StopCoroutine("ProcessMessages");
    }

    public void RegisterOscAddress(string oscAddress, OscReceivedHandler handler) 
    {
        if (registeredAddresses.ContainsKey(oscAddress)) throw new OscAddressExistsException("OSC Address already registered");
        registeredAddresses[oscAddress] = handler;
    }

    private IEnumerator ProcessMessages() 
    {
        while (true)
        {
            while (oscReciever.hasWaitingMessages())
            {
                OSCMessage message = oscReciever.getNextMessage();
                foreach (var kv in registeredAddresses)
                {
                    if (kv.Key.Equals(message.Address)) kv.Value(message);
                    else if (kv.Key.Equals("/*")) kv.Value(message);
                }
            }
            yield return new WaitForSeconds(.01f);
        }

    }

    private void DefaultHandler(OSCMessage message)
    {
        Debug.Log(string.Format("Message Received {0}", message));
    }

}
