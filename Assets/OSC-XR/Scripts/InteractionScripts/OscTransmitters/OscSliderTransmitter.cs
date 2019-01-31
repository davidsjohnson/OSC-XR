namespace OSCXR
{
    using UnityEngine;
    using VRTK;
    using VRTK.Controllables.ArtificialBased;

    public class OscSliderTransmitter : BaseOscTransmitter
    {
        // #### Inspector Items ####
        // #########################

        [Header("Slider Config")]

        [Tooltip("The minimum and the maximum step values for the slider (Will override VRTK Slider settings).")]
        public Limits2D sliderRange = new Limits2D(0f, 1f);

        [Tooltip("The increments the slider value will change in between the `Slider Range` (Will override VRTK Slider settings).")]
        public float stepSize = 0.01f;

        [Tooltip("The value that the slider should start at")]
        public float defaultPosition = 0.0f;

        public VRTK_ArtificialSlider controllable;

        private void Start()
        {
            // Set up name and address
            oscAddress = string.IsNullOrEmpty(oscAddress) ? "/slider" : oscAddress;

            //Update VRTK Slider Settings 
            controllable.stepValueRange = sliderRange;
            controllable.stepSize = stepSize;

            float startPos = Utils.MapValue(defaultPosition, sliderRange, new Limits2D(0f, 1f));
            controllable.SetPositionTarget(startPos, 100f);
        }


        // Hack to update VRTK slider maximum length when scaling the OSC Slider object
        private float previousZScale = 1f;
        private float initialLength = .9f;
        void OnDrawGizmosSelected()
        {
            if (transform.lossyScale.z != previousZScale && transform.lossyScale.z != 0)
            {
                //float factor = transform.lossyScale.z / previousZScale;
                float newmax = initialLength * transform.lossyScale.z;
                controllable.maximumLength = newmax;

                previousZScale = transform.lossyScale.z;
            }
        }
    }
}