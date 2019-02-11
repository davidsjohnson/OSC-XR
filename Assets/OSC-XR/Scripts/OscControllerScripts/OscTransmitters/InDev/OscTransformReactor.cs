namespace OSCXR
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK;

    public class OscTransformReactor : MonoBehaviour
    {
        public OscTransformTransmitter transmitter;

        [Header("VRTK Interactable Config")]
        public VRTK_InteractableObject interactableObject;

        protected Vector3 positionLocal = new Vector3();
        protected Vector3 positionWorld = new Vector3();
        protected Vector3 scaleLocal = new Vector3();
        protected Vector3 scaleWorld = new Vector3();
        protected Vector3 rotationLocal = new Vector3();
        protected Vector3 rotationWorld = new Vector3();

        protected float equalityFidelity = 0.001f;

        protected bool localPosChanged;
        protected bool worldPosChanged;
        protected bool localScaleChanged;
        protected bool worldScaleChanged;
        protected bool localRotationChanged;
        protected bool worldRotationChanged;

        protected virtual void Start()
        {
            CalculateTransforms();
        }

        protected virtual void Update()
        {
            CalculateTransforms();
            SendTransformData();
        }

        protected virtual void SendTransformData()
        {
            if (transmitter.sendLocalPosition && localPosChanged)
                transmitter.SendOSCMessage(string.Format("{0}/position/local", transmitter.oscAddress),
                    positionLocal.x, positionLocal.y, positionLocal.z);

            if (transmitter.sendWorldPosition && worldPosChanged)
                transmitter.SendOSCMessage(string.Format("{0}/position/world", transmitter.oscAddress),
                    positionWorld.x, positionWorld.y, positionWorld.z);

            if (transmitter.sendLocalScale && localScaleChanged)
                transmitter.SendOSCMessage(string.Format("{0}/scale/local", transmitter.oscAddress),
                    scaleLocal.x, scaleLocal.y, scaleLocal.z);

            if (transmitter.sendWorldScale && worldScaleChanged)
                transmitter.SendOSCMessage(string.Format("{0}/scale/world", transmitter.oscAddress),
                    scaleWorld.x, scaleWorld.y, scaleWorld.z);

            if (transmitter.sendLocalRotation && localRotationChanged)
                transmitter.SendOSCMessage(string.Format("{0}/rotation/local", transmitter.oscAddress),
                    rotationLocal.x, rotationLocal.y, rotationLocal.z);

            if (transmitter.sendWorldRotation && worldRotationChanged)
                transmitter.SendOSCMessage(string.Format("{0}/rotation/world", transmitter.oscAddress),
                    rotationWorld.x, rotationWorld.y, rotationWorld.z);
        }

        protected virtual void CalculateTransforms()
        {
            
            // Calculate new values
            var newPositionLocal = Utils.GetStepValue(interactableObject.transform.localPosition, transmitter.positionStepSize);
            var newPositionWorld = Utils.GetStepValue(interactableObject.transform.position, transmitter.positionStepSize);
            var newScaleLocal = Utils.GetStepValue(interactableObject.transform.localScale, transmitter.scaleStepSize);
            var newScaleWorld = Utils.GetStepValue(interactableObject.transform.lossyScale, transmitter.scaleStepSize);
            var newRotationLocal = Utils.GetStepValue(interactableObject.transform.localEulerAngles, transmitter.rotationStepSize);
            var newRotationWorld = Utils.GetStepValue(interactableObject.transform.eulerAngles, transmitter.rotationStepSize);

            //update changed flags
            localPosChanged = !VRTK_SharedMethods.Vector3ShallowCompare(positionLocal, newPositionLocal, equalityFidelity);
            worldPosChanged = !VRTK_SharedMethods.Vector3ShallowCompare(positionWorld, newPositionWorld, equalityFidelity);
            localScaleChanged = !VRTK_SharedMethods.Vector3ShallowCompare(scaleLocal, newScaleLocal, equalityFidelity);
            worldScaleChanged = !VRTK_SharedMethods.Vector3ShallowCompare(scaleWorld, newScaleWorld, equalityFidelity);
            localRotationChanged = !VRTK_SharedMethods.Vector3ShallowCompare(rotationLocal, newRotationLocal, equalityFidelity);
            worldRotationChanged = !VRTK_SharedMethods.Vector3ShallowCompare(rotationWorld, newRotationWorld, equalityFidelity);

            // set current values
            positionLocal = newPositionLocal;
            positionWorld = newPositionWorld;
            scaleLocal = newScaleLocal;
            scaleWorld = newScaleWorld;
            rotationLocal = newRotationLocal;
            rotationWorld = newRotationWorld;
        }
    }
}