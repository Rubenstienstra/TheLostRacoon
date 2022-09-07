using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour {
    [Header("changeable variables", order = 0)]
    public int speed;
    [Header("Debug variables", order = 1)]
    public float cameraPitch = 0.0f;
    [Header("Config variables", order = 2)]
    public int camSensitivity;
    public Transform cam;
    public Rigidbody rb;
    //Input
    public PlayerInput playerInput;
    public InputAction moveAction;
    // Update is called once per frame
    private void Start() {
        moveAction = playerInput.actions["WASD"];
        cameraPitch = 0f;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update() {

        //Movement
        Vector3 move = new Vector3();
        Vector2 input = moveAction.ReadValue<Vector2>();
        move = new Vector3(input.x, 0, input.y).normalized;
        float v = new float();
        float h = new float();
        float j = new float();


        if (move.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            transform.Translate(move * Time.deltaTime * speed);
            
        }
    }
}
