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
            savingInfo.mouseInZone = true;
        }
        if (col.gameObject == mouseRectangleInfo.squares[savingInfo.strengthStage])
        {
            savingInfo.mouseInZone = true;
            savingInfo.strengthStage++;
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        for (int i = 0; i < mouseRectangleInfo.squares.Length; i++)
        {
            if (col.gameObject == mouseRectangleInfo.squares[i])
            {
                savingInfo.strengthStage--;
            }
        }
        if (col.gameObject == mouseInfo.circles[0])
        {
            savingInfo.mouseInZone = false;
        }
        else if (col.gameObject == mouseRectangleInfo.squares[0])
        {
            savingInfo.mouseInZone = false;
            savingInfo.strengthStage = 0;
        }
        
    }
}
