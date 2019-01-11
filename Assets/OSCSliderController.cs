using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.PhysicsBased;

public class OSCSliderController : OSCTransmitterObjectBase
{
    private static int counter = 0;

    // #### Inspector Items ####
    // #########################

    [Header("Slider Config")]

    [Tooltip("The minimum and the maximum step values for the slider (Will override VRTK Slider settings).")]
    public Limits2D sliderRange = new Limits2D(0f, 1f);

    [Tooltip("The increments the slider value will change in between the `Slider Range` (Will override VRTK Slider settings).")]
    public float stepSize = 0.01f;

    public VRTK_PhysicsSlider controllable;

    protected void Start()
    {
        counter++;

        controllable.stepValueRange = sliderRange;
        controllable.stepSize = stepSize;

        if (oscAddress == null) oscAddress = (controllerName != null) ? string.Format("{0}/", controllerName) : 
            string.Format("/slider/{0}/", counter);
    }
}
