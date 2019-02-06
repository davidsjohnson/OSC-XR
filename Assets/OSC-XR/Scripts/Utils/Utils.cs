namespace OSCXR
{
    using VRTK;
    using UnityEngine;

    class Utils
    {

        public static float MapValue(float value, Limits2D inRange, Limits2D outRange)
        {
            return outRange.minimum + (outRange.maximum - outRange.minimum) * ((value - inRange.minimum) / (inRange.maximum - inRange.minimum));
        }

        public static float GetStepValue(float currentValue, float stepSize)
        {
            return Mathf.Round(currentValue / stepSize) * stepSize;
        }

        public static Vector3 GetStepValue(Vector3 vec, float stepSize)
        {
            vec.x = GetStepValue(vec.x, stepSize);
            vec.y = GetStepValue(vec.y, stepSize);
            vec.z = GetStepValue(vec.z, stepSize);

            return vec;
        }
    }
}