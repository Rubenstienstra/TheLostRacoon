using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public float health;
    public Transform player;
    public GameObject camSystem;
    public Transform spawnPoint;
    private void Update() {
        if (health <= 0) {
            //unlock cursor and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //lock movement and cam lookaround
            player.GetComponent<PlayerMovement>().movementLock = true;
            camSystem.SetActive(false);
            //Triger animation
            //enable death UI
        }
    }
    public void Respawn() {
        //reset HP
        //Disable ui
        player.GetComponent<PlayerMovement>().movementLock = false;
        player = spawnPoint;
    }
}
