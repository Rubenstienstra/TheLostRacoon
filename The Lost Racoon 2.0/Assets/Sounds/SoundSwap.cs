using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSwap : MonoBehaviour
{
    public GameObject soundOff;
    public GameObject soundOn;

        private void OnTriggerEnter(Collider other)
    {
        soundOff.gameObject.SetActive(false);
        soundOn.gameObject.SetActive(true);
    }
}
