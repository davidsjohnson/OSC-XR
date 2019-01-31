namespace OSCXR
{
    using System.Collections;
    using UnityEngine;
    using VRTK;
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    [RequireComponent(typeof(VRTK_InteractableObject))]
    public class ObjectRegistration : MonoBehaviour
    {

        public float positionDelay = 10f;

        private VRTK_InteractableObject interactable;
        private string filename;

        private void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
        {
            //Starting Registration
            Debug.Log("USed");
            StartCoroutine(StartRegistration(positionDelay));
        }

        IEnumerator StartRegistration(float delay = 6f)
        {
            Debug.Log(string.Format("Starting Registration in {0} seconds", delay));
            yield return new WaitForSeconds(delay);
            PositionObject();
            Debug.Log("Object Positioned");
        }

        private void Awake()
        {
            filename = string.Format("{0}/{1}.dat", Application.persistentDataPath, transform.parent.gameObject.name);

            //Setup Required options for interactable
            interactable = GetComponent<VRTK_InteractableObject>();
            interactable.InteractableObjectUsed += InteractableObjectUsed;
            interactable.isUsable = true;
            interactable.holdButtonToUse = false;
            interactable.pointerActivatesUseAction = true;
            interactable.useOnlyIfGrabbed = false;

            LoadPositionState();
        }

        protected void PositionObject()
        {
            //1.Get motion controller positions(x, z); we can ignore y for now
            Transform leftController = VRTK_DeviceFinder.GetControllerLeftHand().transform;
            Transform rightController = VRTK_DeviceFinder.GetControllerRightHand().transform;

            if (leftController != null && rightController != null)
            {
                //2.Calculate center point(x, z) point
                Vector3 centerPos = leftController.transform.position + rightController.transform.position;
                centerPos /= 2;

                //UnityEngine.Debug.Log("Left" + leftController.transform.position);
                //UnityEngine.Debug.Log("Right" + rightController.transform.position);
                //UnityEngine.Debug.Log("Center" + centerPos);

                //3.Calculate(x, z) point X inches away from center point
                Vector2 rightPt = new Vector2(rightController.transform.position.x, rightController.transform.position.z);
                Vector2 centerPt = new Vector2(centerPos.x, centerPos.z);
                float angle = Vector2.SignedAngle(centerPt - rightPt, Vector2.right);

                //4.Update Theremin position to this point
                Vector3 pos = transform.parent.transform.localPosition;
                pos.x = centerPt.x;
                pos.z = centerPt.y;
                transform.parent.transform.localPosition = pos;

                Vector3 angles = transform.localEulerAngles;
                angles.y = 180 + angle;
                transform.parent.transform.localEulerAngles = angles;

                //Save State
                SavePositionState();
            }
        }

        private void SavePositionState()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filename, FileMode.OpenOrCreate);

            PositionData positionData = new PositionData();
            positionData.Position = transform.parent.transform.position;
            positionData.Rotation = transform.parent.transform.eulerAngles;
            positionData.Scale = transform.parent.transform.localScale;

            bf.Serialize(file, positionData);
            file.Close();
        }

        private void LoadPositionState()
        {
            print(filename);
            if (File.Exists(filename))
            {
                print("here");
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(filename, FileMode.Open);
                PositionData positionData = (PositionData)bf.Deserialize(file);
                file.Close();

                transform.parent.position = positionData.Position;
                transform.parent.eulerAngles = positionData.Rotation;
                transform.parent.localScale = positionData.Scale;
            }
        }
    }

    [Serializable]
    class PositionData
    {
        private float[] position = new float[3];
        private float[] rotation = new float[3];
        private float[] scale = new float[3];

        public Vector3 Position
        {
            set
            {
                position[0] = value.x;
                position[1] = value.y;
                position[2] = value.z;
            }

            get
            {
                return new Vector3(position[0], position[1], position[2]);
            }
        }

        public Vector3 Rotation
        {
            set
            {
                rotation[0] = value.x;
                rotation[1] = value.y;
                rotation[2] = value.z;
            }

            get
            {
                return new Vector3(rotation[0], rotation[1], rotation[2]);
            }
        }

        public Vector3 Scale
        {
            set
            {
                scale[0] = value.x;
                scale[1] = value.y;
                scale[2] = value.z;
            }

            get
            {
                return new Vector3(scale[0], scale[1], scale[2]);
            }
        }
    }
}