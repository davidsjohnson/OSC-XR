namespace OSCXR
{
    using System.Collections.Generic;
    using UnityEngine;

    public class OscTriggerController : BaseOscController
    {

        [Header("On Trigger Enter Messages")]
        public bool sendTriggerEnter = true;

        [Header("On Trigger Stay Messages")]
        public bool sendTriggerStay = true;
 
        [Header("On Trigger Exit Messages")]
        public bool sendTriggerExit = true;

        [Header("Include Position with Messages")]
        public bool includeLocalPosition = true;

        [Header("Colliders to Ignore")]
        public List<Collider> ignoreColliders;

        //Reactor Interactable
        [Header("Unity Game Object Controller")]
        public GameObject controlObject = null;

        private void OnEnable()
        {
            oscAddress = string.IsNullOrEmpty(oscAddress) ? string.Format("/trigger") : oscAddress;     // Set up name and address
            controlObject = controlObject ?? gameObject;                                                // Set up Control Object
        }

        private void OnTriggerEnter(Collider other)
        {

            if (sendTriggerEnter && !ignoreColliders.Contains(other))
            {
                List<object> args = new List<object>();
                if (includeLocalPosition)
                {
                    Vector3 localPos = transform.InverseTransformPoint(other.transform.localPosition);
                    args.Add(localPos.x);
                    args.Add(localPos.y);
                    args.Add(localPos.z);
                }
                SendOscMessage(string.Format("{0}/enter", oscAddress), args.ToArray());
            }
        }

        private void OnTriggerStay(Collider other)
        {
            List<object> args = new List<object>();
            if (sendTriggerStay && !ignoreColliders.Contains(other))
            {
                if (includeLocalPosition)
                {
                    Vector3 localPos = transform.InverseTransformPoint(other.transform.localPosition);
                    args.Add(localPos.x);
                    args.Add(localPos.y);
                    args.Add(localPos.z);
                }
                SendOscMessage(string.Format("{0}/stay", oscAddress), args.ToArray());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (sendTriggerExit && !ignoreColliders.Contains(other))
            {
                List<object> args = new List<object>();
                if (includeLocalPosition)
                {
                    Vector3 localPos = transform.InverseTransformPoint(other.transform.localPosition);
                    args.Add(localPos.x);
                    args.Add(localPos.y);
                    args.Add(localPos.z);
                }
                SendOscMessage(string.Format("{0}/exit", oscAddress), args.ToArray());
            }
        }


        // Research this...
        private Vector3 FindTriggerPoint(Collider other)
        {
            Vector3 localPos = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.transform.gameObject == other.gameObject)
                {
                    localPos = transform.InverseTransformPoint(hit.point);
                }
            }
            return localPos;
        }
    }
}