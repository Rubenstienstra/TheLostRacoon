using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjectSaving", fileName = "ScriptableObjectSaving")]

public class ScriptableSaving : ScriptableObject
{
    public int totalMissionsCompleted;

    public bool minigame3x3;
    public bool minigame4x4;

    //mousetracker movement minigame completed
    public int mouseTrackerTimesDone;
    //public int strengthStage;
}
