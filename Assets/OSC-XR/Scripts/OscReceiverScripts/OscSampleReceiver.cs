namespace OSCXR
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityOscLib;

    public class OscSampleReceiver : MonoBehaviour
    {
        private Text text;
        private void Start()
        {
            OscReceiverManager.Instance.RegisterOscAddress("/tnseprocessed/done", OscReceivedHandler);
            text = GetComponent<Text>();
            text.text = "Press to Run...";
        }

        public void OscReceivedHandler(OSCMessage message)
        {
            text.text = "Done";
        }
    }
}