using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour {
    [Header("changeable variables", order = 0)]
    public int speed;
    //Input
    public PlayerInput playerInput;
    public InputAction moveAction;
    public Transform orientation;
    // Update is called once per frame
    private void Start() {
        moveAction = playerInput.actions["WASD"];
    }
    void Update() {

        //Movement
        Vector3 move = new Vector3();
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 inputDir = orientation.forward * -input.x + orientation.right * input.y;
        //move = new Vector3(input.x, 0, input.y).normalized;
        float v = new float();
        float h = new float();
        float j = new float();

        transform.Translate(inputDir.normalized * Time.deltaTime * speed);
    }
}