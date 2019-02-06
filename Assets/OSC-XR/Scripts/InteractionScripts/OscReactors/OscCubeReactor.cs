namespace OSCXR
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using VRTK;

    public class OscCubeReactor : OscTransformReactor
    {

        public OscCubeTransmitter cubeTransmitter;

        private bool wasMoving = false;
        private bool isMoving = false;

        private List<float> faceUpChecks = new List<float>(new float[6]);
        private List<float> faceRightChecks = new List<float>(new float[6]);
        private List<float> faceForwardChecks = new List<float>(new float[6]);

        private Vector3Int faceOrientations = new Vector3Int();
        private Vector3Int newOrientations = new Vector3Int();

        protected override void Start()
        {
            base.Start();

            faceOrientations = FindFaceOrientation();
            cubeTransmitter.SendOSCMessage(string.Format("{0}/faceup", cubeTransmitter.oscAddress), faceOrientations.y);
        }

        // Update is called once per frame
        protected override void Update()
        {
            isMoving = localRotationChanged || localPosChanged;

            if (!isMoving && wasMoving)  // object not moving or rotating after it was moving
            {
                var newOrientations = FindFaceOrientation();
                if (newOrientations.y != faceOrientations.y)
                {
                    faceOrientations = newOrientations;
                    cubeTransmitter.SendOSCMessage(string.Format("{0}/faceup", cubeTransmitter.oscAddress), faceOrientations.y);
                }
                
                wasMoving = false;
            }
            wasMoving = isMoving;

            CalculateTransforms();
            SendTransformData();
        }

        //protected override void CalculateTransforms()
        //{
        //    // Calculate new values
        //    var newPositionLocal = Utils.GetStepValue(interactableObject.transform.localPosition, transmitter.positionStepSize);
        //    var newPositionWorld = Utils.GetStepValue(interactableObject.transform.position, transmitter.positionStepSize);
        //    var newScaleLocal = Utils.GetStepValue(interactableObject.transform.localScale, transmitter.scaleStepSize);
        //    var newScaleWorld = Utils.GetStepValue(interactableObject.transform.lossyScale, transmitter.scaleStepSize);
        //    var newRotationLocal = Utils.GetStepValue(interactableObject.transform.localEulerAngles, transmitter.rotationStepSize);
        //    var newRotationWorld = Utils.GetStepValue(interactableObject.transform.eulerAngles, transmitter.rotationStepSize);


        //    //update changed flags
        //    localPosChanged = !VRTK_SharedMethods.Vector3ShallowCompare(positionLocal, newPositionLocal, equalityFidelity);
        //    worldPosChanged = !VRTK_SharedMethods.Vector3ShallowCompare(positionWorld, newPositionWorld, equalityFidelity);
        //    localScaleChanged = !VRTK_SharedMethods.Vector3ShallowCompare(scaleLocal, newScaleLocal, equalityFidelity);
        //    worldScaleChanged = !VRTK_SharedMethods.Vector3ShallowCompare(scaleWorld, newScaleWorld, equalityFidelity);
        //    localRotationChanged = !VRTK_SharedMethods.Vector3ShallowCompare(rotationLocal, newRotationLocal, equalityFidelity);
        //    worldRotationChanged = !VRTK_SharedMethods.Vector3ShallowCompare(rotationWorld, newRotationWorld, equalityFidelity);

        //    // set current values
        //    positionLocal = newPositionLocal;
        //    positionWorld = newPositionWorld;
        //    scaleLocal = newScaleLocal;
        //    scaleWorld = newScaleWorld;
        //    rotationLocal = newRotationLocal;
        //    rotationWorld = newRotationWorld; 
        //}

        protected Vector3Int FindFaceOrientation()
        {
            faceRightChecks[0] = Vector3.Angle(transform.up, Vector3.right);
            faceRightChecks[1] = Vector3.Angle(transform.forward, Vector3.right);
            faceRightChecks[2] = Vector3.Angle(transform.right, Vector3.right);
            faceRightChecks[3] = Vector3.Angle(-transform.forward, Vector3.right);
            faceRightChecks[4] = Vector3.Angle(-transform.right, Vector3.right);
            faceRightChecks[5] = Vector3.Angle(-transform.up, Vector3.right);

            faceUpChecks[0] = Vector3.Angle(transform.up, Vector3.up);
            faceUpChecks[1] = Vector3.Angle(transform.forward, Vector3.up);
            faceUpChecks[2] = Vector3.Angle(transform.right, Vector3.up);
            faceUpChecks[3] = Vector3.Angle(-transform.forward, Vector3.up);
            faceUpChecks[4] = Vector3.Angle(-transform.right, Vector3.up);
            faceUpChecks[5] = Vector3.Angle(-transform.up, Vector3.up);

            faceForwardChecks[0] = Vector3.Angle(transform.up, Vector3.forward);
            faceForwardChecks[1] = Vector3.Angle(transform.forward, Vector3.forward);
            faceForwardChecks[2] = Vector3.Angle(transform.right, Vector3.forward);
            faceForwardChecks[3] = Vector3.Angle(-transform.forward, Vector3.forward);
            faceForwardChecks[4] = Vector3.Angle(-transform.right, Vector3.forward);
            faceForwardChecks[5] = Vector3.Angle(-transform.up, Vector3.forward);

            return new Vector3Int(faceRightChecks.IndexOf(faceRightChecks.Min()), 
                              faceUpChecks.IndexOf(faceUpChecks.Min()),
                              faceForwardChecks.IndexOf(faceForwardChecks.Min()));
        }
    }
}