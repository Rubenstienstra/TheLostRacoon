using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [Header("References", order = 0)]
    public PlayerInput playerInput;
    [Header("Detection Sphere", order = 1)]
    public Transform detectionAria;
    public float detectDiameter;
    [Header("Interaction Sphere", order = 1)]
    public Transform interactionAria;
    public float interactionDiameter;
    bool interacted;
    //piemel -davido 
    private void Start() {
        
    }
    void Update()
    {
        float interactInput = playerInput.actions["Interact"].ReadValue<float>();
        Collider[] detectedColliders = Physics.OverlapSphere(detectionAria.position, detectDiameter * 2);
        foreach (Collider coll in detectedColliders) {
            if(coll.gameObject.tag == "interactible") {
                Debug.Log("Interactible nearby");
                //Ui elliment stage 1
            }
        }
        Collider[] interactionColliders = Physics.OverlapSphere(interactionAria.position, interactionDiameter * 2);
        
        foreach (Collider coll in interactionColliders) {
            if (coll.gameObject.tag == "interactible") {
                Debug.Log("Interactible in reach, press E to interact");
                //Ui elliment stage 2
                if (interactInput == 1) {
                    for (int i = 0; i < interactionColliders.Length; i++) {
                        //
                    }
                }
            }
        }
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionAria.position, detectDiameter * 2);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(interactionAria.position, interactionDiameter * 2);
    }
}
