using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CursorMovement : MonoBehaviour {

    public int P1Color;
    public int P2Color;
    public float speed;
    private int playerPaused;
    private int buttonIndex;
    public bool isPaused;
    public bool P1Ready;
    public bool P2Ready;
    private bool acceptP1Input;
    private bool acceptP2Input;
    private bool preventDeselect = true;
    private bool BackMenuUI = false;
    private int P1ColorIndex = 1;
    private int P2ColorIndex = 1;

    private string p1Cross = "Cross_P1";
    private string p1Circle = "Circle_P1";
    private string p1Hor = "Horizontal_P1";
    private string p1Ver = "Vertical_P1";
    private string p2Cross = "Cross_P2";
    private string p2Circle = "Circle_P2";
    private string p2Hor = "Horizontal_P2";
    private string p2Ver = "Vertical_P2";

    private int p1Num = 0;
    private int p2Num = 1;

    public GameObject backMenuUI;
    public GameObject P1Cursor;
    public GameObject P2Cursor;
    public GameObject fightButton;
    public GameObject loadingScreen;
    public GameObject P1ColorSelect;
    public GameObject P2ColorSelect;
    public GameObject P1ReadyText;
    public GameObject P2ReadyText;
    public GameObject stageSelect;
    public GameObject CharacterModels;

    public GameObject[] icons;
    public GameObject[] P1Models;
    public GameObject[] P2Models;

    public CursorDetection P1;
    public CursorDetection P2;

    public Button yesButton;
    public Button noButton;

    public AudioSource P1Announcer;
    public AudioSource P2Announcer;

    public AudioClip DhaliaAnnouncer;
    public AudioClip AchealisAnnouncer;

    void Start()
    {
        //Reset Timescale
        Time.timeScale = 1;

        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "PvP")
        {
            //Check P1 Side
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
            {
                p1Cross = "Cross_P2";
                p1Circle = "Circle_P2";
                p1Hor = "Horizontal_P2";
                p1Ver = "Vertical_P2";

                p1Num = 1;
            }

            //Check P2Side
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Left")
            {
                p2Cross = "Cross_P1";
                p2Circle = "Circle_P1";
                p2Hor = "Horizontal_P1";
                p2Ver = "Vertical_P1";

                p2Num = 0;
            }
        }

        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "AI"  || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            //P1 will control both cursors (one at a time)
            p2Cross = "Cross_P1";
            p2Circle = "Circle_P1";
            p2Hor = "Horizontal_P1";
            p2Ver = "Vertical_P1";

            p2Num = 0;

            //First Deactivate P1/P2 cursor depending on the chosen side
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
            {
                P2Cursor.SetActive(false);
            }
            else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
            {
                P1Cursor.SetActive(false);
            }
        }
        
        p1Cross += UpdateControls(CheckXbox(p1Num));
        p2Cross += UpdateControls(CheckXbox(p2Num));
        p1Circle += UpdateControls(CheckXbox(p1Num));
        p2Circle += UpdateControls(CheckXbox(p2Num));
        p1Ver += UpdateControls(CheckXbox(p1Num));
        p1Hor += UpdateControls(CheckXbox(p1Num));
        p2Ver += UpdateControls(CheckXbox(p2Num));
        p2Hor += UpdateControls(CheckXbox(p2Num));


    }

    void Update()
    {
        //Prevent cross input from selecting a character and exiting back menu simultaneously
        if (backMenuUI.activeSelf)
        {
            BackMenuUI = true;
        }
        else
        {
            BackMenuUI = false;
        }
        //Handle Back Menu PvP
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "PvP")
        {
            if (!isPaused)
            {
                if (Input.GetButtonDown(p1Circle) && !P1.P1Selected)
                {
                    backMenuUI.SetActive(true);
                    isPaused = true;
                    playerPaused = 1;
                    buttonIndex = 2;
                    yesButton.Select();
                    noButton.Select();
                }

                if (Input.GetButtonDown(p2Circle) && !P2.P2Selected)
                {
                    backMenuUI.SetActive(true);
                    isPaused = true;
                    playerPaused = 2;
                    buttonIndex = 2;
                    yesButton.Select();
                    noButton.Select();
                }
            }
            else if (isPaused)
            {
                if (playerPaused == 1)
                {
                    if (Input.GetAxis(p1Hor) == -1)
                    {
                        yesButton.Select();
                        buttonIndex = 1;
                    }
                    else if (Input.GetAxis(p1Hor) == 1)
                    {
                        noButton.Select();
                        buttonIndex = 2;
                    }

                    if (Input.GetButtonDown(p1Cross))
                    {
                        switch (buttonIndex)
                        {
                            case 1:
                                yesButton.onClick.Invoke();
                                break;
                            case 2:
                                noButton.onClick.Invoke();
                                break;
                        }
                    }

                    if (Input.GetButtonDown(p1Circle))
                    {
                        backMenuUI.SetActive(false);
                        isPaused = false;
                        playerPaused = 0;
                        buttonIndex = 0;
                    }
                }
                else if (playerPaused == 2)
                {
                    if (Input.GetAxis(p2Hor) == -1)
                    {
                        yesButton.Select();
                        buttonIndex = 1;
                    }
                    else if (Input.GetAxis(p2Hor) == 1)
                    {
                        noButton.Select();
                        buttonIndex = 2;
                    }

                    if (Input.GetButtonDown(p2Cross))
                    {
                        switch (buttonIndex)
                        {
                            case 1:
                                yesButton.onClick.Invoke();
                                break;
                            case 2:
                                noButton.onClick.Invoke();
                                break;
                        }
                    }

                    if (Input.GetButtonDown(p2Circle))
                    {
                        backMenuUI.SetActive(false);
                        isPaused = false;
                        playerPaused = 0;
                        buttonIndex = 0;
                    }
                }
                if (buttonIndex == 1)
                {
                    yesButton.Select();
                }
                else if(buttonIndex == 2)
                {
                    noButton.Select();
                }
            }
        }
        //Back Menu AI/Training
        else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "AI" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            if (!isPaused)
            {
                if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
                {
                    if (!P1.P1Selected && Input.GetButtonDown(p1Circle))
                    {
                        backMenuUI.SetActive(true);
                        isPaused = true;
                        buttonIndex = 2;
                        yesButton.Select();
                        noButton.Select();
                    }
                } else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
                {
                    if (!P2.P2Selected && Input.GetButtonDown(p2Circle))
                    {
                        backMenuUI.SetActive(true);
                        isPaused = true;
                        buttonIndex = 2;
                        yesButton.Select();
                        noButton.Select();
                    }
                }              
            }
            else if (isPaused)
            {
                if (Input.GetAxis(p1Hor) == -1)
                {
                    yesButton.Select();
                    buttonIndex = 1;
                }
                else if (Input.GetAxis(p1Hor) == 1)
                {
                    noButton.Select();
                    buttonIndex = 2;
                }

                if (Input.GetButtonDown(p1Cross))
                {
                    switch (buttonIndex)
                    {
                        case 1:
                            yesButton.onClick.Invoke();
                            break;
                        case 2:
                            noButton.onClick.Invoke();
                            break;
                    }
                }

                if (Input.GetButtonDown(p1Circle))
                {
                    backMenuUI.SetActive(false);
                    isPaused = false;
                    playerPaused = 0;
                    buttonIndex = 0;
                }
                if (buttonIndex == 1)
                {
                    yesButton.Select();
                }
                else if (buttonIndex == 2)
                {
                    noButton.Select();
                }
            }
        }

        //Cursor Movement and Boundaries
        if (!isPaused)
        {
            if (!P1.P1Selected && P1Cursor.activeSelf)
            {
                //Manage P1Cursor movement
                float x = Input.GetAxis(p1Hor);
                float y = Input.GetAxis(p1Ver);

                P1Cursor.transform.position += new Vector3(x, y, 0) * Time.deltaTime * speed;

                Vector3 worldSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

                P1Cursor.transform.position = new Vector3(Mathf.Clamp(P1Cursor.transform.position.x, -worldSize.x, worldSize.x),
                Mathf.Clamp(P1Cursor.transform.position.y, -worldSize.y, worldSize.y),
                P1Cursor.transform.position.z);
            }
            if (!P2.P2Selected && P2Cursor.activeSelf)
            {
                //Manage P2Cursor movement
                float x2 = Input.GetAxis(p2Hor);
                float y2 = Input.GetAxis(p2Ver);

                P2Cursor.transform.position += new Vector3(x2, y2, 0) * Time.deltaTime * speed;

                Vector3 worldSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

                P2Cursor.transform.position = new Vector3(Mathf.Clamp(P2Cursor.transform.position.x, -worldSize.x, worldSize.x),
                Mathf.Clamp(P2Cursor.transform.position.y, -worldSize.y, worldSize.y),
                P2Cursor.transform.position.z);
            }

            //P1 MENUS
            //Bring up P1 Color Select Menu
            if (P1.P1Selected && !P1Ready)
            {
                P1ColorSelect.SetActive(true);

                //Receive P1 inputs for color select
                if (Input.GetAxis(p1Hor) < 0 && acceptP1Input)
                {
                    P1ColorIndex--;
                    acceptP1Input = false;
                    //Prevent P1 from highlighting P2 color choice
                    if (P1ColorIndex == P2Color && P1.currentChar == P2.currentChar && P1ColorIndex == 1)
                    {
                        P1ColorIndex = 5;
                    }
                    else if (P1ColorIndex == P2Color && P1.currentChar == P2.currentChar)
                    {
                        P1ColorIndex--;
                    }
                }
                else if (Input.GetAxis(p1Hor) > 0 && acceptP1Input)
                {
                    P1ColorIndex++;
                    acceptP1Input = false;
                    //Prevent P1 from highlighting P2 color choice
                    if (P1ColorIndex == P2Color && P1.currentChar == P2.currentChar && P1ColorIndex == 5)
                    {
                        P1ColorIndex = 1;
                    }
                    else if (P1ColorIndex == P2Color && P1.currentChar == P2.currentChar)
                    {
                        P1ColorIndex++;
                    }
                }
                else if (Input.GetAxis(p1Hor) == 0) 
                {
                    acceptP1Input = true;
                }

                if (P1ColorIndex == P2Color && P1.currentChar == P2.currentChar)
                {
                    P1ColorIndex++;
                }

                //Cycle color index
                if (P1ColorIndex > 5)
                {
                    P1ColorIndex = 1;
                }
                else if (P1ColorIndex < 1)
                {
                    P1ColorIndex = 5;
                }

                //Update Character Model with highlighted color
                switch (P1.currentChar)
                {
                    case "Dhalia":
                        P1ColorSelect.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "< " + P1ColorIndex + " >";
                        P1Models[0].transform.GetChild(0).transform.GetComponent<ColorSwapDHA>().colorNum = P1ColorIndex;
                        break;
                    case "Achealis":
                        P1ColorSelect.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "< " + P1ColorIndex + " >";
                        P1Models[1].transform.GetChild(0).transform.GetComponent<ColorSwapACH>().colorNum = P1ColorIndex;
                        break;
                }

                //Check for P1 confirmation
                if (Input.GetButtonDown(p1Cross))
                {
                    P1Color = P1ColorIndex;

                    //Check to ensure no colors are the same
                    if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character == GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character)
                    {
                        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
                        {
                            if ((P1Color == 0 && P2Color == 0) || P1Color != GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Color)
                            {
                                GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Color = P1Color;
                                P1Ready = true;
                                P1ColorSelect.SetActive(false);
                            }
                        }
                        else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Left")
                        {
                            if ((P1Color == 0 && P2Color == 0) || P1Color != GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Color)
                            {
                                GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Color = P1Color;
                                P1Ready = true;
                                P1ColorSelect.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
                        {
                            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Color = P1Color;
                        }
                        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Left")
                        {
                            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Color = P1Color;
                        }
                        P1Ready = true;
                        P1ColorSelect.SetActive(false);
                    }
                }
            }
            else
            {
                P1ColorSelect.SetActive(false);
            }

            if ((GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "AI" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice") && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
            {
                if (!P1.P1Selected && Input.GetButtonDown(p1Circle) && P2Ready)
                {
                    preventDeselect = false;
                }
            }

            //Deselect from the Character
            if (P1.P1Selected && Input.GetButtonDown(p1Circle) && !P1Ready)
            {
                //Disable Model
                switch (P1.currentChar)
                {
                    case "Dhalia":
                        P1Models[0].SetActive(false);
                        break;
                    case "Achealis":
                        P1Models[1].SetActive(false);
                        break;
                }
                P1.P1Selected = false;
                P1ColorIndex = 1;
                if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
                {
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character = "";
                }
                else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Left")
                {
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character = "";
                }

            }

            //Deselect P1 from Ready state
            if (Input.GetButtonDown(p1Circle) && P1Ready)
            {
                P1Color = 0;
                if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "PvP")
                {
                    if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
                    {
                        P1Color = 0;
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Color = 0;
                        P1Ready = false;
                        P1ColorSelect.SetActive(true);
                    }
                    else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Left")
                    {
                        P1Color = 0;
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Color = 0;
                        P1Ready = false;
                        P1ColorSelect.SetActive(true);
                    }

                }
                else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "AI" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
                {
                    if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left" & !P2.P2Selected)
                    {
                        P1Color = 0;
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Color = 0;
                        P1Ready = false;
                        P1ColorSelect.SetActive(true);
                    }
                    else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
                    {
                        P1Color = 0;
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Color = 0;
                        P1Ready = false;
                        P1ColorSelect.SetActive(true);
                    }
                }

            }

            //Set Ready Text
            if (P1Ready)
            {
                P1ReadyText.SetActive(true);
                if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left" && (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "AI" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice"))
                {
                    P2Cursor.SetActive(true);
                }
            }
            else
            {
                P1ReadyText.SetActive(false);
                if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left" && (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "AI" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice"))
                {
                    P2Cursor.SetActive(false);
                }
            }

            //Check for character selection
            if (P1.isOverlap)
            {
                if (Input.GetButtonDown(p1Cross) && !BackMenuUI)
                {
                    //Play announcer audio
                    if (!P1.P1Selected)
                    {
                        switch (P1.currentChar)
                        {
                            case "Dhalia":
                                P1Announcer.PlayOneShot(DhaliaAnnouncer, .8f);
                                P1Models[0].SetActive(true);
                                break;
                            case "Achealis":
                                P1Announcer.PlayOneShot(AchealisAnnouncer, .8f);
                                P1Models[1].SetActive(true);
                                break;
                        }
                    }
                    P1.P1Selected = true;
                    if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
                    {
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character = P1.currentChar;
                    }
                    if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Left")
                    {
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character = P1.currentChar;
                    }
                }
            }

            //P2 MENUS
            //Bring up P2 Color Select Menu
            if (P2.P2Selected && !P2Ready)
            {
                P2ColorSelect.SetActive(true);

                //Receive P1 inputs for color select
                if (Input.GetAxis(p2Hor) < 0 && acceptP2Input)
                {
                    P2ColorIndex--;
                    acceptP2Input = false;
                    //Prevent P1 from highlighting P2 color choice
                    if (P2ColorIndex == P1Color && P1.currentChar == P2.currentChar && P2ColorIndex == 1)
                    {
                        P2ColorIndex = 5;
                    }
                    else if (P2ColorIndex == P1Color && P1.currentChar == P2.currentChar)
                    {
                        P2ColorIndex--;
                    }
                }
                else if (Input.GetAxis(p2Hor) > 0 && acceptP2Input)
                {
                    P2ColorIndex++;
                    acceptP2Input = false;
                    //Prevent P1 from highlighting P2 color choice
                    if (P2ColorIndex == P1Color && P1.currentChar == P2.currentChar && P2ColorIndex == 5)
                    {
                        P2ColorIndex = 1;
                    }
                    else if (P2ColorIndex == P1Color && P1.currentChar == P2.currentChar)
                    {
                        P2ColorIndex++;
                    }
                }
                else if (Input.GetAxis(p2Hor) == 0)
                {
                    acceptP2Input = true;
                }

                if (P2ColorIndex == P1Color && P1.currentChar == P2.currentChar)
                {
                    P2ColorIndex++;
                }

                //Cycle color index
                if (P2ColorIndex > 5)
                {
                    P2ColorIndex = 1;
                }
                else if (P2ColorIndex < 1)
                {
                    P2ColorIndex = 5;
                }

                //Update Character Model with highlighted color
                switch (P2.currentChar)
                {
                    case "Dhalia":
                        P2ColorSelect.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "< " + P2ColorIndex + " >";
                        P2Models[0].transform.GetChild(0).transform.GetComponent<ColorSwapDHA>().colorNum = P2ColorIndex;
                        break;
                    case "Achealis":
                        P2ColorSelect.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "< " + P2ColorIndex + " >";
                        P2Models[1].transform.GetChild(0).transform.GetComponent<ColorSwapACH>().colorNum = P2ColorIndex;
                        break;
                }

                //Check for P2 confirmation
                if (Input.GetButtonDown(p2Cross))
                {
                    P2Color = P2ColorIndex;

                    //Check to ensure colors are not the same
                    if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character == GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character)
                    {
                        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
                        {
                            if ((P1Color == 0 && P2Color == 0) || P2Color != GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Color)
                            {
                                GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Color = P2Color;
                                P2Ready = true;
                                P2ColorSelect.SetActive(false);
                                preventDeselect = true;
                            }
                        }
                        else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Right")
                        {
                            if ((P1Color == 0 && P2Color == 0) || P2Color != GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Color)
                            {
                                GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Color = P2Color;
                                P2Ready = true;
                                P2ColorSelect.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Right")
                        {
                            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Color = P2Color;
                        }
                        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
                        {
                            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Color = P2Color;
                            preventDeselect = true;
                        }
                        P2Ready = true;
                        P2ColorSelect.SetActive(false);
                    }
                }
            }
            else
            {
                P2ColorSelect.SetActive(false);
            }

            //Deselect from the Character
            if (P2.P2Selected && Input.GetButtonDown(p2Circle) && !P2Ready)
            {
                //Disable Model
                switch (P2.currentChar)
                {
                    case "Dhalia":
                        P2Models[0].SetActive(false);
                        break;
                    case "Achealis":
                        P2Models[1].SetActive(false);
                        break;
                }
                P2.P2Selected = false;
                P2ColorIndex = 1;
                if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
                {
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character = "";
                }
                else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Right")
                {
                    GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character = "";
                }
            }

            //Deselect P2 from Ready state
            if (Input.GetButtonDown(p2Circle) && P2Ready)
            {
                P2Color = 0;
                if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "PvP")
                {
                    if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
                    {
                        P2Color = 0;
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Color = 0;
                        P2Ready = false;
                        P2ColorSelect.SetActive(true);
                    }
                    else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Right")
                    {
                        P2Color = 0;
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Color = 0;
                        P2Ready = false;
                        P2ColorSelect.SetActive(true);
                    }
                }
                else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "AI" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
                {
                    if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
                    {
                        P2Color = 0;
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Color = 0;
                        P2Ready = false;
                        P2ColorSelect.SetActive(true);
                    }
                    else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right" && preventDeselect == false)
                    {
                        P2Color = 0;
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Color = 0;
                        P2Ready = false;
                        P2ColorSelect.SetActive(true);
                    }
                }
            }

            //Set Ready Text
            if (P2Ready)
            {
                P2ReadyText.SetActive(true);
                if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right" && (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "AI" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice"))
                {
                    P1Cursor.SetActive(true);
                }
            }
            else
            {
                P2ReadyText.SetActive(false);
                if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right" && (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "AI" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice"))
                {
                    P1Cursor.SetActive(false);
                }
            }

            //Check for character selection
            if (P2.isOverlap)
            {
                if (Input.GetButtonDown(p2Cross) && !BackMenuUI)
                {
                    //Play announcer audio
                    if (!P2.P2Selected)
                    {
                        switch (P2.currentChar)
                        {
                            case "Dhalia":
                                P2Announcer.PlayOneShot(DhaliaAnnouncer, .8f);
                                P2Models[0].SetActive(true);
                                break;
                            case "Achealis":
                                P2Announcer.PlayOneShot(AchealisAnnouncer, .8f);
                                P2Models[1].SetActive(true);
                                break;
                        }
                    }

                    P2.P2Selected = true;
                    if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Right")
                    {
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character = P2.currentChar;
                    }
                    if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
                    {
                        GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character = P2.currentChar;
                    }


                }
            }

            //Bring up Stage Select once both players are ready
            if (P1Ready && P2Ready)
            {
                CharacterModels.SetActive(false);
                stageSelect.SetActive(true);               
                //Disable Icons so hitboxes don't detect in the background
                for (int i = 0; i < icons.Length; i++)
                {
                    icons[i].SetActive(false);
                }
            }
            else
            {
                CharacterModels.SetActive(true);
                //Re-Enable Icons when brought back to character select
                for (int i = 0; i < icons.Length; i++)
                {
                    icons[i].SetActive(true);
                }
            }
        }

        
    }

    private void resetP1Cursor()
    {
        P1Cursor.transform.GetComponent<RectTransform>().localPosition = new Vector3(-101, -97, -307);
    }

    private void resetP2Cursor()
    {
        P2Cursor.transform.GetComponent<RectTransform>().localPosition = new Vector3(87, -97, -307);
    }

    public void ActivateMenu() // <-Figure out later
     {
         backMenuUI.SetActive(true);
     }

     public void DeactivateMenu()
     {
        backMenuUI.SetActive(false);
        isPaused = false;
        playerPaused = 0;
        buttonIndex = 0;
    }

     public void QuitToMenu()
     {
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
