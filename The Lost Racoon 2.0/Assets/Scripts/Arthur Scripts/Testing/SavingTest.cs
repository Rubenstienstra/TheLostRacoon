using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SavingTest : MonoBehaviour, IDataPresistence
{
    public PlayerInput playerInput;
    public int timesJumped;
    private bool jumped;
    private void Update() {
        if(playerInput.actions["Jump"].ReadValue<float>() == 1 && !jumped) {
            timesJumped++;
            jumped = true;
        }
        if(playerInput.actions["Jump"].ReadValue<float>() == 0 && jumped) {
            jumped = false;
        }
        
    }
    public void LoadData(GameData data) {
        this.timesJumped = data.timesJumped;
    }

    public void SaveData(ref GameData data) {
        data.timesJumped = this.timesJumped;
    }
}
