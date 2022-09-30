using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPuzzleColor : MonoBehaviour
{
    //public PlayerScript playerInfo;
    public ScriptableSaving savingInfo;

    public PlayerScript playerInfo;
    public GameObject[] colorButtons;

    public bool startRed;
    public int totalButtons;
    public int totalButtonsCorrect;

    public GameObject currentGameObject;
    public GameObject UIPuzzleGameObject;

    private void Awake()
    {
        if (savingInfo.minigame3x3 == true)
        {
            totalButtons = 9;
        }
        else if (savingInfo.minigame4x4 == true)
        {
            totalButtons = 16;
        }
    }
    void Start()
    {
        //find main player
        GameObject playerScript = GameObject.Find("Player");
        playerInfo = playerScript.GetComponent<PlayerScript>();
        UIPuzzleGameObject = this.gameObject.transform.parent.gameObject;

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
        for (int i = 1; i < totalButtons+1; i++)
        {
          currentGameObject = GameObject.Find("Color Change Button " + i.ToString());
            if (currentGameObject.GetComponent<Image>().color == Color.red)
            {
                totalButtonsCorrect++;
                if(totalButtonsCorrect >= totalButtons)
                {
                    savingInfo.totalMissionsCompleted++;
                    UIPuzzleGameObject.SetActive(false);
                }
            }
        }
        //print(totalButtonsCorrect);
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
        UIPuzzleGameObject.SetActive(true);
    }
}
