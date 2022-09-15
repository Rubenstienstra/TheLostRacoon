using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPuzzleColor : MonoBehaviour
{
    //public PlayerScript playerInfo;
    public ScriptableSaving savingInfo;

    public GameObject[] colorButtons;

    public bool startRed;
    public int totalButtons;
    public int totalButtonsCorrect;


    public GameObject UIPuzzleColorGameObject;
    
    void Start()
    {
        if(savingInfo.minigame3x3 == true)
        {
            totalButtons = 9;
        }
        else if(savingInfo.minigame4x4 == true)
        {
            totalButtons = 16;
        }
        ResetColors();
    }
    public void OnButtonColorPress()
    {
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
        for (int i = 0; i < totalButtons; i++)
        {
            if(gameObject.name == "Color Change Button " + i.ToString() && gameObject.GetComponent<Image>().color == Color.red)
            {
                totalButtonsCorrect++;
                if(totalButtonsCorrect >= totalButtons)
                {
                    UIPuzzleColorGameObject.SetActive(false);
                }
            }
        }
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
}
