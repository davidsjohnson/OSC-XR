namespace OSCXR
{
    using System.Collections;
    using UnityEngine;
    using VRTK;

    public class OscGyroController : BaseOscController
    {
        // Control Params
        public float velocityStep = .001f;
        
        // Reactor Members
        private VRTK_ControllerEvents leftCtrlEvents;
        private VRTK_ControllerEvents rightCtrlEvents;

        private Rigidbody rb;

        Vector3 prevAngularVelocity = new Vector3();

        protected override void OnEnable()
        {
            base.OnEnable();

            OscAddress = string.IsNullOrEmpty(OscAddress) ? "/gyro" : OscAddress;
        }

        protected override void Start()
        {
            base.Start();

            rb = controlObject.GetComponent<Rigidbody>();
            rb.maxAngularVelocity = 7f;

            leftCtrlEvents = VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>();
            rightCtrlEvents = VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();

        }

        protected override void ControlRateUpdate()
        {
            base.ControlRateUpdate();

            TransmitVelocity();
        }


        private void TransmitVelocity()
        {
            Vector3 localAngularVelocity = controlObject.transform.InverseTransformDirection(rb.angularVelocity / rb.maxAngularVelocity);
            Vector3 steppedVelocity = Utils.GetStepValue(localAngularVelocity, velocityStep);
            if (!VRTK_SharedMethods.Vector3ShallowCompare(prevAngularVelocity, steppedVelocity, velocityStep))
            {
                SendOscMessage(string.Format("{0}/velocity", OscAddress),
                                               steppedVelocity.x, steppedVelocity.y, steppedVelocity.z);
            }
            prevAngularVelocity = steppedVelocity;
        }
    }
}