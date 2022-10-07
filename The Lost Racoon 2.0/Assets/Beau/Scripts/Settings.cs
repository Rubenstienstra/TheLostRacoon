using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer sfxAudioMixer;
    public AudioMixer musicAudioMixer;
    public Slider sliderSFX;
    public Slider sliderMusic;
    public Dropdown resolutionDropdown;
    public ScriptableSaving scriptableinfo;
    Resolution[] resolutions;



    public void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }

        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        sliderMusic.value = scriptableinfo.volumeMUSIC;
        sliderSFX.value = scriptableinfo.volumeSFX;
        
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolumeSfx(float volume)
    {
        sfxAudioMixer.SetFloat("volume", volume);
    }
    public void SetVolumeMusic(float volume)
    {
        musicAudioMixer.SetFloat("volume", volume);
    }


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


}
