﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AI : MonoBehaviour
{
    // General
    public float difficulty = 50;

    // Player data
    int pArmor;
    int pDurability;
    int pCharging;
    float pHealth;
    double p1x;
    double p1y;
    bool pIsBlocking;
    bool pIsAirborne;
    bool pIsCrouching;
    bool pIsAttacking;
    bool pIsActive;
    bool pIsRecovering;
    public bool pIsHitstun;
    public bool pIsBlockstun;
    bool pIsSupering;
    string pAttackingGuard;
    string pCharacter;
    string pGuard;

    // AI data
    public int armor;
    int durability;
    float health;
    double p2x;
    double p2y;
    public bool faceLeft;
    bool isAirborne;
    bool isCrouching;
    bool isAttacking;
    bool isRecovering;
    bool isHitstun;
    bool finishMove;
    bool finishDash;
    bool pauseAI;
    public bool keepInput;
    public string keepAction;
    string aiCharacter;

    // Command inputs
    public int doingQCF;
    public int doingQCB;
    public int doingHCF;
    public int doingHCB;

    // Combos
    public int doing5L_1;
    public int doing2H_1;

    // Distance between players
    double distanceBetweenX;
    double distanceBetweenY;

    // Timers
    float pastryTimer;
    float circleTimer;
    float squareTimer;
    float triangleTimer;
    float crossTimer;
    float breakTimer;
    float noBreakTimer;
    public float delayTimer;

    public AIInput AIInput;
    private MaxInput MaxInput;
    private CharacterProperties PlayerProp;
    private CharacterProperties AIProp;
    private GameObject playerInput;
    private GameObject playerHit;
    private GameObject aiInput;
    private GameObject aiHit;
    private SelectedCharacterManager characterManager;

    public Dictionary<string, double> states = new Dictionary<string, double>();
    public Dictionary<string, double> attackStates = new Dictionary<string, double>();

    // Registering the values' initial states
    void Start()
	{
        Debug.Log("AI is Starting");
        // Player data
        pIsBlocking = false;
        pIsAirborne = false;
        pIsCrouching = false;
        pIsAttacking = false;
        pIsActive = false;
        pIsRecovering = false;
        pIsHitstun = false;
        pIsBlockstun = false;
        pIsSupering = false;
        pAttackingGuard = "";
        pCharacter = "";
        pGuard = "";

        // AI data
        faceLeft = true;
        isAirborne = false;
        isCrouching = false;
        isAttacking = false;
        isRecovering = false;
        isHitstun = false;
        finishMove = false;
        finishDash = false;
        pauseAI = false;
        keepInput = false;
        keepAction = "";
        aiCharacter = "";

        // Command inputs
        doingQCF = 0;
        doingQCB = 0;
        doingHCF = 0;
        doingHCB = 0;

        // Combos
        doing5L_1 = 0;
        doing2H_1 = 0;

        // Distance between players
        distanceBetweenX = 0;
        distanceBetweenY = 0;

        // Timers
        pastryTimer = 0;
        circleTimer = 0;
        squareTimer = 0;
        triangleTimer = 0;
        crossTimer = 0;
        breakTimer = 0;
        noBreakTimer = 0;
        delayTimer = 0;

        MaxInput = GetComponent<MaxInput>();
        if (!MaxInput.AI)
		{
            Debug.Log("AI disabled");
            enabled = false;
        }
        PlayerProp = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
        AIProp = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();
        playerInput = GameObject.Find("Player1").transform.GetChild(0).GetChild(0).gameObject;
        playerHit = GameObject.Find("Player1").transform.GetChild(0).GetChild(2).gameObject;
        aiInput = GameObject.Find("Player2").transform.GetChild(0).GetChild(0).gameObject;
        aiHit = GameObject.Find("Player2").transform.GetChild(0).GetChild(2).gameObject;
        characterManager = GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>();

        states.Add("Attack", 0);
        states.Add("Defend", 0);
        states.Add("Approach", 0);
        states.Add("Recover", 0);

        attackStates.Add("Zone", 0);
        attackStates.Add("Overhead", 0);
        attackStates.Add("Mid", 0);
        attackStates.Add("Low", 0);
        attackStates.Add("Grab", 0);
        attackStates.Add("Setup", 0);       // Figure out how to see if the player is down on the floor
    }

    void Update()
	{
        //Debug.Log("AI Update");
        //Stops ai if player has lost
        pHealth = PlayerProp.currentHealth;
        if (pHealth <= 0 || !RoundManager.startReady)
        {
            Debug.Log("paused: " + pHealth + " " + RoundManager.startReady);
            pauseAI = true;
        }
        else
        {
            Debug.Log("unpaused");
            pauseAI = false;
        }

        //If AI has not been paused
        if (!pauseAI)
		{
            decreaseTimers();

            if (!keepInput && breakTimer <= 0)
            {
                MaxInput.ClearInput("Player2");
            }

            updateProperties();

            resetStateValues();

            calculateWeights();

            if (delayTimer <= 0 && breakTimer <= 0)
            {
                if (doingQCF > 0)
                {
                    AIInput.QCF();
                }
                else if (doingQCB > 0)
                {
                    AIInput.QCB();
                }
                else if (doingHCF > 0)
                {
                    AIInput.HCF();
                }
                else if (doingHCB > 0)
                {
                    AIInput.HCB();
                }
                else if (doing5L_1 > 0)
                {
                    AIInput.combo5L_1();
                }
                else if (doing2H_1 > 0)
                {
                    AIInput.combo2H_1();
                }
                // else if (difficulty < 100)
                // {
                //     delay();
                // }
                else
                {

                    var max = states.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;   // Gets key with highest value
                    Debug.Log(max);

                    //If ai is on ground set jumping to false
                    if (GameObject.Find("Player2").transform.GetChild(0).transform.position.y <= 0)
                    {
                        isAirborne = false;
                    }

                    // Executes AI's state
                    if (aiCharacter == "Dhalia")
                    {
                        if (max == "Attack") attack();
                        if (max == "Defend") defend();
                        if (max == "Approach") approach();
                        if (max == "Recover") recover();
                    }

                    //testActions();  // REMEMBER TO COMMENT OUT WHEN DONE TESTING
                }
            }
        }
    }

    // AI attacking player, attacking has it's own internal state system
    void attack()
    {
        calculateAttackWeights();
        var rand = new System.Random().Next(101);    // Random int from 0 to 100

        var maxAttack = attackStates.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;   // Gets key with highest value
        Debug.Log(maxAttack);

        if (aiCharacter == "Dhalia")
        {
            // Attacks that hit low
            if (maxAttack == "Low")
            {
                // 5M
                if (rand <= 25 && triangleTimer <= 0)
                {
                    MaxInput.Triangle("Player2");
                    circleTimer = .5f;
                }
                else
                {
                    MaxInput.Crouch("Player2");

                    // 2L (doesn't actually hit low)
                    if (rand > 25 && rand <= 50 && squareTimer <= 0)
                    {
                        MaxInput.Square("Player2");
                        crossTimer = .5f;
                    }
                    // 2M
                    else if (rand > 50 && rand <= 70 && triangleTimer <= 0)
                    {
                        MaxInput.Triangle("Player2");
                        circleTimer = .5f;
                    }
                    // 2B
                    else if (rand > 70 && rand <= 85 && crossTimer <= 0)
                    {
                        MaxInput.Cross("Player2");
                        squareTimer = .5f;
                        if (noBreakTimer <= 0) holdBreak(.8f);
                    }
                    // Head rush (63214M)
                    else if (rand > 85)
                    {
                        keepAction = "Triangle";
                        doingHCB = 1;
                        AIInput.HCB();
                        if (noBreakTimer <= 0) holdBreak(1.2f);
                    }
                    else MaxInput.Square("Player2");
                }
            }

            // Attacks that hit mid
            if (maxAttack == "Mid")
            {
                // 5L
                if (rand <= 35 && squareTimer <= 0)
                {
                    doing5L_1 = 1;
                    AIInput.combo5L_1();
                    //MaxInput.Square("Player2");
                    crossTimer = .5f;
                }
                // 2H
                else if (rand > 35 && rand <= 60 && circleTimer <= 0)
                {
                    doing2H_1 = 1;
                    AIInput.combo2H_1();
                    triangleTimer = .5f;
                }
                // 5H
                else if (rand > 60 && rand <= 85 && circleTimer <= 0)
                {
                    MaxInput.Circle("Player2");
                    triangleTimer = .5f;
                }
                // 5B
                else if (rand > 85 && rand <= 98 && crossTimer <= 0)
                {
                    MaxInput.Cross("Player2");
                    squareTimer = .5f;
                    if (noBreakTimer <= 0) holdBreak(.8f);
                }

                // Judgment Sabre
                else if (rand > 98 && !isAirborne)
                {
                    keepAction = "RBumper";
                    doingQCB = 1;
                    AIInput.HCB();
                }
                else MaxInput.Square("Player2");
            }

            if (maxAttack == "Overhead")
            {
                // 6B
                if (rand > 50)
                {
                    if (faceLeft == true)
                    {
                        MaxInput.MoveLeft("Player2");
                    }
                    else
                    {
                        MaxInput.MoveRight("Player2");
                    }
                    MaxInput.Cross("Player2");
                    if (noBreakTimer <= 0) holdBreak(.8f);
                }
                // Blood brave (214H)
                else
                {
                    keepAction = "Circle";
                    doingQCB = 1;
                    AIInput.QCB();
                }
            }

            // If very close to the player, attempt to grab
            if (maxAttack == "Grab")
            {
                MaxInput.LBumper("Player2");
            }

            // Attacking from a distance
            if (maxAttack == "Zone")
            {
                // Pastry Throw (236L)
                if (rand >= 1 && pastryTimer <= 0)
                {
                    keepAction = "Square";
                    doingQCF = 1;
                    AIInput.QCF();
                    pastryTimer = 3f;
                }
                // Toaster (236HB)
                else if (rand < 1 && armor >= 2)
                {
                    keepAction = "RTrigger";
                    doingQCF = 1;
                    AIInput.QCF();
                }
                // If a pastry is already out, just move forward
                else if (pastryTimer > 0)
                {
                    approach();
                }
            }
        }
    }

    void holdBreak(float hold)
    {
        var rand = new System.Random().Next(101);    // Random int from 0 to 100
        if (rand < 30 && !isAirborne)
        {
            MaxInput.Cross("Player2");
            breakTimer = hold;
            //delayTimer = 1f;
        }
        else noBreakTimer = hold;
    }

    // Calculating weights for what kind of attack the AI should use
    void calculateAttackWeights()
    {
        if (distanceBetweenX >= 2.5) attackStates["Zone"] += 3 + new System.Random().NextDouble() * 2;
        if (pGuard == "Low") attackStates["Overhead"] += 1 + new System.Random().NextDouble() * 2;
        if (pGuard == "High") attackStates["Low"] += 1 + new System.Random().NextDouble() * 2;
        attackStates["Grab"] += (1 - distanceBetweenX) - (distanceBetweenY * 2);
        attackStates["Mid"] = 0.75 + new System.Random().NextDouble() * 2;
    }

    // AI defending attacks based off of direction
    void defend()
    {
        if (aiCharacter == "Dhalia")
        {
            if (pAttackingGuard == "Low")
            {
                if (faceLeft)
                {
                    MaxInput.Crouch("Player2");
                    MaxInput.MoveRight("Player2");
                }
                else
                {
                    MaxInput.Crouch("Player2");
                    MaxInput.MoveLeft("Player2");
                }
            }
            else
            {
                if (faceLeft)
                {
                    MaxInput.MoveRight("Player2");
                }
                else
                {
                    MaxInput.MoveLeft("Player2");
                }
            }
        }
    }

    // AI movement options
    void approach()
    {
        var rand = new System.Random();

        //If ai is not crouching, back dashing, or combo-ing, move in direction of player
        if (faceLeft)
        {
            MaxInput.MoveLeft("Player2");
        }
        else
        {
            MaxInput.MoveRight("Player2");
        }

        //Foward Dash
        if (rand.NextDouble() * distanceBetweenX * 100 >= 140)
        {
            if(faceLeft)
            {
                MaxInput.MoveLeft("Player2");
                MaxInput.LStick("Player2");
            }
            else
            {
                MaxInput.MoveRight("Player2");
                MaxInput.LStick("Player2");
            }
        }

        // Jumping
        if (distanceBetweenX < 1.8 && p2y < p1y - 0.5 && rand.Next(10) == 1)
        {
            MaxInput.Jump("Player2");
        }
    }

    // AI teching out of hitstun
    void recover()
    {
        var rand = new System.Random().Next(101);    // Random int from 0 to 100

        // Randomness for directional teching, may develop further in the future
        if (rand < 33) MaxInput.MoveLeft("Player2");
        if (rand >= 66) MaxInput.MoveRight("Player2");
        MaxInput.Square("Player2");
    }

    void decreaseTimers()
    {
        if (pastryTimer > 0)
        {
            pastryTimer -= Time.deltaTime;
        }
        if (crossTimer > 0)
        {
            crossTimer -= Time.deltaTime;
        }
        if (triangleTimer > 0)
        {
            triangleTimer -= Time.deltaTime;
        }
        if (squareTimer > 0)
        {
            squareTimer -= Time.deltaTime;
        }
        if (circleTimer > 0)
        {
            circleTimer -= Time.deltaTime;
        }
        if (breakTimer > 0)
        {
            breakTimer -= Time.deltaTime;
        }
        if (noBreakTimer > 0)
        {
            noBreakTimer -= Time.deltaTime;
        }
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
    }

    // Calculates weights for what state the AI will go into
    void calculateWeights()
    {
        // If the AI is in hitstun, teching is the only thing it can do
        if (isHitstun && isAirborne)
        {
            states["Recover"] = 100;
            return;
        }

        var rand = new System.Random();

        states["Attack"] += 0.5;
        if (pIsRecovering) states["Attack"] += 1;
        states["Attack"] -= distanceBetweenX + distanceBetweenY - 1;
        if (isAttacking) states["Attack"] -= 1;
        if (distanceBetweenX > 2 && rand.Next(101) <= 5) states["Attack"] += 1.5 * distanceBetweenX;

        if (pIsAttacking) states["Defend"] += 2;
        if (pIsActive) states["Defend"] += 1;
        if (pIsSupering) states["Defend"] += 4;
        states["Defend"] += pCharging / 2;

        // The farther away, the more likely to move closer
        states["Approach"] += distanceBetweenX * .8 + distanceBetweenY * 2;
    }

    // Update variables every frame based on the state of the scene
    void updateProperties()
    {
        // Getting all the current player data
        pCharacter = characterManager.P1Character;

        pArmor = PlayerProp.armor;
        pDurability = PlayerProp.durability;
        pCharging = playerInput.GetComponent<HitboxDHA>().sinCharge;

        pIsBlocking = playerInput.GetComponent<Animator>().GetBool("Blocked");
        pIsAirborne = playerInput.GetComponent<AcceptInputs>().airborne;
        pIsCrouching = playerInput.GetComponent<Animator>().GetBool("Crouch");

        pIsAttacking = playerInput.GetComponent<AcceptInputs>().attacking;
        pIsActive = playerInput.GetComponent<AcceptInputs>().active;
        pIsRecovering = playerInput.GetComponent<AcceptInputs>().recovering;
        pIsHitstun = playerHit.GetComponent<HitDetector>().hitStun > 0;
        pIsBlockstun = playerHit.GetComponent<HitDetector>().blockStun > 0;
        pIsSupering = GameObject.Find("Player1").transform.GetChild(3).gameObject.activeSelf;
        pAttackingGuard = playerHit.GetComponent<HitDetector>().guard;

        if (playerInput.GetComponent<Animator>().GetBool("HighGuard") == true) pGuard = "High";
        else if (playerInput.GetComponent<Animator>().GetBool("LowGuard") == true) pGuard = "Low";
        else pGuard = "None";

        p1x = GameObject.Find("Player1").transform.GetChild(0).transform.position.x;
        p1y = GameObject.Find("Player1").transform.GetChild(0).transform.position.y;

        // Getting all the current AI data
        aiCharacter = characterManager.P2Character;

        armor = AIProp.armor;
        durability = AIProp.durability;
        health = AIProp.currentHealth;
        isAirborne = aiInput.GetComponent<AcceptInputs>().airborne;
        isAttacking = aiInput.GetComponent<AcceptInputs>().attacking;
        isRecovering = aiInput.GetComponent<AcceptInputs>().recovering;
        isHitstun = aiHit.GetComponent<HitDetector>().hitStun > 0;
        p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x;
        p2y = GameObject.Find("Player2").transform.GetChild(0).transform.position.y;

        // AI direction
        if (p1x - p2x < 0) faceLeft = true;
        else faceLeft = false;

        // Distance between player and AI
        difficulty = characterManager.CPUDifficulty;
        distanceBetweenX = Math.Abs(p1x - p2x);
        distanceBetweenY = Math.Abs(p1y - p2y);
    }

    // Resets the states values of the Dictionaries
    void resetStateValues()
    {
        states["Attack"] = 0;
        states["Defend"] = 0;
        states["Approach"] = 0;
        states["Recover"] = 0;

        attackStates["Zone"] = 0;
        attackStates["Overhead"] = 0;
        attackStates["Mid"] = 0;
        attackStates["Low"] = 0;
        attackStates["Grab"] = 0;
        attackStates["Setup"] = 0;
    }

    void delay()
    {
        var rand = new System.Random();

        if (rand.Next((100 - (int)difficulty) * 100) < 10)
        {
            delayTimer = (float)rand.NextDouble();
            Debug.Log("Delayed");
        }
    }

    // Testing specific actions
    void testActions()
    {
        doing2H_1 = 1;
        AIInput.combo2H_1();
        triangleTimer = .5f;
    }
}
