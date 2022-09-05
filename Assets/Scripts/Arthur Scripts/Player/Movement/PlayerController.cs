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
    public InputAction playerControls;
    // Update is called once per frame
    private void Start() {
        cameraPitch = 0f;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable() {
        playerControls.Enable();
    }
    private void OnDisable() {
        playerControls.Disable();
    }
    void Update() {

        //Movement
        Vector3 move = new Vector3();
        Vector3 rotateBody = new Vector3();
        Vector3 rotateCam = new Vector3();
        move = playerControls.ReadValue<Vector2>();
        float v = new float();
        float h = new float();
        float j = new float();
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        move.x = h;
        move.z = v;
        transform.Translate(move * Time.deltaTime * speed);

        //Mouse
        float mouseX = new float();
        float mouseY = new float();
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        rotateCam.x = mouseY;
        rotateBody.y = mouseX;
        transform.Rotate(rotateBody * camSensitivity);
        //Camera clamping
        cameraPitch -= mouseY * camSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        cam.localEulerAngles = new Vector3(cameraPitch, 0, 0);
    }
}
