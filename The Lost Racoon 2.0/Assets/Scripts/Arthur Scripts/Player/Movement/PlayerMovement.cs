using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References", order = 0)]
    public PlayerInput playerInput;
    public InputAction moveAction, sprintAction, jumpAction;
    public CharacterController controller;
    public Transform cam;
    public Rigidbody rb;
    [Header("Changeable", order = 1)]
    public float speed;
    public float sprintSpeed;
    public float jumpCharge, jumpMin, jumpMax, chargePerSec;
    [Header("Debug", order = 2)]
    public bool movementLock;
    float turnSmoothVelocity;
    bool isGrounded;
    bool charged;
    
    Vector3 jump;

    private void Start() {
        moveAction = playerInput.actions["WASD"];
        sprintAction = playerInput.actions["Sprint"];
        jumpAction = playerInput.actions["Jump"];
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        jump = new Vector3(0.0f, 0.2f, 0.0f);
    }
    private void Update() {
        //input
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        float sprintInput = sprintAction.ReadValue<float>();
        float jumpInput = jumpAction.ReadValue<float>();

        if (!movementLock) {
            //Movement
            if (direction.magnitude >= 0.1f) {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                if (sprintInput == 0) {
                    controller.Move(moveDir.normalized * speed * Time.deltaTime);
                } else { //Sprint
                    controller.Move(moveDir.normalized * sprintSpeed * Time.deltaTime);
                }
            }
            //controller.transform.position = transform.position;
            if (jumpInput == 1 && isGrounded == true) {
                if (jumpCharge <= jumpMax) {
                    jumpCharge += chargePerSec * Time.deltaTime;
                }
                charged = true;
            }
            if (jumpInput == 0 && charged == true) {
                rb.AddForce(jump * jumpCharge, ForceMode.Impulse);
                jumpCharge = jumpMin;
                charged = false;
                isGrounded = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Ground") {
            isGrounded = true;
        }
    }
}
