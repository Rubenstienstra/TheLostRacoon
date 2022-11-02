using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [Header("References", order = 0)]
    public Pickup pickupscript;
    public CamFreezeScript camFreezeInfo;
    public PlayerMovementBetter playerMovementInfo;
    private PlayerInputUIController UIInfo;
    [Header("Detection Sphere", order = 1)]
    public Transform detectionAria;
    public float detectDiameter;
    public Collider[] detectedColliders;
    public bool minigameDetected;
    [Header("Interaction Sphere", order = 1)]
    public Transform sphereLocation;
    public float interactionDiameter;
    public Collider[] interactionColliders;

    public bool allowInteraction;
    public Collider crCollider;
    
    public bool minigameActive;
    public bool minigameBeingPlayed;
    public bool minigameActiveMouseCircle;
    public bool minigameActiveMouseRectangle;
    public bool minigameActive3x3Puzzle;
    //piemel -davido 
    void Update()
    {
        if (playerMovementInfo.moving)
        {
            detectedColliders = Physics.OverlapSphere(detectionAria.position, detectDiameter * 2);
            foreach (Collider coll in detectedColliders)
            {
                if (coll.gameObject.tag == "Interactible" || coll.gameObject.tag == "Item")
                {
                    //Ui elliment stage 1
                }
                if (coll.gameObject.tag == "Minigame")
                {
                    minigameDetected = true;
                }
                else if (minigameDetected != true)
                {
                    minigameDetected = false;
                }
            }
            interactionColliders = Physics.OverlapSphere(sphereLocation.position, interactionDiameter * 2);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphereLocation.position, interactionDiameter * 2);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(detectionAria.position, detectDiameter * 2);
    }
    public void OnInteract(InputValue value)
    {
        if(value.Get<float>() == 1)
        {
            foreach (Collider coll in interactionColliders)
            {
                if (coll.gameObject.tag == "Minigame")
                {
                    //Ui element stage 2
                    if (minigameDetected && !minigameBeingPlayed)
                    {
                        OnEnterMinigame();
                        CollidedMinigame(coll.gameObject);
                    }
                }
                else if (coll.gameObject.tag == "Item")
                {
                    //Pick up here
                    pickupscript.PickUp(coll);
                }
            }
            allowInteraction = true;
        }
        else
        {
            allowInteraction = !allowInteraction;
        }
    }

    public void CollidedMinigame(GameObject minigame) //elke keer dat er nieuwe minigame komt moet hier een nieuwe GetComponent te staan.
    {
        if (minigame.GetComponent<MouseTrackerMovement>())
        {
            minigameActiveMouseCircle = true;
            minigame.GetComponent<MouseTrackerMovement>().StartAreaMinigame();
             
        }
        else if (minigame.GetComponent<MouseTrackerRectangleMovement>())
        {
            minigameActiveMouseRectangle = true;
            minigame.GetComponent<MouseTrackerRectangleMovement>().StartAreaMinigame();
        }
        else if (minigame.GetComponent<Better3X3Puzzle>())
        {
            minigameActive3x3Puzzle = true;
            minigame.GetComponent<Better3X3Puzzle>().SpawnPuzzleUI();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Minigame")
        {
            print(other);
            crCollider = other;
            if (!minigameBeingPlayed)
            {
                OnEnterMinigame();
                Debug.Log("Interacted Minigame");
            }
        }
        else if (other.gameObject.tag == "Item")
        {
            pickupscript.PickUp(other);
        }
    }
    public void OnEnterMinigame()
    {
        Cursor.lockState = CursorLockMode.None;
        playerMovementInfo.movementLock = true;
        minigameBeingPlayed = true;
        camFreezeInfo.CamFreeze();
    }
    public void OnExitMinigame()
    {
        Cursor.visible = false;
        playerMovementInfo.movementLock = false;
        Cursor.lockState = CursorLockMode.Locked;
        minigameBeingPlayed = false;
        camFreezeInfo.CamUnfreeze();

        minigameActive3x3Puzzle = false;
        minigameActiveMouseCircle = false;
        minigameActiveMouseRectangle = false;
    }
}
