using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSwap : MonoBehaviour
{
    public ScriptableSaving savingInfo;

    public GameObject soundOff;
    public GameObject soundOn;

    public bool toggleSwitch;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TLR Racoon")
        {
            if (!toggleSwitch)
            {
                soundOff.GetComponent<AudioSource>().enabled = !enabled;
                soundOn.GetComponent<AudioSource>().enabled = enabled;
                toggleSwitch = true;
            }
            else if (toggleSwitch)
            {
                soundOn.GetComponent<AudioSource>().enabled = !enabled;
                soundOff.GetComponent<AudioSource>().enabled = enabled;
                toggleSwitch = false;
            }
        }
    }
}
