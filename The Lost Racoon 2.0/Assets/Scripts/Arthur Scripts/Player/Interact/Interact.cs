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
    public PlayerInputUIController UIInfo;
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
    
    public bool minigameBeingPlayed;
    public bool minigameActiveMouseCircle;
    public bool minigameActiveMouseRectangle;
    public bool minigameActive3x3Puzzle;
    //piemel -davido 
    void FixedUpdate()
    {
        if (playerMovementInfo.moving)
        {
            detectedColliders = Physics.OverlapSphere(detectionAria.position, detectDiameter * 2);
            foreach (Collider col in detectedColliders)
            {
                if (col.gameObject.tag == "Minigame")
                {
                    minigameDetected = true;
                    DetectedMinigame(col.gameObject,"Blue");
                }
                else if (minigameDetected != true)
                {
                    minigameDetected = false;
                }
            }
            foreach (Collider coll in interactionColliders)
            {
                if (coll.gameObject.tag == "Minigame")
                {
                    if (minigameDetected && !minigameBeingPlayed)
                    {
                        DetectedMinigame(coll.gameObject,"Red");
                    }
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
                    if (minigameDetected && !minigameBeingPlayed)
                    {
                        OnEnterMinigame();
                        CollidedMinigame(coll.gameObject);
                    }
                }
            }
            allowInteraction = true;
        }
        else
        {
            allowInteraction = !allowInteraction;
        }
    }
    public void DetectedMinigame(GameObject minigame, string color)
    {
        if (minigame.GetComponent<MouseTrackerMovement>())
        {
            if (color == "Blue")
            {
                minigame.GetComponent<MouseTrackerMovement>().showingCircleUI.SetActive(true);
                minigame.GetComponent<MouseTrackerMovement>().showingEUI.SetActive(false);
            }
            if (color == "Red")
            {
                minigame.GetComponent<MouseTrackerMovement>().showingEUI.SetActive(true);
                minigame.GetComponent<MouseTrackerMovement>().showingCircleUI.SetActive(false);
            }
        }
        else if (minigame.GetComponent<MouseTrackerRectangleMovement>())
        {
            if(color == "Blue")
            {
                minigame.GetComponent<MouseTrackerRectangleMovement>().showingCircleUI.SetActive(true);
                minigame.GetComponent<MouseTrackerRectangleMovement>().showingEUI.SetActive(false);

            }
            if(color == "Red")
            {
                minigame.GetComponent<MouseTrackerRectangleMovement>().showingEUI.SetActive(true);
                minigame.GetComponent<MouseTrackerRectangleMovement>().showingCircleUI.SetActive(false);
            }
        }
        else if (minigame.GetComponent<Better3X3Puzzle>())
        {
            if (color == "Blue")
            {
                minigame.GetComponent<Better3X3Puzzle>().showingCircleUI.SetActive(true);
                minigame.GetComponent<Better3X3Puzzle>().showingEUI.SetActive(false);
            }
            if (color == "Red")
            {
                minigame.GetComponent<Better3X3Puzzle>().showingEUI.SetActive(true);
                minigame.GetComponent<Better3X3Puzzle>().showingCircleUI.SetActive(false);
            }
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
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Minigame")
    //    {
    //        print(other);
    //        crCollider = other;
    //        if (!minigameBeingPlayed)
    //        {
    //            OnEnterMinigame();
    //            Debug.Log("Interacted Minigame");
    //        }
    //    }
    //    //else if (other.gameObject.tag == "Item")
    //    //{
    //    //    pickupscript.PickUp(other);
    //    //}
    //}
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
