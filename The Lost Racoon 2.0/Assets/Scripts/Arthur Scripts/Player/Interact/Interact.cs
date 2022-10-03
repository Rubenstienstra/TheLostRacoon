using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [Header("References", order = 0)]
    public PlayerInput playerInput;
    public Pickup pickupscript;
    public CamFreezeScript camFreeze;
    public PlayerMovement playerMovementInfo;
    [Header("Detection Sphere", order = 1)]
    public Transform detectionAria;
    public float detectDiameter;
    public Collider[] detectedColliders;
    public bool detected;
    public bool minigameDetected;
    [Header("Interaction Sphere", order = 1)]
    public Transform interactionAria;
    public float interactionDiameter;
    public Collider[] interactionColliders;
    public bool minigameActive;
    public bool minigameBeingPlayed;
    //piemel -davido 
    private void Start()
    {
        if (playerMovementInfo == null)
        {
            GameObject playerMovementInfoHolder = GameObject.Find("racoon lookin ass");
            playerMovementInfo = playerMovementInfoHolder.GetComponent<PlayerMovement>();
        }
    }
    void Update()
    {
        float interactInput = playerInput.actions["Interact"].ReadValue<float>();
        detectedColliders = Physics.OverlapSphere(detectionAria.position, detectDiameter * 2);
        foreach (Collider coll in detectedColliders) {
            if(coll.gameObject.tag == "Interactible" || coll.gameObject.tag == "Item") {
                Debug.Log("Interactible or Item nearby");
                //Ui elliment stage 1
            }
            if(coll.gameObject.tag == "Minigame")
            {
                print("Is in Range!");
                minigameDetected = true;
                //coll.gameObject.SetActive(false);
            }
            else if(minigameDetected != true)
            {
                minigameDetected = false;
                // - Ruben
            }
        }
        interactionColliders = Physics.OverlapSphere(interactionAria.position, interactionDiameter * 2);
        
        foreach (Collider coll in interactionColliders) {
            if (coll.gameObject.tag == "Minigame") {
                Debug.Log("Interactible or Item in reach, press E to interact or pick up");
                //Ui element stage 2
                if (interactInput == 1 && minigameDetected && minigameBeingPlayed == false)
                {
                    playerMovementInfo.OnEnterMinigame();
                    CollidedMinigame(coll);
                  //interaction here
                    Debug.Log("Interacted");
                }
            }
            else if (coll.gameObject.tag == "Item") {
                if (interactInput == 1) {
                    //Pick up here
                    pickupscript.PickUp(coll);
                }
            }
        }
    }
    private void OnDrawGizmos() {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionAria.position, interactionDiameter * 2);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(detectionAria.position, detectDiameter * 2);
    }
    public void CollidedMinigame(Collider coll)
    {
        if (coll.gameObject.GetComponent<MouseTrackerMovement>())
        {
            coll.gameObject.GetComponent<MouseTrackerMovement>().StartAreaMinigame();
        }
        else if (coll.gameObject.GetComponent<MouseTrackerRectangleMovement>())
        {
            coll.gameObject.GetComponent<MouseTrackerRectangleMovement>().StartAreaMinigame();
        }
        else if (coll.gameObject.GetComponent<UIPuzzleColor>())
        {
            coll.gameObject.GetComponent<UIPuzzleColor>().SpawnPuzzleUI();
        }
        
    }
}
