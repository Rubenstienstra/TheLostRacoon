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

        yield return new WaitForSeconds(0.2f);
        menuClose.SetActive(false);
    }
}
