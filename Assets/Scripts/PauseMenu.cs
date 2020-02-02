using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour{
    public GameObject pauseMenuUI;
    public bool isPaused;
    private int playerPaused;

    private string pauseCode1 = "Start_P1";
    private string pauseCode = "Start_P2";


    void Start()
    {
        playerPaused = 0;

        pauseCode1 += UpdateControls(CheckXbox(0));
        pauseCode += UpdateControls(CheckXbox(1));

    }

    // Update is called once per frame
    void Update(){
        if (isPaused)
        {
            //Disable player inputs in background
            DisableControls(true);
            ActivateMenu();
            //Record which player paused
            if (Input.GetButtonDown(pauseCode1) && playerPaused == 1)
            {
                isPaused = !isPaused;
                playerPaused = 0;
            }
            if (Input.GetButtonDown(pauseCode) && playerPaused == 2)
            {
                isPaused = !isPaused;
                playerPaused = 0;
            }
        }
        else if (!isPaused)
        {
            DisableControls(false);
            DeactivateMenu();
            //Unpause the game (Only the player that paused can unpause)
            if (Input.GetButtonDown(pauseCode1) && playerPaused == 0)
            {
                isPaused = !isPaused;
                playerPaused = 1;
            }
            if (Input.GetButtonDown(pauseCode) && playerPaused == 0)
            {
                isPaused = !isPaused;
                playerPaused = 2;
            }
        }
    }

    private void DisableControls(bool enable)
    {
        GameObject FPC = GameObject.FindWithTag("Player");
        FPC.transform.GetComponent<AttackHandlerDHA>().enabled = !enable;
        FPC.transform.GetComponent<MovementHandler>().enabled = !enable;
    }

    public void ActivateMenu()
    {
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        isPaused = false;
        playerPaused = 0;
    }

    public void QuitToMenu()
    {
        StartText.startReady = false;
        GameOver.lockInputs = false;
        SceneManager.LoadScene(0);
    }

    private bool CheckXbox(int player)
    {
        if (Input.GetJoystickNames().Length > player)
        {
            if (Input.GetJoystickNames()[player].Contains("Xbox"))
            {
                return true;
            }
        }
        return false;
    }

    private string UpdateControls(bool xbox)
    {
        if (xbox)
            return "_Xbox";
        return "";
    }
}
