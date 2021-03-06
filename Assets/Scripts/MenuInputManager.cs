﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuInputManager : MonoBehaviour
{
    public Animator MarkerAnimator;
    public Image Marker;
	private float buttonIndex;
	//private string xboxInput;
	//private string ps4Input;
	private float vertical;
	private float horizontal;
	private float InputTimer;
	private float InputTimer2;
	private float InputDelay;

	public Button PlayLocalButton;
	public Button PlayOnlineButton;
	public Button OptionsButton;
	public Button QuitButton;
	public Button PlayVsPlayerButton;
	public Button PlayVsAiButton;
	public Button PracticeButton;
	public Button TutorialButton;
	public Button BackButton;
	public Button OptionsBackButton;
	public Button soundOptionsBackButton;
	public Button displayOptionsBackButton;
    public Button controlsOptionsBackButton;
	public Button soundOptions;
	public Button displayOptions;
	public Button controlsOptions;
	public Toggle windowToggle;
	public Slider MusicSlider;
	public Slider MasterSlider;
	public Slider VoiceSlider;
	public Slider EffectsSlider;

	private MainMenu menu;

	private string state;
	private string optionState;
    private string mode;

    private bool isXbox;
    private bool resetDifficulty;
    private bool acceptConfirm;
    private bool pressingLeft;
    private bool pressingRight;

    public GameObject sideSelectScreen;
    public GameObject AIDifficultyScreen;

    public GameObject P1Controller;
    public GameObject P2Controller;
    public GameObject P1COMText;
    public GameObject P2COMText;
    public GameObject P1Arrows;
    public GameObject P2Arrows;
    public GameObject CPULevel;

    private int P1Position;
    private int P2Position;
    private int COMLevel;
    private float x;
    private float y;
    private float x2;
    private float y2 = -205;

    private string inputCross = "Cross_P1";
    private string inputCircle = "Circle_P1";
    private string inputHorizontal = "Horizontal_P1";
    private string inputVertical = "Vertical_P1";

    private string inputCross2 = "Cross_P2";
    private string inputCircle2 = "Circle_P2";
    private string inputHorizontal2 = "Horizontal_P2";
    private string inputVertical2 = "Vertical_P2";

    public Dropdown resoutionDropdown;
    Resolution[] resolutions;
    private int dropdownIndex;
    private bool inDropdown;
    private float xValue;
    private float yValue;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonIndex = 1;
        InputTimer = 0;
        //xboxInput = "Controller (Xbox One For Windows)";
        //ps4Input = "Wireless Controller";
        PlayLocalButton.Select();
        menu = GetComponent<MainMenu>();
        state = "main";
        optionState = "mainOptions";
        COMLevel = 1;
        //isXbox = false;
        Time.timeScale = 1;
        /*
        inputCross += UpdateControls(CheckXbox(0));
        inputCircle += UpdateControls(CheckXbox(0));
        inputHorizontal += UpdateControls(CheckXbox(0));
        inputVertical += UpdateControls(CheckXbox(0));

        inputCross2 += UpdateControls(CheckXbox(1));
        inputCircle2 += UpdateControls(CheckXbox(1));
        inputHorizontal2 += UpdateControls(CheckXbox(1));
        inputVertical2 += UpdateControls(CheckXbox(1));
        */
        SetControllers();

        resolutions = Screen.resolutions;
        resoutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
        	string option = resolutions[i].width + " x " + resolutions[i].height;
        	options.Add(option);
        	if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
        	{
        		currentResolutionIndex = i;
        	}
        }
        resoutionDropdown.AddOptions(options);
        resoutionDropdown.value = currentResolutionIndex;
        resoutionDropdown.RefreshShownValue();

        inDropdown = false;

        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode = "";
        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side = "";
        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side= "";
        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().CPUDifficulty = 50;

        xValue = Marker.GetComponent<RectTransform>().localPosition.x + 15;
        MarkerAnimator.Play("MarkAnimation", -1, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        SetControllers();
        horizontal = Input.GetAxis(inputHorizontal);
    	vertical = Input.GetAxis(inputVertical);
    	if (InputTimer > 0) InputTimer -= Time.deltaTime;
    	else InputTimer = 0;

        if (InputTimer2 > 0) InputTimer2 -= Time.deltaTime;
        else InputTimer2 = 0;

        if (InputDelay > 0) InputDelay -= Time.deltaTime;
        else InputDelay = 0;

        if (!sideSelectScreen.activeSelf && !AIDifficultyScreen.activeSelf)
        {
            if (InputTimer == 0 && !inDropdown)
            {
                if (vertical < 0 && optionState != "controlsOptions")
                {
                    buttonIndex += 1;
                    InputTimer = 0.2f;
                    MarkerAnimator.Play("MarkAnimation", -1, 0f);
                }
                else if (vertical > 0 && optionState != "controlsOptions")
                {
                    buttonIndex -= 1;
                    InputTimer = 0.2f;
                    MarkerAnimator.Play("MarkAnimation", -1, 0f);
                }
            }
            else if (InputTimer == 0 && inDropdown)
            {
                if (vertical < 0)
                {
                    dropdownIndex += 1;
                    InputTimer = 0.2f;
                }
                else if (vertical > 0)
                {
                    dropdownIndex -= 1;
                    InputTimer = 0.2f;
                }

                if (dropdownIndex < 0) dropdownIndex = 0;
                else if (dropdownIndex > 32) dropdownIndex = 32;
                resoutionDropdown.value = dropdownIndex;
            }
        }   	
    	
        //Main Menu Management
    	if (state == "main")
    	{
	        if (buttonIndex < 1) buttonIndex = 4;
			else if (buttonIndex > 4) buttonIndex = 1;
			if (buttonIndex == 1)
			{
                yValue = PlayLocalButton.GetComponent<RectTransform>().localPosition.y;
                Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue-30, yValue+15, 0);
                Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(410f, 88.3f);
                //PlayLocalButton.Select();
                if (Input.GetButtonDown(inputCross))
				{
					state = "local";
					buttonIndex = 1;
					PlayLocalButton.onClick.Invoke();
                    MarkerAnimator.Play("MarkAnimation", -1, 0f);
                }
			}
			else if (buttonIndex == 2)
			{
                yValue = PlayOnlineButton.GetComponent<RectTransform>().localPosition.y;
                Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue-30, yValue+15, 0);
                Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(410f, 88.3f);
                //PlayOnlineButton.Select();		
                if (Input.GetButtonDown(inputCross)) PlayOnlineButton.onClick.Invoke();
			}
			else if (buttonIndex == 3)
			{
                yValue = OptionsButton.GetComponent<RectTransform>().localPosition.y;
                Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue-70, yValue+15, 0);
                Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(320f, 88.3f);
                //OptionsButton.Select();
                if (Input.GetButtonDown(inputCross))
				{
					state = "options";
					buttonIndex = 1;
					OptionsButton.onClick.Invoke();
                    MarkerAnimator.Play("MarkAnimation", -1, 0f);
                }
			}
			else if (buttonIndex == 4)
			{
                yValue = QuitButton.GetComponent<RectTransform>().localPosition.y;
                Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue-100, yValue+10, 0);
                Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(240f, 88.3f);
                //QuitButton.Select();
                if (Input.GetButtonDown(inputCross)) menu.QuitGame();
			}
		}
        //Local Menu Management
		else if (state == "local")
    	{
	        if (buttonIndex < 1) buttonIndex = 5;
			else if (buttonIndex > 5) buttonIndex = 1;
			if (buttonIndex == 1)
			{
                yValue = PlayVsPlayerButton.GetComponent<RectTransform>().localPosition.y;
                Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue+10, yValue-70, 0);
                Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 88.3f);
                //PlayVsPlayerButton.Select();

                if ((Input.GetButtonDown(inputCross)) && !sideSelectScreen.activeSelf)
                {
                    x = 0;
                    y = 126;
                    P1Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 126, 0);
                    x2 = 0;
                    y2 = -205;
                    P2Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -205, 0);
                    sideSelectScreen.SetActive(true);
                    mode = "PvP";
                } 
			}
			else if (buttonIndex == 2)
			{
                yValue = PlayVsAiButton.GetComponent<RectTransform>().localPosition.y;
                Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue-30, yValue - 80, 0);
                Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(410f, 88.3f);
                //PlayVsAiButton.Select();
                if ((Input.GetButtonDown(inputCross)) && !sideSelectScreen.activeSelf && !AIDifficultyScreen.activeSelf)
                {
                    x = 0;
                    y = 126;
                    P1Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 126, 0);
                    acceptConfirm = false;
                    AIDifficultyScreen.SetActive(true);
                    InputTimer = 0;
                    mode = "AI";
                } 
				
			}
            else if (buttonIndex == 3)
            {
                yValue = PracticeButton.GetComponent<RectTransform>().localPosition.y;
                Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue-60, yValue - 90, 0);
                Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(340f, 88.3f);
                //PracticeButton.Select();
                if ((Input.GetButtonDown(inputCross)) && !sideSelectScreen.activeSelf)
                {
                    x = 0;
                    y = 126;
                    P1Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 126, 0);
                    sideSelectScreen.SetActive(true);
                    mode = "Practice";
                }

            }
            else if (buttonIndex == 4)
            {
                yValue = TutorialButton.GetComponent<RectTransform>().localPosition.y;
                Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue-60, yValue - 105, 0);
                Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(340f, 88.3f);
                //TutorialButton.Select();
                if (Input.GetButtonDown(inputCross))
                {
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode = "Tutorial";
                    //GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode = "Practice";
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().stage = "TrainingStage";
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character = "Dhalia";
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character = "Dhalia";
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side = "Left";
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side = "Right";
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Color = 1;
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Color = 2;
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().CPUDifficulty = 50;
                    TutorialButton.onClick.Invoke();
                }
            }
            else if (buttonIndex == 5)
			{
                yValue = BackButton.GetComponent<RectTransform>().localPosition.y;
                Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue-100, yValue - 120, 0);
                Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(240f, 88.3f);
                //BackButton.Select();
                if (Input.GetButtonDown(inputCross))
				{
					state = "main";
					buttonIndex = 1;
					BackButton.onClick.Invoke();
                    MarkerAnimator.Play("MarkAnimation", -1, 0f);
                }
			}

			if ((Input.GetButtonDown(inputCircle)) && !sideSelectScreen.activeSelf && !AIDifficultyScreen.activeSelf)
			{
				state = "main";
				buttonIndex = 1;
				BackButton.onClick.Invoke();
                MarkerAnimator.Play("MarkAnimation", -1, 0f);
            }
		}
        //Options Menu Management
		else if (state == "options")
    	{
            if (optionState == "mainOptions")
            {
                if (buttonIndex < 1) buttonIndex = 4;
                else if (buttonIndex > 4) buttonIndex = 1;

                if (buttonIndex == 1)
                {
                    yValue = soundOptions.GetComponent<RectTransform>().localPosition.y;
                    Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue - 70, yValue - 10, 0);
                    Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(240f, 88.3f);
                    //soundOptions.Select();

                    if (Input.GetButtonDown(inputCross))
					{
						optionState = "soundOptions";
						buttonIndex = 1;
						soundOptions.onClick.Invoke();
                        Marker.color = new Color(Marker.color.r, Marker.color.g, Marker.color.b, 0f);
                    }

                }
                else if (buttonIndex == 2)
                {
                    yValue = displayOptions.GetComponent<RectTransform>().localPosition.y;
                    Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue - 50, yValue - 10, 0);
                    Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(270f, 88.3f);
                    //displayOptions.Select();
                    if (Input.GetButtonDown(inputCross))
					{
						optionState = "displayOptions";
						buttonIndex = 1;
						displayOptions.onClick.Invoke();
                        Marker.color = new Color(Marker.color.r, Marker.color.g, Marker.color.b, 0f);
                    }
                }
                else if (buttonIndex == 3)
                {
                    yValue = controlsOptions.GetComponent<RectTransform>().localPosition.y;
                    Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue - 30, yValue - 10, 0);
                    Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(320f, 88.3f);
                    //controlsOptions.Select();
                    if (Input.GetButtonDown(inputCross))
                    {
                        optionState = "controlsOptions";
                        yValue = controlsOptionsBackButton.GetComponent<RectTransform>().localPosition.y;
                        Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue + 10, yValue, 0);
                        Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(280f, 88.3f);
                        buttonIndex = 1;
                        controlsOptions.onClick.Invoke();
                        MarkerAnimator.Play("MarkAnimation", -1, 0f);
                    }
                }
                else if (buttonIndex == 4)
                {
                    yValue = OptionsBackButton.GetComponent<RectTransform>().localPosition.y;
                    Marker.GetComponent<RectTransform>().localPosition = new Vector3(xValue - 70, yValue + 10, 0);
                    Marker.GetComponent<RectTransform>().sizeDelta = new Vector2(240f, 88.3f);
                    //OptionsBackButton.Select();
                    if (Input.GetButtonDown(inputCross))
					{
						state = "main";
						buttonIndex = 3;
						OptionsBackButton.onClick.Invoke();
                        MarkerAnimator.Play("MarkAnimation", -1, 0f);
                    }
				}

				if (Input.GetButtonDown(inputCircle))
				{
					state = "main";
					buttonIndex = 3;
					OptionsBackButton.onClick.Invoke();
                    MarkerAnimator.Play("MarkAnimation", -1, 0f);

                }
			}
			else if (optionState == "soundOptions")
			{
				if (buttonIndex < 1) buttonIndex = 5;
				else if (buttonIndex > 5) buttonIndex = 1;

				if (buttonIndex == 1)
				{
					MasterSlider.Select();
					if (horizontal > 0) MasterSlider.value += 0.01f;
					else if (horizontal < 0) MasterSlider.value -= 0.01f;
				}
				else if (buttonIndex == 2)
				{
					MusicSlider.Select();
					if (horizontal > 0) MusicSlider.value += 0.01f;
					else if (horizontal < 0) MusicSlider.value -= 0.01f;
				}
				else if (buttonIndex == 3)
				{
					VoiceSlider.Select();
					if (horizontal > 0) VoiceSlider.value += 0.01f;
					else if (horizontal < 0) VoiceSlider.value -= 0.01f;
				}
				else if (buttonIndex == 4)
				{
					EffectsSlider.Select();
					if (horizontal > 0) EffectsSlider.value += 0.01f;
					else if (horizontal < 0) EffectsSlider.value -= 0.01f;
				}
				else if (buttonIndex == 5)
				{
					soundOptionsBackButton.Select();
					if (Input.GetButtonDown(inputCross))
					{
						optionState = "mainOptions";
						buttonIndex = 1;
						soundOptionsBackButton.onClick.Invoke();
                        Marker.color = new Color(Marker.color.r, Marker.color.g, Marker.color.b, 1f);
                        MarkerAnimator.Play("MarkAnimation", -1, 0f);
                    }
				}
				if (Input.GetButtonDown(inputCircle))
				{
					optionState = "mainOptions";
					buttonIndex = 1;
					soundOptionsBackButton.onClick.Invoke();
                    Marker.color = new Color(Marker.color.r, Marker.color.g, Marker.color.b, 1f);
                    MarkerAnimator.Play("MarkAnimation", -1, 0f);
                }
			}
			else if (optionState == "displayOptions")
			{
				if (buttonIndex < 1) buttonIndex = 3;
				else if (buttonIndex > 3) buttonIndex = 1;

				if (buttonIndex == 1)
				{
					windowToggle.Select();
					if (Input.GetButtonDown(inputCross))
					{
						windowToggle.isOn = !windowToggle.isOn;
						toggleWindowed();
					}
				}
				else if (buttonIndex == 2)
				{
					resoutionDropdown.Select();
					if ((Input.GetButtonDown(inputCross)) && !inDropdown)
					{
						dropdownIndex = 24;
						resoutionDropdown.value = dropdownIndex;
						inDropdown = true;
					}
					if (Input.GetButtonDown(inputCircle))
					{
						inDropdown = false;
						optionState = "mainOptions";
						buttonIndex = 2;
						displayOptionsBackButton.onClick.Invoke();
					}
				}
				else if (buttonIndex == 3)
				{
					displayOptionsBackButton.Select();
					if (Input.GetButtonDown(inputCross))
					{
						optionState = "mainOptions";
						buttonIndex = 2;
						displayOptionsBackButton.onClick.Invoke();
                        Marker.color = new Color(Marker.color.r, Marker.color.g, Marker.color.b, 1f);
                        MarkerAnimator.Play("MarkAnimation", -1, 0f);
                    }
				}
				if (Input.GetButtonDown(inputCircle) && !inDropdown)
				{
					optionState = "mainOptions";
					buttonIndex = 2;
					displayOptionsBackButton.onClick.Invoke();
                    Marker.color = new Color(Marker.color.r, Marker.color.g, Marker.color.b, 1f);
                    MarkerAnimator.Play("MarkAnimation", -1, 0f);
                }
            }
            else if (optionState == "controlsOptions")
            {
                if (Input.GetButtonDown(inputCross) || Input.GetButtonDown(inputCircle))
                {
                    optionState = "mainOptions";
                    buttonIndex = 3;
                    controlsOptionsBackButton.onClick.Invoke();
                    MarkerAnimator.Play("MarkAnimation", -1, 0f);
                }
            }
		}

        //AI Difficulty ManageMent (Placed here for input execution purposes)
        if (AIDifficultyScreen.activeSelf)
        {
            if (horizontal < 0 && InputTimer == 0)
            {
                COMLevel -= 1;
                InputTimer = 0.2f;
            }
            else if (horizontal > 0 && InputTimer == 0)
            {
                COMLevel += 1;
                InputTimer = 0.2f;
            }

            //Cycle COM Levels
            if (COMLevel < 0)
            {
                COMLevel = 2;
            }
            if (COMLevel > 2)
            {
                COMLevel = 0;
            }

            //Update COM Level Text
            switch (COMLevel)
            {
                case 0:
                    CPULevel.GetComponent<Text>().text = "< Easy >";
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().CPUDifficulty = 10;
                    break;
                case 1:
                    CPULevel.GetComponent<Text>().text = "< Medium >";
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().CPUDifficulty = 50;
                    break;
                case 2:
                    CPULevel.GetComponent<Text>().text = "< Hard >";
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().CPUDifficulty = 90;
                    break;
            }

            //Accept Confirmation
            if (Input.GetButtonDown(inputCross) && acceptConfirm)
            {
                x = 0;
                y = 126;
                P1Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 126, 0);
                AIDifficultyScreen.SetActive(false);
                sideSelectScreen.SetActive(true);
            }
            else if (Input.GetButtonDown(inputCircle))
            {
                AIDifficultyScreen.SetActive(false);
            }
            //Allow accept input to avoid accepting two screens at once
            acceptConfirm = true;
        }

        //SideSelection Management
        if (sideSelectScreen.activeSelf && !SceneTransitions.lockinputs)
        {
            //Handle Player vs. Player side selection
            if (mode == "PvP")
            {
                //Disable Computer Text for AI Mode
                P1COMText.SetActive(false);
                P2COMText.SetActive(false);
                P2Controller.SetActive(true);

                //Handle P1 Controller Movement
                if (Input.GetAxis(inputHorizontal) == -1 && InputTimer == 0)
                {
                    if (P1Position != -1)
                    {
                        if ((P1Position-1 == -1 && P2Position != -1) || P1Position-1 == 0)
                        {
                            P1Position -= 1;
                        }  
                    }
                    InputTimer = 0.15f;                   
                }
                else if (Input.GetAxis(inputHorizontal) == 1 && InputTimer == 0)
                {
                    if (P1Position != 1)
                    {
                        if ((P1Position+1 == 1 && P2Position != 1) || P1Position+1 == 0)
                        {
                            P1Position += 1;
                        }
                    }
                    InputTimer = 0.15f;  
                }
                if (Input.GetButtonDown(inputCircle))
                {
                    sideSelectScreen.SetActive(false);
                    P1Position = 0;
                    P2Position = 0;
                    acceptConfirm = false;
                }

                //Handle P2 Controller Movement
                if (Input.GetAxis(inputHorizontal2) == -1 && InputTimer2 == 0)
                {
                    if (P2Position != -1) {
                        if ((P2Position - 1 == -1 && P1Position != -1) || P2Position - 1 == 0)
                        {
                            P2Position -= 1;
                        }
                    }
                    InputTimer2 = 0.15f;
                }
                else if (Input.GetAxis(inputHorizontal2) == 1 && InputTimer2 == 0)
                {
                    if (P2Position != 1)
                    {
                        if ((P2Position + 1 == 1 && P1Position != 1) || P2Position + 1 == 0)
                        {
                            P2Position += 1;
                        }
                    }
                    InputTimer2 = 0.15f;
                }
                if (Input.GetButtonDown(inputCircle2))
                {
                    sideSelectScreen.SetActive(false);
                    P1Position = 0;
                    P2Position = 0;
                }

                //Accept start input if both players have selected different sides
                if (P1Position != 0 && P2Position !=0 && P1Position != P2Position) {
                    if (Input.GetButtonDown(inputCross) || Input.GetButtonDown(inputCross2))
                    {
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode = "PvP";
                        PlayVsPlayerButton.onClick.Invoke();
                    }
                }
            }

            //Handle Player vs. AI selection
            if (mode == "AI")
            {
                //Enable Computer Text for AI Mode
                P1COMText.SetActive(true);
                P2COMText.SetActive(true);
                P2Controller.SetActive(false);
                
                //Handle P1 Controller Movement
                if (Input.GetAxis(inputHorizontal) == -1 && InputTimer == 0)
                {
                    if (P1Position != -1)
                    {
                        P1Position -= 1;
                    }
                    InputTimer = 0.15f;
                }
                else if (Input.GetAxis(inputHorizontal) == 1 && InputTimer == 0)
                {
                    if (P1Position != 1)
                    {
                        P1Position += 1;
                    }
                    InputTimer = 0.15f;
                }
                if (Input.GetButtonDown(inputCircle))
                {
                    sideSelectScreen.SetActive(false);
                    AIDifficultyScreen.SetActive(true);
                    P1Position = 0;
                    P2Position = 0;
                }

                //Accept start input if player has selected a side
                if (P1Position != 0)
                {
                    if (Input.GetButtonDown(inputCross))
                    {
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode = "AI";
                        PlayVsAiButton.onClick.Invoke();
                    }
                }
            }

            //Handle Practice Selection
            if (mode == "Practice")
            {
                //Enable Computer Text for AI Mode
                P1COMText.SetActive(true);
                P2COMText.SetActive(true);
                P2Controller.SetActive(false);

                //Handle P1 Controller Movement
                if (Input.GetAxis(inputHorizontal) == -1 && InputTimer == 0)
                {
                    if (P1Position != -1)
                    {
                        P1Position -= 1;
                    }
                    InputTimer = 0.15f;
                }
                else if (Input.GetAxis(inputHorizontal) == 1 && InputTimer == 0)
                {
                    if (P1Position != 1)
                    {
                        P1Position += 1;
                    }
                    InputTimer = 0.15f;
                }
                if (Input.GetButtonDown(inputCircle))
                {
                    sideSelectScreen.SetActive(false);
                    P1Position = 0;
                    P2Position = 0;
                }

                //Accept start input if player has selected a side
                if (P1Position != 0)
                {
                    if (Input.GetButtonDown(inputCross))
                    {
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode = "Practice";
                        PlayVsPlayerButton.onClick.Invoke();
                    }
                }
            }

            //Update P1Controller Position
            switch (P1Position)
            {
                case -1:
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side = "Left";
                    P1Arrows.GetComponent<Text>().text = "                     >";
                    //Smoothly move controller icon
                    if (x > -425)
                    {
                        x -= 85;
                        
                    }
                    if (y > -21)
                    {
                        y -= 29.6f;
                    }
                    P1Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                    if (mode == "AI" || mode == "Practice")
                    {
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side = "Right";
                    }
                    break;
                case 0:
                    P1Arrows.GetComponent<Text>().text = "<                    >";
                    //Smoothly move controller icon
                    if (x > 0)
                    {
                        x -= 85;

                    }
                    else if (x < 0)
                    {
                        x += 85;

                    }
                    if (y < 126)
                    {
                        y += 29.6f;
                    }
                    P1Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                    break;
                case 1:
                    P1Arrows.GetComponent<Text>().text = "<                     ";
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side = "Right";
                    //Smoothly move controller icon
                    if (x < 425)
                    {
                        x += 85;

                    }
                    if (y > -21)
                    {
                        y -= 29.6f;
                    }
                    if (mode == "AI" || mode == "Practice")
                    {
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side = "Left";
                    }
                    P1Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);                  
                    break;
            }

            //Update P2Controller Position
            switch (P2Position)
            {
                case -1:
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side = "Left";
                    P2Arrows.GetComponent<Text>().text = "                     >";
                    //Smoothly move controller icon
                    if (x2 > -425)
                    {
                        x2 -= 85;

                    }
                    if (y2 < -22)
                    {
                        y2 += 36.6f;
                    }
                    P2Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(x2, y2, 0);                  
                    break;
                case 0:
                    P2Arrows.GetComponent<Text>().text = "<                    >";
                    //Smoothly move controller icon
                    if (x2 > 0)
                    {
                        x2 -= 85;

                    }
                    else if (x2 < 0)
                    {
                        x2 += 85;

                    }
                    if (y2 > -205)
                    {
                        y2 -= 36.6f;
                    }
                    P2Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(x2, y2, 0);
                    break;
                case 1:
                    P2Arrows.GetComponent<Text>().text = "<                     ";
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side = "Right";
                    //Smoothly move controller icon
                    if (x2 < 425)
                    {
                        x2 += 85;

                    }
                    if (y2 < -22)
                    {
                        y2 += 36.6f;
                    }
                    P2Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(x2, y2, 0);                  
                    break;
            }
        }

        //Update CPU Difficulty
        if (SceneManager.GetActiveScene().name == "MainMenu" && resetDifficulty == false)
        {
            resetDifficulty = true;
        }
        else if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            resetDifficulty = false;
        }
    }

    private void SetControllers()
    {
        inputCross = "Cross_P1" + UpdateControls(CheckXbox(0));
        inputCircle = "Circle_P1" + UpdateControls(CheckXbox(0));
        inputHorizontal = "Horizontal_P1" + UpdateControls(CheckXbox(0));
        inputVertical = "Vertical_P1" + UpdateControls(CheckXbox(0));

        inputCross2 = "Cross_P2" + UpdateControls(CheckXbox(1));
        inputCircle2 = "Circle_P2" + UpdateControls(CheckXbox(1));
        inputHorizontal2 = "Horizontal_P2" + UpdateControls(CheckXbox(1));
        inputVertical2 = "Vertical_P2" + UpdateControls(CheckXbox(1));
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

    public void toggleWindowed()
    {
    	if (Screen.fullScreen == true)
    	{
			Screen.fullScreen = false;
		} 
		else
		{
			Screen.fullScreen = true;
		}
    }
}