using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine.Utility;

public class CamTestScript : MonoBehaviour
{
    public PlayerInput playerInput;
    public InputAction interact;
    public Transform camFollow;
    public InputAction look;
    bool camFroze;
    // Start is called before the first frame update
    void Start()
    {
        camFroze = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        float interactInput = playerInput.actions["Interact"].ReadValue<float>();
        if (interactInput == 1 && !camFroze) {
            
            look.Disable();
            camFollow.parent = null;
            camFroze = true;
            Debug.Log("Cam pos locked");
        }
        if (interactInput == 1 && camFroze) {
            camFollow.parent = this.transform;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            look.Enable();
            Debug.Log("Cam pos unlocked");
        }
      
    }
}
