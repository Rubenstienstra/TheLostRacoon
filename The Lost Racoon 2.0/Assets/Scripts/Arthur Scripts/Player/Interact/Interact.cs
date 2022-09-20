using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [Header("References", order = 0)]
    public PlayerInput playerInput;
    public Pickup pickupscript;
    [Header("Detection Sphere", order = 1)]
    public Transform detectionAria;
    public float detectDiameter;
    public Collider[] detectedColliders;
    public bool detected;
    [Header("Interaction Sphere", order = 1)]
    public Transform interactionAria;
    public float interactionDiameter;
    public Collider[] interactionColliders;
    //piemel -davido 
    private void Start() {
        
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
        }
        interactionColliders = Physics.OverlapSphere(interactionAria.position, interactionDiameter * 2);
        
        foreach (Collider coll in interactionColliders) {
            if (coll.gameObject.tag == "Interactible") {
                Debug.Log("Interactible or Item in reach, press E to interact or pick up");
                //Ui elliment stage 2
                if (interactInput == 1) {
                    //interaction here
                    Debug.Log("Interacted");
                    //coll.gameObject.SetActive(false);
                }
            }else if (coll.gameObject.tag == "Item") {
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
}
