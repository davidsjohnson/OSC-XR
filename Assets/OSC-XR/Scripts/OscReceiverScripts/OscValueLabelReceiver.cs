namespace OSCXR
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityOscLib;

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

        public void OscXTextHandler(OSCMessage message)
        {
            // value messages should have two values an ID and the value
            int incomingID = (int)message.Data[0];
            float value = (float)message.Data[1];


            if (string.IsNullOrEmpty(ID) || int.Parse(ID) == incomingID)
            {
                Debug.Log("HERE X " + value);
                canvasText.text = string.Format(stringFormat, value);
            }
        }

        public void OscYTextHandler(OSCMessage message)
        {
            // value messages should have two values an ID and the value
            int incomingID = (int)message.Data[0];
            float value = (float)message.Data[2];

            if (string.IsNullOrEmpty(ID) || int.Parse(ID) == incomingID)
                canvasText.text = string.Format(stringFormat, value);
        }

        public void OscZTextHandler(OSCMessage message)
        {
            // value messages should have two values an ID and the value
            int incomingID = (int)message.Data[0];
            float value = (float)message.Data[3];

            if (string.IsNullOrEmpty(ID) || int.Parse(ID) == incomingID)
                canvasText.text = string.Format(stringFormat, value);
        }
    }
}