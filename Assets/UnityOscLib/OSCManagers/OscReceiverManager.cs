namespace UnityOscLib
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    // Class to support UnityEvents for handling OSCMessages
    [Serializable]
    public class UnityEventOscMessage : UnityEvent<OSCMessage> { }

    // Delegate class for OSC address handling
    public delegate void OscReceivedHandler(OSCMessage message);         

    public class OscReceiverManager : MonoBehaviour
    {

        // #### Unity Inspetor Properties ####
        // ###################################

        // Class to allow Unity Inspector Configuration of Event Handling
        [Serializable]
        public class OscAddressHandler
        {
            public string oscAddress;
            public UnityEventOscMessage oscMessageHandler = new UnityEventOscMessage();

            public OscAddressHandler() { }

            public OscAddressHandler(string addr)
            {
                oscAddress = addr;
            }

            public OscAddressHandler(string addr, UnityAction<OSCMessage> handler)
            {
                oscAddress = addr;
                oscMessageHandler.AddListener(handler);
            }
        }

        public int listeningPort = 10101;

        // #### Class  Properties ####
        // ###########################

        public static OscReceiverManager Instance { get; private set; }     // Provide access to Receiver singleton
        private OSCReciever oscReciever = new OSCReciever();                             

        // Add OSC address registration to the Unity Inspector; 
        // Adding a blank item to add visual cue to user about the usage 
        // and too provide a default method for handling all messages
        [SerializeField]
        private List<OscAddressHandler> oscAddressHandlers = new List<OscAddressHandler>()
        {
            new OscAddressHandler("/*"),
        };



        void Awake()
        {
            // Implement Pseudo-Singleton 
            // Should only need one receiver per project
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            oscReciever.Open(listeningPort);
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
                        if (addrHandler.oscAddress.Equals(message.Address)) addrHandler.oscMessageHandler.Invoke(message);
                        else if (addrHandler.oscAddress.Equals("/*")) addrHandler.oscMessageHandler.Invoke(message);
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
}