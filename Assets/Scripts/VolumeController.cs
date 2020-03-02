using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour {

    public AudioMixer mixer;

    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat("Music_Volume", Mathf.Log10(sliderValue) * 20);
    }
}