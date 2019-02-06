namespace OSCXR
{
    using UnityEngine;
    using VRTK;

    public class OscGrid3dReactor : MonoBehaviour
    {

        public OscGrid3dTransmitter spaceController;

        private float equalityFidelity = 0.001f;
        private Vector3 prevPosition;
        private Vector3 prevStepValues;

        private Limits2D constrainLimit = new Limits2D(-0.5f, 0.5f);

        private void Start()
        {
            prevPosition = transform.localPosition;
        }

        private void FixedUpdate()
        {
            if (spaceController.sendContinously)
            {
                Vector3 stepValues = CalcOSCValues(transform.localPosition);
                spaceController.SendOSCMessage(string.Format("{0}/values", spaceController.oscAddress), stepValues.x, stepValues.y, stepValues.z);
            }
        }

        private void Update()
        {
            Vector3 stepValues = CalcOSCValues(transform.localPosition);
            bool stepsChanged = !VRTK_SharedMethods.Vector3ShallowCompare(prevStepValues, stepValues, equalityFidelity);
            if (stepsChanged)
            {
                spaceController.SendOSCMessage(string.Format("{0}/values", spaceController.oscAddress), stepValues.x, stepValues.y, stepValues.z);
            }
            prevStepValues = stepValues;
        }

        private Vector3 CalcOSCValues(Vector3 pos)
        {
            Vector3 values = new Vector3();

            values.x = Utils.GetStepValue(Utils.MapValue(pos.x, constrainLimit, spaceController.xValueRange), spaceController.xStep);
            values.y = Utils.GetStepValue(Utils.MapValue(pos.y, constrainLimit, spaceController.yValueRange), spaceController.yStep);
            values.z = Utils.GetStepValue(Utils.MapValue(pos.z, constrainLimit, spaceController.zValueRange), spaceController.zStep);

            return values;
        }
    }
}