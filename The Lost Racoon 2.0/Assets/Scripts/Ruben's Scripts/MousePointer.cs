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


        if (col.gameObject == mouseRectangleInfo.squares[mouseRectangleInfo.strengthStage++])
        {
            mouseRectangleInfo.mouseInZone = true;

            if (mouseRectangleInfo.strengthStage <= mouseRectangleInfo.squares.Length - 1)
            {
            mouseRectangleInfo.strengthStage++;
            }
        }
        if (col.gameObject == mouseRectangleInfo.squares[0])
        {
            mouseRectangleInfo.strengthStage = 0;
        }
        
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == mouseInfo.circles[0])
        {
            mouseInfo.mouseInZone = false;
        }

        for (int i = 0; i < mouseRectangleInfo.squares.Length; i++)
        {
            if (col.gameObject == mouseRectangleInfo.squares[i])
            {
                if(mouseRectangleInfo.strengthStage >= 1)
                {
                    
                    mouseRectangleInfo.strengthStage--;
                }
                
            }
        }
        if (col.gameObject == mouseRectangleInfo.squares[0])
        {
            mouseRectangleInfo.mouseInZone = false;
            mouseRectangleInfo.strengthStage = 0;
        }
        
    }
    public void GoingUp()
    {

    }
    public void GoingDown()
    {
        mouseRectangleInfo.strengthStage--;
    }
}
