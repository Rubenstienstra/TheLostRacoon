using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjectSaving", fileName = "ScriptableObjectSaving")]

public class ScriptableSaving : ScriptableObject
{
    public int totalMissionsCompleted;

    public float volumeSFX;
    public float volumeMUSIC;

    //mousetracker movement minigame completed
    public int mouseTrackerTimesDone;
}
