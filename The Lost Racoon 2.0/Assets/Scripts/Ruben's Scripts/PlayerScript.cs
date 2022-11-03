using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    public ScriptableSaving savingInfo;
    public MouseTrackerRectangleMovement mouseRectangleInfo;
    public PlayerMovementBetter playerMovingInfo;

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

        if (!savingInfo.saveAndLoadSystem)
        {
            savingInfo.crCheckpointVector3 = new Vector3(0, 0, 0);
            savingInfo.crCheckpointRotation = new Vector3(0, 0, 0);

            for (int i = 0; i < savingInfo.tutorialStepsCompleted.Length; i++)
            {
                savingInfo.tutorialStepsCompleted[i] = false;
            }
            for (int i = savingInfo.crActivatedCheckpoints.Count - 1; i >= 0; i--)
            {
                savingInfo.crActivatedCheckpoints.RemoveAt(i);
            }
            for (int i = savingInfo.checkpointNames.Count - 1; i >= 0; i--)
            {
                savingInfo.checkpointNames.RemoveAt(i);
            }
        }
    }
    //public void SaveAndLoadSystemReset()
    //{
    //    savingInfo.crCheckpointVector3 = new Vector3(0, 0, 0);
    //    savingInfo.crCheckpointRotation = new Vector3(0, 0, 0);

    //    for (int i = playerMovingInfo.activatedCheckpoints.Count - 1; i >= 0; i--)
    //    {
    //        playerMovingInfo.activatedCheckpoints.RemoveAt(i);
    //        print(i);
    //    }
    //}
}
