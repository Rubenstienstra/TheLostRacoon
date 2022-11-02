using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjectSaving", fileName = "ScriptableObjectSaving")]

public class ScriptableSaving : ScriptableObject
{
    public int totalMissionsCompleted;

    public float volumeSFX;
    public float volumeMUSIC;

    public List<GameObject> activatedCheckpoints;
    public Vector3 crCheckpointVector3;
    public Vector3 crCheckpointRotation;
    public bool[] tutorialStepsCompleted;

    //mousetracker movement minigame completed
    public int mouseTrackerTimesDone;
}
