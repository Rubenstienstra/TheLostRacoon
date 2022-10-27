using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Better3X3Puzzle : MonoBehaviour
{
    public ScriptableSaving savingInfo;
    private PlayerMovementBetter playerMovementInfo;
    private Interact interactInfo;

    public GameObject[] colorButtons;
    public Material[] emissionButtonMaterials;
    
    

    public bool[] startingGray;
    public bool[] buttonCorrect;
    public int totalButtons = 9;
    public int totalButtonsCorrect;
    public float animationButtonDelay;
    public bool is3x3Minigame;
    public bool is4x4Minigame;

    public GameObject currentGameObject;
    public GameObject UIPuzzleMinigame;
    private GameObject racoonMesh;

    public Animator buttonAnimations;
    public SkinnedMeshRenderer emissionRenderer;

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

        //finding main player + finding racoon mesh
        GameObject playerScript = GameObject.Find("RacoonPlayer");
        playerMovementInfo = playerScript.GetComponent<PlayerMovementBetter>();
        interactInfo = playerScript.GetComponent<Interact>();

        racoonMesh = GameObject.Find("RaccoonModel");

        ResetColors();
    }
    public void ForButtonOnClick(int buttonNumber) //Deze functie is om de IEnumarator aan te roepen (vanwege de UI buttons)
    {
        StartCoroutine(Animationtrigger(buttonNumber));
    }
    public IEnumerator Animationtrigger(int buttonNumber)
    {
        buttonNumber++;
        buttonAnimations.SetTrigger(buttonNumber.ToString());
        buttonNumber--;
        yield return new WaitForSeconds(animationButtonDelay);
        OnButtonColorPress(buttonNumber);
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
                CheckingCorrectButtons();
                return;
            case 1:
                SwitchEmission(0,9);
                SwitchEmission(1,3);
                SwitchEmission(2,4);
                SwitchEmission(3,7);
                SwitchEmission(4,1);
                SwitchEmission(5,2);
                CheckingCorrectButtons();
                return;
            case 2:
                SwitchEmission(1,3);
                SwitchEmission(2,4);
                SwitchEmission(4,1);
                SwitchEmission(5,2);
                CheckingCorrectButtons();
                return;
            case 3:
                SwitchEmission(0,9);
                SwitchEmission(1,3);
                SwitchEmission(3,7);
                SwitchEmission(4,1);
                SwitchEmission(6,8);
                SwitchEmission(7,5);
                CheckingCorrectButtons();
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
                CheckingCorrectButtons();
                return;
            case 5:
                SwitchEmission(1,3);
                SwitchEmission(2,4);
                SwitchEmission(4,1);
                SwitchEmission(5,2);
                SwitchEmission(7,5);
                SwitchEmission(8,6);
                CheckingCorrectButtons();
                return;
            case 6:
                SwitchEmission(3,7);
                SwitchEmission(4,1);
                SwitchEmission(6,8);
                SwitchEmission(7,5);
                CheckingCorrectButtons();
                return;
            case 7:
                SwitchEmission(3,7);
                SwitchEmission(4,1);
                SwitchEmission(5,2);
                SwitchEmission(6,8);
                SwitchEmission(7,5);
                SwitchEmission(8,6);
                CheckingCorrectButtons();
                return;
            case 8:
                SwitchEmission(4,1);
                SwitchEmission(5,2);
                SwitchEmission(7,5);
                SwitchEmission(8,6);
                CheckingCorrectButtons();
                return;
        }
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
            WinMinigame();
        }
        print(totalButtonsCorrect);
        totalButtonsCorrect = 0;
    }
    public void ResetColors()
    {
        for (int i = 0; i < colorButtons.Length; i++)
        {
            if (startingGray[i] == true)
            {
                emissionButtonMaterials[i].SetFloat("_EmissiveExposureWeight", 1);
                buttonCorrect[i] = false;
            }
            else
            {
                emissionButtonMaterials[i].SetFloat("_EmissiveExposureWeight", 0);
                buttonCorrect[i] = true;
            }
        }
    }
    public void SpawnPuzzleUI()
    {
        if (!hasBeenInteracted)
        {
            UIPuzzleMinigame.SetActive(true);
            playerMovementInfo.movementLock = true;
            racoonMesh.SetActive(false);
            ResetColors();
        }
        else
        {
            UIPuzzleMinigame.SetActive(false);
            interactInfo.OnExitMinigame();
        }
    }
    public void WinMinigame()
    {
        hasBeenInteracted = true;

        playerMovementInfo.ResettingAllAnimations();
        playerMovementInfo.moving = false;
        playerMovementInfo.movementLock = false;
                                                        //zet hier de animaties neer die moeten afgespeeld worden als de minigame klaar is.
        
        racoonMesh.SetActive(true);
        savingInfo.totalMissionsCompleted++;
        UIPuzzleMinigame.SetActive(false);
        interactInfo.OnExitMinigame();
    }
}
