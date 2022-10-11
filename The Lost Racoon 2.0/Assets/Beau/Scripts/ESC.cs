using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ESC : MonoBehaviour
{
    public GameObject esc;
     void OnESC()
    {
        esc.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
