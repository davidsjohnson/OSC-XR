using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class OSCSpaceReactor : MonoBehaviour
{

    public OSCSpaceController spaceController;

    private float equalityFidelity = 0.001f;
    private Vector3 prevPosition;

    private Limits2D constrainLimit = new Limits2D(-0.5f, 0.5f);

    private void Start()
    {
        prevPosition = transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (spaceController.sendContinously)
        {
            Vector3 pos = transform.localPosition;
            spaceController.SendOSCMessage(spaceController.oscAddress, CalcOSCValues(pos));
        }
    }

    private void Update()
    {
        ConstrainToParent();

        Vector3 pos = transform.localPosition;
        bool localPosChanged = !VRTK_SharedMethods.Vector3ShallowCompare(prevPosition, pos, equalityFidelity);
        if (localPosChanged)
        {
            spaceController.SendOSCMessage(spaceController.oscAddress, CalcOSCValues(pos));
        }

        prevPosition = pos;
    }

    private object[] CalcOSCValues(Vector3 pos)
    {
        object[] values = new object[3];

        values[0] = MapValue(pos.x, constrainLimit, spaceController.xValueRange);
        values[1] = MapValue(pos.x, constrainLimit, spaceController.yValueRange);
        values[2] = MapValue(pos.x, constrainLimit, spaceController.zValueRange);

        return values;
    }


    private void ConstrainToParent()
    {
        //Constrain Object to parent
        Vector3 newPos = transform.localPosition;

        newPos.x += Random.Range(-0.02f, 0.02f);
        newPos.y += Random.Range(-0.02f, 0.02f);
        newPos.z += Random.Range(-0.02f, 0.02f);

        newPos.x = Mathf.Min(Mathf.Max(newPos.x, constrainLimit.minimum), constrainLimit.maximum);
        newPos.y = Mathf.Min(Mathf.Max(newPos.y, constrainLimit.minimum), constrainLimit.maximum);
        newPos.z = Mathf.Min(Mathf.Max(newPos.z, constrainLimit.minimum), constrainLimit.maximum);

        transform.localPosition = newPos;
    }

    private float MapValue(float value, Limits2D inRange, Limits2D outRange)
    {
        return outRange.minimum + (outRange.maximum - outRange.minimum) * ((value - inRange.minimum) / (inRange.maximum - inRange.minimum));
    }

}
