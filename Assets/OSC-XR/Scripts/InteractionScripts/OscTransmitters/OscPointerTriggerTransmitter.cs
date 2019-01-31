namespace OSCXR
{
    public class OscPointerTriggerTransmitter : BaseOscTransmitter
    {
        public void Start()
        {
            oscAddress = string.IsNullOrEmpty(oscAddress) ? "/pointer/trigger" : oscAddress;
        }
    }
}