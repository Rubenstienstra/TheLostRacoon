using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPopup : MonoBehaviour
{
    public float crMetersAway;
    public float maxMetersAway;
    private GameObject playerTargetGameObject;

    public bool isPopupE;
    public bool isPopupCircle;

    void Start()
    {
        if (isPopupCircle)
        {
            playerTargetGameObject = GameObject.Find("RacoonPlayer");
        }
        else if(isPopupE)
        {
            playerTargetGameObject = GameObject.Find("Transform interaction circle");
        }
       
    }

    public void FixedUpdate()
    {
        if (playerTargetGameObject == null)
        {
            if (isPopupCircle)
            {
                playerTargetGameObject = GameObject.Find("RacoonPlayer");
            }
            else if (isPopupE)
            {
                playerTargetGameObject = GameObject.Find("Transform interaction circle");
            }
        }

        crMetersAway = Vector3.Distance(transform.position, playerTargetGameObject.transform.position);
        if (crMetersAway > maxMetersAway)
        {
            gameObject.SetActive(false);
        }
    }
}
