using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MouseTrackerMovement : MonoBehaviour
{
    public PlayerScript playerInfo;
    public ScriptableSaving savingInfo;

    public float mousePosX;
    public float mousePosY;
    public Level mission;

    public bool mouseInZone;
    
    void Start()
    {
        mission.next = savingInfo.mouseTrackTimesDone;
    }
    public void OnMouseX(InputValue value)
    {
        if (playerInfo.minigameActiveMouse == true)
        {
            mousePosX = value.Get<float>();
            CheckMouseTracker();
            
        }
        
    }
    public void OnMouseY(InputValue value)
    {
        if (playerInfo.minigameActiveMouse == true)
        {
            mousePosY = value.Get<float>();
            CheckMouseTracker();

        }

    }
    public void CheckMouseTracker()
    {
        if (mousePosX >= mission.requirementLeftX[mission.next] && mousePosX <= mission.requirementRightX[mission.next] && mousePosY <= mission.requirementUpY[mission.next] && mousePosY >= mission.requirementDownY[mission.next])
        {
            mouseInZone = true;
        }
        else
        {
            mouseInZone = false;
        }
    }
    public void InsideRadius()
    {
        mouseInZone = true;
    }
    public void OutsideRadius()
    {
        mouseInZone = false;
    }
}
[System.Serializable]
public class Level
{
    public int next;
    public int[] requirementRightX;
    public int[] requirementLeftX;
    public int[] requirementUpY;
    public int[] requirementDownY;
}
