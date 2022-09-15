using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


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
        print("All Progress Reset");
        savingInfo.totalMissionsCompleted = 0;
        savingInfo.mouseTrackerTimesDone = 0;
    }
    public void OnInteract(InputValue value)
    {
        if(value.Get<float>() >= 1)
        {
            //Testing Press E
            mouseInfo.StartAreaMinigame();
        }
    }
}
