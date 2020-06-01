using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    CharacterProperties P1Prop;

    public GameObject popUps;
    public GameObject popUpsBackground;

    private float popUpDelay;
    private float popupDelayDuration = 0.5f;
    private float P1CurrentHitDamage;
    private float P1CurrentComboTotalDamage;
    private float startTime, endTime;

    //private bool preventReset = false;

    private int popUpIndex;

    private string inputSelect = "Select_P1";
    private string inputL2 = "L2_P1";
    private string inputCross = "Cross_P1";
    private string inputCircle = "Circle_P1";
    private string inputTriangle = "Triangle_P1";
    private string inputSquare = "Square_P1";
    private string inputHorizontal = "Horizontal_P1";
    private string inputVertical = "Vertical_P1";
    private string inputR3 = "R3_P1";

    // Start is called before the first frame update
    void Start()
    {
        P1Prop = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();

        inputSelect += UpdateControls(CheckXbox(0));
        inputL2 += UpdateControls(CheckXbox(0));
        inputCross += UpdateControls(CheckXbox(0));
        inputCircle += UpdateControls(CheckXbox(0));
        inputTriangle += UpdateControls(CheckXbox(0));
        inputSquare += UpdateControls(CheckXbox(0));
        inputHorizontal += UpdateControls(CheckXbox(0));
        inputVertical += UpdateControls(CheckXbox(0));
        inputR3 += UpdateControls(CheckXbox(0));
    }

    // Update is called once per frame
    void Update()
    {
        P1CurrentHitDamage = GameObject.Find("PracticeModeManager").GetComponent<PracticeMode>().P1CurrentHitDamage;
        P1CurrentComboTotalDamage = GameObject.Find("PracticeModeManager").GetComponent<PracticeMode>().P1CurrentComboTotalDamage;

        if (popUpDelay > 0) popUpDelay -= Time.deltaTime;
        else popUpDelay = 0;

        //Changed popups to be the same text box, just changing what text displays                
        if (popUpIndex == 0)
        {
            popUps.SetActive(true);
            popUpsBackground.SetActive(true);
            popUps.GetComponent<Text>().text = "Welcome to the tutorial! Here you will learn the basics of Breakthrough! \n (Press C or Select to continue)";
            if ((Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown(inputSelect)) && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 1)
        {
            popUps.GetComponent<Text>().text = "Let's start with movement! \n (Press C or Select to continue)";
            if ((Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown(inputSelect)) && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 2)
        {
            popUps.GetComponent<Text>().text = "Try moving closer to your opponent! \n Pressing the right or left buttons will move your character in that direction \n (Note :  You can also double tap to dash in that direction)";
            if ((Input.GetAxis(inputHorizontal) > 0 || Input.GetAxis(inputHorizontal) < 0) && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 3)
        {
            popUps.GetComponent<Text>().text = "Press the down button to crouch!";
            if (Input.GetAxis(inputVertical) < 0 && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 4)
        {
            popUps.GetComponent<Text>().text = "Press the up botton once to jump or twice to double jump! ";
            if (Input.GetAxis(inputVertical) > 0 && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 5)
        {
            popUps.GetComponent<Text>().text = "Now that you know how to move, it's time to learn how to attack! \n (Press C or Select to continue)";
            if ((Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown(inputSelect)) && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 6)
        {
            popUps.GetComponent<Text>().text = "Press U or Square to do a light attack!";
            if (Input.GetButtonDown(inputSquare) && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 7)
        {
            popUps.GetComponent<Text>().text = "Press I or Triangle to do a medium attack!";
            if (Input.GetButtonDown(inputTriangle) && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 8)
        {
            popUps.GetComponent<Text>().text = "Press O or Circle to do a heavy attack!";
            if (Input.GetButtonDown(inputCircle) && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 9)
        {
            popUps.GetComponent<Text>().text = "Press J or Cross to do a BREAK attack!";
            if (Input.GetButtonDown(inputCross) && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        } 
        else if (popUpIndex == 10)
        {
            startTime = 0f;
            endTime = 0f;
            popUps.GetComponent<Text>().text = "Now hold J or Cross to power up a BREAK attack!";
            if (Input.GetButtonDown(inputCross))
            {
                startTime = Time.time;
            }
            if (Input.GetButtonUp(inputCross))
            {
                endTime = Time.time;
            }
            if ((endTime - startTime > 0.4f) && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 11)
        {
            popUps.GetComponent<Text>().text = "You can also grab your enemy by pressing Y or !";
            if (Input.GetKeyDown(KeyCode.Y) && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 12)
        {
            popUps.GetComponent<Text>().text = "Now perform a combo attack on the dummy! \n (Note :  You can view your characters move list in the Pause Menu)";
            if (P1Prop.HitDetect.comboCount >= 5 && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 13)
        {
            popUps.GetComponent<Text>().text = "Now use one of your characters super moves by pressing 236 H + B";
            if (P1CurrentComboTotalDamage >= 250 && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 14)
        {
            popUps.GetComponent<Text>().text = "Hold the direction away from your opponent to block incoming attacks! \n (Note :  This can be used on the ground and in the air)";
            if (Input.GetAxis(inputHorizontal) < 0 && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 15)
        {
            popUps.GetComponent<Text>().text = "Consume one segment of the resolve meter by pressing H or L2. \n (Note :  This allows you to cancel your current action which can be used to set up some cool combos!) ";
            if ((Input.GetAxis(inputL2) > 0 || Input.GetButtonDown(inputL2)) && popUpDelay == 0)
            {
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else if (popUpIndex == 16)
        {
            popUps.GetComponent<Text>().text = "Now use everthing you've learned and try to defeat your oppenent! \n (Press C or Select to start)";
            if ((Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown(inputSelect)) && popUpDelay == 0)
            {
                GameObject.Find("PracticeModeManager").GetComponent<PracticeMode>().resetPositions();
                GameObject.Find("PauseManager").GetComponentInChildren<PauseMenu>().CPUState = 6;
                popUpIndex++;
                popUpDelay = popupDelayDuration;
            }
        }
        else
        {
            popUps.GetComponent<Text>().text = "";
            popUpsBackground.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown(inputR3))
        {
            popUpIndex++;
            popUps.GetComponent<Text>().text = "";
            popUps.SetActive(false);
            popUpsBackground.SetActive(false);
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
