using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public GameObject menuClose;
    public Animator fade;
    public GameObject menuOpen;
    public GameObject fadeGameObject;
    public void Button()
    {
        fade.SetBool("Fade", true);
        
    }
    public void Update()
    {
        if (fadeGameObject.GetComponent<AnimationIsDone>().done == true)
        {
            menuClose.SetActive(false);
            menuOpen.SetActive(true);
        }
    }

}
