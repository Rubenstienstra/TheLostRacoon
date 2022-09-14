using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    [Header("Debug", order = 0)]
    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    [Header("References", order = 1)]
    public PlayerInput playerInput;

    [Header("References", order = 2)]
    public Rigidbody rb;
    public Rigidbody playerRB;
    public BoxCollider coll;
    public Transform mouth, itemContainer, player;

    public bool equipped;
    public bool slotfull;

    private void Start() {
        if (!equipped) {
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped) {
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotfull = true;
        }
    }
    private void Update() {
        //input
        float interactInput = playerInput.actions["Interact"].ReadValue<float>();
        float dropInput = playerInput.actions["Drop"].ReadValue<float>();
        //Check if item is near hand and player pressed interaction key
        Vector3 distanceToPaw = mouth.position - transform.position;
        if(!equipped && distanceToPaw.magnitude <= pickUpRange && interactInput == 1 && !slotfull) {
            PickUp();
        }
        if(equipped && dropInput == 1) {
            Drop();
        }
    }

    private void PickUp() {
        equipped = true;
        slotfull = true;

        transform.SetParent(itemContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        rb.isKinematic = true;
        coll.isTrigger = true;
    }

    private void Drop() {
        equipped = false;
        slotfull = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.velocity = playerRB.velocity;

        //Add dropforce
        rb.AddForce(player.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(player.forward * dropUpwardForce, ForceMode.Impulse);
    }
}
