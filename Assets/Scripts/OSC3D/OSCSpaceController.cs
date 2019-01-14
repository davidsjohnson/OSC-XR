using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class OSCSpaceController : OSCTransmitterObjectBase
{
    [Header("Controller Settings")]

    [Tooltip("")]
    public Limits2D xValueRange = new Limits2D(0f, 1.0f);
    public float xStep = 0.01f;

    [Tooltip("")]
    public Limits2D yValueRange = new Limits2D(0f, 1.0f);
    public float yStep = 0.01f;
    [Tooltip("")]
    public Limits2D zValueRange = new Limits2D(0f, 1.0f);
    public float zStep = 0.01f;

    private static int counter;
    private void Start()
    {
        counter++;
        // Set up name and address
        controllerName = string.IsNullOrEmpty(controllerName) ? "space" : controllerName;
        oscAddress = string.IsNullOrEmpty(oscAddress) ? string.Format("/{0}/{1}", controllerName, counter) : oscAddress;
    }
}
