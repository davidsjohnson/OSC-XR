namespace OSCXR
{
    using UnityEngine;
    using VRTK.Controllables;
    using VRTK.Controllables.PhysicsBased;

    public class OscPadController : BaseOscController
    {
        [Tooltip("Send Trigger message when button position passes specified threshold")]
        public bool sendPressedReleased;
        public float triggerThreshold = 0.5f;

        [Tooltip("Send velocity information with Trigger message")]
        public bool sendVelocity;

        //Reactor Interactable
        [Header("VRTK Controllable Object")]
        public VRTK_BaseControllable controlObject = null;


        // Reactor Private Members
        private float prevValue = 0f;
        private float currValue = 0f;
        private bool padPressed;
        private Rigidbody rb;

        protected override void OnEnable()
        {
            base.OnEnable();

            // Initialize Controller members
            oscAddress = string.IsNullOrEmpty(oscAddress) ? string.Format("/pad") : oscAddress;     // Set up name and address
            controlObject = controlObject ?? GetComponent<VRTK_BaseControllable>();                 // Set up control object
            rb = controlObject.GetComponent<Rigidbody>();                                           // Get Rigidbody from control object

            padPressed = false;                                                                     // Pad is not pressed by default
        }

        private void Update()
        {
            // Store Values to calculate simple velocity
            prevValue = currValue;
            currValue = controlObject.GetNormalizedValue();
            currValue = Utils.Approximately(0, currValue) ? 0f : // if close to 0 set to zero      (Should probably used Stepped Function instead)
                        Utils.Approximately(1, currValue) ? 1f : // if close to 1 set to one
                        Mathf.Round(currValue * 1000) / 1000f;   // otherwise round value

            // Simple Press Release check (should update to VRTK event based mechanism)
            if (!padPressed && currValue > triggerThreshold)
            {
                if (sendPressedReleased)
                {
                    if (sendVelocity)
                        SendOscMessage(string.Format("{0}/pressed", oscAddress), Mathf.Min(rb.velocity.magnitude, 1));
                    else
                        SendOscMessage(string.Format("{0}/pressed", oscAddress));
                }
                padPressed = true;
            }
            else if (padPressed && currValue < triggerThreshold)
            {
                if (sendPressedReleased)
                {
                    if (sendVelocity)
                        SendOscMessage(string.Format("{0}/released", oscAddress), Mathf.Min(rb.velocity.magnitude, 1));
                    else
                        SendOscMessage(string.Format("{0}/released", oscAddress));
                }
                padPressed = false;
            }
        }

        // Hack to update VRTK slider maximum length when scaling the OSC Slider object
        private float previousYScale = 1f;
        void OnDrawGizmosSelected()
        {
            if (transform.localScale.y != previousYScale && transform.localScale.y != 0)
            {
                float factor = transform.localScale.y / previousYScale;
                float newmax = controlObject.GetComponent<VRTK_PhysicsPusher>().pressedDistance * factor;
                controlObject.GetComponent<VRTK_PhysicsPusher>().pressedDistance = newmax;

                previousYScale = transform.localScale.y;
            }
        }
    }
}