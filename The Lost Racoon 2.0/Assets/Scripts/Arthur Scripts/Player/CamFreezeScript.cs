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

    // Update is called once per frame
    void Update()
    {
      
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "CamFreezeZone" && camFroze) {
            freezeLook.SetActive(false);
            if (!smoothTransition) {
                freezeLook.GetComponentInChildren<GameObject>().SetActive(false);
                cam.SetActive(true);
            }
            freeLook.SetActive(true);

            camFroze = false;
            Debug.Log("Cam pos unlocked");
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "CamFreezeZone" && !camFroze) {
            freezeLook.SetActive(true); 
            freeLook.SetActive(false);
            if (!smoothTransition) {
                Camera.main.gameObject.SetActive(false);
            } else {
                if (snapToTransform) {
                    freezeLook.transform.position = camSpot.position;
                    freezeLook.transform.rotation = camSpot.rotation;
                } else {

                }

            }
            freezeLook.transform.position = freeLook.transform.position;
            freezeLook.transform.rotation = freeLook.transform.rotation;
            camFroze = true;
            Debug.Log("Cam pos locked");
        }
    }
}
