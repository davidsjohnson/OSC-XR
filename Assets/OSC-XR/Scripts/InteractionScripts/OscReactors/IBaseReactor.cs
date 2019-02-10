namespace OSCXR
{
    public interface IBaseReactor
    {
        string OscAddress { get; }
        void SendOSCMessage(string address, params object[] values);
    }
}
