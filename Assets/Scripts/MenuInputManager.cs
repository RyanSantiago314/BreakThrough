using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInputManager : MonoBehaviour
{
	private float buttonIndex;
	//private string xboxInput;
	//private string ps4Input;
	private float vertical;
	private float horizontal;
	private float InputTimer;
	private float InputTimer2;
	public Button PlayLocalButton;
	public Button PlayOnlineButton;
	public Button OptionsButton;
	public Button QuitButton;
	public Button PlayVsPlayerButton;
	public Button PlayVsAiButton;
	public Button PracticeButton;
	public Button BackButton;
	public Button OptionsBackButton;
	public Button soundOptionsBackButton;
	public Button displayOptionsBackButton;
	public Button soundOptions;
	public Button displayOptions;
	public Toggle windowToggle;
	public Slider MusicSlider;
	public Slider MasterSlider;
	public Slider VoiceSlider;
	public Slider EffectsSlider;
	private MainMenu menu;
	private string state;
	private string optionState;
	private bool isXbox;
    public GameObject sideSelectScreen;
    private string mode;
    public GameObject P1Controller;
    public GameObject P2Controller;
    public GameObject P1COMText;
    public GameObject P2COMText;
    private int P1Position;
    private int P2Position;
    private float x;
    private float y = 126;
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
        //isXbox = false;
        Time.timeScale = 1;

        inputCross += UpdateControls(CheckXbox(0));
        inputCircle += UpdateControls(CheckXbox(0));
        inputHorizontal += UpdateControls(CheckXbox(0));
        inputVertical += UpdateControls(CheckXbox(0));

        inputCross2 += UpdateControls(CheckXbox(1));
        inputCircle2 += UpdateControls(CheckXbox(1));
        inputHorizontal2 += UpdateControls(CheckXbox(1));
        inputVertical2 += UpdateControls(CheckXbox(1));

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
    }

    // Update is called once per frame
    void Update()
    {
    	horizontal = Input.GetAxis(inputHorizontal);
    	vertical = Input.GetAxis(inputVertical);
    	if (InputTimer > 0) InputTimer -= Time.deltaTime;
    	else InputTimer = 0;

        if (InputTimer2 > 0) InputTimer2 -= Time.deltaTime;
        else InputTimer2 = 0;

        if (!sideSelectScreen.activeSelf)
        {
            if (InputTimer == 0 && !inDropdown)
            {
                if (vertical < 0)
                {
                    buttonIndex += 1;
                    InputTimer = 0.1f;
                }
                else if (vertical > 0)
                {
                    buttonIndex -= 1;
                    InputTimer = 0.1f;
                }
            }
            else if (InputTimer == 0 && inDropdown)
            {
                if (vertical < 0)
                {
                    dropdownIndex += 1;
                    InputTimer = 0.1f;
                }
                else if (vertical > 0)
                {
                    dropdownIndex -= 1;
                    InputTimer = 0.1f;
                }

                if (dropdownIndex < 0) dropdownIndex = 0;
                else if (dropdownIndex > 32) dropdownIndex = 32;
                resoutionDropdown.value = dropdownIndex;
            }
        }   	
    	
        //Main Menu Management
    	if (state == "main")
    	{
	        if (buttonIndex < 1) buttonIndex = 1;
			else if (buttonIndex > 4) buttonIndex = 4;
			if (buttonIndex == 1)
			{
				PlayLocalButton.Select();
				if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit"))
				{
					state = "local";
					buttonIndex = 1;
					PlayLocalButton.onClick.Invoke();
				}
			}
			else if (buttonIndex == 2)
			{
				PlayOnlineButton.Select();		
				if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit")) PlayOnlineButton.onClick.Invoke();
			}
			else if (buttonIndex == 3)
			{
				OptionsButton.Select();
				if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit"))
				{
					state = "options";
					buttonIndex = 1;
					OptionsButton.onClick.Invoke();
				}
			}
			else if (buttonIndex == 4)
			{
				QuitButton.Select();
				if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit")) menu.QuitGame();
			}
		}
        //Local Menu Management
		else if (state == "local")
    	{
	        if (buttonIndex < 1) buttonIndex = 1;
			else if (buttonIndex > 4) buttonIndex = 4;
			if (buttonIndex == 1)
			{
				PlayVsPlayerButton.Select();
				
                if ((Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit")) && !sideSelectScreen.activeSelf)
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
				PlayVsAiButton.Select();
                if ((Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit")) && !sideSelectScreen.activeSelf)
                {
                    x = 0;
                    y = 126;
                    P1Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 126, 0);
                    sideSelectScreen.SetActive(true);
                    mode = "AI";
                } 
				
			}
            else if (buttonIndex == 3)
            {
                PracticeButton.Select();
                if ((Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit")) && !sideSelectScreen.activeSelf)
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
				BackButton.Select();
				if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit"))
				{
					state = "main";
					buttonIndex = 1;
					BackButton.onClick.Invoke();
				}
			}

			if ((Input.GetButtonDown(inputCircle) || Input.GetButtonDown("Cancel")) && !sideSelectScreen.activeSelf)
			{
				state = "main";
				buttonIndex = 1;
				BackButton.onClick.Invoke();
			}
		}
        //Options Menu Management
		else if (state == "options")
    	{
			if (optionState == "mainOptions")
			{
		        if (buttonIndex < 1) buttonIndex = 1;
				else if (buttonIndex > 3) buttonIndex = 3;

				if (buttonIndex == 1)
				{
					soundOptions.Select();
					if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit"))
					{
						optionState = "soundOptions";
						buttonIndex = 1;
						soundOptions.onClick.Invoke();
					}

				}
				else if (buttonIndex == 2)
				{
					displayOptions.Select();
					if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit"))
					{
						optionState = "displayOptions";
						buttonIndex = 1;
						displayOptions.onClick.Invoke();
					}
				}
				else if (buttonIndex == 3)
				{
					OptionsBackButton.Select();
					if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit"))
					{
						state = "main";
						buttonIndex = 3;
						OptionsBackButton.onClick.Invoke();
					}
				}

				if (Input.GetButtonDown(inputCircle) || Input.GetButtonDown("Cancel"))
				{
					state = "main";
					buttonIndex = 3;
					OptionsBackButton.onClick.Invoke();
				}
			}
			else if (optionState == "soundOptions")
			{
				if (buttonIndex < 1) buttonIndex = 1;
				else if (buttonIndex > 5) buttonIndex = 5;

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
					if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit"))
					{
						optionState = "mainOptions";
						buttonIndex = 1;
						soundOptionsBackButton.onClick.Invoke();
					}
				}
				if (Input.GetButtonDown(inputCircle) || Input.GetButtonDown("Cancel"))
				{
					optionState = "mainOptions";
					buttonIndex = 1;
					soundOptionsBackButton.onClick.Invoke();
				}
			}
			else if (optionState == "displayOptions")
			{
				if (buttonIndex < 1) buttonIndex = 1;
				else if (buttonIndex > 3) buttonIndex = 3;

				if (buttonIndex == 1)
				{
					windowToggle.Select();
					if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit"))
					{
						windowToggle.isOn = !windowToggle.isOn;
						toggleWindowed();
					}
				}
				else if (buttonIndex == 2)
				{
					resoutionDropdown.Select();
					if ((Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit")) && !inDropdown)
					{
						dropdownIndex = 24;
						resoutionDropdown.value = dropdownIndex;
						inDropdown = true;
						Debug.Log("HWER");
					}
					if (Input.GetButtonDown(inputCircle) || Input.GetButtonDown("Cancel"))
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
					if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit"))
					{
						optionState = "mainOptions";
						buttonIndex = 2;
						displayOptionsBackButton.onClick.Invoke();
					}
				}
				if (Input.GetButtonDown(inputCircle) || Input.GetButtonDown("Cancel") && !inDropdown)
				{
					optionState = "mainOptions";
					buttonIndex = 2;
					displayOptionsBackButton.onClick.Invoke();
				}
			}
		}
        //SideSelection Management (Everything Below needs Xbox support as well)
        if (sideSelectScreen.activeSelf)
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
                        sideSelectScreen.SetActive(false);
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
                    P1Position = 0;
                    P2Position = 0;
                }

                //Accept start input if player has selected a side
                if (P1Position != 0)
                {
                    if (Input.GetButtonDown(inputCross))
                    {
                        sideSelectScreen.SetActive(false);
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode = "AI";
                        PlayVsAiButton.onClick.Invoke();
                    }
                }
            }

            //Handle Practice Selection
            if (mode == "Practice")
            {
                //Enable Computer Text for AI Mode
                P1COMText.SetActive(false);
                P2COMText.SetActive(false);
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
                        sideSelectScreen.SetActive(false);
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
                    //P2Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -205, 0);
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