using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MouseTrackerRectangleMovement : MonoBehaviour
{
    public PlayerScript playerInfo;
    public ScriptableSaving savingInfo;
    public MouseTrackerMovement circleMouseInfo;

    public GameObject mouseCursor;
    public float mousePosX;
    public float mousePosY;
    private float randomMousePosX;
    private float randomMousePosY;

    public Vector2 mouseStartPos;
    public GameObject[] squares;

    // 0 = default, 1 = phase 1, 2 = phase end.
    public float[] strengthBuff;
    public float waitingTime;

                                    //   up  down  
    private float minRandomLengthY; //   0>   <0
    private float maxRandomLengthY; //   2     2


    void Start()
    {
        //saving data
        minRandomLengthY = 0;
        maxRandomLengthY = -4;//6
    }
    //If in Area load this
    public void StartAreaMinigame()
    {
        for (int i = 0; i < squares.Length; i++)
        {
            squares[i].SetActive(true);
        }

        Mouse.current.WarpCursorPosition(mouseStartPos);
        mousePosX = mouseStartPos.x;
        mousePosY = mouseStartPos.y;
        savingInfo.strengthStage = 0;

        savingInfo.mouseInZone = true;
        playerInfo.minigameActiveMouse = true;

        StartMinigame();

        //safety
        if (waitingTime == 0)
        {
            waitingTime = 0.01f;
        }

    }
    // Use Interactable enter
    public void StartMinigame()
    {
        if (playerInfo.minigameActiveMouse == true)
        {
            StartCoroutine(MouseMover());
        }
    }

    public void OnMouseX(InputValue value)
    {
        mousePosX = value.Get<float>();
        mouseCursor.transform.position = new Vector2(mousePosX, mousePosY);
    }
    public void OnMouseY(InputValue value)
    {
        mousePosY = value.Get<float>();
        mouseCursor.transform.position = new Vector2(mousePosX, mousePosY);
    }
    public IEnumerator MouseMover()
    {
        randomMousePosY = Random.Range(minRandomLengthY, maxRandomLengthY) * strengthBuff[savingInfo.strengthStage];
        Mouse.current.WarpCursorPosition(new Vector2(mousePosX, randomMousePosY + mousePosY));

        yield return new WaitForSeconds(0.01f);//NO CHANGE depents on MouseMover
        if (playerInfo.minigameActiveMouse == true) // double check
        {
            StartCoroutine(MouseMover());
        }
        if (savingInfo.mouseInZone == false)
        {
            StartCoroutine(WaitingForShutDown());
        }
    }
    public IEnumerator WaitingForShutDown()
    {
        yield return new WaitForSeconds(waitingTime);
        if (savingInfo.mouseInZone == false)
        {
            ShutDown();
        }
    }
    public void ShutDown()
    {
        print("ended minigame at Phase: " + savingInfo.strengthStage.ToString());
        StopCoroutine(MouseMover());
        for (int i = 0; i < squares.Length; i++)
        {
            squares[i].SetActive(false);
        }
        savingInfo.strengthStage = 0;
        playerInfo.minigameActiveMouse = false;

        //Completed
        if (savingInfo.mouseInZone == true)
        {
            playerInfo.minigameActiveMouse = false;
            savingInfo.totalMissionsCompleted++;
            savingInfo.mouseTrackerTimesDone++;
            print("Completed/Victory!:D");
        }
    }
    public void InSquare()
    {
        StopCoroutine(WaitingForShutDown());
        savingInfo.mouseInZone = true;
    }
    public void OutOfSquare()
    {
        StartCoroutine(WaitingForShutDown());
        savingInfo.mouseInZone = false;
    }

}
