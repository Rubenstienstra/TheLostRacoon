using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    public ScriptableSaving savingInfo;
    public PlayerMovementBetter playermovementInfo;
    public MouseTrackerRectangleMovement mouseRectangleInfo;

    public bool minigameActiveMouse;
    public bool minigameActiveMouseRectangle;

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

        if (!playermovementInfo.saveAndLoadSystem)
        {
            savingInfo.crCheckpointVector3 = new Vector3(0, 0, 0);
            savingInfo.crCheckpointRotation = new Vector3(0, 0, 0);

            for (int i = savingInfo.activatedCheckpoints.Count - 1; i >= 0; i--)
            {
                savingInfo.activatedCheckpoints.RemoveAt(i);
                print(i);
            }
        }
    }
    public void SaveAndLoadSystemReset()
    {
        savingInfo.crCheckpointVector3 = new Vector3(0, 0, 0);
        savingInfo.crCheckpointRotation = new Vector3(0, 0, 0);

        for (int i = savingInfo.activatedCheckpoints.Count - 1; i >= 0; i--)
        {
            savingInfo.activatedCheckpoints.RemoveAt(i);
            print(i);
        }
    }
}
