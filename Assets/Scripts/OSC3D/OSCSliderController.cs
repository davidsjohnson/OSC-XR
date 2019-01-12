using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.ArtificialBased;

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

    public VRTK_ArtificialSlider controllable;

    private void Start()
    {
        counter++;

        // Set up name and address
        controllerName = string.IsNullOrEmpty(controllerName) ? "slider" : controllerName;
        oscAddress = string.IsNullOrEmpty(oscAddress) ? string.Format("/{0}/{1}/", controllerName, counter) : oscAddress;

        //Update VRTK Slider Settings 
        controllable.stepValueRange = sliderRange;
        controllable.stepSize = stepSize;
    }


    // Hack to update VRTK slider maximum length when scaling the OSC Slider object
    private float previousZScale = 1f;
    void OnDrawGizmosSelected()
    {
        if (transform.localScale.z != previousZScale && transform.localScale.z != 0)
        {
            float factor = transform.localScale.z / previousZScale;
            float newmax = controllable.maximumLength * factor;
            controllable.maximumLength = newmax;

            previousZScale = transform.localScale.z;
        }
    }
}
