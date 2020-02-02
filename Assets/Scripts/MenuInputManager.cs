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

    	if (InputTimer > 0)
    	{
    		Debug.Log("herebish");
    		InputTimer -= Time.deltaTime;
    	}
    	else InputTimer = 0;

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
		else if (state == "local")
    	{
	        if (buttonIndex < 1) buttonIndex = 1;
			else if (buttonIndex > 3) buttonIndex = 3;

			if (buttonIndex == 1)
			{
				PlayVsPlayerButton.Select();
				if (isXbox)
				{
					if (Input.GetButtonDown("Square_P1") || Input.GetButtonDown("Submit")) PlayVsPlayerButton.onClick.Invoke();	
				}
				else
				{
					if (Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Submit")) PlayVsPlayerButton.onClick.Invoke();
				}
				
			}
			else if (buttonIndex == 2)
			{
				PlayVsAiButton.Select();
				if (isXbox)
				{
					if (Input.GetButtonDown("Square_P1") || Input.GetButtonDown("Submit")) PlayVsAiButton.onClick.Invoke();
				}
				else
				{
					if (Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Submit")) PlayVsAiButton.onClick.Invoke();
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
				if (Input.GetButtonDown("Cross_P1") || Input.GetButtonDown("Cancel"))
				{
					state = "main";
					buttonIndex = 1;
					BackButton.onClick.Invoke();
				}
			}
			else
			{
				if (Input.GetButtonDown("Circle_P1") || Input.GetButtonDown("Cancel"))
				{
					state = "main";
					buttonIndex = 1;
					BackButton.onClick.Invoke();
				}
			}
		}
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
    }
}
