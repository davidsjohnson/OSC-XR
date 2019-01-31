namespace OSCXR
{
    using UnityEngine;
    using VRTK;

    public class OscPointerTriggerReactor : MonoBehaviour
    {
        public OscPointerTriggerTransmitter oscTrigger;
        public Color hoverColor = Color.cyan;
        public Color selectColor = Color.yellow;
        public bool logEnterEvent = true;
        public bool logHoverEvent = false;
        public bool logExitEvent = true;
        public bool logSetEvent = true;


        public virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
        {
            oscTrigger.SendOSCMessage(string.Format("{0}/enter", oscTrigger.oscAddress));
        }

        public void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)
        {
            //oscTrigger.SendOSCMessage(string.Format("{0}/hover", oscTrigger.oscAddress), oscTrigger.mark.datum["fpath"]);
        }

        public virtual void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
        {
            oscTrigger.SendOSCMessage(string.Format("{0}/exit", oscTrigger.oscAddress));
        }

        public virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            oscTrigger.SendOSCMessage(string.Format("{0}/selected", oscTrigger.oscAddress));
        }

        protected virtual void DebugLogger(uint index, string action, Transform target, RaycastHit raycastHit, float distance, Vector3 tipPosition)
        {
            string targetName = (target ? target.name : "<NO VALID TARGET>");
            string colliderName = (raycastHit.collider ? raycastHit.collider.name : "<NO VALID COLLIDER>");
            VRTK_Logger.Info("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named [" + targetName + "] on the collider named [" + colliderName + "] - the pointer tip position is/was: " + tipPosition);
        }
    }
}