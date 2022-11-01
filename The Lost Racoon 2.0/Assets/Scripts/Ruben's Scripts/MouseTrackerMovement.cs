using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MouseTrackerMovement : MonoBehaviour
{
    public ScriptableSaving savingInfo;
    private PlayerInputUIController UIInfo;
    private Interact interactInfo;
    public PlayerMovementBetter playerMovementInfo;

    private float randomMousePosX;
    private float randomMousePosY;

    public Vector2 mouseStartPos;
    public GameObject[] circles;

    // 0 = default, 1 = phase 1, 2 = phase end.
    public int currentPhase;
    public float[] phaseTime;
    public float[] strengthBuff;
    public float timeOutCircle = 0.1f;

                                    //default  up  down  left  Right
    private float minRandomLengthX; //  -2,    -     -    <0     0>
    private float maxRandomLengthX; //   2 ,   -     -     -2    2
    private float minRandomLengthY; //  -2,    0>   <0     -     -
    private float maxRandomLengthY; //   2 ,   2     2     -     -

    private RectTransform bigCircleSchrinkRect;
    private CircleCollider2D bigCircleSchrinkCollider;
    public float circleSchrinkStrengthBuff = 4.76f;

    public bool hasBeenInteracted;
    public GameObject activateGameObject;
    public Rigidbody rb;
    public bool mouseInZone;
    
    void Start()
    {
        //safety
        GameObject playerGameObject = GameObject.Find("RacoonPlayer");
        UIInfo = playerGameObject.GetComponent<PlayerInputUIController>();
        playerMovementInfo = playerGameObject.GetComponent<PlayerMovementBetter>();
        interactInfo = playerGameObject.GetComponent<Interact>();
    }
    //If in Area load this
    public void StartAreaMinigame()
    {
        if (hasBeenInteracted == false)
        {
            for (int i = 0; i < circles.Length; i++)
            {
                circles[i].SetActive(true);
            }
            activateGameObject.SetActive(true);

            Cursor.visible = true;
            Mouse.current.WarpCursorPosition(mouseStartPos);
            UIInfo.mousePosX = mouseStartPos.x;
            UIInfo.mousePosY = mouseStartPos.y;
            currentPhase = 0;

            mouseInZone = true;
            interactInfo.minigameActiveMouseCircle = true;
            hasBeenInteracted = false;

            //safety
            StopCoroutine(CountDown());
            bigCircleSchrinkCollider = circles[0].GetComponent<CircleCollider2D>();
            bigCircleSchrinkRect = circles[0].GetComponent<RectTransform>();

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
        StartCoroutine(MouseMover());
        StartCoroutine(CountDown());
    }
    public IEnumerator MouseMover()
    {     
          randomMousePosX = Random.Range(minRandomLengthX, maxRandomLengthX) * strengthBuff[currentPhase];
          randomMousePosY = Random.Range(minRandomLengthY, maxRandomLengthY) * strengthBuff[currentPhase];
          Mouse.current.WarpCursorPosition(new Vector2(randomMousePosX + UIInfo.mousePosX ,randomMousePosY + UIInfo.mousePosY));

          bigCircleSchrinkRect.sizeDelta = new Vector2(bigCircleSchrinkRect.rect.width - (0.25f * circleSchrinkStrengthBuff), bigCircleSchrinkRect.rect.height - (0.25f * circleSchrinkStrengthBuff));
          bigCircleSchrinkCollider.radius = bigCircleSchrinkRect.rect.width/2;

          yield return new WaitForSeconds(0.01f);//NO CHANGE depents on MouseMover
          if(interactInfo.minigameActiveMouseCircle == true) // double check
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

        //Winning
        if (mouseInZone == true)
        {
            interactInfo.minigameActiveMouseCircle = false;
            playerMovementInfo.movementLock = false;
            savingInfo.totalMissionsCompleted++;
            savingInfo.mouseTrackerTimesDone++;

            gameObject.GetComponent<BoxCollider>().enabled = !enabled;
            rb.constraints = RigidbodyConstraints.None;
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
        if(currentPhase >= circles.Length -1)
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
        yield return new WaitForSeconds(timeOutCircle);
        if(mouseInZone == false)
        {
            print("waiting for shutdown game end");
            ShutDown();
        }
    }
    public void ShutDown()
    {
        if (hasBeenInteracted == false)
        {
            if(interactInfo.minigameBeingPlayed == false) // if he has won:
            {
                hasBeenInteracted = true;
            }
            print("ended minigame at Phase: " + currentPhase.ToString());
            StopCoroutine(CountDown());
            StopCoroutine(MouseMover());

            for (int i = 0; i < circles.Length; i++)
            {
                circles[i].SetActive(false);
            }
            activateGameObject.SetActive(false);

            currentPhase = 0;
            interactInfo.minigameActiveMouseCircle = false;
            bigCircleSchrinkRect.sizeDelta = new Vector2(400, 400); // default size

            interactInfo.minigameBeingPlayed = false;

            interactInfo.OnExitMinigame();
        }
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