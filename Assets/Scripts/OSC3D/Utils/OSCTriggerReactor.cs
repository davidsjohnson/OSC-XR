using UnityEngine;
using VRTK.Highlighters;
using VRTK;

public class OSCTriggerReactor : MonoBehaviour
{
    public OSCTriggerController oscTrigger;
    public Color hoverColor = Color.cyan;
    public Color selectColor = Color.yellow;
    public bool logEnterEvent = true;
    public bool logHoverEvent = false;
    public bool logExitEvent = true;
    public bool logSetEvent = true;

    private VRTK_DestinationMarker leftHandPointer;
    private VRTK_DestinationMarker rightHandPointer;

    protected virtual void OnEnable()
    {
        leftHandPointer = oscTrigger.leftHandPointer;
        rightHandPointer = oscTrigger.rightHandPointer;

        if (leftHandPointer != null && rightHandPointer == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTKExample_PointerObjectHighlighterActivator", "VRTK_DestinationMarker", "the Controller Alias"));
        }

        if (leftHandPointer != null)
        {
            leftHandPointer.DestinationMarkerEnter += DestinationMarkerEnter;
            leftHandPointer.DestinationMarkerHover += DestinationMarkerHover;
            leftHandPointer.DestinationMarkerExit += DestinationMarkerExit;
            leftHandPointer.DestinationMarkerSet += DestinationMarkerSet;
        }

        if (rightHandPointer != null)
        {
            rightHandPointer.DestinationMarkerEnter += DestinationMarkerEnter;
            rightHandPointer.DestinationMarkerHover += DestinationMarkerHover;
            rightHandPointer.DestinationMarkerExit += DestinationMarkerExit;
            rightHandPointer.DestinationMarkerSet += DestinationMarkerSet;
        }

    }

    protected virtual void OnDisable()
    {
        if (leftHandPointer != null)
        {
            leftHandPointer.DestinationMarkerEnter -= DestinationMarkerEnter;
            leftHandPointer.DestinationMarkerHover -= DestinationMarkerHover;
            leftHandPointer.DestinationMarkerExit -= DestinationMarkerExit;
            leftHandPointer.DestinationMarkerSet -= DestinationMarkerSet;
        }

        if (rightHandPointer != null)
        {
            rightHandPointer.DestinationMarkerEnter -= DestinationMarkerEnter;
            rightHandPointer.DestinationMarkerHover -= DestinationMarkerHover;
            rightHandPointer.DestinationMarkerExit -= DestinationMarkerExit;
            rightHandPointer.DestinationMarkerSet -= DestinationMarkerSet;
        }
    }

    protected virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
    {
        ToggleHighlight(e.target, hoverColor);
        if (logEnterEvent)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER ENTER", e.target, e.raycastHit, e.distance, e.destinationPosition);
        }
        oscTrigger.SendOSCMessage(string.Format("{0}/enter", oscTrigger.oscAddress));
    }

    private void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)
    {
        if (logHoverEvent)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER HOVER", e.target, e.raycastHit, e.distance, e.destinationPosition);
        }
        oscTrigger.SendOSCMessage(string.Format("{0}/hover", oscTrigger.oscAddress));
    }

    protected virtual void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
    {
        ToggleHighlight(e.target, Color.clear);
        if (logExitEvent)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER EXIT", e.target, e.raycastHit, e.distance, e.destinationPosition);
        }
        oscTrigger.SendOSCMessage(string.Format("{0}/exit", oscTrigger.oscAddress));
    }

    protected virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
    {
        ToggleHighlight(e.target, selectColor);
        if (logSetEvent)
        {
            DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER SET", e.target, e.raycastHit, e.distance, e.destinationPosition);
        }
        oscTrigger.SendOSCMessage(string.Format("{0}/selected", oscTrigger.oscAddress));
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

    protected virtual void DebugLogger(uint index, string action, Transform target, RaycastHit raycastHit, float distance, Vector3 tipPosition)
    {
        string targetName = (target ? target.name : "<NO VALID TARGET>");
        string colliderName = (raycastHit.collider ? raycastHit.collider.name : "<NO VALID COLLIDER>");
        VRTK_Logger.Info("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named [" + targetName + "] on the collider named [" + colliderName + "] - the pointer tip position is/was: " + tipPosition);
    }
}