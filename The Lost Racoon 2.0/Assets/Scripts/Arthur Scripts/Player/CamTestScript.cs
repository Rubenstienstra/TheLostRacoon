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
      
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "CamFreezeZone" && camFroze) {
            camFollow.position = Vector3.zero;

            transform.position = Vector3.zero;
            camFollow.parent = this.transform;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            look.Enable();
            camFroze = false;
            Debug.Log("Cam pos unlocked");
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "CamFreezeZone" && !camFroze) {
            look.Disable();
            camFollow.parent = null;
            camFroze = true;
            Debug.Log("Cam pos locked");
        }
    }
}
