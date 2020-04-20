using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static Animator ScreenGraphics;
    //GAMEOVER
    //Variables for character properties for both player 1 and 2
    private CharacterProperties P1Prop;
    private CharacterProperties P2Prop;
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

    //Various float timer variables
    static public float roundTimer;
    static public bool suddenDeath = false;
    private float endTimer;
    private float replayTimer;
    private float overtimeTimer;

    //Bool variable deciding if timer should be running or not
    private bool timeWarningPlayed = false;

    Vector3 p1Start;
    Vector3 p2Start;

    private bool isXbox;
    private string xboxInput;
    private string ps4Input;

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

    void Awake()
    {
        roundCount = 0;

        ScreenGraphics = GetComponent<Animator>();
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

        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice")
        {
            //Setting private character property variables to their appropriate player 1 and 2 child respectively
            P1Prop = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
            P2Prop = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();

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
            isXbox = false;

            xboxInput = "Controller (Xbox One For Windows)";
            ps4Input = "Wireless Controller";
        }
        else if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            //Setting private character property variables to their appropriate player 1 and 2 child respectively
            P1Prop = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
            P2Prop = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();
            gameActive = true;
            roundCount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ScreenGraphics.SetInteger("RoundCount", roundCount);

        if (P1Prop.HitDetect.anim.GetCurrentAnimatorStateInfo(0).IsName("IdleStand") && P2Prop.HitDetect.anim.GetCurrentAnimatorStateInfo(0).IsName("IdleStand") &&
            ScreenGraphics.GetBool("NextRound"))
        {
            ScreenGraphics.SetBool("NextRound", false);
        }

        //temporary function until system using victory pose anims is implemented (automatically set nextround to true when win pose ends or if break is pressed during win pose)
        if (ScreenGraphics.GetCurrentAnimatorStateInfo(0).IsName("Inactive") && !gameActive && (P1Prop.currentHealth == 0 || P2Prop.currentHealth == 0) && p1Win != 2 && p2Win != 2)
            NextRound();

        //STARTTEXT LOGIC
        //Debug.Log(BoBB.time);
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice")
        {
            //GAMEOVER LOGIC

            if (roundTimer <= 10f && !timeWarningPlayed)
            {
                //.Play();
                timeWarningPlayed = true;
            }

            //Decrementing replay timer
            if (replayTimer > 0) replayTimer -= Time.deltaTime;
            if (overtimeTimer > 0) overtimeTimer -= Time.deltaTime;
            //If inputs are being allowed the game has started and so should the timer (this is a global variable)
            //If round time is still greater than 0 and timer is allowed to be on, time ticks
            if (roundTimer > 0 && gameActive && !suddenDeath && !matchOver && !lockInputs) roundTimer -= Time.deltaTime / 1.5f;

            //If an input device is detected then establish what device it is in order to properly decipher inputs
            if (Input.GetJoystickNames().Length > 0)
            {
                if (Input.GetJoystickNames()[0] == xboxInput) isXbox = true;
                else if (Input.GetJoystickNames()[0] == ps4Input) isXbox = false;
                else if (Input.GetJoystickNames()[0] == "") isXbox = false;
            }

            //If the round timer runs out decide who wins
            if (roundTimer < 0)
            {
                if (!suddenDeath && gameActive && (((float)P1Prop.currentHealth / (float)P1Prop.maxHealth) == ((float)P2Prop.currentHealth / (float)P2Prop.maxHealth)))
                {
                    suddenDeath = true;
                    ScreenGraphics.SetBool("SuddenDeath", true);
                }
                RoundStop();
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
                //KO.Play();
                if (p1Win == 1 && p2Win < 1)
                {
                    ++p1Win;
                }
                else if (p2Win == 1 && p1Win < 1)
                {
                    ++p2Win;
                }
                else if (p1Win == 0 && p2Win == 0)
                {
                    ++p1Win;
                    ++p2Win;
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
                ++p2Win;
                RoundStop();
            }
            //If player 2 loses then player 1 gets a win and reset round after 6 seconds
            else if (P2Prop.currentHealth <= 0 && gameActive && p1Win != 2)
            {
                ++p1Win;
                RoundStop();
            }
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
        roundTimer = 99;
        //If someone has won the game, reset wins for both players to 0 and reset armor to max
        if (p1Win == 2 || p2Win == 2)
        {
            p1Win = 0;
            p2Win = 0;
            P1Prop.armor = 4;
            P2Prop.armor = 4;
            P1Prop.durability = 100;
            P2Prop.durability = 100;
            roundCount = 0;
        }
    }

    public void RoundStart()
    {
        gameActive = true;
        lockInputs = false;
    }

    public void RoundStop()
    {
        gameActive = false;
        lockInputs = true;
        ScreenGraphics.SetBool("RoundOver", true);
    }

    public void DetermineWinMethod()
    {
        if ((P1Prop.currentHealth > 0 && P2Prop.currentHealth == 0) || (P2Prop.currentHealth > 0 && P1Prop.currentHealth == 0))
        {
            centerText.text = "BreakDown";
            centerShadow.text = "BreakDown";
        }
        else if (P1Prop.currentHealth == 0 && P2Prop.currentHealth == 0)
        {
            centerText.text = "Double KO";
            centerShadow.text = "Double KO";
        }
        else if (roundTimer <= 0)
        {
            centerText.text = "Time Up";
            centerShadow.text = "Time Up";
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
            ScreenGraphics.SetBool("RoundOver", false);
    }

    public void SetCharacterNames()
    {
        centerText.text = "VS";

        if (P1Prop.transform.root.GetChild(0).name.Contains("Dhalia"))
            leftText.text = "Dhalia";
        else if (P1Prop.transform.root.GetChild(0).name.Contains("Achealis"))
            leftText.text = "Achealis";

        if (P2Prop.transform.root.GetChild(0).name.Contains("Dhalia"))
            rightText.text = "Dhalia";
        else if (P1Prop.transform.root.GetChild(0).name.Contains("Achealis"))
            rightText.text = "Achealis";
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
        lockInputs = false;
        gameActive = false;
        SceneManager.LoadScene(0);
        p1Win = 0;
        p2Win = 0;
    }

    public void MatchEndMenus(){
        if (p1Win == 2) {
            p1menu.SetActive(true);
            p1Replay.Select();
            if (isXbox)
            {
                if (Input.GetAxis("Horizontal_P1") < 0) p1Quit.Select();
                else if (Input.GetAxis("Horizontal_P1") > 0) p1Replay.Select();
            }
            else
            {
                if (Input.GetAxis("Vertical_P1") < 0) p1Quit.Select();
                else if (Input.GetAxis("Vertical_P1") > 0) p1Replay.Select();
            }
        }
        else if (p2Win == 2) {
            p2menu.SetActive(true);
            p2Replay.Select();
            if (isXbox)
            {
                if (Input.GetAxis("Horizontal_P2") < 0) p2Quit.Select();
                else if (Input.GetAxis("Horizontal_P2") > 0) p2Replay.Select();
            }
            else
            {
                if (Input.GetAxis("Vertical_P2") < 0) p2Quit.Select();
                else if (Input.GetAxis("Vertical_P2") > 0) p2Replay.Select();
            }
        }
    }

    public void ReplayGame() {
        p1menu.SetActive(false);
        p2menu.SetActive(false);
        ResetPositions();
        NextRound();
    }
}
