using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    public ScriptableSaving savingInfo;
    public MouseTrackerMovement mouseInfo;
    public MouseTrackerRectangleMovement mouseRectangleInfo;

    public bool minigameActiveMouse;
    public bool minigameActive3x3Puzzle;

    void Start()
    {
        ResetProgress();
        Cursor.visible = false;
    }

    public void ResetProgress()
    {
        print("All Progress Reset");
        savingInfo.totalMissionsCompleted = 0;
        savingInfo.mouseTrackerTimesDone = 0;
        savingInfo.strengthStage = 0;
        savingInfo.mouseInZone = false;
    }
    
    public void OnInteract(InputValue value)
    {
        if(value.Get<float>() >= 1)
        {
            //Testing Press E
            if(mouseInfo.currentPhase == 0 && minigameActiveMouse == false)
            {
                //mouseInfo.StartAreaMinigame();
                mouseRectangleInfo.StartAreaMinigame();
            }
        }
    }
}
