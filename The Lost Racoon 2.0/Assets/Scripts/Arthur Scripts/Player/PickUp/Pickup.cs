using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    [Header("Debug", order = 0)]
    public float dropForwardForce;
    public float dropUpwardForce;
    public bool slotfull;

    [Header("input", order = 1)]
    public PlayerInput playerInput;

    [Header("References", order = 2)]
    public Transform itemContainer;
    private Collider item;
    private Rigidbody rb;

    private void Start() {
    }
    private void Update() {
        if(slotfull && playerInput.actions["Drop"].ReadValue<float>() == 1) {
            Drop();
        }
    }

    public void PickUp(Collider coll) {
        if (!slotfull) {
            //bool for if slot is full
            slotfull = true;
            //getting the items references
            item = coll;
            rb = item.GetComponent<Rigidbody>();
            //making it only visible
            item.isTrigger = true;
            rb.isKinematic = true;
            //putting it in the actual slot
            item.transform.SetParent(itemContainer);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.Euler(Vector3.zero);
            item.transform.localScale = Vector3.one;
            Debug.Log("Picked up");
        } else {
            Debug.Log("Can not pick up item because item = " + item + ", not null so an item is already in the slot");
        }
    }

    private void Drop() {
        item.transform.SetParent(null);

        rb.isKinematic = false;
        item.isTrigger = false;

        //rb.velocity = this.GetComponent<Rigidbody>().velocity;

        //Add dropforce
        rb.AddForce(this.transform.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(this.transform.forward * dropUpwardForce, ForceMode.Impulse);
        slotfull = false;
        Debug.Log("Dropped Item: " + item);
        item = null;
    }
}
