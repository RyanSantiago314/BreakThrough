﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private AudioSource music;

	void start() {
		music = GetComponent<AudioSource>();
		music.Play();
	}

    public void PlayGameVsPlayer() {
    	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    	MaxInput.disableAI();
    	music.Stop();
    }

    public void PlayGameVsAI() {
    	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    	MaxInput.enableAI();
    	music.Stop();
    }

    public void QuitGame() {
    	Application.Quit();
    }
}