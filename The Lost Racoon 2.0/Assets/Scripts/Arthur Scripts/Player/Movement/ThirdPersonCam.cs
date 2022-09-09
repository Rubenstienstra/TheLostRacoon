using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References", order = 0)]
    public PlayerInput playerInput;
    public InputAction moveAction;
    public InputAction sprintAction;
    public CharacterController controller;
    public Transform cam;
    [Header("Debug", order = 1)]
    public int speed;
    public int sprintSpeed;
    float turnSmoothVelocity;

    private void Start() {
        moveAction = playerInput.actions["WASD"];
        sprintAction = playerInput.actions["Sprint"];
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update() {
        controller.transform.position = transform.position;
        Vector2 input = moveAction.ReadValue<Vector2>();
        
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;

        //Sprint
        float sprint = sprintAction.ReadValue<float>();
        
        if(direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (sprint == 0) {
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            } else {
                controller.Move(moveDir.normalized * sprintSpeed * Time.deltaTime);
            }
        }
    }
}
