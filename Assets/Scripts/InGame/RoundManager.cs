using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static Animator ScreenGraphics;
    AnnouncerVoice announcer;
    //GAMEOVER
    //Variables for character properties for both player 1 and 2
    public CharacterProperties P1Prop;
    public CharacterProperties P2Prop;
    AcceptInputs P1Inputs;
    AcceptInputs P2Inputs;
    //Menu object variables
    public GameObject p1menu;
    public GameObject p2menu;
    private GameObject child1;
    private GameObject child2;
    public Button p1Replay;
    public Button p1Quit;
    public Button p2Replay;
    public Button p2Quit;
    //Global variables keeping track of each players win count
    static public int p1Win;
    static public int p2Win;

    static public bool dizzyKO;
    static public bool matchOver;
    static public bool lockInputs;
    static public bool startReady;

    //Various float timer variables
    static public float roundTimer;
    static public bool suddenDeath = false;
    private float endTimer;
    private float replayTimer;
    private float overtimeTimer;

    //Bool variable deciding if timer should be running or not
    private bool timeWarningPlayed = false;

    private bool reset = false;

    Vector3 p1Start;
    Vector3 p2Start;


    private string inputHorizontal = "Horizontal_P1";
    private string inputVertical = "Vertical_P1";
    private string p1cross = "Cross_P1";
    
    private string inputHorizontal2 = "Horizontal_P2";
    private string inputVertical2 = "Vertical_P2";
    private string p2cross = "Cross_P2";

    private float vertical;
    private float vertical2;
    private float horizontal;
    private float horizontal2;

    private int postGameMenuIndex = 2;
    private int postGameMenuOpen = 0;


    //STARTTEXT
    //Global bool controlling whether or not user input is allowed
    static public bool gameActive;
    //Global int that keeps track of the round count
    static public int roundCount;
    //Text objects
    public Text leftText;
    public Text centerText;
    public Text rightText;
    public Text centerShadow;

    //Timer variable
    float timer;

    //bool to set and hold starting positions
    private bool holdpositions;

    //bool for Replaying a game
    private bool resetValues = false;

    void Awake()
    {
        roundCount = 0;
        p1Win = 0;
        p2Win = 0;

        ScreenGraphics = GetComponent<Animator>();
        announcer = GetComponent<AnnouncerVoice>();
    }

    void Start()
    {
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
            p1Start = new Vector3(-1f, 1.10f, -3);
        else
            p1Start = new Vector3(1f, 1.10f, -3);

        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Right")
            p2Start = new Vector3(1f, 1.10f, -3);
        else
            p2Start = new Vector3(-1f, 1.10f, -3);

        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice" && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Tutorial")
        {
            //Setting private character property variables to their appropriate player 1 and 2 child respectively
            P1Prop = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
            P2Prop = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();

            //Setting AcceptInputs to lock them on transitions
            P1Inputs = GameObject.Find("Player1").transform.GetComponentInChildren<AcceptInputs>();
            P2Inputs = GameObject.Find("Player2").transform.GetComponentInChildren<AcceptInputs>();

            //Setting private menu child game obejcts to their appropriate menu children respectively
            child1 = p1menu.transform.GetChild(0).gameObject;
            child2 = p2menu.transform.GetChild(0).gameObject;
            //Setting timers to an arbitrarily picked -2 for standby
            endTimer = -2;
            replayTimer = -2;
            overtimeTimer = -2;
            //Setting round timer to 99 (eventually will make it into a public variable for easier manipulation/access)
            roundTimer = 99;
            roundCount = 1;
            //Setting text variables to proper win text for each player
            //Telling round not to start yet
            gameActive = false;
            lockInputs = true;

            dizzyKO = false;
            matchOver = false;
        }
        else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice" || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Tutorial")
        {
            //Setting private character property variables to their appropriate player 1 and 2 child respectively
            P1Prop = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
            P2Prop = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();
            gameActive = true;
            roundCount = 0;
            startReady = true;
        }
        holdpositions = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetControllers();
        horizontal = Input.GetAxis(inputHorizontal);
        vertical = Input.GetAxis(inputVertical);
        horizontal2 = Input.GetAxis(inputHorizontal2);
        vertical2 = Input.GetAxis(inputVertical2);
        ScreenGraphics.SetInteger("RoundCount", roundCount);

        //temporary function until system using victory pose anims is implemented (automatically set nextround to true when win pose ends or if break is pressed during win pose)
        if (ScreenGraphics.GetCurrentAnimatorStateInfo(0).IsName("BreakDown") && !gameActive && (P1Prop.currentHealth == 0 || P2Prop.currentHealth == 0) &&
            P1Prop.HitDetect.hitStop <= 0 && P2Prop.HitDetect.hitStop <= 0 && p1Win < 2 && p2Win < 2)
            NextRound();

        if (P1Prop.HitDetect.anim.GetCurrentAnimatorStateInfo(0).IsName("IdleStand") && P2Prop.HitDetect.anim.GetCurrentAnimatorStateInfo(0).IsName("IdleStand") &&
            ScreenGraphics.GetBool("NextRound"))
        {
            ScreenGraphics.SetBool("NextRound", false);
        }

        //STARTTEXT LOGIC
        //Debug.Log(BoBB.time);
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice" && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Tutorial")
        {
            //GAMEOVER LOGIC

            if (roundTimer <= 10f && !timeWarningPlayed)
            {
                announcer.PlayHurry();
                timeWarningPlayed = true;
            }

            //Decrementing replay timer
            if (replayTimer > 0) replayTimer -= Time.deltaTime;
            if (overtimeTimer > 0) overtimeTimer -= Time.deltaTime;
            //If inputs are being allowed the game has started and so should the timer (this is a global variable)
            //If round time is still greater than 0 and timer is allowed to be on, time ticks
            if (roundTimer > 0 && gameActive && !suddenDeath && !matchOver && !lockInputs) roundTimer -= Time.deltaTime / 1.5f;


            //If the round timer runs out decide who wins
            if (roundTimer < 0)
            {
                if (!suddenDeath && gameActive && (((float)P1Prop.currentHealth / (float)P1Prop.maxHealth) == ((float)P2Prop.currentHealth / (float)P2Prop.maxHealth)))
                {
                    suddenDeath = true;
                    RoundStop();
                }
                //Setting roundTimer to round 0
                roundTimer = 0;
            }
            if (roundTimer == 0)
            {
                //Set dizzy KO to true
                if (!suddenDeath)
                    dizzyKO = true;

                //If player 1 has more health, player 2 loses
                if (((float)P1Prop.currentHealth / (float)P1Prop.maxHealth) > ((float)P2Prop.currentHealth / (float)P2Prop.maxHealth))
                    P2Prop.currentHealth = 0;
                //If player 2 has more health, player 1 loses
                else if (((float)P1Prop.currentHealth / (float)P1Prop.maxHealth) < ((float)P2Prop.currentHealth / (float)P2Prop.maxHealth))
                    P1Prop.currentHealth = 0;
            }

            if (P1Prop.currentHealth <= 0 && P2Prop.currentHealth <= 0 && gameActive && p1Win != 2 && p2Win != 2)
            {
                if (p1Win == 1 && p2Win < 1)
                {
                    p1Win++;
                }
                else if (p2Win == 1 && p1Win < 1)
                {
                    p2Win++;
                }
                else if (p1Win == 0 && p2Win == 0)
                {
                    p1Win++;
                    p2Win++;
                }
                else if (p1Win == 1 && p2Win == 1)
                {
                    //Play an extra round if the final round results in a double ko
                }
                RoundStop();
            }
            //If player 1 loses then player 2 gets a win and reset round after 6 seconds
            else if (P1Prop.currentHealth <= 0 && gameActive && p2Win != 2)
            {
                p2Win++;
                RoundStop();
            }
            //If player 2 loses then player 1 gets a win and reset round after 6 seconds
            else if (P2Prop.currentHealth <= 0 && gameActive && p1Win != 2)
            {
                p1Win++;
                RoundStop();
            }
        }
        if (holdpositions)
        {
            //Setting players to starting location vectors
            GameObject.Find("Player1").transform.GetChild(0).transform.position = p1Start;
            GameObject.Find("Player2").transform.GetChild(0).transform.position = p2Start;
        }

        if(postGameMenuOpen > 0)
        {
            PostGameMenuControl();
        }
    }

    //Function that restarts a round
    public void ResetPositions()
    {
        //Setting player health to max
        P1Prop.currentHealth = P1Prop.maxHealth;
        P2Prop.currentHealth = P2Prop.maxHealth;

        //Setting players to starting location vectors
        GameObject.Find("Player1").transform.GetChild(0).transform.position = p1Start;
        GameObject.Find("Player2").transform.GetChild(0).transform.position = p2Start;

        GameObject.Find("CameraPos").transform.GetChild(1).transform.position = GameObject.Find("CameraPos").transform.position;

        ScreenGraphics.SetBool("RoundOver", false);
        roundCount++;
        //Disabling player inputs
        suddenDeath = false;
        timeWarningPlayed = false;
        roundTimer = 99;
        if (resetValues)
        {
            P1Prop.armor = 4;
            P2Prop.armor = 4;
            P1Prop.durability = 100;
            P2Prop.durability = 100;
            resetValues = false;
        }
    }

    public void RoundStart()
    {
        gameActive = true;
        lockInputs = false;
        startReady = true;
    }

    public void RoundStop()
    {
        gameActive = false;
        lockInputs = true;
        startReady = false;
        ScreenGraphics.SetBool("RoundOver", true);
    }

    public void DetermineWinMethod()
    {
        if (roundTimer <= 0 && ((P1Prop.currentHealth != 0 && P2Prop.currentHealth != 0) ||
            ((P1Prop.currentHealth != 0 && P2Prop.currentHealth == 0) || (P1Prop.currentHealth == 0 && P2Prop.currentHealth != 0))))
        {
            centerText.text = "Time Up";
            centerShadow.text = "Time Up";
            if (suddenDeath)
                ScreenGraphics.SetBool("SuddenDeath", true);
        }
        else if ((P1Prop.currentHealth == P1Prop.maxHealth && P2Prop.currentHealth == 0) || (P2Prop.currentHealth == P2Prop.maxHealth && P1Prop.currentHealth == 0))
        {
            centerText.text = "PERFECT";
            centerShadow.text = "PERFECT";
        }
        else if ((P1Prop.currentHealth > 0 && P2Prop.currentHealth == 0) || (P2Prop.currentHealth > 0 && P1Prop.currentHealth == 0))
        {
            centerText.text = "BreakDown";
            centerShadow.text = "BreakDown";
        }
        else if (P1Prop.currentHealth == 0 && P2Prop.currentHealth == 0)
        {
            centerText.text = "Double KO";
            centerShadow.text = "Double KO";
        }


        if ((int)((float)(P1Prop.currentHealth / P1Prop.maxHealth) * 100) == (int)((float)(P2Prop.currentHealth / P2Prop.maxHealth) * 100)
            || (P1Prop.currentHealth == 0 && P2Prop.currentHealth == 0))
            centerShadow.color = new Color32(198, 158, 0, 200);
        else if ((float)(P1Prop.currentHealth / P1Prop.maxHealth) > (float)(P2Prop.currentHealth / P2Prop.maxHealth))
            centerShadow.color = new Color32(210, 0, 0, 200);
        else if ((float)(P1Prop.currentHealth / P1Prop.maxHealth) < (float)(P2Prop.currentHealth / P2Prop.maxHealth))
            centerShadow.color = new Color32(0, 50, 171, 200);
    }

    public void SuddenDeath()
    {
        centerShadow.text = "Sudden Death";
        centerText.text = "Sudden Death";
        centerShadow.color = new Color32(180, 0, 210, 200);
        ScreenGraphics.SetBool("SuddenDeath", false);
        ScreenGraphics.SetBool("RoundOver", false);
    }

    public void MatchEndCheck()
    {
        if (p1Win == 2 || p2Win == 2)
        {
            postGameMenuIndex = 2;
            postGameMenuOpen = 1;
            ScreenGraphics.SetBool("RoundOver", false);
        }
    }

    public void SetCharacterNames()
    {
        centerText.text = "VS";

        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
        {
            if (P1Prop.transform.root.GetChild(0).name.Contains("Dhalia"))
                leftText.text = "Dhalia";
            else if (P1Prop.transform.root.GetChild(0).name.Contains("Achealis"))
                leftText.text = "Achealis";
        }
        else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Left")
        {
            if (P2Prop.transform.root.GetChild(0).name.Contains("Dhalia"))
                leftText.text = "Dhalia";
            else if (P2Prop.transform.root.GetChild(0).name.Contains("Achealis"))
                leftText.text = "Achealis";
        }

        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
        {
            if (P1Prop.transform.root.GetChild(0).name.Contains("Dhalia"))
                rightText.text = "Dhalia";
            else if (P1Prop.transform.root.GetChild(0).name.Contains("Achealis"))
                rightText.text = "Achealis";
        }
        else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Right")
        {
            if (P2Prop.transform.root.GetChild(0).name.Contains("Dhalia"))
                rightText.text = "Dhalia";
            else if (P2Prop.transform.root.GetChild(0).name.Contains("Achealis"))
                rightText.text = "Achealis";
        }
    }

    public void BreakOrBeBroken()
    {
        rightText.text = "Break or";
        leftText.text = "Be Broken";
    }

    public void Duel()
    {
        centerText.text = "Duel";
        if (roundCount <= 1)
        {
            leftText.text = "1";
        }
        else if (roundCount == 2)
        {
            leftText.text = "2";
        }
        else if (roundCount >= 3)
        {
            leftText.text = "Duel";
            if (roundCount == 3)
                centerText.text = "Final";
            else
                centerText.text = "Extra";
        }
    }

    public void NextRound()
    {
        ScreenGraphics.SetBool("NextRound", true);
    }

    //Function to load main menu scene
    public void QuitToMenu()
    {
        postGameMenuOpen = 0;
        p1menu.SetActive(false);
        p2menu.SetActive(false);
        lockInputs = false;
        gameActive = false;
        GameObject.Find("TransitionCanvas").transform.GetComponentInChildren<SceneTransitions>().LoadScene(0);
        p1Win = 0;
        p2Win = 0;
    }

    public void PostGameMenuControl()
    {
        if(postGameMenuOpen == 1 && p1menu.activeSelf)
        {
            if (vertical < 0)
            {
                p1Quit.Select();
                postGameMenuIndex = 1;
                print("hover quit");
            }
            else if (vertical > 0)
            {
                p1Replay.Select();
                postGameMenuIndex = 2;
                print("hover replay");
            }
            if (Input.GetButtonDown(p1cross) && postGameMenuIndex == 1)
            {
                QuitToMenu();
            }
            else if (Input.GetButtonDown(p1cross) && postGameMenuIndex == 2)
            {
                ReplayGame();
            }
        }
        else if(postGameMenuOpen == 2 && p2menu.activeSelf)
        {
            if (vertical2 < 0)
            {
                p2Quit.Select();
                postGameMenuIndex = 1;
            }
            else if (vertical2 > 0)
            {
                p2Replay.Select();
                postGameMenuIndex = 2;
            }
            if (Input.GetButtonDown(p2cross) && postGameMenuIndex == 1)
            {
                QuitToMenu();
            }
            else if (Input.GetButtonDown(p2cross) && postGameMenuIndex == 2)
            {
                ReplayGame();
            }
        }
    }

    public void MatchEndMenus(){
        if (p1Win == 2) {
            p1menu.SetActive(true);
            p1Replay.Select();
            print("player 1 won");
            postGameMenuOpen = 1;
        }
        else if (p2Win == 2) {
            p2menu.SetActive(true);
            p2Replay.Select();
            print("player 2 won");
            postGameMenuOpen = 2;
        }
    }

    public void EnablePause()
    {
        PauseMenu.allowPause = true;
    }

    public void DisablePause()
    {
        PauseMenu.allowPause = false;
    }

    public void ReplayGame() {
        p1menu.SetActive(false);
        p2menu.SetActive(false);
        p1Win = 0;
        p2Win = 0;
        resetValues = true;
        roundCount = 0;
        NextRound();
        postGameMenuOpen = 0;
    }

    public void HoldPositions()
    {
        holdpositions = true;
    }

    public void FreePositions()
    {
        holdpositions = false;
    }

    private void SetControllers()
    {
        p1cross = "Cross_P1" + UpdateControls(CheckXbox(0));
        
        inputHorizontal = "Horizontal_P1" + UpdateControls(CheckXbox(0));
        inputVertical = "Vertical_P1" + UpdateControls(CheckXbox(0));
        

        p2cross = "Cross_P2" + UpdateControls(CheckXbox(1));
        
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
}
