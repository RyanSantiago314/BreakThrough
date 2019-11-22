using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGameVsPlayer() {
    	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    	MaxInput.disableAI();
    }

    public void PlayGameVsAI() {
    	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    	MaxInput.enableAI();
    }

    public void QuitGame() {
    	Application.Quit();
    }
}