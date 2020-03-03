using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour {

    public AudioMixer mixer;

    public Slider MusicSlider;
	public Slider MasterSlider;
	public Slider VoiceSlider;
	public Slider EffectsSlider;

	void start()
	{
		MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
		MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
		VoiceSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 0.75f);
		EffectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 0.75f);
	}

    public void SetMasterLevel (float sliderValue)
    {
        mixer.SetFloat("Master_Volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
        PlayerPrefs.Save();
    }

    public void SetMusicLevel (float sliderValue)
    {
        mixer.SetFloat("Music_Volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        PlayerPrefs.Save();
    }

    public void SetVoiceLevel (float sliderValue)
    {
        mixer.SetFloat("Voice_Volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("VoiceVolume", sliderValue);
        PlayerPrefs.Save();
    }

    public void SetEffectsLevel (float sliderValue)
    {
        mixer.SetFloat("Effects_Volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("EffectsVolume", sliderValue);
        PlayerPrefs.Save();
    }
}