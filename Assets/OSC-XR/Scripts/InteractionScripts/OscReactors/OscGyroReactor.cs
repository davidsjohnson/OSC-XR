namespace OSCXR
{
    using System.Collections;
    using UnityEngine;
    using VRTK;

    public class OscGyroReactor : MonoBehaviour
    {
        public OscGyroTransmitter gyroTransmitter;

        private VRTK_ControllerEvents leftCtrlEvents;
        private VRTK_ControllerEvents rightCtrlEvents;

        private Rigidbody rb;

        private float equalityThresh = .0001f;
        Vector3 prevAngularVelocity = new Vector3();

        bool frozen = false;
        bool grabbed = false;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.maxAngularVelocity = 7f;

            leftCtrlEvents = VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>();
            rightCtrlEvents = VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();

            leftCtrlEvents.TriggerReleased += LockObjectInPlace;
            rightCtrlEvents.TriggerReleased += LockObjectInPlace;

            GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += ObjectGrabbed;
            GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += ObjectUngrabbed;

            StartCoroutine(UpdateOsc());
        }

        private void Update()
        {
            TransmitOsc();
        }

        private IEnumerator UpdateOsc()
        {
            while (true)
            {
                TransmitOsc();
                yield return new WaitForSecondsRealtime(1.0f / 30.0f); 
            }
        }

        private void TransmitOsc()
        {
            Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity / rb.maxAngularVelocity);
            Vector3 steppedVelocity = Utils.GetStepValue(localAngularVelocity, gyroTransmitter.velocityStep);
            if (!VRTK_SharedMethods.Vector3ShallowCompare(prevAngularVelocity, steppedVelocity, gyroTransmitter.velocityStep))
            {
                gyroTransmitter.SendOSCMessage(string.Format("{0}/velocity", gyroTransmitter.OscAddress),
                                               steppedVelocity.x, steppedVelocity.y, steppedVelocity.z);
            }
            prevAngularVelocity = steppedVelocity;
        }

        private void LockObjectInPlace(object sender, ControllerInteractionEventArgs e)
        { 
            if (!frozen & GetComponent<VRTK_InteractableObject>().IsGrabbed())
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
                frozen = true;
                gyroTransmitter.SendOSCMessage(string.Format("{0}/frozen", gyroTransmitter.OscAddress));
            }
        }

        private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
        {
            if (frozen)
            {
                rb.constraints = RigidbodyConstraints.None;
                frozen = false;
                gyroTransmitter.SendOSCMessage(string.Format("{0}/unfrozen", gyroTransmitter.OscAddress));
            }
            else
            {
                gyroTransmitter.SendOSCMessage(string.Format("{0}/grabbed", gyroTransmitter.OscAddress));
            }
        }

        private void ObjectUngrabbed(object sender, InteractableObjectEventArgs e)
        {
            gyroTransmitter.SendOSCMessage(string.Format("{0}/ungrabbed", gyroTransmitter.OscAddress));
        }

    }
}