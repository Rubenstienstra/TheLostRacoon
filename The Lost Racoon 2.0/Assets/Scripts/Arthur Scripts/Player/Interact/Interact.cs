using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public PlayerInput playerInput;
    public InputAction interact;

    // Update is called once per frame
    private void Start() {
        
    }
    void Update()
    {
        float interactInput = playerInput.actions["Interact"].ReadValue<float>();
        if (interactInput == 1) {
            //intraction here
        }
    }
}
