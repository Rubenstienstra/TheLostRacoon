using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waitsriptus : MonoBehaviour
{
    public GameObject menuClose;

    public void WaitToTurnOffGameObject()
    {
        StartCoroutine(WaitGameObjectOff());
    }

    IEnumerator WaitGameObjectOff()
    {
        yield return new WaitForSecondsRealtime (0.2f);
        Time.timeScale = 1;
        menuClose.SetActive(false);
    }
}
