using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadToScene : MonoBehaviour
{

    public Animator fade;
    public GameObject fadeGameObject;
    bool allowedToChange;
    public int sceneToLoad;


    public void PlayButton()
    {
        fade.SetTrigger("Fade use");
        allowedToChange = true;

    }
        public void Update()
    {

        if (fadeGameObject.GetComponent<AnimationIsDone>().done == true)
        {
            if (allowedToChange == true)
            {
                SceneManager.LoadScene(sceneToLoad);
            }

        }
        if (allowedToChange == false)
        {
            fadeGameObject.GetComponent<AnimationIsDone>().done = false;
        }
    }
}