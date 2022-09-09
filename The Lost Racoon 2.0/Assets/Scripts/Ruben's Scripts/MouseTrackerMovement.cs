using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MouseTrackerMovement : MonoBehaviour
{
    public PlayerScript playerInfo;
    public ScriptableSaving savingInfo;

    private float mousePosX;
    private float mousePosY;
    private float randomMousePosX;
    private float randomMousePosY;

    public Level mission;
    public bool startPosActivate;

    public int currentPhase;
    public float[] phaseTime;
    public float[] strengthBuff;

    public float waitingTime;
    // 0 = default, 1 = phase 1, 2 = phase end.

    public bool mouseInZone;
    
    void Start()
    {
        //saving data

        //start minigame
        StartAreaMinigame();
        
    }
    public void StartAreaMinigame()
    {
        playerInfo.minigameActiveMouse = true;
        startPosActivate = true;
    }
    public void StartMinigame()
    {
        StartCoroutine(CountDown());
    }

    public void OnMouseX(InputValue value)
    {
        if (playerInfo.minigameActiveMouse == true)
        {
            mousePosX = value.Get<float>();
            CheckMouseTracker();
        }
    }
    public void OnMouseY(InputValue value)
    {
        if (playerInfo.minigameActiveMouse == true)
        {
            mousePosY = value.Get<float>();
            CheckMouseTracker();
        }

    }
    public void CheckMouseTracker()
    {
        if (mousePosX >= mission.requirementLeftX[savingInfo.mouseTrackerTimesDone] && mousePosX <= mission.requirementRightX[savingInfo.mouseTrackerTimesDone] && mousePosY <= mission.requirementUpY[savingInfo.mouseTrackerTimesDone] && mousePosY >= mission.requirementDownY[savingInfo.mouseTrackerTimesDone])
        {
            if(startPosActivate == true)
            {
                startPosActivate = false;
                StartMinigame();
            }
            mouseInZone = true;
            StartCoroutine(MouseMover());
        }
        else
        {
            mouseInZone = false;
           StartCoroutine(WaitingForShutDown());
        }
    }
    public IEnumerator MouseMover()
    {
        if(mouseInZone == true)
        {
            randomMousePosX = Random.Range(-2, 3) * strengthBuff[currentPhase];
            randomMousePosY = Random.Range(-2, 3) * strengthBuff[currentPhase];
            Mouse.current.WarpCursorPosition(new Vector2(randomMousePosX + mousePosX ,randomMousePosY + mousePosY));
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(MouseMover());
        }
    }
    public IEnumerator CountDown()
    { 
        yield return new WaitForSeconds(phaseTime[0]);
        currentPhase++;
        yield return new WaitForSeconds(phaseTime[1]);
        currentPhase++;
        yield return new WaitForSeconds(phaseTime[2]);
        playerInfo.minigameActiveMouse = false;
        startPosActivate = false;
        savingInfo.totalMissionsCompleted++;
        savingInfo.mouseTrackerTimesDone++;
        ShutDown();
    }
    public IEnumerator WaitingForShutDown()
    {
        yield return new WaitForSeconds(waitingTime);
        if(mouseInZone == false)
        {
            ShutDown();
        }
    }
    public void ShutDown()
    {
        mouseInZone = false;
        StopCoroutine(CountDown());
        currentPhase = 0;
    }
}
[System.Serializable]
public class Level
{
    public int[] requirementRightX;
    public int[] requirementLeftX;
    public int[] requirementUpY;
    public int[] requirementDownY;
}