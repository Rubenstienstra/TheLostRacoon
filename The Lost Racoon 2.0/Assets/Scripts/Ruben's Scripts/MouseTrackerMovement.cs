using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MouseTrackerMovement : MonoBehaviour
{
    public PlayerScript playerInfo;
    public ScriptableSaving savingInfo;

    public float mousePosX;
    public float mousePosY;
    private float randomMousePosX;
    private float randomMousePosY;
    //public bool activatedOnEnter;

    public Vector2 mouseStartPos;
    public GameObject[] images;

    // 0 = default, 1 = phase 1, 2 = phase end.
    public int currentPhase;
    public float[] phaseTime;
    public float[] strengthBuff;

                                  //default  up  down  left  Right
    private int minRandomLengthX; //  -2,    -     -    <0     0>
    private int maxRandomLengthX; //   3 ,   -     -     -3    3
    private int minRandomLengthY; //  -2,    0>   <0     -     -
    private int maxRandomLengthY; //   3 ,   3     3     -     -

    public float waitingTime;
    
    public bool mouseInZone;
    
    void Start()
    {
        //saving data
        
    }
    //If in Area load this
    public void StartAreaMinigame()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].SetActive(true);
        }

        Mouse.current.WarpCursorPosition(mouseStartPos);
        mousePosX = mouseStartPos.x;
        mousePosY = mouseStartPos.y;

        mouseInZone = true;
        playerInfo.minigameActiveMouse = true;

        StartMinigame();

        //safety
        StopCoroutine(CountDown());
        if(waitingTime == 0)
        {
            waitingTime = 0.01f;
        }
    }
    // Use Interactable enter
    public void StartMinigame()
    {
        if(playerInfo.minigameActiveMouse == true)
        {
            StartCoroutine(MouseMover());
            StartCoroutine(CountDown());
        }
    }

    public void OnMouseX(InputValue value)
    {
        if (playerInfo.minigameActiveMouse == true)
        {
            mousePosX = value.Get<float>();
        }
    }
    public void OnMouseY(InputValue value)
    {
        if (playerInfo.minigameActiveMouse == true)
        {
            mousePosY = value.Get<float>();
        }

    }
    public IEnumerator MouseMover()
    {
        if(playerInfo.minigameActiveMouse == true)
        {
            randomMousePosX = Random.Range(minRandomLengthX, maxRandomLengthX) * strengthBuff[currentPhase];
            randomMousePosY = Random.Range(minRandomLengthY, maxRandomLengthY) * strengthBuff[currentPhase];
            Mouse.current.WarpCursorPosition(new Vector2(randomMousePosX + mousePosX ,randomMousePosY + mousePosY));
            yield return new WaitForSeconds(0f);
            StartCoroutine(MouseMover());
        }
    }
    public IEnumerator CountDown()
    {
        RandomRangeMinMax();
        yield return new WaitForSeconds(phaseTime[0]);
        currentPhase++;
        RandomRangeMinMax();

        yield return new WaitForSeconds(phaseTime[1]);
        currentPhase++;
        RandomRangeMinMax();

        yield return new WaitForSeconds(phaseTime[2]);
        if (mouseInZone == true)
        {
            playerInfo.minigameActiveMouse = false;
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
       int i = Random.Range(0, 5);
        switch (i)
        {
            case 0: //Default
                {
                    minRandomLengthX = -4;
                    maxRandomLengthX = 6;
                    minRandomLengthY = -4;
                    maxRandomLengthY = 6;
                    return;
                }
            case 1: //Up
                {
                    minRandomLengthX = -4;
                    maxRandomLengthX = 6;
                    minRandomLengthY = 0;
                    maxRandomLengthY = 6;
                    return;
                }
            case 2: //Down
                {
                    minRandomLengthX = -4;
                    maxRandomLengthX = 6;
                    minRandomLengthY = 0;
                    maxRandomLengthY = -6;
                    return;
                }
            case 3: //Left
                {
                    minRandomLengthX = 0;
                    maxRandomLengthX = -6;
                    minRandomLengthY = -4;
                    maxRandomLengthY = 6;
                    return;
                }
            case 4: //Right
                {
                    minRandomLengthX = 0;
                    maxRandomLengthX = 6;
                    minRandomLengthY = -4;
                    maxRandomLengthY = 6;
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
        print("ended minigame at Phase: " + currentPhase.ToString());
        StopCoroutine(CountDown());
        StopCoroutine(MouseMover());
        for (int i = 0; i < images.Length; i++)
        {
            images[i].SetActive(false);
        }
        playerInfo.minigameActiveMouse = false;
        currentPhase = 0;
    }
    

    //For button Event trigger
    public void ActivateWaitingForShutDown()
    {
        StartCoroutine(WaitingForShutDown());
    }
    //For Image animating/editing
    public void InFirstCircle()
    {

    }
    public void OutOfFirstCircle()
    {

    }
    public void InSecondCircle()
    {

    }
    public void OutOfSecondCircle()
    {

    }
    public void InThirdCircle()
    {
        StopCoroutine(WaitingForShutDown());
        mouseInZone = true;
    }
    public void OutOfThirdCircle()
    {
        StartCoroutine(WaitingForShutDown());
        mouseInZone = false;
    }

}
