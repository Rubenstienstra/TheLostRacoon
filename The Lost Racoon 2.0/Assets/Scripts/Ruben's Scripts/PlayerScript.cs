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
    public bool minigameActiveMouseRectangle;
    public bool minigameActive3x3Puzzle;

    private void Awake()
    {
        ResetProgress();
        Cursor.visible = false;
    }

    public void ResetProgress()
    {
        print("All Progress Reset");
        savingInfo.totalMissionsCompleted = 0;
        savingInfo.mouseTrackerTimesDone = 0;
        savingInfo.crCheckpointVector3 = new Vector3(0,0,0);
        savingInfo.crCheckpointRotation = new Vector3(0,0,0);

        for (int i = savingInfo.activatedCheckpoints.Count -1; i >= 0; i--)
        {
            savingInfo.activatedCheckpoints.RemoveAt(i);
            print(i);
        }
        
    }
}
