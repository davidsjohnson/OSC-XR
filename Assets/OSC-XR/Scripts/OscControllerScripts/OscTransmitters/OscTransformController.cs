namespace OSCXR
{
    using UnityEngine;
    using VRTK;

    public class OscTransformController : BaseOscController
    {
        [Header("OSC Transform Settings")]
        public bool sendLocalPosition;
        public bool sendWorldPosition;
        public float positionStepSize = 0.01f;
        public bool sendLocalScale;
        public bool sendWorldScale;
        public float scaleStepSize = 0.01f;
        public bool sendLocalRotation;
        public bool sendWorldRotation;
        public float rotationStepSize = 1f;

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

        protected override void OnEnable()
        {
            base.OnEnable();
            
            OscAddress = string.IsNullOrEmpty(OscAddress) ? "/transform" : OscAddress;      // Set up name and address
        }

        protected override void ControlRateUpdate()
        {
            base.ControlRateUpdate();

            CalculateTransforms();
            SendTransformData();
        }

        private void CalculateTransforms()
        {

            // Calculate new values
            var newPositionLocal = Utils.GetStepValue(controlObject.transform.localPosition, positionStepSize);
            var newPositionWorld = Utils.GetStepValue(controlObject.transform.position, positionStepSize);
            var newScaleLocal = Utils.GetStepValue(controlObject.transform.localScale, scaleStepSize);
            var newScaleWorld = Utils.GetStepValue(controlObject.transform.lossyScale, scaleStepSize);
            var newRotationLocal = Utils.GetStepValue(controlObject.transform.localEulerAngles, rotationStepSize);
            var newRotationWorld = Utils.GetStepValue(controlObject.transform.eulerAngles, rotationStepSize);

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

        private void SendTransformData()
        {
            if (sendLocalPosition && localPosChanged)
                SendOscMessage(string.Format("{0}/position/local", OscAddress),
                    positionLocal.x, positionLocal.y, positionLocal.z);

            if (sendWorldPosition && worldPosChanged)
                SendOscMessage(string.Format("{0}/position/world", OscAddress),
                    positionWorld.x, positionWorld.y, positionWorld.z);

            if (sendLocalScale && localScaleChanged)
                SendOscMessage(string.Format("{0}/scale/local", OscAddress),
                    scaleLocal.x, scaleLocal.y, scaleLocal.z);

            if (sendWorldScale && worldScaleChanged)
                SendOscMessage(string.Format("{0}/scale/world", OscAddress),
                    scaleWorld.x, scaleWorld.y, scaleWorld.z);

            if (sendLocalRotation && localRotationChanged)
                SendOscMessage(string.Format("{0}/rotation/local", OscAddress),
                    rotationLocal.x, rotationLocal.y, rotationLocal.z);

            if (sendWorldRotation && worldRotationChanged)
                SendOscMessage(string.Format("{0}/rotation/world", OscAddress),
                    rotationWorld.x, rotationWorld.y, rotationWorld.z);
        }
    }
}