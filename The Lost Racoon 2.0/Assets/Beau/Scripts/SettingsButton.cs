using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public GameObject newMenu;
    public GameObject lastButton;

    public void SetttingsButton()
    {       
        lastButton.GetComponent<PlayButton>().menuOpen = newMenu;
        lastButton.GetComponent<PlayButton>().Button();

    }
}
