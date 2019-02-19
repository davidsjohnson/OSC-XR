namespace OSCXR {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class OscCubeTransmitter : OscTransformTransmitter
    {
        // Start is called before the first frame update
        void OnEnable()
        {
            oscAddress = string.IsNullOrEmpty(oscAddress) ? "/osc" +
                "cube" : oscAddress;
        }

        private void Update()
        {
            if (Time.frameCount % 5 == 0)
            {
                Debug.Log("World Angles: " + transform.eulerAngles);
                Debug.Log("Local Angles: " + transform.localEulerAngles);
            }

        }
    }
}