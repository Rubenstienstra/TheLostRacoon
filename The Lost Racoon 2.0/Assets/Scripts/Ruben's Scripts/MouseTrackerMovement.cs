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
    public PlayerInputUIController UIInfo;
    
    private float randomMousePosX;
    private float randomMousePosY;

    public Vector2 mouseStartPos;
    public GameObject[] circles;

    // 0 = default, 1 = phase 1, 2 = phase end.
    public int currentPhase;
    public float[] phaseTime;
    public float[] strengthBuff;
    public float waitingTime;

                                    //default  up  down  left  Right
    private float minRandomLengthX; //  -2,    -     -    <0     0>
    private float maxRandomLengthX; //   2 ,   -     -     -2    2
    private float minRandomLengthY; //  -2,    0>   <0     -     -
    private float maxRandomLengthY; //   2 ,   2     2     -     -

    public RectTransform bigCircleSchrink;
    public CircleCollider2D bigCircleSchrinkCollider;
    public float circleSchrinkStrengthBuff = 1;

    public PlayerInput crPlayerInput;

    public bool mouseInZone;
    
    void Start()
    {
        //saving data
        
    }
    //If in Area load this
    public void StartAreaMinigame()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            circles[i].SetActive(true);
        }

        Mouse.current.WarpCursorPosition(mouseStartPos);
        UIInfo.mousePosX = mouseStartPos.x;
        UIInfo.mousePosY = mouseStartPos.y;
        currentPhase = 0;

        mouseInZone = true;
        playerInfo.minigameActiveMouse = true;

        StartMinigame();

        //safety
        StopCoroutine(CountDown());
        if(waitingTime == 0)
        {
            waitingTime = 0.1f;
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
    public IEnumerator MouseMover()
    {     
          randomMousePosX = Random.Range(minRandomLengthX, maxRandomLengthX) * strengthBuff[currentPhase];
          randomMousePosY = Random.Range(minRandomLengthY, maxRandomLengthY) * strengthBuff[currentPhase];
          Mouse.current.WarpCursorPosition(new Vector2(randomMousePosX + UIInfo.mousePosX ,randomMousePosY + UIInfo.mousePosY));

          bigCircleSchrink.sizeDelta = new Vector2(bigCircleSchrink.rect.width - (0.25f * circleSchrinkStrengthBuff), bigCircleSchrink.rect.height - (0.25f * circleSchrinkStrengthBuff));
          bigCircleSchrinkCollider.radius = bigCircleSchrink.rect.width/2;

          yield return new WaitForSeconds(0.01f);//NO CHANGE depents on MouseMover
          if(playerInfo.minigameActiveMouse == true) // double check
          {
             StartCoroutine(MouseMover());
          }
          if(mouseInZone == false)
          {
             StartCoroutine(WaitingForShutDown());
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
            print("Completed/Victory! :D");
            ShutDown();
        }
        else
        {
            StopCoroutine(CountDown());
            ShutDown();
        }
    }
    public void RandomRangeMinMax()
    {
        if(currentPhase >= 3)
        {
            print("ERROR to many phases ):");
            currentPhase--;
        }
       int i = Random.Range(0, 5);
        switch (i)
        {
            case 0: //Default
                {
                    minRandomLengthX = -2;
                    maxRandomLengthX = 2;//6
                    minRandomLengthY = -2;
                    maxRandomLengthY = 2;//6
                    return;
                }
            case 1: //Up
                {
                    minRandomLengthX = -2;
                    maxRandomLengthX = 2;//6
                    minRandomLengthY = 0;
                    maxRandomLengthY = 2;//6
                    return;
                }
            case 2: //Down
                {
                    minRandomLengthX = -2;
                    maxRandomLengthX = 2;//6
                    minRandomLengthY = 0;
                    maxRandomLengthY = -2;//6
                    return;
                }
            case 3: //Left
                {
                    minRandomLengthX = 0;
                    maxRandomLengthX = -2;//6
                    minRandomLengthY = -2;
                    maxRandomLengthY = 2;//6
                    return;
                }
            case 4: //Right
                {
                    minRandomLengthX = 0;
                    maxRandomLengthX = 2;//6
                    minRandomLengthY = -2;
                    maxRandomLengthY = 2;//6
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
        for (int i = 0; i < circles.Length; i++)
        {
            circles[i].SetActive(false);
        }
        currentPhase = 0;
        playerInfo.minigameActiveMouse = false;
        bigCircleSchrink.sizeDelta = new Vector2(400,400);
    }
    

    //For button Event triggers
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