using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    public MouseTrackerMovement mouseInfo;
    public PlayerInputUIController UIInfo;

    private GameObject playerGameObject;

    public void Start()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (playerGameObject == null)
        {
            RefindingInfo();
        }
        else if (col.gameObject.name == "Big circle")
        {
            mouseInfo.mouseInZone = true;
        }        
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (playerGameObject == null)
        {
            RefindingInfo();
        }
        else if (col.gameObject.name == "Big circle")
        {
            mouseInfo.mouseInZone = false;
        }        
    }
    public void RefindingInfo()
    {
        playerGameObject = GameObject.Find("RacoonPlayer");
        UIInfo = playerGameObject.GetComponent<PlayerInputUIController>();
    }
}
