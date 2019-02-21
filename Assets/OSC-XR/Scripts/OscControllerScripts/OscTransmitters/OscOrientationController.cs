namespace OSCXR
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using VRTK;

    public class OscOrientationController : BaseOscController
    {
        private float equalityFidelity = .00001f;

        private Vector3 prevPos;
        private Vector3 prevRot;

        private bool wasMoving = false;

        private List<float> faceUpChecks = new List<float>(new float[6]);
        private List<float> faceRightChecks = new List<float>(new float[6]);
        private List<float> faceForwardChecks = new List<float>(new float[6]);

        private Vector3Int orientations = new Vector3Int();
        private Vector3Int prevOrientation = new Vector3Int();

        protected override void OnEnable()
        {
            base.OnEnable();

            OscAddress = string.IsNullOrEmpty(OscAddress) ? "/orientation" : OscAddress;
        }

        protected override void Start()
        {
            base.Start();

            UpdateOrientation();
            SendOscMessage(string.Format("{0}/faceup", OscAddress), orientations.y);
        }

        // Update is called once per frame
        private void Update()
        {
            bool localPosChanged = !VRTK_SharedMethods.Vector3ShallowCompare(prevPos, controlObject.transform.localPosition, equalityFidelity);
            bool localRotChanged = !VRTK_SharedMethods.Vector3ShallowCompare(prevRot, controlObject.transform.localEulerAngles, equalityFidelity);

            bool isMoving = localRotChanged || localPosChanged;

            if (!isMoving && wasMoving)  // object not moving or rotating after it was moving
            {
                UpdateOrientation();
                if (prevOrientation.y != orientations.y)
                {
                    SendOscMessage(string.Format("{0}/faceup", OscAddress), orientations.y);
                }
                
                wasMoving = false;
            }

            wasMoving = isMoving;
            prevPos = controlObject.transform.localPosition;
            prevRot = controlObject.transform.localEulerAngles;
        }

        protected void UpdateOrientation()
        {
            faceRightChecks[0] = Vector3.Angle(controlObject.transform.up, Vector3.right);
            faceRightChecks[1] = Vector3.Angle(controlObject.transform.forward, Vector3.right);
            faceRightChecks[2] = Vector3.Angle(controlObject.transform.right, Vector3.right);
            faceRightChecks[3] = Vector3.Angle(-controlObject.transform.forward, Vector3.right);
            faceRightChecks[4] = Vector3.Angle(-controlObject.transform.right, Vector3.right);
            faceRightChecks[5] = Vector3.Angle(-controlObject.transform.up, Vector3.right);

            faceUpChecks[0] = Vector3.Angle(controlObject.transform.up, Vector3.up);
            faceUpChecks[1] = Vector3.Angle(controlObject.transform.forward, Vector3.up);
            faceUpChecks[2] = Vector3.Angle(controlObject.transform.right, Vector3.up);
            faceUpChecks[3] = Vector3.Angle(-controlObject.transform.forward, Vector3.up);
            faceUpChecks[4] = Vector3.Angle(-controlObject.transform.right, Vector3.up);
            faceUpChecks[5] = Vector3.Angle(-controlObject.transform.up, Vector3.up);

            faceForwardChecks[0] = Vector3.Angle(controlObject.transform.up, Vector3.forward);
            faceForwardChecks[1] = Vector3.Angle(controlObject.transform.forward, Vector3.forward);
            faceForwardChecks[2] = Vector3.Angle(controlObject.transform.right, Vector3.forward);
            faceForwardChecks[3] = Vector3.Angle(-controlObject.transform.forward, Vector3.forward);
            faceForwardChecks[4] = Vector3.Angle(-controlObject.transform.right, Vector3.forward);
            faceForwardChecks[5] = Vector3.Angle(-controlObject.transform.up, Vector3.forward);

            prevOrientation = orientations;

            orientations.x = faceRightChecks.IndexOf(faceRightChecks.Min());
            orientations.y = faceUpChecks.IndexOf(faceUpChecks.Min());
            orientations.z = faceForwardChecks.IndexOf(faceForwardChecks.Min());
        }


    }
}