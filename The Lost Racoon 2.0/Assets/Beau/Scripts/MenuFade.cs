using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFade : MonoBehaviour
{
    public GameObject menuClose;
    public Animator fade;
    public GameObject menuOpen;
    public GameObject fadeGameObject;
    bool allowedToChange;
    public void Button()
    {
        fade.SetTrigger("Fade to play");
        allowedToChange = true;
    }
    public void Update()
    {

        if (fadeGameObject.GetComponent<AnimationIsDone>().done == true)
        {
            if (allowedToChange == true)
            {
                menuClose.SetActive(false);
                menuOpen.SetActive(true);
                allowedToChange = false;
            }
            
        }
        if (allowedToChange == false)
        {
            fadeGameObject.GetComponent<AnimationIsDone>().done = false;
        }
    }

}
