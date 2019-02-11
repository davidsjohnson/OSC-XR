namespace OSCXR
{
    using UnityEngine;
    using VRTK;

    public class OscPointerController : BaseOscController
    {
        // Reactor Private Members
        public Color hoverColor = Color.cyan;
        public Color selectColor = Color.yellow;

        public void Start()
        {
            oscAddress = string.IsNullOrEmpty(oscAddress) ? "/pointer/trigger" : oscAddress;
        }

        public virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
        {
            SendOSCMessage(string.Format("{0}/enter", oscAddress));
        }

        public void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)
        {
            SendOSCMessage(string.Format("{0}/hover", oscAddress));
        }

        public virtual void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
        {
            SendOSCMessage(string.Format("{0}/exit", oscAddress));
        }

        public virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            SendOSCMessage(string.Format("{0}/selected", oscAddress));
        }

        protected virtual void DebugLogger(uint index, string action, Transform target, RaycastHit raycastHit, float distance, Vector3 tipPosition)
        {
            string targetName = (target ? target.name : "<NO VALID TARGET>");
            string colliderName = (raycastHit.collider ? raycastHit.collider.name : "<NO VALID COLLIDER>");
            VRTK_Logger.Info("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named [" + targetName + "] on the collider named [" + colliderName + "] - the pointer tip position is/was: " + tipPosition);
        }
    }
}