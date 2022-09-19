using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    public PlayerScript playerInfo;
    public MouseTrackerMovement mouseInfo;
    public ScriptableSaving savingInfo;

    public GameObject pointer;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == mouseInfo.circles[0])
        {
            savingInfo.mouseInZone = true;
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == mouseInfo.circles[0])
        {
            savingInfo.mouseInZone = false;
        }
    }
}
