using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    private MouseTrackerMovement mouseInfo;
    private PlayerInputUIController UIInfo;
    private GameObject playerGameObject;

    public GameObject gameObjectTrigger;

    public void Start()
    {
        playerGameObject = GameObject.Find("RacoonPlayer");
        mouseInfo = playerGameObject.GetComponent<MouseTrackerMovement>();
        UIInfo = playerGameObject.GetComponent<PlayerInputUIController>();
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        gameObjectTrigger = col.gameObject;
        if (gameObjectTrigger)
        {
            mouseInfo.mouseInZone = true;
            
        }        
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        print("out");
        if (col.gameObject == mouseInfo.circles[0])
        {
            mouseInfo.mouseInZone = false;
            
        }        
    }
}
