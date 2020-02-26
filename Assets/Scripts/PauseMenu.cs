using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public GameOver GameOver;

    public GameObject pauseMenuUI;
    public GameObject practicePauseMenuUI;
    public GameObject moveListUI;
    public bool isPaused = false;
    static public bool pauseQuit;
    public bool moveList;
    private int playerPaused;

    private string pauseCode1 = "Start_P1";
    private string pauseCode = "Start_P2";

    private int optionIndex = 0;
    public Button resumeButton;
    public Button CPUStateHighlight;
    public Button P1ValorHighlight;
    public Button P2ValorHighlight;
    public Button armorRefillHighlight;
    public Button moveListButton;
    public Button quitButton;
    private float InputTimer;
    private string inputHorizontal = "Horizontal_P1";
    private string inputVertical = "Vertical_P1";
    private float vertical;
    private float horizontal;
    private bool acceptInputVer;
    private bool acceptInputHor;

    public int CPUState = 0;
    public int P1Valor = 0;
    public int P2Valor = 0;
    public int ArmorRefill = 0;
    public GameObject CPUStateText;
    public GameObject P1ValorText;
    public GameObject P2ValorText;
    public GameObject ArmorRefillText;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        playerPaused = 0;

        pauseCode1 += UpdateControls(CheckXbox(0));
        pauseCode += UpdateControls(CheckXbox(1));

        pauseQuit = false;
        moveList = false;
        moveListUI.SetActive(false);

        inputHorizontal += UpdateControls(CheckXbox(0));
        inputVertical += UpdateControls(CheckXbox(0));
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice")
        {
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
        //Handle Practice Mode Pause Menu
        else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            if (Input.GetButtonDown(pauseCode1) && !isPaused)
            {
                DisableControls(true);
                ActivateMenu();
                isPaused = true;
                optionIndex = 0;
            } else if (Input.GetButtonDown(pauseCode1) && isPaused)
            {
                DisableControls(false);
                DeactivateMenu();
                isPaused = false;
            }

            if (isPaused)
            {             
                //Handle Vertical Selection
                vertical = Input.GetAxis(inputVertical);
                
                //Check for input
                if (!acceptInputVer)
                {
                    if (vertical == 0)
                    {
                        acceptInputVer = true;
                    }
                }
                if (acceptInputVer)
                {
                    if (vertical < 0)
                    {
                        optionIndex += 1;
                        acceptInputVer = false;
                    }
                    else if (vertical > 0)
                    {
                        optionIndex -= 1;
                        acceptInputVer = false;
                    }
                }

                //Check Horizontal Input
                horizontal = Input.GetAxis(inputHorizontal);
                if (!acceptInputHor)
                {
                    if (horizontal == 0)
                    {
                        acceptInputHor = true;
                    }
                }

                //Cycle option scrolling
                //optionIndex scrolling
                if (optionIndex == 7)
                {
                    optionIndex = 0;
                }
                else if (optionIndex == -1)
                {
                    optionIndex = 6;
                }               

                //Resume Button
                if (optionIndex == 0)
                {
                    resumeButton.Select();
                }
                //CPU Action
                else if (optionIndex == 1)
                {
                    CPUStateHighlight.Select();
                    if (acceptInputHor)
                    {
                        if (horizontal < 0)
                        {
                            CPUState -= 1;
                            acceptInputHor = false;
                        }
                        else if (horizontal > 0)
                        {
                            CPUState += 1;
                            acceptInputHor = false;
                        }
                    }
                }
                //P1 Valor
                else if (optionIndex == 2)
                {
                    P1ValorHighlight.Select();
                    if (acceptInputHor)
                    {
                        if (horizontal < 0)
                        {
                            P1Valor -= 1;
                            acceptInputHor = false;
                        }
                        else if (horizontal > 0)
                        {
                            P1Valor += 1;
                            acceptInputHor = false;
                        }
                    }
                }
                //P2 Valor
                else if (optionIndex == 3)
                {
                    P2ValorHighlight.Select();
                    if (acceptInputHor)
                    {
                        if (horizontal < 0)
                        {
                            P2Valor -= 1;
                            acceptInputHor = false;
                        }
                        else if (horizontal > 0)
                        {
                            P2Valor += 1;
                            acceptInputHor = false;
                        }
                    }
                }
                //Armor Refill
                else if (optionIndex == 4)
                {
                    armorRefillHighlight.Select();
                    if (acceptInputHor)
                    {
                        if (horizontal < 0)
                        {
                            ArmorRefill -= 1;
                            acceptInputHor = false;
                        }
                        else if (horizontal > 0)
                        {
                            ArmorRefill += 1;
                            acceptInputHor = false;
                        }
                    }                   
                }
                //MoveList
                else if (optionIndex == 5)
                {
                    moveListButton.Select();
                }
                //Quit Button
                else if (optionIndex == 6)
                {
                    quitButton.Select();
                }

                //Options scrolling
                //CPUState scrolling
                if (CPUState == 7)
                {
                    CPUState = 0;
                }
                else if (CPUState == -1)
                {
                    CPUState = 6;
                }
                //Valor scrolling
                if (P1Valor == 4)
                {
                    P1Valor = 0;
                }
                else if (P1Valor == -1)
                {
                    P1Valor = 4;
                }
                if (P2Valor == 4)
                {
                    P2Valor = 0;
                }
                else if (P2Valor == -1)
                {
                    P2Valor = 4;
                }
                //ArmorRefill scrolling
                if (ArmorRefill == 2)
                {
                    ArmorRefill = 0;
                }
                else if (ArmorRefill == -1)
                {
                    ArmorRefill = 1;
                }

                //Update Text for options
                switch (CPUState)
                {
                    case 0:
                        CPUStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "Stand";
                        break;
                    case 1:
                        CPUStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "Crouch";
                        break;
                    case 2:
                        CPUStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "Jump";
                        break;
                    case 3:
                        CPUStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "Guard";
                        break;
                    case 4:
                        CPUStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "Low Guard";
                        break;
                    case 5:
                        CPUStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "CPU";
                        break;
                    case 6:
                        CPUStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "Player";
                        break;
                }
                switch (P1Valor)
                {
                    case 0:
                        P1ValorText.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
                        break;
                    case 1:
                        P1ValorText.GetComponent<TMPro.TextMeshProUGUI>().text = "1";
                        break;
                    case 2:
                        P1ValorText.GetComponent<TMPro.TextMeshProUGUI>().text = "2";
                        break;
                    case 3:
                        P1ValorText.GetComponent<TMPro.TextMeshProUGUI>().text = "3";
                        break;
                }
                switch (P2Valor)
                {
                    case 0:
                        P2ValorText.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
                        break;
                    case 1:
                        P2ValorText.GetComponent<TMPro.TextMeshProUGUI>().text = "1";
                        break;
                    case 2:
                        P2ValorText.GetComponent<TMPro.TextMeshProUGUI>().text = "2";
                        break;
                    case 3:
                        P2ValorText.GetComponent<TMPro.TextMeshProUGUI>().text = "3";
                        break;
                }
                switch (ArmorRefill)
                {
                    case 0:
                        ArmorRefillText.GetComponent<TMPro.TextMeshProUGUI>().text = "On";
                        break;
                    case 1:
                        ArmorRefillText.GetComponent<TMPro.TextMeshProUGUI>().text = "Off";
                        break;
                }
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
        if (!moveList)
        {
            moveListUI.SetActive(false);
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
            {
                practicePauseMenuUI.SetActive(true);
            }
            else
            {
                pauseMenuUI.SetActive(true);
            }
        }
        else
        {
            moveListUI.SetActive(true);
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
            {
                practicePauseMenuUI.SetActive(false);
            }
            else
            {
                pauseMenuUI.SetActive(false);
            }
        }
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        isPaused = false;
        playerPaused = 0;
        DisableControls(false);
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            practicePauseMenuUI.SetActive(false);
        }
        else
        {
            pauseMenuUI.SetActive(false);
        }
    }

    public void MoveList()
    {
        pauseMenuUI.SetActive(false);
        moveList = true;
        //moveListUI.SetActive(true);
    }

    public void MoveListBack()
    {
        moveList = false;
        moveListUI.SetActive(false);
    }

    public void QuitToMenu()
    {
        StartText.startReady = false;
        GameOver.lockInputs = false;
        GameOver.p1Win = 0;
        GameOver.p2Win = 0;
        pauseQuit = true;

        //Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
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