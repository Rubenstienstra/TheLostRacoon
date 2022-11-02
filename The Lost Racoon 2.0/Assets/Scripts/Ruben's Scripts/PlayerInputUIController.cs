using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputUIController : MonoBehaviour
{
    public GameObject mouseCursor; // mouse cursor word gepakt in het interact script
    public float mousePosX;
    public float mousePosY;

    public void OnMouseX(InputValue value)
    {
        mousePosX = value.Get<float>();
        MouseMoving();
    }
    public void OnMouseY(InputValue value)
    {
        mousePosY = value.Get<float>();
        MouseMoving();
    }
    public void MouseMoving()
    {
        if(mouseCursor != null)
        {
            mouseCursor.transform.position = new Vector2(mousePosX, mousePosY);
        }
    }
    
}
