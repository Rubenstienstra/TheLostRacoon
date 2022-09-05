using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MouseTrackerMovement : MonoBehaviour
{
    public float mousePos;
    public float mousePosTest;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void OnMouse(InputValue value)
    {
        mousePos = value.Get<float>();
    }
}
