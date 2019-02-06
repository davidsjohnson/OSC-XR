namespace OSCXR
{
    using UnityEngine;
    using VRTK;

    public class OscTransformTransmitter : BaseOscTransmitter
    {
        [Header("OSC Transform Settings")]
        public bool sendLocalPosition;
        public bool sendWorldPosition;
        public float positionStepSize = 0.1f;
        public bool sendLocalScale;
        public bool sendWorldScale;
        public float scaleStepSize = 0.01f;
        public bool sendLocalRotation;
        public bool sendWorldRotation;
        public float rotationStepSize = 1f;



        private void Start()
        {
            // Set up name and address
            oscAddress = string.IsNullOrEmpty(oscAddress) ? "/transform" : oscAddress;
        }
    }
}