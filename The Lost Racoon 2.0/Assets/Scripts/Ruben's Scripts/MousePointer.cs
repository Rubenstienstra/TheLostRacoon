using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    public MouseTrackerMovement mouseInfo;

    private GameObject findingScriptGameobject;

    public void Start()
    {
        findingScriptGameobject = GameObject.Find("Gate circle shrink minigame");
        if(mouseInfo == null)
        {
            mouseInfo = findingScriptGameobject.GetComponent<MouseTrackerMovement>();
        }
    }
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
