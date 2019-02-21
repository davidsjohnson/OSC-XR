namespace OSCXR
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK;

    public class OscGrabFreezeController : BaseOscController
    {
        private Rigidbody rb;

        private VRTK_ControllerEvents leftCtrlEvents;
        private VRTK_ControllerEvents rightCtrlEvents;

        private VRTK_InteractableObject interactable;

        bool frozen = false;

        protected override void OnEnable()
        {
            base.OnEnable();

            OscAddress = string.IsNullOrEmpty(OscAddress) ? "/freeze" : OscAddress;
        }

        protected override void Start()
        {
            base.Start();

            interactable = controlObject.GetComponent<VRTK_InteractableObject>();

            rb = controlObject.GetComponent<Rigidbody>();

            leftCtrlEvents = VRTK.VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>();
            rightCtrlEvents = VRTK.VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();

            leftCtrlEvents.TriggerReleased += LockObjectInPlace;
            rightCtrlEvents.TriggerReleased += LockObjectInPlace;

            GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += ObjectGrabbed;
            GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += ObjectUngrabbed;
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