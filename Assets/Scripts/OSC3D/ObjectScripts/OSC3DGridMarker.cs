using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSC3DGridMarker : MonoBehaviour
{
    public Transform followTransform;

    [Header("Constrain Axis")]
    public bool x;
    public bool y;
    public bool z;

    private void Update()
    {
        Vector3 newPos = followTransform.localPosition;
        if (x) newPos.x = transform.localPosition.x;
        else if (y) newPos.y = transform.localPosition.y;
        else if (z) newPos.z = transform.localPosition.z;

        transform.localPosition = newPos;
    }
}
