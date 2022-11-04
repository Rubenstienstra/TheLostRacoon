using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public ScriptableSaving savingInfo;

    public GameObject playerGameObject;
    public void Start()
    {
        
    }
    public void ResetProgress()
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

        if (GameObject.Find("RacoonPlayer") != null)
        {
            playerGameObject = GameObject.Find("RacoonPlayer");
            playerGameObject.GetComponent<PlayerMovementBetter>().OnReset();
        }
    }
}
