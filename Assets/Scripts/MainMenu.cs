using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private AudioSource music;
	private LoadingScreen loadScreen;

	void start() {
		music = GetComponent<AudioSource>();
		music.Play();
		loadScreen = GetComponent<LoadingScreen>();
	}

    public void PlayGameVsPlayer() {
    	SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    	//loadScreen.startLoad(SceneManager.GetActiveScene().buildIndex + 1);
    	MaxInput.disableAI();
    }

    public void PlayGameVsAI() {
    	SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    	MaxInput.enableAI();
    }

    public void QuitGame() {
    	Application.Quit();
    }
}