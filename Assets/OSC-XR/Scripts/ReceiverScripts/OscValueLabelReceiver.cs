namespace OSCXR
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityOSC;

    public class OscValueLabelReceiver : MonoBehaviour
    {
        public Text canvasText;
        public string stringFormat = "Value:{0:0.00}";
        public string ID;

        public void OscTextHandler(OSCMessage message)
        {
            // value messages should have two values an ID and the value
            int incomingID = (int)message.Data[0];
            float value = (float)message.Data[1];

            if (string.IsNullOrEmpty(ID) || int.Parse(ID) == incomingID)
                canvasText.text = string.Format(stringFormat, value);
        }
    }
}