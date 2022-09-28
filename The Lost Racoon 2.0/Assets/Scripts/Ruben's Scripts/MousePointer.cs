using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    public PlayerScript playerInfo;
    public MouseTrackerMovement mouseInfo;
    public MouseTrackerRectangleMovement mouseRectangleInfo;
    public ScriptableSaving savingInfo;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == mouseInfo.circles[0])
        {
            mouseInfo.mouseInZone = true;
        }        
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == mouseInfo.circles[0])
        {
            mouseInfo.mouseInZone = false;
        }        
    }
}
