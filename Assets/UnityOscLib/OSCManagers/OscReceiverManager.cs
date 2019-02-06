using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityOSC;

public class OscAddressExistsException : System.Exception
{
    public OscAddressExistsException(string message) : base(message) { }
}

[Serializable]
public class UnityEventOscMessage : UnityEvent<OSCMessage> { }

public class OscReceiverManager : MonoBehaviour
{
    [Serializable]
    public class OscAddressHandler
    {
        public string oscAddress;
        public UnityEventOscMessage oscEventHandler = new UnityEventOscMessage();

        public OscAddressHandler() { }

        public OscAddressHandler(string addr)
        {
            oscAddress = addr;
        }

        public OscAddressHandler(string addr, UnityAction<OSCMessage> handler)
        {
            oscAddress = addr;
            oscEventHandler.AddListener(handler);
        }
    }

    public int listeningPort = 10101;
    public bool registerDefaultHandler = true;

    public delegate void OscReceivedHandler(OSCMessage message);

    public static OscReceiverManager ReceiverManager { get; private set; }

    private OSCReciever oscReciever = new OSCReciever();

    //Finish This...
    [SerializeField]

    private List<OscAddressHandler> oscAddressHandlers = new List<OscAddressHandler>()
    {
        new OscAddressHandler("/*"),
    };

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

    public void RegisterOscAddress(string oscAddress, UnityAction<OSCMessage> handler) 
    {
        // Change to dict or tree implementation for better routing capabilities
        oscAddressHandlers.Add(new OscAddressHandler(oscAddress, handler));
    }

    private IEnumerator ProcessMessages() 
    {
        while (true)
        {
            while (oscReciever.hasWaitingMessages())
            {
                OSCMessage message = oscReciever.getNextMessage();
                foreach (var addrHandler in oscAddressHandlers)
                {
                    if (addrHandler.oscAddress.Equals(message.Address)) addrHandler.oscEventHandler.Invoke(message);
                    else if (addrHandler.oscAddress.Equals("/*")) addrHandler.oscEventHandler.Invoke(message);
                }
            }
            yield return new WaitForSeconds(.01f);
        }

    }

    public void DefaultHandler(OSCMessage message)
    {
        Debug.Log(string.Format("Message Received {0}", message));
    }

}
