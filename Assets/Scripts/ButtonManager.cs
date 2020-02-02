using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    //Function to load main menu scene
    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
