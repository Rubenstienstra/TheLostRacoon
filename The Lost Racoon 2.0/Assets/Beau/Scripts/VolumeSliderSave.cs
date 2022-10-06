using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSliderSave : MonoBehaviour
{
    public AudioMixer sfxSlider;
    public AudioMixer musicSlide;
    public ScriptableSaving scriptableInfo;
    float music;
    float sfx;
    public void VolumeSlider()
    {
        musicSlide.GetFloat("volume", out music);
        scriptableInfo.volumeMUSIC = music;

        sfxSlider.GetFloat("volume", out sfx);
        scriptableInfo.volumeSFX = sfx;
    }

}
