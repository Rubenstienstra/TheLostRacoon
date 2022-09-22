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
    [Header("Changeable", order = 1)]
    public float speed;
    public float sprintSpeed;
    public float jumpCharge, jumpMin, jumpMax, chargePerSec;
    [Header("Debug", order = 2)]
    public bool movementLock;
    public Vector3 moveDir;
    public float verticalVelocity = 0f;
    public float gravityValue = 9.81f;
    public Vector3 playerVelocity;
    float turnSmoothVelocity;
    public Collider[] coll;
    public bool isGrounded;
    bool charged;

    private void Start() {
        moveAction = playerInput.actions["WASD"];
        sprintAction = playerInput.actions["Sprint"];
        jumpAction = playerInput.actions["Jump"];
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        jumpCharge = jumpMin;
    }
    private void Update() {
        //input
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
        float sprintInput = sprintAction.ReadValue<float>();
        float jumpInput = jumpAction.ReadValue<float>();
        transform.parent.position = transform.position;

        bool groundedPlayer = controller.isGrounded;

        // gravity
        coll = Physics.OverlapSphere(transform.position, 0.43f);
        if (coll.Length >= 3) {
            playerVelocity.y = 0;
            isGrounded = true;
        } else {
            isGrounded = false;
        }
        //verticalVelocity -= gravityValue * Time.deltaTime;


        if (!movementLock) {
            //Movement
            if (direction.magnitude >= 0.1f) {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                if (sprintInput == 1) {
                    moveDir.x = moveDir.x * sprintSpeed;
                    moveDir.z = moveDir.z * sprintSpeed;
                    controller.Move(moveDir * Time.deltaTime);
                    verticalVelocity = 0f;
                } else { //Sprint
                    moveDir.x = moveDir.x * speed;
                    moveDir.z = moveDir.z * speed;
                    controller.Move(moveDir * speed * Time.deltaTime);
                    verticalVelocity = 0f;
                }
            } else {
                moveDir.x = 0f;
                moveDir.z = 0f;
                verticalVelocity = 0f;
            }
            if (jumpInput == 1 && isGrounded == true) {
                if (jumpCharge <= jumpMax) {
                    jumpCharge += chargePerSec * Time.deltaTime;
                }
                charged = true;
            }
            if (jumpInput == 0 && charged == true) {
                playerVelocity.y += Mathf.Sqrt(jumpCharge * -7.0f * gravityValue);
                jumpCharge = jumpMin;
                charged = false;
            }
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    private void OnDrawGizmos() {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.43f);
    }
}
