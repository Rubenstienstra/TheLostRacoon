using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIPuzzleColor : MonoBehaviour
{
    //public PlayerScript playerInfo;
    public ScriptableSaving savingInfo;
    public PlayerMovementBetter playerMovementInfo;

    public PlayerScript playerInfo;
    public GameObject[] colorButtons;

    public bool startRed;
    public int totalButtons;
    public int totalButtonsCorrect;
    public bool is3x3Minigame;
    public bool is4x4Minigame;

    public GameObject currentGameObject;
    public GameObject UIPuzzleGameObject;
    public GameObject gatePuzzleGameObject;

    public bool hasBeenInteracted;

    private void Awake()
    {
        if (is3x3Minigame == true)
        {
            totalButtons = 9;
        }
        else if (is4x4Minigame == true)
        {
            totalButtons = 16;
        }
    }
    void Start()
    {
        //find main player
        GameObject playerScript = GameObject.Find("RacoonPlayer");
        playerInfo = playerScript.GetComponent<PlayerScript>();
        playerMovementInfo = playerScript.GetComponent<PlayerMovementBetter>();
        
        ResetColors();
    }
    public void OnButtonColorPress()
    {
        //changing colors and to neighbors
        for (int i = 0; i < colorButtons.Length; i++)
        {
            if(colorButtons[i].GetComponent<Image>().color == Color.red)
            {
                colorButtons[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                colorButtons[i].GetComponent<Image>().color = Color.red;
            }
        }
        if (this.gameObject.GetComponent<Image>().color == Color.red)
        {
            this.gameObject.GetComponent<Image>().color = Color.white;
        }
        else
        {
            this.gameObject.GetComponent<Image>().color = Color.red;
        }

        //Checking witch buttons is color.Red
        //need +1 because button 1 is not 0
        for (int i = 1; i < totalButtons +1; i++)
        {
          currentGameObject = GameObject.Find("Color Change Button " + i.ToString());
            if (currentGameObject.GetComponent<Image>().color == Color.red)
            {
                totalButtonsCorrect++;
                if(totalButtonsCorrect >= totalButtons)
                {
                    gatePuzzleGameObject.GetComponent<UIPuzzleColor>().hasBeenInteracted = true;
                    savingInfo.totalMissionsCompleted++;
                    UIPuzzleGameObject.SetActive(false);
                    playerMovementInfo.OnExitMinigame();

                }
            }
        }
        totalButtonsCorrect = 0;
    }
    public void ResetColors()
    {
        for (int i = 0; i < colorButtons.Length; i++)
        {
            colorButtons[i].GetComponent<UIPuzzleColor>().ResetCurrent();
        }
    }
    public void ResetCurrent()
    {
        if (startRed == true)
        {
            this.gameObject.GetComponent<Image>().color = Color.red;
        }
        else
        {
            this.gameObject.GetComponent<Image>().color = Color.white;
        }
    }
    public void SpawnPuzzleUI()
    {
        if (hasBeenInteracted == false)
        {
            Cursor.visible = false;
            UIPuzzleGameObject.SetActive(true);
            playerMovementInfo.OnEnterMinigame();
        }
        else
        {
            playerMovementInfo.OnExitMinigame();
        }
    }
}
