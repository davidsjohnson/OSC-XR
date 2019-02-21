namespace UnityOscLib
{

    using System;
    using System.Net;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class OscTransmitManager : MonoBehaviour
    {
        // #### Unity Inspector Items ####
        // ###############################
        [Tooltip("Set the Control Rate for the OnSendOsc event. Setting to 0 used Fixed Update instead of OnSendOsc")]
        [SerializeField]
        private int controlRateHz = 0;

        // List of OscReceiverInfo structs to support receiver config
        // in the Unity Inspector (since it doesn't support dicts)
        // Struct for Receiver List in Unity Inspector 
        [Serializable]
        public class OscReceiverInfo
        {
            public string name;
            public string host;
            public int port;
        }
        [SerializeField]
        private List<OscReceiverInfo> oscReceivers = new List<OscReceiverInfo>(); 


        // #### Class Items ####
        // #####################
        public static OscTransmitManager Instance { get; private set; }                                         // Provide access to the Transmitter Singlton

        private readonly Dictionary<string, OSCClient> oscReceiversDict = new Dictionary<string, OSCClient>();  // Store Receiver Configuration

        //Control Rate Delegate and Event
        public delegate void SendOsc();
        public event SendOsc OnSendOsc;

        private void Awake()
        {
            // Implement Pseudo-Singleton 
            // Should only need one transmitter per project
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            //Initialize Dict from Receivers added in Inspector
            foreach (var rec in oscReceivers)
            {
                oscReceiversDict[rec.name] = new OSCClient(IPAddress.Parse(rec.host), rec.port);
            }
        }

        private void Start()
        {
            if (controlRateHz > 0)
            {
                StartCoroutine("ControlRateUpdate");
            }
        }

        private void OnDestroy()
        {
            if (controlRateHz > 0)
            {
                StopCoroutine("ControlRateUpdate");
            }
        }

        private IEnumerator ControlRateUpdate()
        {
            Debug.Log("Starting Control Reate Upeate");
            while (true)
            {
                
                if (OnSendOsc != null)
                    OnSendOsc.Invoke();
                yield return new WaitForSecondsRealtime(1.0f / controlRateHz);
            }
        }

        private void FixedUpdate()
        {
            if (controlRateHz <= 0)
            {
                if (OnSendOsc != null)
                    OnSendOsc.Invoke();
            }
        }

        public void AddReceiver(string name, string host, int port)
        {
            oscReceivers.Add(new OscReceiverInfo { name = name, host = host, port = port });
            oscReceiversDict[name] = new OSCClient(IPAddress.Parse(host), port);
        }

        public void SendOscMessageAll(string addresss, params object[] values)
        {
            foreach (var entry in oscReceiversDict)
            {
                SendOscMessage(entry.Key, addresss, values);
            }
        }

        public void SendOscMessage(string name, string address, params object[] values)
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
}