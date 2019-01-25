using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCSpaceReactor : MonoBehaviour
{
    public OSCSpaceController controller;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");

        if (controller.sendTriggerEnter && !controller.ignoreColliders.Contains(other))
        {
            List<object> args = new List<object>();
            if (controller.includeEnterLocation)
            {
                Vector3 localPos = FindTriggerPoint(other);
                args.Add(localPos.x);
                args.Add(localPos.y);
                args.Add(localPos.z);
            }
            controller.SendOSCMessage(string.Format("{0}/{1}", controller.oscAddress, "triggerEnter"), args.ToArray());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        List<object> args = new List<object>();
        if (controller.sendTriggerStay && !controller.ignoreColliders.Contains(other))
        {
            if (controller.includeLocalPosition)
            {
                Vector3 localPos = transform.InverseTransformPoint(other.transform.localPosition);
                args.Add(localPos.x);
                args.Add(localPos.y);
                args.Add(localPos.z);
            }
            controller.SendOSCMessage(string.Format("{0}/{1}", controller.oscAddress, "triggerStay"), args.ToArray());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit");

        if (controller.sendTriggerExit && !controller.ignoreColliders.Contains(other))
        {
            List<object> args = new List<object>();
            if (controller.includeExitLocation)
            {
                Vector3 localPos = FindTriggerPoint(other);
                args.Add(localPos.x);
                args.Add(localPos.y);
                args.Add(localPos.z);
            }
            controller.SendOSCMessage(string.Format("{0}/{1}", controller.oscAddress, "triggerExit"), args.ToArray());
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
