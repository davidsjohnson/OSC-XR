using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class OSC3DGridController : OSCTransmitterObjectBase
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

    private void Start()
    {
        // Set up name and address
        oscAddress = string.IsNullOrEmpty(oscAddress) ? string.Format("/grid3d") : oscAddress;
    }
}
