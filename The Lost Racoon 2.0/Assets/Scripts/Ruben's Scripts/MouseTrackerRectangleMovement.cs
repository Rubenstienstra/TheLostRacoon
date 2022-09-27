using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MouseTrackerRectangleMovement : MonoBehaviour
{
    public PlayerScript playerInfo;
    public ScriptableSaving savingInfo;
    public MouseTrackerMovement circleMouseInfo;
    public PlayerInputUIController UIInfo;

    public GameObject mouseCursor;

    public float randomMousePosY;

    public Vector2 mouseStartPos;
    public Vector2 mouseEndPos;
    public GameObject[] UIComponents;

    // 0 = default, 1 = phase 1, 2 = phase end.
    public float waitingTime;
    public bool IsWaiting;

    public Slider movingSlider;

    public bool mouseInZone;
    public int strengthStage;

    public float strengthDebuff;
    public float totalMousePos;

    public PlayerInput crPlayerInput;

    void Start()
    {
        
    }
    //If in Area load this
    public void StartAreaMinigame()
    {
        for (int i = 0; i < UIComponents.Length; i++)
        {
            UIComponents[i].SetActive(true);
        }
        movingSlider.gameObject.SetActive(true);

        Mouse.current.WarpCursorPosition(mouseStartPos);
        UIInfo.mousePosX = mouseStartPos.x;
        UIInfo.mousePosY = mouseStartPos.y;
        strengthStage = 0;

        mouseInZone = true;
        playerInfo.minigameActiveMouseRectangle = true;
        mouseCursor.GetComponent<Image>().enabled = !enabled; //!

        StartMinigame();

        //safety
        if (waitingTime == 0)
        {
            waitingTime = 0.01f;
        }
        if(strengthDebuff == 0)
        {
            strengthDebuff = 1;
        }
        movingSlider.maxValue = mouseEndPos.y - mouseStartPos.y;
    }
    // Use Interactable enter
    public void StartMinigame()
    {
        if (playerInfo.minigameActiveMouseRectangle == true)
        {
            StartCoroutine(MouseMover());
        }
    }
    public IEnumerator MouseMover()
    {
        totalMousePos = UIInfo.mousePosY - mouseStartPos.y;
        totalMousePos /= strengthDebuff;
        if(totalMousePos > 0)
        {
            totalMousePos = -totalMousePos;
        }
        movingSlider.value = UIInfo.mousePosY - mouseStartPos.y;
        if (UIInfo.mousePosY > mouseStartPos.y)
        {
            Mouse.current.WarpCursorPosition(new Vector2(mouseStartPos.x, totalMousePos + UIInfo.mousePosY));
        }
        yield return new WaitForSeconds(0.01f);//NO CHANGE depents on MouseMover

        if (playerInfo.minigameActiveMouseRectangle == true) // double check
        {
            StartCoroutine(MouseMover());
        }
        if (mouseInZone == false)
        {
            ReconnectPosition();
        }
        else if (UIInfo.mousePosX >= mouseStartPos.x || UIInfo.mousePosX <= mouseStartPos.x)
        {
            ReconnectPosition();
        }
        if (UIInfo.mousePosY > mouseEndPos.y && IsWaiting == false)
        {
            IsWaiting = true;
            StartCoroutine(WaitingForShutDown());
        }
        
    }
    public IEnumerator WaitingForShutDown()
    {
        print("Activating WaitingShutdown");
        yield return new WaitForSeconds(waitingTime);
        if (UIInfo.mousePosY > mouseEndPos.y)
        {
            ShutDown();
        }
        else
        {
            StopCoroutine(WaitingForShutDown());
        }
        IsWaiting = false;
    }
    public void ShutDown()
    {
        print("ended minigame at Phase: " + strengthStage.ToString() + " Victory!");
        StopCoroutine(MouseMover());
        StopCoroutine(WaitingForShutDown());

        for (int i = 0; i < UIComponents.Length; i++)
        {
            UIComponents[i].SetActive(false);
        }
        movingSlider.gameObject.SetActive(false);
        mouseCursor.GetComponent<Image>().enabled = enabled;

        strengthStage = 0;
        playerInfo.minigameActiveMouseRectangle = false;

        //Completed
        if (mouseInZone == true)
        {
            playerInfo.minigameActiveMouseRectangle = false;
            savingInfo.totalMissionsCompleted++;
            savingInfo.mouseTrackerTimesDone++;
            print("Completed/Victory!:D");
        }
    }
    public void InSquare()
    {
        mouseInZone = true;
    }
    public void OutOfSquare()
    {
        mouseInZone = false;
    }
    public void ReconnectPosition()
    {
        if(UIInfo.mousePosY >= mouseEndPos.y)
        {
            print("Mouse to far!");
            Mouse.current.WarpCursorPosition(new Vector2(mouseStartPos.x, mouseEndPos.y));
            strengthStage = UIComponents.Length -1;
            mouseInZone = true;
            IsWaiting = true;

        }
        else if (UIInfo.mousePosY <= mouseStartPos.y)
        {
            Mouse.current.WarpCursorPosition(new Vector2(mouseStartPos.x, mouseStartPos.y));
        } 
        else
        {
            Mouse.current.WarpCursorPosition(new Vector2(mouseStartPos.x, UIInfo.mousePosY));
        }
    }

}