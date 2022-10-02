using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine.Utility;

public class CamFreezeScript : MonoBehaviour
{
    [Header("References", order = 0)]
    public GameObject freeLook;
    public GameObject freezeLook;
    public GameObject cam;
    public GameObject freezeCam;
    [Header("Snap to Transform", order = 1)]
    public bool snapToTransform;
    public Transform camSpot;
    bool camFroze;
    public bool smoothTransition;
    //fix prefab
    // Start is called before the first frame update
    void Start()
    {
        camFroze = false;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "CamFreezeZone" && camFroze)
        {
            CamUnfreeze();
        }

    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "CamFreezeZone" && !camFroze)
        {
            CamFreeze();
        }
    }
    public void CamFreeze()
    {
        freezeLook.SetActive(true);
        freezeCam.SetActive(true);
        freeLook.SetActive(false);

        if (!smoothTransition)
        {
            cam.SetActive(false);
        }
        else
        {
            if (snapToTransform)
            {
                freezeLook.transform.position = camSpot.position;
                freezeLook.transform.rotation = camSpot.rotation;
            }
            else
            {

            }

        }
        freezeLook.transform.position = freeLook.transform.position;
        freezeLook.transform.rotation = freeLook.transform.rotation;
        camFroze = true;
        Debug.Log("Cam pos locked");
    }
    public void CamUnfreeze()
    {

        freezeLook.SetActive(false);
        if (!smoothTransition)
        {
            cam.SetActive(true);
            freezeCam.SetActive(false);
        }
        freeLook.SetActive(true);

        camFroze = false;
        Debug.Log("Cam pos unlocked");
    }
}
