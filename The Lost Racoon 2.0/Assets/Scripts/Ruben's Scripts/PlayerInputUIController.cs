using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputUIController : MonoBehaviour
{
    public GameObject mouseCursor;
    public float mousePosX;
    public float mousePosY;

    public void Start()
    {
        if(mouseCursor == null)
        {
          mouseCursor = GameObject.Find("MousePointer");
        }
        
    }
    public void OnMouseX(InputValue value)
    {
        mousePosX = value.Get<float>();
        mouseCursor.transform.position = new Vector2(mousePosX, mousePosY);
    }
    public void OnMouseY(InputValue value)
    {
        mousePosY = value.Get<float>();
        mouseCursor.transform.position = new Vector2(mousePosX, mousePosY);
    }
    
}
