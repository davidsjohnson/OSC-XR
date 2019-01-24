using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCSpaceController : OSCTransmitterObjectBase
{

    [Header("On Trigger Enter Messages")]
    public bool sendTriggerEnter = true;
    public bool includeEnterLocation= false;

    [Header("On Trigger Stay Messages")]
    public bool sendTriggerStay = true;
    public bool includeLocalPosition = true;

    [Header("On Trigger Exit Messages")]
    public bool sendTriggerExit = true;
    public bool includeExitLocation = false;

    [Header("Colliders to Ignore")]
    public List<Collider> ignoreColliders;

    private void OnTriggerEnter(Collider other)
    {
        if (sendTriggerEnter && !ignoreColliders.Contains(other))
        {
            List<object> args = new List<object>();
            if (includeEnterLocation)
            {
                Vector3 localPos = FindTriggerPoint(other);
                args.Add(localPos.x);
                args.Add(localPos.y);
                args.Add(localPos.z);
            }
            SendOSCMessage(string.Format("{0}/{1}", oscAddress, "triggerEnter"), args.ToArray());
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (sendTriggerStay && !ignoreColliders.Contains(other))
        {
            List<object> args = new List<object>();
            if (includeLocalPosition)
            {
                args.Add(other.transform.localPosition.x);
                args.Add(other.transform.localPosition.y);
                args.Add(other.transform.localPosition.z);
            }
            SendOSCMessage(string.Format("{0}/{1}", oscAddress, "triggerStay"), args.ToArray());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (sendTriggerExit && !ignoreColliders.Contains(other))
        {
            List<object> args = new List<object>();
            if (includeExitLocation)
            {
                Vector3 localPos = FindTriggerPoint(other);
                args.Add(localPos.x);
                args.Add(localPos.y);
                args.Add(localPos.z);
            }
            SendOSCMessage(string.Format("{0}/{1}", oscAddress, "triggerExit"), args.ToArray());
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
