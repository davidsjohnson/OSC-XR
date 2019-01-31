namespace OSCXR
{
    using UnityEngine;
    using VRTK;
    using VRTK.Highlighters;

    public class OscPointerTriggerActivator : MonoBehaviour
    {
        public VRTK_DestinationMarker pointer;
        public Color hoverColor = Color.cyan;
        public Color selectColor = Color.yellow;

        protected virtual void OnEnable()
        {
            pointer = (pointer == null ? GetComponent<VRTK_DestinationMarker>() : pointer);

            if (pointer != null)
            {
                pointer.DestinationMarkerEnter += DestinationMarkerEnter;
                pointer.DestinationMarkerHover += DestinationMarkerHover;
                pointer.DestinationMarkerExit += DestinationMarkerExit;
                pointer.DestinationMarkerSet += DestinationMarkerSet;
            }
            else
            {
                VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTKExample_PointerObjectHighlighterActivator", "VRTK_DestinationMarker", "the Controller Alias"));
            }
        }

        protected virtual void OnDisable()
        {
            if (pointer != null)
            {
                pointer.DestinationMarkerEnter -= DestinationMarkerEnter;
                pointer.DestinationMarkerHover -= DestinationMarkerHover;
                pointer.DestinationMarkerExit -= DestinationMarkerExit;
                pointer.DestinationMarkerSet -= DestinationMarkerSet;
            }
        }

        protected virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
        {
            ToggleHighlight(e.target, hoverColor);
            OscPointerTriggerReactor reactor = e.target.GetComponent<OscPointerTriggerReactor>();
            if (reactor != null)
            {
                reactor.DestinationMarkerEnter(sender, e);
            }
        }

        private void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)
        {
            OscPointerTriggerReactor reactor = e.target.GetComponent<OscPointerTriggerReactor>();
            if (reactor != null)
            {
                reactor.DestinationMarkerHover(sender, e);
            }
        }

        protected virtual void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
        {
            ToggleHighlight(e.target, Color.clear);
            OscPointerTriggerReactor reactor = e.target.GetComponent<OscPointerTriggerReactor>();
            if (reactor != null)
            {
                reactor.DestinationMarkerExit(sender, e);
            }
        }

        protected virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            ToggleHighlight(e.target, selectColor);
            OscPointerTriggerReactor reactor = e.target.GetComponent<OscPointerTriggerReactor>();
            if (reactor != null)
            {
                reactor.DestinationMarkerSet(sender, e);
            }
        }

        protected virtual void ToggleHighlight(Transform target, Color color)
        {
            VRTK_BaseHighlighter highligher = (target != null ? target.GetComponentInChildren<VRTK_BaseHighlighter>() : null);
            if (highligher != null)
            {
                highligher.Initialise();
                if (color != Color.clear)
                {
                    highligher.Highlight(color);
                }
                else
                {
                    highligher.Unhighlight();
                }
            }
        }

        //protected virtual void DebugLogger(uint index, string action, Transform target, RaycastHit raycastHit, float distance, Vector3 tipPosition)
        //{
        //    string targetName = (target ? target.name : "<NO VALID TARGET>");
        //    string colliderName = (raycastHit.collider ? raycastHit.collider.name : "<NO VALID COLLIDER>");
        //    VRTK_Logger.Info("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named [" + targetName + "] on the collider named [" + colliderName + "] - the pointer tip position is/was: " + tipPosition);
        //}
    }
}