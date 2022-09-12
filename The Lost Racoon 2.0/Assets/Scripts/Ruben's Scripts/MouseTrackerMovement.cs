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

                                  //default  up  down  left  Right
    private int minRandomLengthX; //  -2,    -     -    <0     0>
    private int maxRandomLengthX; //   3 ,   -     -     -3    3
    private int minRandomLengthY; //  -2,    0>   <0     -     -
    private int maxRandomLengthY; //   3 ,   3     3     -     -

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

        //safety
        StopCoroutine(CountDown());
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
        else if(startPosActivate == false && playerInfo.minigameActiveMouse == true)
        {
            mouseInZone = false;
           StartCoroutine(WaitingForShutDown());
        }
    }
    public IEnumerator MouseMover()
    {
        if(mouseInZone == true && playerInfo.minigameActiveMouse == true)
        {
            randomMousePosX = Random.Range(minRandomLengthX, maxRandomLengthX) * strengthBuff[currentPhase];
            randomMousePosY = Random.Range(minRandomLengthY, maxRandomLengthY) * strengthBuff[currentPhase];
            Mouse.current.WarpCursorPosition(new Vector2(randomMousePosX + mousePosX ,randomMousePosY + mousePosY));
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(MouseMover());
        }
    }
    public IEnumerator CountDown()
    {
        RandomRangeMinMax();
        //print("Phase Begin");
        yield return new WaitForSeconds(phaseTime[0]);
        currentPhase++;
        RandomRangeMinMax();
        //print("Phase1");

        yield return new WaitForSeconds(phaseTime[1]);
        currentPhase++;
        RandomRangeMinMax();
        //print("Phase2");

        yield return new WaitForSeconds(phaseTime[2]);
        //print("Phase Done");
        if (mouseInZone == true)
        {
            playerInfo.minigameActiveMouse = false;
            startPosActivate = false;
            savingInfo.totalMissionsCompleted++;
            savingInfo.mouseTrackerTimesDone++;
            ShutDown();
        }
        else
        {
            StopCoroutine(CountDown());
        }
    }
    public void RandomRangeMinMax()
    {
       int i = Random.Range(0, 4);
        switch (i)
        {
            case 0: //Default
                {
                    minRandomLengthX = -2;
                    maxRandomLengthX = 3;
                    minRandomLengthY = -2;
                    maxRandomLengthY = 3;
                    return;
                }
            case 1: //Up
                {
                    minRandomLengthX = -2;
                    maxRandomLengthX = 3;
                    minRandomLengthY = 0;
                    maxRandomLengthY = 3;
                    return;
                }
            case 2: //Down
                {
                    minRandomLengthX = -2;
                    maxRandomLengthX = 3;
                    minRandomLengthY = 0;
                    maxRandomLengthY = -3;
                    return;
                }
            case 3: //Left
                {
                    minRandomLengthX = 0;
                    maxRandomLengthX = -3;
                    minRandomLengthY = -2;
                    maxRandomLengthY = 3;
                    return;
                }
            case 4: //Right
                {
                    minRandomLengthX = 0;
                    maxRandomLengthX = 3;
                    minRandomLengthY = -2;
                    maxRandomLengthY = 3;
                    return;
                }

        }
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
        //print("Shutdown");
        StopCoroutine(CountDown());
        playerInfo.minigameActiveMouse = false;
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