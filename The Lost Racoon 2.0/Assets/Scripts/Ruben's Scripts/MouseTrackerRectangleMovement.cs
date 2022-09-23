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

    public GameObject mouseCursor;
    public float mousePosX;
    public float mousePosY;

    public float randomMousePosY;

    public Vector2 mouseStartPos;
    public Vector2 mouseEndPos;
    public GameObject[] squares;

    // 0 = default, 1 = phase 1, 2 = phase end.
    public float[] strengthBuff;
    public float waitingTime;
    public bool IsWaiting;

                                    //   up  down  
    private float minRandomLengthY; //   0>   <0
    private float maxRandomLengthY; //   2     2

    public Slider movingSlider;

    public bool mouseInZone;
    public int strengthStage;

    public bool ActivateOption1;
    public float strengthDebuff;
    public float totalMousePos;

    void Start()
    {
        //start settings
        if(ActivateOption1 == false)
        {
            minRandomLengthY = 0;
            maxRandomLengthY = -4;//6
        }
        
        
        
    }
    //If in Area load this
    public void StartAreaMinigame()
    {
        for (int i = 0; i < squares.Length; i++)
        {
            squares[i].SetActive(true);
        }
        movingSlider.gameObject.SetActive(true);

        Mouse.current.WarpCursorPosition(mouseStartPos);
        mousePosX = mouseStartPos.x;
        mousePosY = mouseStartPos.y;
        strengthStage = 0;

        mouseInZone = true;
        playerInfo.minigameActiveMouseRectangle = true;
        mouseCursor.GetComponent<Image>().enabled = enabled; //!

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

    public void OnMouseX(InputValue value)
    {
        mousePosX = value.Get<float>();
        if(playerInfo.minigameActiveMouseRectangle == true)
        {
            ReconnectPosition();
        }
    }
    public void OnMouseY(InputValue value)
    {
        mousePosY = value.Get<float>();
    }
    //else
    //{
    //    randomMousePosY = Random.Range(minRandomLengthY, maxRandomLengthY) * strengthBuff[strengthStage];
    //    Mouse.current.WarpCursorPosition(new Vector2(mouseStartPos.x, randomMousePosY + mousePosY));
    //}
    //formule: player pos + moustartpos /2 = gemmidelde pos: zorgt er voor dat als je hoger gaat het getal verschil steeds hoger wordt. Met strength Debuff maak je het nog getal kleiner.
    //randomMousePosY = mouseStartPos.y + mousePosY;
    //randomMousePosY /= 2;
    //randomMousePosY /= strengthDebuff;
    //totalMousePos = mousePosY - randomMousePosY;
    public IEnumerator MouseMover()
    {
        if (ActivateOption1 == true)
        {
            randomMousePosY = mousePosY - mouseStartPos.y;
            randomMousePosY /= strengthDebuff;
            totalMousePos = randomMousePosY;
        }

        movingSlider.value = mousePosY - mouseStartPos.y;
        if (mousePosY > mouseStartPos.y)
        {
            Mouse.current.WarpCursorPosition(new Vector2(mouseStartPos.x, totalMousePos));
        }
        if (playerInfo.minigameActiveMouseRectangle == true) // double check
        {
            StartCoroutine(MouseMover());
        }
        if (mouseInZone == false)
        {
            ReconnectPosition();
        }
        if (strengthStage == squares.Length - 1 && IsWaiting == false)
        {
            IsWaiting = true;
            StartCoroutine(WaitingForShutDown());
        }
        yield return new WaitForSeconds(0.01f);//NO CHANGE depents on MouseMover
    }
    public IEnumerator WaitingForShutDown()
    {
        print("activating shutdown");
        yield return new WaitForSeconds(waitingTime);
        if (strengthStage == squares.Length -1)
        {
            ShutDown();
        }
        IsWaiting = false;
    }
    public void ShutDown()
    {
        print("ended minigame at Phase: " + strengthStage.ToString() + " Victory!");
        StopCoroutine(MouseMover());
        StopCoroutine(WaitingForShutDown());

        for (int i = 0; i < squares.Length; i++)
        {
            squares[i].SetActive(false);
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
        if(mousePosY >= mouseEndPos.y)
        {
            print("Mouse to far!");
            Mouse.current.WarpCursorPosition(new Vector2(mouseStartPos.x, mouseEndPos.y));
            strengthStage = squares.Length -1;
            mouseInZone = true;
            IsWaiting = true;

        }
        else if (mousePosY <= mouseStartPos.y)
        {
            Mouse.current.WarpCursorPosition(new Vector2(mouseStartPos.x, mouseStartPos.y));
        } 
        else
        {
            Mouse.current.WarpCursorPosition(new Vector2(mouseStartPos.x, mousePosY));
        }
    }

}