using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private AudioSource music;
	private LoadingScreen loadScreen;

	void start()
	{
		music = GetComponent<AudioSource>();
		music.Play();
		loadScreen = GetComponent<LoadingScreen>();
    }

    public void PlayGameVsPlayer()
    {
    	SceneManager.LoadSceneAsync(2);
    	MaxInput.disableAI();
    }

	public void PlayGameVsOnline()
	{
		BREAKTHROUGH.PLAYER_ONLINE = true;
		SceneManager.LoadSceneAsync(2); //using name for testing
		MaxInput.disableAI();
	}

    public void PlayGameVsAI()
    {
    	SceneManager.LoadSceneAsync(2);
    	MaxInput.enableAI();
    }

    public void QuitGame()
    {
    	Application.Quit();
    }
}