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
            savingInfo.crCheckpointVector3 = new Vector3(17.95f, 20.65f, -6);
            savingInfo.crCheckpointRotation = new Vector3(0, -90, 0);

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

            for (int i = playerMovingInfo.allInGameSoundSwaps.Length - 1; i >= 0; i--)
            {
                playerMovingInfo.allInGameSoundSwaps[i].GetComponent<SoundSwap>().toggleSwitch = false;
            }
        }
    }
    public void ResetProgressGuaranteedSucces()
    {
         print("All Progress Reset");
         savingInfo.totalMissionsCompleted = 0;
         savingInfo.mouseTrackerTimesDone = 0;

         savingInfo.crCheckpointVector3 = new Vector3(17.95f, 20.65f, -6);
         savingInfo.crCheckpointRotation = new Vector3(0, -90, 0);

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

         for (int i = playerMovingInfo.allInGameSoundSwaps.Length - 1; i >= 0; i--)
         {
            playerMovingInfo.allInGameSoundSwaps[i].GetComponent<SoundSwap>().toggleSwitch = false;
         }
    }
}
