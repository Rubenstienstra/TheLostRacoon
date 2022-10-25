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
    public PlayerMovementBetter playerMovementInfo;
    [Header("Detection Sphere", order = 1)]
    public Transform detectionAria;
    public float detectDiameter;
    public Collider[] detectedColliders;
    public bool detected;
    public bool minigameDetected;
    [Header("Interaction Sphere", order = 1)]
    public Vector3 interactionAreaOfset;
    public float interactionDiameter;
    public Collider[] interactionColliders;

    public bool allowInteraction;
    public Collider crCollider;
    
    public bool minigameActive;
    public bool minigameBeingPlayed;
    //piemel -davido 
    private void Start()
    {
        if (playerMovementInfo == null)
        {
            GameObject playerMovementInfoHolder = GameObject.Find("RacoonPlayer");
            playerMovementInfo = playerMovementInfoHolder.GetComponent<PlayerMovementBetter>();
        }
    }
    //void Update()
    //{
    //    float interactInput = playerInput.actions["Interact"].ReadValue<float>();
    //    detectedColliders = Physics.OverlapSphere(detectionAria.position, detectDiameter * 2);
    //    foreach (Collider coll in detectedColliders) {
    //        if(coll.gameObject.tag == "Interactible" || coll.gameObject.tag == "Item") {
    //            Debug.Log("Interactible or Item nearby");
    //            //Ui elliment stage 1
    //        }
    //        if(coll.gameObject.tag == "Minigame")
    //        {
    //            print("Is in Range!");
    //            minigameDetected = true;
    //            //coll.gameObject.SetActive(false);
    //        }
    //        else if(minigameDetected != true)
    //        {
    //            minigameDetected = false;
    //            // - Ruben
    //        }
    //    }
        //interactionColliders = Physics.OverlapSphere(interactionAreaOfset, interactionDiameter * 2);
        
        //foreach (Collider coll in interactionColliders) {
        //    if (coll.gameObject.tag == "Minigame") {
        //        Debug.Log("Interactible or Item in reach, press E to interact or pick up");
        //        //Ui element stage 2
        //        if (interactInput == 1 && minigameDetected && minigameBeingPlayed == false)
        //        {
        //            playerMovementInfo.OnEnterMinigame();
        //            CollidedMinigame(coll);
        //            Debug.Log("Interacted");
        //        }
        //    }
        //    else if (coll.gameObject.tag == "Item") {
        //        if (interactInput == 1) {
        //            //Pick up here
        //            pickupscript.PickUp(coll);
        //        }
        //    }
        //}
    //}
    public void OnInteract(InputValue value)
    {
        if(value.Get<float>() == 1)
        {
            allowInteraction = true;
        }
        else
        {
            allowInteraction = !allowInteraction;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Minigame")
        {
            print(other);
            crCollider = other;
            if (!minigameBeingPlayed)
            {
                playerMovementInfo.OnEnterMinigame();
                Debug.Log("Interacted Minigame");
            }
        }
        else if(other.gameObject.tag == "Item")
        {
            pickupscript.PickUp(other);
        }
    }
}
