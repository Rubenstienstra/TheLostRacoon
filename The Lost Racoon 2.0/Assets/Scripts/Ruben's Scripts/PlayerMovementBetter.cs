using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementBetter : MonoBehaviour
{
    public float[] forwardWASD; 
    public void OnForward(InputValue value)
    {
       forwardWASD[0] = value.Get<float>();
    }
    
    public void OnLeft(InputValue value)
    {
        forwardWASD[1] = value.Get<float>();
    }
    public void OnDown(InputValue value)
    {
        forwardWASD[2] = value.Get<float>();
    }
    public void OnRight(InputValue value)
    {
        forwardWASD[3] = value.Get<float>();
    }
    
}
