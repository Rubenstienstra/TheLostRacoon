using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Better3X3Puzzle : MonoBehaviour
{
    public ScriptableSaving savingInfo;
    public PlayerMovementBetter playerMovementInfo;

    public PlayerScript playerInfo;
    public GameObject[] colorButtons;
    public Material[] emissionButtonMaterials;
    public SkinnedMeshRenderer emissionRenderer;

    public bool[] startingGray;
    public bool[] buttonCorrect;
    public int totalButtons = 9;
    public int totalButtonsCorrect;
    public bool is3x3Minigame;
    public bool is4x4Minigame;

    public GameObject currentGameObject;
    public GameObject UIPuzzleMinigame;
    public GameObject gatePuzzleGameObject;

    public Animator buttonAnimations;

    public bool hasBeenInteracted;

    void Start()
    {
        if (is3x3Minigame)
        {
            totalButtons = 9;
        }
        else if (is4x4Minigame)
        {
            totalButtons = 16;
        }

        //find main player
        GameObject playerScript = GameObject.Find("RacoonPlayer");
        playerInfo = playerScript.GetComponent<PlayerScript>();
        playerMovementInfo = playerScript.GetComponent<PlayerMovementBetter>();

        ResetColors();
    }
    public void OnButtonColorPress(int buttonNumber)
    {
        switch(buttonNumber)
        {
            //volgorde van links naar rechts : moet je invullen
            //4:1, 5:2, 1:3, 2:4, 7:5, 8:6, 3:7, 6:8, 0:9;
            case 0:
                SwitchEmission(0,9);
                SwitchEmission(1,3);
                SwitchEmission(3,7);
                SwitchEmission(4,1);
                return;
            case 1:
                SwitchEmission(0,9);
                SwitchEmission(1,3);
                SwitchEmission(2,4);
                SwitchEmission(3,7);
                SwitchEmission(4,1);
                SwitchEmission(5,2);
                return;
            case 2:
                SwitchEmission(1,3);
                SwitchEmission(2,4);
                SwitchEmission(4,1);
                SwitchEmission(5,2);
                return;
            case 3:
                SwitchEmission(0,9);
                SwitchEmission(1,3);
                SwitchEmission(3,7);
                SwitchEmission(4,1);
                SwitchEmission(6,8);
                SwitchEmission(7,5);
                return;
            case 4:
                SwitchEmission(0,9);
                SwitchEmission(1,3);
                SwitchEmission(2,4);
                SwitchEmission(3,7);
                SwitchEmission(4,1);
                SwitchEmission(5,2);
                SwitchEmission(6,8);
                SwitchEmission(7,5);
                SwitchEmission(8,6);
                return;
            case 5:
                SwitchEmission(1,3);
                SwitchEmission(2,4);
                SwitchEmission(4,1);
                SwitchEmission(5,2);
                SwitchEmission(7,5);
                SwitchEmission(8,6);
                return;
            case 6:
                SwitchEmission(3,7);
                SwitchEmission(4,1);
                SwitchEmission(6,8);
                SwitchEmission(7,5);
                return;
            case 7:
                SwitchEmission(3,7);
                SwitchEmission(4,1);
                SwitchEmission(5,2);
                SwitchEmission(6,8);
                SwitchEmission(7,5);
                SwitchEmission(8,6);
                return;
            case 8:
                SwitchEmission(4,1);
                SwitchEmission(5,2);
                SwitchEmission(7,5);
                SwitchEmission(8,6);
                return;
        }
        CheckingCorrectButtons();
    }
    public void SwitchEmission(int buttonNumber,int materialNumber)
    {
        if(buttonCorrect[buttonNumber] == false)
        {
            emissionRenderer.materials[materialNumber].SetFloat("_EmissiveExposureWeight", 0);
            buttonCorrect[buttonNumber] = true;
        }
        else if(buttonCorrect[buttonNumber] == true)
        {
            emissionRenderer.materials[materialNumber].SetFloat("_EmissiveExposureWeight", 1);
            buttonCorrect[buttonNumber] = false;
        }
        print(buttonNumber);
        
    }
    public void CheckingCorrectButtons()
    {
        //Checking witch buttons has emission enabled
        for (int i = 0; i < totalButtons; i++)
        {
            if (buttonCorrect[i])
            {
                totalButtonsCorrect++;
            }
        }
        if (totalButtonsCorrect >= totalButtons)
        {
            hasBeenInteracted = true;
            savingInfo.totalMissionsCompleted++;
            UIPuzzleMinigame.SetActive(false);
            playerMovementInfo.OnExitMinigame();
        }
        totalButtonsCorrect = 0;
    }
    public void ResetColors()
    {
        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (startingGray[i] == true)
            {
                emissionRenderer.materials[i].SetFloat("_EmissiveExposureWeight", 1);
                buttonCorrect[i] = false;
            }
            else
            {
                emissionRenderer.materials[i].SetFloat("_EmissiveExposureWeight", 0);
                buttonCorrect[i] = true;
            }
        }
    }
    public void SpawnPuzzleUI()
    {
        if (hasBeenInteracted == false)
        {
            UIPuzzleMinigame.SetActive(true);
            ResetColors();
            playerMovementInfo.OnEnterMinigame();
        }
        else
        {
            playerMovementInfo.OnExitMinigame();
        }
    }
}
