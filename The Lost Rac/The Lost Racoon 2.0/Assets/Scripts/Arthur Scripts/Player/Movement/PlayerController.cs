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
    // Update is called once per frame
    private void Start() {
        moveAction = playerInput.actions["WASD"];
    }
    void Update() {

        //Movement
        Vector3 move = new Vector3();
        Vector2 input = moveAction.ReadValue<Vector2>();
        move = new Vector3(input.x, 0, input.y).normalized;
        float v = new float();
        float h = new float();
        float j = new float();

        transform.Translate(move * Time.deltaTime * speed);
    }
}
