using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeDoneScene : MonoBehaviour
{
    public bool inScene;
    public GameObject fadeCanvas;
    public void InScene()
    {
        inScene = true;
    }

    public void Update()
    {
        if(inScene == true)
        {
            fadeCanvas.SetActive(false);
        }
    }

}
