namespace OSCXR
{
    using System.Collections;
    using UnityEngine;
    using VRTK;

    public class OscGyroController : BaseOscController
    {
        // Control Params
        public VRTK_InteractableObject controlObject = null;
        public float velocityStep = .001f;


        // Reactor Members
        private VRTK_ControllerEvents leftCtrlEvents;
        private VRTK_ControllerEvents rightCtrlEvents;

        private Rigidbody rb;

        Vector3 prevAngularVelocity = new Vector3();
        bool frozen = false;

        protected override void OnEnable()
        {
            base.OnEnable();

            OscAddress = string.IsNullOrEmpty(OscAddress) ? "/gyro" : OscAddress;
            controlObject = controlObject ?? GetComponent<VRTK_InteractableObject>();
        }

        private void Start()
        {
            rb = controlObject.GetComponent<Rigidbody>();
            rb.maxAngularVelocity = 7f;

            leftCtrlEvents = VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>();
            rightCtrlEvents = VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();

            leftCtrlEvents.TriggerReleased += LockObjectInPlace;
            rightCtrlEvents.TriggerReleased += LockObjectInPlace;

            GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += ObjectGrabbed;
            GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += ObjectUngrabbed;

            //StartCoroutine(UpdateOsc());
        }

        private void FixedUpdate()
        {
            TransmitVelocity();
        }

        private IEnumerator UpdateOsc()
        {
            while (true)
            {
                TransmitVelocity();
                yield return new WaitForSecondsRealtime(1.0f / 30.0f);
            }
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

        private void LockObjectInPlace(object sender, ControllerInteractionEventArgs e)
        {
            if (!frozen & controlObject.GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
                frozen = true;
                SendOscMessage(string.Format("{0}/frozen", OscAddress));
            }
        }

        private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
        {
            SendOscMessage(string.Format("{0}/grabbed", OscAddress));
            if (frozen)
            {
                rb.constraints = RigidbodyConstraints.None;
                frozen = false;
                SendOscMessage(string.Format("{0}/unfrozen", OscAddress));
            }
            
        }

        private void ObjectUngrabbed(object sender, InteractableObjectEventArgs e)
        {
            SendOscMessage(string.Format("{0}/ungrabbed", OscAddress));
        }
    }
}