using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Graficos : MonoBehaviour
{
    public AudioMixer mainMixer;
    public void SetVolume(float volume)
    {

        mainMixer.SetFloat("volume", volume);

    }


    public void setfullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

    }




}
