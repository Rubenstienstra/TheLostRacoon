using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MouseTrackerRectangleMovement : MonoBehaviour
{
    public ScriptableSaving savingInfo;
    private PlayerScript playerInfo;
    private PlayerInputUIController UIInfo;
    private Interact interactInfo;
    private PlayerMovementBetter playerMovementInfo;

    private GameObject mouseCursor;
    private GameObject playerGameObject;
    public GameObject parentGameobject;

    public GameObject showingCircleUI;
    public GameObject showingEUI;

    //public float randomMousePosY;

    public Vector2 mouseStartPos;
    public Vector2 mouseEndPos;
    public GameObject[] UIComponents;

    // 0 = default, 1 = phase 1, 2 = phase end.
    public float waitingTime;
    public bool IsWaiting;
    public bool hasBeenInteracted;

    public bool isParentContainer;
    public int timesMinigameDone;

    public Slider movingSlider;
    public float endPosZoneY;
    public float endPosModifier;
    public bool mouseInZone;

    public float strengthDebuff;
    public float totalMousePos;

    public Rigidbody rig;

    void Start()
    {
       playerGameObject = GameObject.Find("RacoonPlayer");

        UIInfo = playerGameObject.GetComponent<PlayerInputUIController>();
        playerMovementInfo = playerGameObject.GetComponent<PlayerMovementBetter>();
        playerInfo = playerGameObject.GetComponent<PlayerScript>();
        interactInfo = playerGameObject.GetComponent<Interact>();

        //mouseCursor = GameObject.Find("MousePointer");
    }
    //If in Area load this
    public void StartAreaMinigame()
    {
        if (hasBeenInteracted == false)
        {
            for (int i = 0; i < UIComponents.Length; i++)
            {
                UIComponents[i].SetActive(true);
            }
            movingSlider.gameObject.SetActive(true);
            if(showingCircleUI != null && showingEUI != null)
            {
                showingCircleUI.SetActive(false);
                showingEUI.SetActive(false);
            }

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
            Mouse.current.WarpCursorPosition(mouseStartPos);
            UIInfo.mousePosX = mouseStartPos.x;
            UIInfo.mousePosY = mouseStartPos.y;

            mouseInZone = true;
            playerInfo.minigameActiveMouseRectangle = true;

            //mouseCursor.GetComponent<Image>().enabled = !enabled;
            
            //safety
            if (waitingTime == 0)
            {
                waitingTime = 0.01f;
            }
            if (strengthDebuff == 0)
            {
                strengthDebuff = 1;
            }
            movingSlider.maxValue = mouseEndPos.y - mouseStartPos.y;
            endPosZoneY = mouseEndPos.y - (mouseEndPos.y / endPosModifier);

            StartMinigame();
        }
        else
        {
            interactInfo.OnExitMinigame();
        }
    }
    // Use Interactable enter
    public void StartMinigame()
    {
        playerMovementInfo.movementLock = true;
        playerMovementInfo.ResettingAllAnimations();
        StartCoroutine(MouseMover());
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
            UIInfo.mousePosY = totalMousePos + UIInfo.mousePosY;
        }
        yield return new WaitForSeconds(0.01f);//NO CHANGE depents on MouseMover

        if (playerInfo.minigameActiveMouseRectangle == true) // double check
        {
            print("Reactivated");            
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
        if (UIInfo.mousePosY > endPosZoneY && IsWaiting == false)
        {
            IsWaiting = true;
            StartCoroutine(WaitingForShutDown());
        }
        else if (UIInfo.mousePosY < endPosZoneY)
        {
            IsWaiting = false;
        }
        
    }
    public IEnumerator WaitingForShutDown()
    {
        print("Activating WaitingShutdown");
        yield return new WaitForSeconds(waitingTime);
        if (UIInfo.mousePosY > endPosZoneY && IsWaiting == true)
        {
            ShutDown();
        }
        else
        {
            StopCoroutine(WaitingForShutDown());
        }
        
    }
    public void ShutDown()
    {
        StopCoroutine(MouseMover());
        StopCoroutine(WaitingForShutDown());

        for (int i = 0; i < UIComponents.Length; i++)
        {
            UIComponents[i].SetActive(false);
        }
        movingSlider.gameObject.SetActive(false);
        //mouseCursor.GetComponent<Image>().enabled = enabled;

        playerInfo.minigameActiveMouseRectangle = false;
        interactInfo.minigameActiveMouseCircle = false;

        interactInfo.minigameBeingPlayed = false;

        playerMovementInfo.movementLock = false;
        playerMovementInfo.moving = false;
        playerMovementInfo.ResettingAllAnimations();

        //Completed
        if (mouseInZone == true)
        {
            if (hasBeenInteracted == false)
            {
                hasBeenInteracted = true;
                playerInfo.minigameActiveMouseRectangle = false;
                showingCircleUI.GetComponentInParent<GameObject>().SetActive(false);
                savingInfo.totalMissionsCompleted++;
                savingInfo.mouseTrackerTimesDone++;
                OnMinigameCompleted();

                interactInfo.OnExitMinigame();
                print("Completed/Victory!:D");
            }
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
    public void OnMinigameCompleted()
    {
        if (parentGameobject.GetComponent<MouseTrackerRectangleMovement>().timesMinigameDone < 4 -1)
        {
            parentGameobject.GetComponent<MouseTrackerRectangleMovement>().timesMinigameDone++;
        }
        else
        {
            rig.constraints = RigidbodyConstraints.None;
            rig.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}