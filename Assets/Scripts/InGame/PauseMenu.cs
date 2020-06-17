using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class PauseMenu : MonoBehaviour
{
    public Animator MarkAnimator;
    public Animator MoveListMarkAnimator;
    public Animator SelectorMarkAnimator;

    public RawImage VideoScreen;

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
    public Button CPUAirTechHighlight;
    public Button CPUGuardAfterFirstHithHighlight;
    public Button characterselectButton;
    public Button moveListButton;
    public Button quitButton;
    public Button resumeButtonMatch;
    public Button characterselectButtonMatch;
    public Button moveListButtonMatch;
    public Button quitButtonMatch;
    public GameObject UIBar;
    private float InputTimer;
    private string inputHorizontal = "Horizontal_P1";
    private string inputVertical = "Vertical_P1";
    private string p1cross = "Cross_P1";
    private string p1circle = "Circle_P1";
    private string inputHorizontal2 = "Horizontal_P2";
    private string inputVertical2 = "Vertical_P2";
    private string p2cross = "Cross_P2";
    private string p2circle = "Circle_P2";
    private float vertical;
    private float horizontal;
    private bool acceptInputVer;
    private bool acceptInputHor;
    private bool acceptInputCirc;
    private int inputTimer = 0;
    private bool holdScroll = false;
    private bool updateVideo = false;

    public int CPUState = 0;
    public int P1Valor;
    public int P2Valor;
    public int ArmorRefill = 0;
    public int CPUAirRecover = 0;
    public int CPUGroundGuard = 0;
    public GameObject CPUStateText;
    public GameObject P1ValorText;
    public GameObject P2ValorText;
    public GameObject ArmorRefillText;
    public GameObject CPUAirRecoverText;
    public GameObject CPUGroundGuardText;

    public Text PlayerIdentifier;

    static public bool allowPause;

    public MoveList mList;
    private int moveListIndex = 1;
    private int verticalMoveListIndex = 1;
    public Image MoveListMarker;
    private bool keepMaxIndex;
    private bool acceptBack = false;
    private bool acceptMoveList = true;

    //array and elements for video clips
    public VideoClip[] videoToPlay = new VideoClip[26];
    private enum videoClips: int
    {
        DHA_6L,
        DHA_6B,
        DHA_Patissiere,
        DHA_HeadRush,
        DHA_BloodBrave,
        DHA_BasketCase,
        DHA_Toaster,
        DHA_JudgementSabre,
        DHA_L,
        DHA_M,
        DHA_H,
        DHA_B,
        DHA_Cancel,
        DHA_Grab,
        DHA_Neutral,
        ACH_LevelHell,
        ACH_HeavenClimber,
        ACH_Starfall,
        ACH_ForsythiaMarduk,
        ACH_L,
        ACH_M,
        ACH_H,
        ACH_B,
        ACH_Cancel,
        ACH_Grab,
        ACH_Neutral
    }

    //Dev tool to remove HUD
    public GameObject HUD;
    public GameObject PracticeHUD;
    public SelectedCharacterManager PlayerData;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        playerPaused = 0;

        SetControllers();

        pauseQuit = false;
        allowPause = false;
        //moveList = false;

        HUD = GameObject.Find("HUD");
        PracticeHUD = GameObject.Find("PracticeHUD");
        PlayerData = GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>();
    }
    // top bottom
    // Resume:  0 32
    // Movelist:  0 -36
    // character select:  889 703
    // end Match:  959 633
    // 703.7

    // Update is called once per frame
    void Update()
    {
        //Dev tool to disable HUDS
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (HUD.activeSelf)
            {
                HUD.SetActive(false);
            }
            else
            {
                HUD.SetActive(true);
            }

            if (PlayerData.gameMode == "Practice" && PracticeHUD.activeSelf)
            {
                PracticeHUD.SetActive(false);
            }
            else
            {
                PracticeHUD.SetActive(true);
            }
        }

        SetControllers();
        if ((GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice" && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Tutorial") && allowPause)
        {
            if ((Input.GetButtonDown(pauseCode1) || Input.GetButtonDown(pauseCode)) && !isPaused)
            {
                DisableControls(true);
                ActivateMenu();
                isPaused = true;
                optionIndex = -1;
                optionIndex = 0;
                quitButtonMatch.Select();
                if (Input.GetButtonDown(pauseCode1))
                {
                    playerPaused = 1;

                    PlayerIdentifier.text = "P1";
                }
                else if (Input.GetButtonDown(pauseCode))
                {
                    playerPaused = 2;

                    PlayerIdentifier.text = "P2";
                }
            }
            else if (!SceneTransitions.lockinputs && (Input.GetButtonDown(pauseCode1) && isPaused && !moveList && playerPaused == 1) || (Input.GetButtonDown(pauseCode) && isPaused && !moveList && playerPaused == 2))
            {
                DisableControls(false);
                DeactivateMenu();
                isPaused = false;
            }

            if (isPaused && !SceneTransitions.lockinputs)
            {
                //Handle Vertical Selection
                if (playerPaused == 1)
                {
                    vertical = Input.GetAxisRaw(inputVertical);
                    horizontal = Input.GetAxisRaw(inputHorizontal);
                }
                else if(playerPaused == 2)
                {
                    vertical = Input.GetAxisRaw(inputVertical2);
                    horizontal = Input.GetAxisRaw(inputHorizontal2);
                }

                //Check for input
                if (!acceptInputVer)
                {
                    if (vertical == 0)
                    {
                        acceptInputVer = true;
                    }
                }
                if (acceptInputVer && !moveList)
                {
                    if (vertical < 0)
                    {
                        optionIndex += 1;
                        acceptInputVer = false;
                        if (MarkAnimator.GetCurrentAnimatorStateInfo(0).IsName("MarkAnimation"))
                        {
                            MarkAnimator.Play("MarkAnimation", -1, 0f);
                        }
                        else
                        {
                            MarkAnimator.SetTrigger("PlayMarkAnimation");
                        }
                    }
                    else if (vertical > 0)
                    {
                        optionIndex -= 1;
                        acceptInputVer = false;
                        if (MarkAnimator.GetCurrentAnimatorStateInfo(0).IsName("MarkAnimation"))
                        {
                            MarkAnimator.Play("MarkAnimation", -1, 0f);
                        }
                        else
                        {
                            MarkAnimator.SetTrigger("PlayMarkAnimation");
                        }
                    }
                }

                if (!acceptInputCirc)
                {
                    if ((!Input.GetButton(p1circle) && playerPaused == 1) || (!Input.GetButton(p2circle) && playerPaused == 2))
                    {
                        acceptInputCirc = true;
                    }
                }

                if (optionIndex == 4)
                {
                    optionIndex = 0;
                }
                else if (optionIndex == -1)
                {
                    optionIndex = 3;
                }

                if (((Input.GetButton(p1circle) && !moveList && playerPaused == 1) || (Input.GetButton(p2circle) && !moveList && playerPaused == 2)) && acceptInputCirc)
                {
                    DisableControls(false);
                    DeactivateMenu();
                    isPaused = false;
                    acceptInputCirc = false;
                }

                if (optionIndex == 0)
                {
                    resumeButtonMatch.Select();
                    UIBar.transform.localPosition = new Vector2(0, 32);
                    if ((Input.GetButton(p1cross) && playerPaused == 1) || (Input.GetButton(p2cross) && playerPaused == 2))
                    {
                        DisableControls(false);
                        DeactivateMenu();
                        isPaused = false;
                    }
                }
                else if (optionIndex == 1)
                {
                    moveListButtonMatch.Select();
                    UIBar.transform.localPosition = new Vector2(0, -32);

                    if (((Input.GetButtonDown(p1cross) && !moveList && playerPaused == 1) || (Input.GetButtonDown(p2cross) && !moveList && playerPaused == 2)) && acceptMoveList)
                    {
                        MoveList();
                        acceptBack = false;
                        moveListIndex = 1;
                        verticalMoveListIndex = 1;
                        if (playerPaused == 1)
                        {
                            setMoveListPage(1, PlayerData.GetComponent<SelectedCharacterManager>().P1Character);
                            //print(PlayerData.GetComponent<SelectedCharacterManager>().P1Character == "Achealis");
                        }
                        else if (playerPaused == 2)
                        {
                            setMoveListPage(1, PlayerData.GetComponent<SelectedCharacterManager>().P2Character);
                        }
                        MoveListMarker.color = new Color(1f, 1f, 1f, 0f);
                        mList.resetMarker();
                        mList.enableMarker();
                        //Choose what video to display
                        VideoScreen.GetComponent<UnityEngine.Video.VideoPlayer>().clip = videoToPlay[SelectVideo(verticalMoveListIndex, moveListIndex)];//SelectVideo(verticalMoveListIndex, moveListIndex);

                    }
                    if (!acceptMoveList)
                    {
                        acceptMoveList = true;
                    }
                    if (((Input.GetButton(p1circle) && moveList && playerPaused == 1) || (Input.GetButton(p2circle) && moveList && playerPaused == 2)) && acceptInputCirc)
                    {
                        MoveListBack();

                        resumeButtonMatch.Select();
                        acceptInputCirc = false;
                    }

                    //MoveList Interactions
                    if (moveListUI.activeSelf)
                    {
                        MoveListInputs();
                    }
                }
                else if (optionIndex == 2)
                {
                    characterselectButtonMatch.Select();
                    UIBar.transform.localPosition = new Vector2(0, -94);
                    if ((Input.GetButton(p1cross) && playerPaused == 1) || (Input.GetButton(p2cross) && playerPaused == 2))
                    {
                        ReturntoCharacterSelect();
                    }
                }
                else if (optionIndex == 3)
                {
                    quitButtonMatch.Select();
                    UIBar.transform.localPosition = new Vector2(0, -163);
                    if ((Input.GetButton(p1cross) && playerPaused == 1) || (Input.GetButton(p2cross) && playerPaused == 2))
                    {
                        QuitToMenu();
                    }
                }
            }
        }
        //Handle Practice Mode Pause Menu
        else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Tutorial")
        {

            if (Input.GetButtonDown(pauseCode1) && !isPaused)
            {
                playerPaused = 1;
                DisableControls(true);
                ActivateMenu();
                isPaused = true;
                optionIndex = -1;
                optionIndex = 0;
                quitButton.Select();
            } else if (Input.GetButtonDown(pauseCode1) && isPaused && !moveList)
            {
                DisableControls(false);
                DeactivateMenu();
                isPaused = false;
            }

            if (isPaused && !SceneTransitions.lockinputs)
            {
                //Handle Vertical Selection
                vertical = Input.GetAxisRaw(inputVertical);

                //Check for input
                if (!acceptInputVer)
                {
                    if (vertical == 0)
                    {
                        acceptInputVer = true;
                    }
                }
                if (acceptInputVer && !moveList)
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

                if (!acceptInputCirc)
                {
                    if (!Input.GetButton(p1circle))
                    {
                        acceptInputCirc = true;
                    }
                }

                //Check Horizontal Input
                horizontal = Input.GetAxisRaw(inputHorizontal);

                if (!acceptInputHor)
                {
                    if (horizontal == 0)
                    {
                        acceptInputHor = true;
                    }
                }

                //Timer for holding a horizontal input
                if (horizontal > 0 || horizontal < 0)
                {
                    if (inputTimer == 1)
                    {
                        holdScroll = true;
                        inputTimer++;
                    }
                    else if (inputTimer < 30)
                    {
                        inputTimer++;
                        holdScroll = false;
                    }
                    else
                    {
                        holdScroll = true;
                    }
                }
                else
                {
                    inputTimer = 0;
                    holdScroll = false;
                }

                //Cycle option scrolling
                //optionIndex scrolling
                if (optionIndex == 10)
                {
                    optionIndex = 0;
                }
                else if (optionIndex == -1)
                {
                    optionIndex = 9;
                }

                if (Input.GetButton(p1circle) && !moveList && acceptInputCirc)
                {
                    DisableControls(false);
                    DeactivateMenu();
                    isPaused = false;
                    acceptInputCirc = false;
                }

                //Resume Button
                if (optionIndex == 0)
                {
                    resumeButton.Select();
                    if (Input.GetButton(p1cross))
                    {
                        DisableControls(false);
                        DeactivateMenu();
                        isPaused = false;
                    }
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
                //P1 Health
                else if (optionIndex == 2)
                {
                    P1ValorHighlight.Select();
                    if (holdScroll)
                    {
                        if (horizontal < 0)
                        {
                            P1Valor -= 1;
                        }
                        else if (horizontal > 0)
                        {
                            P1Valor += 1;
                        }
                    }
                }
                //P2 Health
                else if (optionIndex == 3)
                {
                    P2ValorHighlight.Select();
                    if (holdScroll)
                    {
                        if (horizontal < 0)
                        {
                            P2Valor -= 1;
                        }
                        else if (horizontal > 0)
                        {
                            P2Valor += 1;
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
                //CPU Air Tech
                else if (optionIndex == 5)
                {
                    CPUAirTechHighlight.Select();
                    if (acceptInputHor)
                    {
                        if (horizontal < 0)
                        {
                            CPUAirRecover -= 1;
                            acceptInputHor = false;
                        }
                        else if (horizontal > 0)
                        {
                            CPUAirRecover += 1;
                            acceptInputHor = false;
                        }
                    }
                }
                //CPU Ground Guard
                else if (optionIndex == 6)
                {
                    CPUGuardAfterFirstHithHighlight.Select();
                    if (acceptInputHor)
                    {
                        if (horizontal < 0)
                        {
                            CPUGroundGuard -= 1;
                            acceptInputHor = false;
                        }
                        else if (horizontal > 0)
                        {
                            CPUGroundGuard += 1;
                            acceptInputHor = false;
                        }
                    }
                }
                //Character Select Button
                else if (optionIndex == 7)
                {
                    characterselectButton.Select();
                    if (Input.GetButton(p1cross))
                    {
                        ReturntoCharacterSelect();
                    }
                }
                //MoveList
                else if (optionIndex == 8)
                {
                    moveListButton.Select();
                    if (Input.GetButtonDown(p1cross) && !moveList)
                    {
                        MoveList();
                        acceptBack = false;
                        moveListIndex = 1;
                        verticalMoveListIndex = 1;
                        mList.setDhaliaPage1();
                        MoveListMarker.color = new Color(1f, 1f, 1f, 0f);
                        mList.resetMarker();
                        mList.enableMarker();
                        //Choose what video to display
                        VideoScreen.GetComponent<UnityEngine.Video.VideoPlayer>().clip = videoToPlay[SelectVideo(verticalMoveListIndex, moveListIndex)];//SelectVideo(verticalMoveListIndex, moveListIndex);
                    }
                    if (!acceptMoveList)
                    {
                        acceptMoveList = true;
                    }
                    if (Input.GetButton(p1circle) && moveList && acceptInputCirc)
                    {
                        MoveListBack();
                        resumeButton.Select();
                        acceptInputCirc = false;
                    }
                    if (moveListUI.activeSelf)
                    {
                        MoveListInputs();
                    }
                }
                //Quit Button
                else if (optionIndex == 9)
                {
                    quitButton.Select();
                    if (Input.GetButton(p1cross))
                    {
                        QuitToMenu();
                    }
                }

                //Options scrolling
                //CPUState scrolling
                if (CPUState == 8)
                {
                    CPUState = 0;
                }
                else if (CPUState == -1)
                {
                    CPUState = 7;
                }
                //Valor scrolling
                if (P1Valor == 101)
                {
                    P1Valor = 1;
                }
                else if (P1Valor == 0)
                {
                    P1Valor = 100;
                }
                if (P2Valor == 101)
                {
                    P2Valor = 1;
                }
                else if (P2Valor == 0)
                {
                    P2Valor = 100;
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
                //CPU Air Recovery
                if (CPUAirRecover == 2)
                {
                    CPUAirRecover = 0;
                }
                else if (CPUAirRecover == -1)
                {
                    CPUAirRecover = 1;
                }
                //CPU Ground Guard
                if (CPUGroundGuard == 2)
                {
                    CPUGroundGuard = 0;
                }
                else if (CPUGroundGuard == -1)
                {
                    CPUGroundGuard = 1;
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
                        CPUStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "Standing Guard";
                        break;
                    case 4:
                        CPUStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "Crouching Guard";
                        break;
                    case 5:
                        CPUStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "Guard All";
                        break;
                    case 6:
                        CPUStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "CPU";
                        break;
                    case 7:
                        CPUStateText.GetComponent<TMPro.TextMeshProUGUI>().text = "Player";
                        break;
                }

                P1ValorText.GetComponent<TMPro.TextMeshProUGUI>().text = "" + P1Valor + "%";
                P2ValorText.GetComponent<TMPro.TextMeshProUGUI>().text = "" + P2Valor + "%";

                switch (ArmorRefill)
                {
                    case 0:
                        ArmorRefillText.GetComponent<TMPro.TextMeshProUGUI>().text = "On";
                        break;
                    case 1:
                        ArmorRefillText.GetComponent<TMPro.TextMeshProUGUI>().text = "Off";
                        break;
                }

                switch (CPUAirRecover)
                {
                    case 0:
                        CPUAirRecoverText.GetComponent<TMPro.TextMeshProUGUI>().text = "Off";
                        break;
                    case 1:
                        CPUAirRecoverText.GetComponent<TMPro.TextMeshProUGUI>().text = "On";
                        break;
                }

                switch (CPUGroundGuard)
                {
                    case 0:
                        CPUGroundGuardText.GetComponent<TMPro.TextMeshProUGUI>().text = "Off";
                        break;
                    case 1:
                        CPUGroundGuardText.GetComponent<TMPro.TextMeshProUGUI>().text = "On";
                        break;
                }
            }
        }
    }

    private void DisableControls(bool enable)
    {
        switch (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character)
        {
            case "Dhalia":
                GameObject.Find("Player1").transform.GetChild(0).GetComponent<AttackHandlerDHA>().enabled = !enable;
                GameObject.Find("Player1").transform.GetChild(0).GetComponent<MovementHandler>().enabled = !enable;
                break;
            case "Achealis":
                GameObject.Find("Player1").transform.GetChild(0).GetComponent<AttackHandlerACH>().enabled = !enable;
                GameObject.Find("Player1").transform.GetChild(0).GetComponent<MovementHandler>().enabled = !enable;
                break;
        }

        switch (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character)
        {
            case "Dhalia":
                GameObject.Find("Player2").transform.GetChild(0).GetComponent<AttackHandlerDHA>().enabled = !enable;
                GameObject.Find("Player2").transform.GetChild(0).GetComponent<MovementHandler>().enabled = !enable;
                break;
            case "Achealis":
                GameObject.Find("Player2").transform.GetChild(0).GetComponent<AttackHandlerACH>().enabled = !enable;
                GameObject.Find("Player2").transform.GetChild(0).GetComponent<MovementHandler>().enabled = !enable;
                break;
        }
    }

    public void ActivateMenu()
    {
        Time.timeScale = 0;
        if (!moveList)
        {
            moveListUI.SetActive(false);
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Tutorial")
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
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Tutorial")
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
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Tutorial")
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
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Tutorial")
        {
            practicePauseMenuUI.SetActive(false);
        }
        else
        {
            pauseMenuUI.SetActive(false);
        }
        moveList = true;
        moveListUI.SetActive(true);
    }

    public void MoveListBack()
    {
        moveList = false;
        moveListUI.SetActive(false);
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Tutorial")
        {
            practicePauseMenuUI.SetActive(true);
        }
        else
        {
            pauseMenuUI.SetActive(true);
        }
    }

    public void QuitToMenu()
    {
        RoundManager.gameActive = false;
        RoundManager.lockInputs = false;
        pauseQuit = true;
        GameObject.Find("TransitionCanvas").transform.GetComponentInChildren<SceneTransitions>().LoadScene(0);
    }

    public void ReturntoCharacterSelect()
    {
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Tutorial")
        {
            RoundManager.gameActive = false;
            RoundManager.lockInputs = false;
            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character = "";
            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character = "";
            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Color = 0;
            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Color = 0;
            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().stage = "";
            pauseQuit = true;
            GameObject.Find("TransitionCanvas").transform.GetComponentInChildren<SceneTransitions>().LoadScene(1);
        }

    }

    //Function to check inputs on movelist
    private void MoveListInputs()
    {
        updateVideo = false;

        if (!acceptInputHor)
        {
            if (horizontal == 0)
            {
                acceptInputHor = true;
            }
        }

        if (acceptInputHor && horizontal < 0)
        {
            moveListIndex -= 1;
            acceptInputHor = false;
            updateVideo = true;

            if (verticalMoveListIndex != mList.maxVerticalIndex && moveListIndex != 0)
            {
                verticalMoveListIndex = 1;
                mList.resetMarker();
                SelectorMarkAnimator.Play("SelectionSlide", -1, 0f);
            }
            else if (verticalMoveListIndex == mList.maxVerticalIndex)
            {
                keepMaxIndex = true;
            }
        }
        else if (acceptInputHor && horizontal > 0)
        {
            moveListIndex += 1;
            acceptInputHor = false;
            updateVideo = true;

            if (verticalMoveListIndex != mList.maxVerticalIndex && moveListIndex != 5)
            {
                verticalMoveListIndex = 1;
                mList.resetMarker();
                SelectorMarkAnimator.Play("SelectionSlide", -1, 0f);
            }
            else if (verticalMoveListIndex == mList.maxVerticalIndex)
            {
                keepMaxIndex = true;
            }
            if (moveListIndex == 4)
            {
                mList.setUniversal1();
                if (playerPaused == 1)
                {
                    mList.setCharacter(PlayerData.GetComponent<SelectedCharacterManager>().P1Character + " Thorne");
                }
                else if(playerPaused == 2)
                {
                    mList.setCharacter(PlayerData.GetComponent<SelectedCharacterManager>().P2Character + " Thorne");
                }
            }
        }

        //Set Pages
        if (playerPaused == 1)
        {
            setMoveListPage(moveListIndex, PlayerData.GetComponent<SelectedCharacterManager>().P1Character);
        }
        else if(playerPaused == 2)
        {
            setMoveListPage(moveListIndex, PlayerData.GetComponent<SelectedCharacterManager>().P2Character);
        }
       
        if (keepMaxIndex)
        {
            verticalMoveListIndex = mList.maxVerticalIndex;
            switch (verticalMoveListIndex)
            {
                case 2:
                    mList.setMarkerPosition(1);
                    break;
                case 3:
                    mList.setMarkerPosition(2);
                    break;
                case 4:
                    mList.setMarkerPosition(3);
                    break;
                default:
                    mList.setMarkerPosition(4);
                    break;
            }
            keepMaxIndex = false;
        }

        //Movelist hortizontal scrolling
        if (moveListIndex < 1)
        {
            moveListIndex = 1;
        }
        else if (moveListIndex > 4)
        {
            moveListIndex = 4;
        }

        //Check Vertical Input
        if (!acceptInputVer)
        {
            if (vertical == 0)
            {
                acceptInputVer = true;
            }
        }

        if (acceptInputVer && vertical < 0)
        {
            verticalMoveListIndex += 1;
            acceptInputVer = false;
            updateVideo = true;

            if (verticalMoveListIndex == mList.maxVerticalIndex)
            {
                mList.disableMarker();
                MoveListMarker.color = new Color(1f, 1f, 1f, 1f);
                MoveListMarkAnimator.Play("MarkAnimation", -1, 0f);
            }
            else if (verticalMoveListIndex < mList.maxVerticalIndex)
            {
                mList.moveMarkerDown();
                SelectorMarkAnimator.Play("SelectionSlide", -1, 0f);
            }

            if (moveListIndex == 4 && mList.bottomCheck() && verticalMoveListIndex == 5)
            {
                mList.setUniversal2();
            }
            else if ((moveListIndex == 4 && mList.bottomCheck() && verticalMoveListIndex == 6))
            {
                mList.setUniversal3();
            }
        }
        else if (acceptInputVer && vertical > 0)
        {
            verticalMoveListIndex -= 1;
            acceptInputVer = false;
            updateVideo = true;

            if (verticalMoveListIndex != 0 && verticalMoveListIndex != (mList.maxVerticalIndex - 1))
            {
                mList.moveMarkerUp();
                SelectorMarkAnimator.Play("SelectionSlide", -1, 0f);
            }
            if (verticalMoveListIndex < mList.maxVerticalIndex)
            {
                mList.enableMarker();
                MoveListMarker.color = new Color(1f, 1f, 1f, 0f);
            }

            if ((moveListIndex == 4 && verticalMoveListIndex == 6) && mList.bottomCheck())
            {
                mList.setUniversal3();
                if (playerPaused == 1)
                {
                    mList.setCharacter(PlayerData.GetComponent<SelectedCharacterManager>().P1Character + " Thorne");
                }
                else if (playerPaused == 2)
                {
                    mList.setCharacter(PlayerData.GetComponent<SelectedCharacterManager>().P2Character + " Thorne");
                }
            }
            else if (moveListIndex == 4 && mList.topCheck() && verticalMoveListIndex == 2)
            {
                mList.setUniversal2();
                if (playerPaused == 1)
                {
                    mList.setCharacter(PlayerData.GetComponent<SelectedCharacterManager>().P1Character + " Thorne");
                }
                else if (playerPaused == 2)
                {
                    mList.setCharacter(PlayerData.GetComponent<SelectedCharacterManager>().P2Character + " Thorne");
                }
            }
            else if (moveListIndex == 4 && mList.topCheck() && verticalMoveListIndex == 1)
            {
                mList.setUniversal1();
                if (playerPaused == 1)
                {
                    mList.setCharacter(PlayerData.GetComponent<SelectedCharacterManager>().P1Character + " Thorne");
                }
                else if (playerPaused == 2)
                {
                    mList.setCharacter(PlayerData.GetComponent<SelectedCharacterManager>().P2Character + " Thorne");
                }
            }
        }

        //Movelist vertical scrolling
        if (verticalMoveListIndex < 1)
        {
            verticalMoveListIndex = 1;
        }
        else if (verticalMoveListIndex > mList.maxVerticalIndex)
        {
            verticalMoveListIndex = mList.maxVerticalIndex;
        }

        //changes video if cursor was moved
        if (updateVideo)
        {
            updateVideo = false;
            VideoScreen.GetComponent<UnityEngine.Video.VideoPlayer>().clip = videoToPlay[SelectVideo(verticalMoveListIndex, moveListIndex)];//SelectVideo(verticalMoveListIndex, moveListIndex);
            VideoScreen.GetComponent<UnityEngine.Video.VideoPlayer>().Play();

        }




        //Go back to pause menu
        if (((Input.GetButton(p1cross) && playerPaused == 1) || (Input.GetButton(p2cross) && playerPaused == 2)) && verticalMoveListIndex == mList.maxVerticalIndex && acceptBack)
        {
            MoveListBack();
            acceptMoveList = false;
            quitButton.Select();
            moveListButton.Select();
        }
        //Prevent double-inputs
        acceptBack = true;
    }

    //creates path to video file based on menu navigation
    private int SelectVideo(int vertical, int horizontal)
    {

        int pathToVideo = -1;
        string characterToShow = "";

        //dhalia menu
        if((PlayerData.GetComponent<SelectedCharacterManager>().P1Character == "Dhalia" && playerPaused == 1) || (PlayerData.GetComponent<SelectedCharacterManager>().P2Character == "Dhalia" && playerPaused == 2))
        {
            characterToShow += "DHA";
            if (horizontal == 1)
            {
                if (vertical == 1)
                {
                    pathToVideo = (int)videoClips.DHA_6L;
                }
                else if (vertical == 2)
                {
                    pathToVideo = (int)videoClips.DHA_6B;
                }
            }
            else if (horizontal == 2)
            {
                if (vertical == 1)
                {
                    pathToVideo = (int)videoClips.DHA_Patissiere;
                }
                else if (vertical == 2)
                {
                    pathToVideo = (int)videoClips.DHA_HeadRush;
                }
                else if (vertical == 3)
                {
                    pathToVideo = (int)videoClips.DHA_BloodBrave;
                }
                else if (vertical == 4)
                {
                    pathToVideo = (int)videoClips.DHA_BasketCase;
                }
            }
            else if (horizontal == 3)
            {
                if (vertical == 1)
                {
                    pathToVideo = (int)videoClips.DHA_Toaster;
                }
                else if (vertical == 2)
                {
                    pathToVideo = (int)videoClips.DHA_JudgementSabre;
                }
            }
            else if (horizontal == 4)
            {
                if (vertical == 1)
                {
                    pathToVideo = (int)videoClips.DHA_L;
                }
                else if (vertical == 2)
                {
                    pathToVideo = (int)videoClips.DHA_M;
                }
                else if (vertical == 3)
                {
                    pathToVideo = (int)videoClips.DHA_H;
                }
                else if (vertical == 4)
                {
                    pathToVideo = (int)videoClips.DHA_B;
                }
                else if (vertical == 5)
                {
                    pathToVideo = (int)videoClips.DHA_Cancel;
                }
                else if (vertical == 6)
                {
                    pathToVideo = (int)videoClips.DHA_Grab;
                }
            }
        }
        //achealis menu
        else if((PlayerData.GetComponent<SelectedCharacterManager>().P1Character == "Achealis" && playerPaused == 1) || (PlayerData.GetComponent<SelectedCharacterManager>().P2Character == "Achealis" && playerPaused == 2))
        {
            characterToShow += "ACH";
            if(horizontal == 2)
            {
                if(vertical == 1)
                {
                    pathToVideo = (int)videoClips.ACH_LevelHell;
                }
                else if (vertical == 2)
                {
                    pathToVideo = (int)videoClips.ACH_HeavenClimber;
                }
                else if (vertical == 3)
                {
                    pathToVideo = (int)videoClips.ACH_Starfall;
                }
            }
            else if (horizontal == 3)
            {
                if (vertical == 1)
                {
                    pathToVideo = (int)videoClips.ACH_ForsythiaMarduk;
                }
            }
            else if (horizontal == 4)
            {
                if(vertical == 1)
                {
                    pathToVideo = (int)videoClips.ACH_L;
                }
                else if (vertical == 2)
                {
                    pathToVideo = (int)videoClips.ACH_M;
                }
                else if (vertical == 3)
                {
                    pathToVideo = (int)videoClips.ACH_H;
                }
                else if (vertical == 4)
                {
                    pathToVideo = (int)videoClips.ACH_B;
                }
                else if (vertical == 5)
                {
                    pathToVideo = (int)videoClips.ACH_Cancel;
                }
                else if (vertical == 6)
                {
                    pathToVideo = (int)videoClips.ACH_Grab;
                }
            }
        }
        //print(pathToVideo);
        //fallback case
        if (pathToVideo == -1)
        {
            if (characterToShow.Equals("DHA"))
            {
                return (int)videoClips.DHA_Neutral;
            }
            else if (characterToShow.Equals("ACH"))
            {
                return (int)videoClips.ACH_Neutral;
            }
            else
            {
                return 0;
            }
        }
        else
            return pathToVideo;

    }

    //Function to set page
    private void setMoveListPage(int page, string character)
    {
        switch (character)
        {
            case "Dhalia":
                switch (page)
                {
                    case 1:
                        mList.setDhaliaPage1();
                        break;
                    case 2:
                        mList.setDhaliaPage2();
                        break;
                    case 3:
                        mList.setDhaliaPage3();
                        break;
                }
                break;
            case "Achealis":
                switch (page)
                {
                    case 1:
                        mList.setAchealisPage1();
                        break;
                    case 2:
                        mList.setAchealisPage2();
                        break;
                    case 3:
                        mList.setAchealisPage3();
                        break;
                }
                break;
        }
    }

    private void SetControllers()
    {
        p1cross = "Cross_P1" + UpdateControls(CheckXbox(0));
        p1circle = "Circle_P1" + UpdateControls(CheckXbox(0));
        inputHorizontal = "Horizontal_P1" + UpdateControls(CheckXbox(0));
        inputVertical = "Vertical_P1" + UpdateControls(CheckXbox(0));
        pauseCode1 = "Start_P1" + UpdateControls(CheckXbox(0));

        p2cross = "Cross_P2" + UpdateControls(CheckXbox(1));
        p2circle = "Circle_P2" + UpdateControls(CheckXbox(1));
        inputHorizontal2 = "Horizontal_P2" + UpdateControls(CheckXbox(1));
        inputVertical2 = "Vertical_P2" + UpdateControls(CheckXbox(1));
        pauseCode = "Start_P2" + UpdateControls(CheckXbox(1));
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