namespace OSCXR
{
    using UnityEngine;
    using UnityEngine.UI;
    using VRTK;
    using VRTK.Controllables;
    using VRTK.Controllables.ArtificialBased;

    public class OscSliderController : BaseOscController
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

        [Header("VRTK Controllable Object")]
        public VRTK_BaseControllable controlObject;

        // Reactor Private Members
        public Image sliderImage;
        private float currentValue = 0;

        protected override void OnEnable()
        {
            base.OnEnable();

            oscAddress = string.IsNullOrEmpty(oscAddress) ? string.Format("/slider") : oscAddress;      // Set up name and address
            controlObject = controlObject ?? GetComponent<VRTK_BaseControllable>();                     // Set up Control Object

            ManageListeners(true);
        }

        private void Start()
        {

            //Update VRTK Slider Settings 
            controlObject.GetComponent<VRTK_ArtificialSlider>().stepValueRange = sliderRange;
            controlObject.GetComponent<VRTK_ArtificialSlider>().stepSize = stepSize;

            float startPos = Utils.MapValue(defaultPosition, sliderRange, new Limits2D(0f, 1f));
            controlObject.GetComponent<VRTK_ArtificialSlider>().SetPositionTarget(startPos, 100f);
        }

        protected virtual void ManageListeners(bool state)
        {
            if (state)
            { controlObject.ValueChanged += ValueChanged; }
            else
            { controlObject.ValueChanged -= ValueChanged; }
        }

        protected virtual void ValueChanged(object sender, ControllableEventArgs e)
        {
            currentValue = e.value;
            SendOscMessage(string.Format("{0}/value", oscAddress), currentValue);

            // Update Progress Slider
            sliderImage.fillAmount = e.normalizedValue;
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
                controlObject.GetComponent<VRTK_ArtificialSlider>().maximumLength = newmax;

                previousZScale = transform.lossyScale.z;
            }
        }
    }
}