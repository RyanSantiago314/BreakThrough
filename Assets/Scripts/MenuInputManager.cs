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

    private string inputCross = "Cross_P1";
    private string inputCircle = "Circle_P1";
    private string inputHorizontal = "Horizontal_P1";
    private string inputVertical = "Vertical_P1";

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
        //isXbox = false;
        Time.timeScale = 1;

        inputCross += UpdateControls(CheckXbox(0));
        inputCircle += UpdateControls(CheckXbox(0));
        inputHorizontal += UpdateControls(CheckXbox(0));
        inputVertical += UpdateControls(CheckXbox(0));
    }

    // Update is called once per frame
    void Update()
    {
    	horizontal = Input.GetAxis(inputHorizontal);
    	vertical = Input.GetAxis(inputVertical);
    	if (InputTimer > 0) InputTimer -= Time.deltaTime;
    	else InputTimer = 0;
        if(InputTimer == 0)
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
		else if (state == "local")
    	{
	        if (buttonIndex < 1) buttonIndex = 1;
			else if (buttonIndex > 3) buttonIndex = 3;
			if (buttonIndex == 1)
			{
				PlayVsPlayerButton.Select();
				if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit")) PlayVsPlayerButton.onClick.Invoke();	
			}
			else if (buttonIndex == 2)
			{
				PlayVsAiButton.Select();
				if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit")) PlayVsAiButton.onClick.Invoke();
			}
			else if (buttonIndex == 3)
			{
				BackButton.Select();
				if (Input.GetButtonDown(inputCross) || Input.GetButtonDown("Submit"))
				{
					state = "main";
					buttonIndex = 1;
					BackButton.onClick.Invoke();
				}
			}
			if (Input.GetButtonDown(inputCircle) || Input.GetButtonDown("Cancel"))
			{
				state = "main";
				buttonIndex = 1;
				BackButton.onClick.Invoke();
			}
		}
		else if (state == "options")
    	{
	        if (buttonIndex < 1) buttonIndex = 1;
			else if (buttonIndex > 1) buttonIndex = 1;
			if (buttonIndex == 1)
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
