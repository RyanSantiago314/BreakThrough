using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInputManager : MonoBehaviour
{
	private float buttonIndex;
	private string xboxInput;
	private string ps4Input;
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
	public Button BackButton;
	public Button OptionsBackButton;
	private MainMenu menu;
	private string state;
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

    // Start is called before the first frame update
    void Start()
    {
     buttonIndex = 1;
     InputTimer = 0;
     xboxInput = "Controller (Xbox One For Windows)";
     ps4Input = "Wireless Controller";
     PlayLocalButton.Select();
     menu = GetComponent<MainMenu>();
     state = "main";
     isXbox = false;
     Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
    	horizontal = Input.GetAxis("Horizontal_P1");
    	vertical = Input.GetAxis("Vertical_P1");

    	if (InputTimer > 0) InputTimer -= Time.deltaTime;
    	else InputTimer = 0;

        if (InputTimer2 > 0) InputTimer2 -= Time.deltaTime;
        else InputTimer2 = 0;

        if (!sideSelectScreen.activeSelf)
        {
            //If an input device is detected then establish what device it is in order to properly decipher inputs
            if (Input.GetJoystickNames().Length > 0)
            {
                if (Input.GetJoystickNames()[0] == xboxInput && InputTimer == 0)
                {
                    if (horizontal == -1)
                    {
                        buttonIndex += 1;
                        InputTimer = 0.25f;
                    }
                    else if (horizontal == 1)
                    {
                        buttonIndex -= 1;
                        InputTimer = 0.25f;
                    }
                    isXbox = true;
                }
                else if (Input.GetJoystickNames()[0] == ps4Input && InputTimer == 0)
                {
                    if (vertical == -1)
                    {
                        buttonIndex += 1;
                        InputTimer = 0.25f;
                    }
                    else if (vertical == 1)
                    {
                        buttonIndex -= 1;
                        InputTimer = 0.25f;
                    }
                    isXbox = false;
                }
                else if (Input.GetJoystickNames()[0] == "" && InputTimer == 0)
                {
                    if (vertical == -1)
                    {
                        buttonIndex += 1;
                        InputTimer = 0.25f;
                    }
                    else if (vertical == 1)
                    {
                        buttonIndex -= 1;
                        InputTimer = 0.25f;
                    }
                    isXbox = false;
                }
            }
            else if (Input.GetJoystickNames().Length < 0 && InputTimer == 0)
            {
                if (vertical == -1)
                {
                    buttonIndex += 1;
                    InputTimer = 0.25f;
                }
                else if (vertical == 1)
                {
                    buttonIndex -= 1;
                    InputTimer = 0.25f;
                }
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
				if (isXbox)
				{
					if (Input.GetButtonDown("Square_P1") || Input.GetButtonDown("Submit"))
					{
						state = "local";
						buttonIndex = 1;
						PlayLocalButton.onClick.Invoke();
					}
				}
				else
				{
					if (Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Submit"))
					{
						state = "local";
						buttonIndex = 1;
						PlayLocalButton.onClick.Invoke();
					}
				}
			}
			else if (buttonIndex == 2)
			{
				PlayOnlineButton.Select();
				if (isXbox)
				{
					if (Input.GetButtonDown("Square_P1") || Input.GetButtonDown("Submit")) PlayOnlineButton.onClick.Invoke();
				}
				else
				{
					if (Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Submit")) PlayOnlineButton.onClick.Invoke();
				}
			}
			else if (buttonIndex == 3)
			{
				OptionsButton.Select();
				if (isXbox)
				{
					if (Input.GetButtonDown("Square_P1") || Input.GetButtonDown("Submit"))
					{
						state = "options";
						buttonIndex = 1;
						OptionsButton.onClick.Invoke();
					}
				}
				else
				{
					if (Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Submit"))
					{
						state = "options";
						buttonIndex = 1;
						OptionsButton.onClick.Invoke();
					}
				}
			}
			else if (buttonIndex == 4)
			{
				QuitButton.Select();
				if (isXbox)
				{
					if (Input.GetButtonDown("Square_P1") || Input.GetButtonDown("Submit")) menu.QuitGame();
				}
				else
				{
					if (Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Submit")) menu.QuitGame();
				}
			}
		}
        //Local Menu Management
		else if (state == "local")
    	{
	        if (buttonIndex < 1) buttonIndex = 1;
			else if (buttonIndex > 3) buttonIndex = 3;

			if (buttonIndex == 1)
			{
				PlayVsPlayerButton.Select();
				if (isXbox)
				{
                    if ((Input.GetButtonDown("Square_P1") || Input.GetButtonDown("Submit")) && !sideSelectScreen.activeSelf)
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
				else
				{
                    if ((Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Submit")) && !sideSelectScreen.activeSelf)
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
				
			}
			else if (buttonIndex == 2)
			{
				PlayVsAiButton.Select();
				if (isXbox)
				{
                    if ((Input.GetButtonDown("Square_P1") || Input.GetButtonDown("Submit")) && !sideSelectScreen.activeSelf)
                    {
                        x = 0;
                        y = 126;
                        P1Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 126, 0);
                        sideSelectScreen.SetActive(true);
                        mode = "AI";
                    } 
				}
				else
				{
                    if ((Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Submit")) && !sideSelectScreen.activeSelf)
                    {
                        x = 0;
                        y = 126;
                        P1Controller.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 126, 0);
                        sideSelectScreen.SetActive(true);
                        mode = "AI";
                    } 
				}
			}
			else if (buttonIndex == 3)
			{
				BackButton.Select();
				if (isXbox)
				{
					if (Input.GetButtonDown("Square_P1") || Input.GetButtonDown("Submit"))
					{
						state = "main";
						buttonIndex = 1;
						BackButton.onClick.Invoke();
					}
				}
				else
				{
					if (Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Submit"))
					{
						state = "main";
						buttonIndex = 1;
						BackButton.onClick.Invoke();
					}
				}
			}

			if (isXbox)
			{
				if ((Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Cancel")) && !sideSelectScreen.activeSelf)
				{
					state = "main";
					buttonIndex = 1;
					BackButton.onClick.Invoke();
				}
			}
			else
			{
				if ((Input.GetButtonDown("Circle_P1") || Input.GetButtonDown("Cancel")) && !sideSelectScreen.activeSelf)
				{
					state = "main";
					buttonIndex = 1;
					BackButton.onClick.Invoke();
				}
			}
		}
        //Options Menu Management
		else if (state == "options")
    	{
	        if (buttonIndex < 1) buttonIndex = 1;
			else if (buttonIndex > 1) buttonIndex = 1;
			
			if (buttonIndex == 1)
			{
				OptionsBackButton.Select();
				if (isXbox)
				{
					if (Input.GetButtonDown("Square_P1") || Input.GetButtonDown("Submit"))
					{
						state = "main";
						buttonIndex = 3;
						OptionsBackButton.onClick.Invoke();
					}
				}
				else
				{
					if (Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Submit"))
					{
						state = "main";
						buttonIndex = 3;
						OptionsBackButton.onClick.Invoke();
					}
				}
			}

			if (isXbox)
			{
				if (Input.GetButtonDown("Square_P1") || Input.GetButtonDown("Cancel"))
				{
					state = "main";
					buttonIndex = 3;
					OptionsBackButton.onClick.Invoke();
				}
			}
			else
			{
				if (Input.GetButtonDown("Circle_P1") || Input.GetButtonDown("Cancel"))
				{
					state = "main";
					buttonIndex = 3;
					OptionsBackButton.onClick.Invoke();
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
                if (Input.GetAxis("Horizontal_P1") == -1 && InputTimer == 0)
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
                else if (Input.GetAxis("Horizontal_P1") == 1 && InputTimer == 0)
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
                if (Input.GetButtonDown("Circle_P1"))
                {
                    sideSelectScreen.SetActive(false);
                    P1Position = 0;
                    P2Position = 0;
                }

                //Handle P2 Controller Movement
                if (Input.GetAxis("Horizontal_P2") == -1 && InputTimer2 == 0)
                {
                    if (P2Position != -1) {
                        if ((P2Position - 1 == -1 && P1Position != -1) || P2Position - 1 == 0)
                        {
                            P2Position -= 1;
                        }
                    }
                    InputTimer2 = 0.15f;
                }
                else if (Input.GetAxis("Horizontal_P2") == 1 && InputTimer2 == 0)
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
                if (Input.GetButtonDown("Circle_P2"))
                {
                    sideSelectScreen.SetActive(false);
                    P1Position = 0;
                    P2Position = 0;
                }

                //Accept start input if both players have selected different sides
                if (P1Position != 0 && P2Position !=0 && P1Position != P2Position) {
                    if (Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Cross_P2"))
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
                if (Input.GetAxis("Horizontal_P1") == -1 && InputTimer == 0)
                {
                    if (P1Position != -1)
                    {
                        P1Position -= 1;
                    }
                    InputTimer = 0.15f;
                }
                else if (Input.GetAxis("Horizontal_P1") == 1 && InputTimer == 0)
                {
                    if (P1Position != 1)
                    {
                        P1Position += 1;
                    }
                    InputTimer = 0.15f;
                }
                if (Input.GetButtonDown("Circle_P1"))
                {
                    sideSelectScreen.SetActive(false);
                    P1Position = 0;
                    P2Position = 0;
                }

                //Accept start input if player has selected a side
                if (P1Position != 0)
                {
                    if (Input.GetButtonDown("Cross_P1"))
                    {
                        sideSelectScreen.SetActive(false);
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode = "AI";
                        PlayVsAiButton.onClick.Invoke();
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
                    if (mode == "AI")
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
                    if (mode == "AI")
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
}
