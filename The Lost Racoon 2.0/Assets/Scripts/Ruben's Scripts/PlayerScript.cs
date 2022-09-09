using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public ScriptableSaving savingInfo;
    public MouseTrackerMovement mouseInfo;
    public bool minigameActiveMouse;
    
    void Start()
    {
        ResetProgress();
    }


    public void ResetProgress()
    {
        savingInfo.totalMissionsCompleted = 0;
        savingInfo.totalMissionsCompleted = 0;
    }
}
