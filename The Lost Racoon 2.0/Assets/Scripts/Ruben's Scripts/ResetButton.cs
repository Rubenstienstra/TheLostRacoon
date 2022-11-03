using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public ScriptableSaving savingInfo;
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
}
