namespace OSCXR
{
    using UnityEngine;
    using VRTK;

    public class OscGrid3dControllerd : BaseOscController
    {
        [Header("Controller Settings")]

        [Tooltip("")]
        public Limits2D xValueRange = new Limits2D(0f, 1.0f);
        public float xStep = 0.01f;

        [Tooltip("")]
        public Limits2D yValueRange = new Limits2D(0f, 1.0f);
        public float yStep = 0.01f;
        [Tooltip("")]
        public Limits2D zValueRange = new Limits2D(0f, 1.0f);
        public float zStep = 0.01f;

        //Reactor Interactable
        [Header("VRTK Interactable Control Object")]
        public VRTK_InteractableObject controlObject = null;

        // Reactor Params
        private float equalityFidelity = 0.001f;

        private Vector3 steppedOscValues;   // Storing as a member so we don't have to create and destroy a new object every function call
        private Vector3 prevStepValues;

        private readonly Limits2D constrainLimit = new Limits2D(-0.5f, 0.5f);

        private void OnEnable()
        {
            oscAddress = string.IsNullOrEmpty(oscAddress) ? string.Format("/grid3d") : oscAddress;      // Set up name and address
            controlObject = controlObject ?? GetComponent<VRTK_InteractableObject>();                   // Set up Control Object
        }

        private void Start()
        {
            CalcOSCValues();
            prevStepValues = steppedOscValues;
        }

        private void FixedUpdate()
        {
            CalcOSCValues();
            if (!VRTK_SharedMethods.Vector3ShallowCompare(prevStepValues, steppedOscValues, equalityFidelity))
            {
                SendOscMessage(string.Format("{0}/values", oscAddress), steppedOscValues.x, steppedOscValues.y, steppedOscValues.z);
            }
            prevStepValues = steppedOscValues;
        }

        private void CalcOSCValues()
        {
            steppedOscValues.x = Utils.GetStepValue(Utils.MapValue(controlObject.transform.localPosition.x, constrainLimit, xValueRange), xStep);
            steppedOscValues.y = Utils.GetStepValue(Utils.MapValue(controlObject.transform.localPosition.y, constrainLimit, yValueRange), yStep);
            steppedOscValues.z = Utils.GetStepValue(Utils.MapValue(controlObject.transform.localPosition.z, constrainLimit, zValueRange), zStep);
        }
    }
}