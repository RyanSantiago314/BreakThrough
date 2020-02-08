﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AI : MonoBehaviour
{
    // Character specific AI
    bool isDhalia;

    // Player data
    int pArmor;
    int pDurability;
    float pHealth;
    double p1x;
    double p1y;
    bool pIsBlocking;
    bool pIsAirborne;
    bool pIsCrouching;
    bool pIsAttacking;
    bool pIsRecovering;
    string pGuard;

    // AI data
    int armor;
    int durability;
    float health;
    double p2x;
    double p2y;
    bool faceLeft;
    bool isJumping;
    bool isCrouching;
    bool isAttacking;
    bool isStunned;
    bool finishMove;
    bool comboAttack;
    bool comboEnd;
    bool finishDash;
    bool pauseAI;
    bool keepInput;
    string keepAction;

    int doingQCF;
    int doingQCB;
    int doingHCF;
    int doingHCB;

    double distanceBetweenX;
    double distanceBetweenY;

    float timer;
    float circleTimer;
    float squareTimer;
    float triangleTimer;
    float crossTimer;
    float delayTimer;

    float crouchTimer;
    float grabTimer;
    float lightTimer;
    float mediumTimer;
    float heavyTimer;
    float specialTimer;
    float backDashTimer;
    float multiInputTimer;
    float moveTimer;

    private MaxInput MaxInput;
    private CharacterProperties PlayerProp;
    private CharacterProperties AIProp;

    // Still deciding on how to do this.
    // Things it must have:
    // Features- value to be decided in calculations
    // Weight of each feature
    // feature x weight

    // To change value use myDictionary[myKey] = myNewValue;
    public Dictionary<string, double> states = new Dictionary<string, double>();
    public Dictionary<string, double> attackStates = new Dictionary<string, double>();

    // Registering the values' initial states
    void Start()
	{
        // Player data
        pIsBlocking = false;
        pIsAirborne = false;
        pIsCrouching = false;
        pIsAttacking = false;
        pIsRecovering = false;
        pGuard = "";

        // AI data
        timer = 0;
        circleTimer = 0;
        squareTimer = 0;
        triangleTimer = 0;
        crossTimer = 0;
        delayTimer = 0;

        crouchTimer = 0;
        grabTimer = 0;
        lightTimer = 0;
        mediumTimer = 0;
        heavyTimer = 0;
        specialTimer = 0;
        backDashTimer = 0;
        multiInputTimer = 0;
        moveTimer = 0;
        faceLeft = true;
        isJumping = false;
        isCrouching = false;
        isAttacking = false;
        isStunned = false;
        finishMove = false;
        comboAttack = false;
        comboEnd = false;
        finishDash = false;
        pauseAI = false;
        keepInput = false;
        keepAction = "";

        doingQCF = 0;
        doingQCB = 0;
        doingHCF = 0;
        doingHCB = 0;

        distanceBetweenX = 0;
        distanceBetweenY = 0;

        MaxInput = GetComponent<MaxInput>();
        if (!MaxInput.AI)
		{
            enabled = false;
        }
        PlayerProp = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
        AIProp = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();

        states.Add("Attack", 0);
        states.Add("Defend", 0);
        states.Add("MoveCloser", 0);

        attackStates.Add("Zone", 0);
        attackStates.Add("Overhead", 0);
        attackStates.Add("Mid", 0);
        attackStates.Add("Low", 0);
        attackStates.Add("Grab", 0);
    }

    void Update()
	{
        //Stops ai if player has lost
        pHealth = PlayerProp.currentHealth;
        if (pHealth <= 0 || !StartText.startReady)
        {
            pauseAI = true;
        }
        else
        {
            pauseAI = false;
        }

        //If AI has not been paused
        if (!pauseAI)
		{
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
            if (delayTimer > 0)
            {
                delayTimer -= Time.deltaTime;
            }

            if (!keepInput)
            {
                MaxInput.ClearInput("Player2");
            }

            if (delayTimer <= 0)
            {
                if (doingQCF > 0)
                {
                    Debug.Log("doing QCF");
                    QCF();
                }
                else
                {
                    // updateProperties();
                    //
                    // resetStateValues();       // May not always do this every time?
                    //
                    // calculateWeights();
                    //
                    // var max = states.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;   // Gets key with highest value
                    // Debug.Log(max);

                    // Evaluate AI's current states



                    //If ai is on ground set jumping to false
                    if (GameObject.Find("Player2").transform.GetChild(0).transform.position.y <= 0)
                    {
                        isJumping = false;
                    }

                    // // // Executes AI's state
                    // if (max == "Attack") attack();
                    // if (max == "Defend") defend();
                    // if (max == "MoveCloser") moveCloser();
                    testActions();  // REMEMBER TO COMMENT OUT WHEN DONE TESTING
                }
            }
        }
    }

    void attack()
    {
        Debug.Log("attack state");

        calculateAttackWeights();
        var rand = new System.Random().Next(101);    // Random int from 0 to 10

        var maxAttack = attackStates.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;   // Gets key with highest value
        Debug.Log(maxAttack);

        if (maxAttack == "Low")
        {
            MaxInput.Crouch("Player2");
            if (rand <= 30 && squareTimer <= 0)
            {
                MaxInput.Square("Player2");
                crossTimer = .25f;
            }
            if (rand > 30 && rand <= 55 && triangleTimer <= 0)
            {
                MaxInput.Triangle("Player2");
                circleTimer = .25f;
            }
            if (rand > 55 && rand <= 80 && circleTimer <= 0)
            {
                MaxInput.Circle("Player2");
                triangleTimer = .25f;
            }
            if (rand > 80 && crossTimer <= 0)
            {
                MaxInput.Cross("Player2");
                squareTimer = .25f;
            }
        }

        if (maxAttack == "Mid")
        {
            if (rand <= 30 && squareTimer <= 0)
            {
                MaxInput.Square("Player2");
                crossTimer = .25f;
            }
            if (rand > 30 && rand <= 55 && triangleTimer <= 0)
            {
                MaxInput.Triangle("Player2");
                circleTimer = .25f;
            }
            if (rand > 55 && rand <= 80 && circleTimer <= 0)
            {
                MaxInput.Circle("Player2");
                triangleTimer = .25f;
            }
            if (rand > 80 && crossTimer <= 0)
            {
                MaxInput.Cross("Player2");
                squareTimer = .25f;
            }
        }

        if (maxAttack == "Overhead")
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
        }

        if (maxAttack == "Grab")
        {
            Debug.Log("grabbed");
            MaxInput.LBumper("Player2");
            return;
        }

        // Disabled until I figure out how to get command inputs working
        // if (maxAttack == "Zone")
        // {
        //     // Pastry Throw
        //     if (rand >= 20)
        //     {
        //         if(faceLeft == true)
        //         {
        //             QCF();
        //             MaxInput.DownLeft("Player2");
        //             MaxInput.Square("Player2");
        //         }
        //         else
        //         {
        //             MaxInput.DownRight("Player2");
        //             MaxInput.Square("Player2");
        //         }
        //     }
        //     // I'M FIRING MY LAZARRRRR
        //     else if (rand < 2 && armor >= 2)
        //     {
        //         if(faceLeft == true)
        //         {
        //             MaxInput.DownLeft("Player2");
        //             //StartCoroutine(Delay(0.25f));
        //             MaxInput.DownMove("Player2");
        //             MaxInput.MoveLeft("Player2");
        //             //StartCoroutine(Delay(0.25f));
        //             MaxInput.CircleCross("Player2");
        //         }
        //         else
        //         {
        //             MaxInput.DownRight("Player2");
        //             //StartCoroutine(Delay(0.25f));
        //             MaxInput.DownMove("Player2");
        //             MaxInput.MoveRight("Player2");
        //             //StartCoroutine(Delay(0.25f));
        //             MaxInput.CircleCross("Player2");
        //         }
        //     }
        // }
    }

    void QCF()
    {
        if (doingQCF == 1)
        {
            if (faceLeft == true) MaxInput.DownLeft("Player2");
            else MaxInput.DownRight("Player2");
            doingQCF = 2;
            delayTimer = .1f;
        }
        else if (doingQCF == 2)
        {
            MaxInput.Stand("Player2");
            if (faceLeft == true)
            {
                MaxInput.MoveLeft("Player2");
            }
            else MaxInput.MoveRight("Player2");
            if (keepAction == "Square") MaxInput.Square("Player2");
            if (keepAction == "Triangle") MaxInput.Triangle("Player2");
            if (keepAction == "Circle") MaxInput.Circle("Player2");
            if (keepAction == "Cross") MaxInput.Cross("Player2");

            doingQCF = 0;
            keepAction = "";
            delayTimer = 5f;
        }
    }

    void calculateAttackWeights()
    {
        if (distanceBetweenX >= 3) attackStates["Zone"] -= 10000000000; // Disabled until I figure out how to get commands inputs working
        if (pGuard == "Low") attackStates["Overhead"] += 1 + new System.Random().NextDouble() * 2;
        if (pGuard == "High") attackStates["Low"] += 1 + new System.Random().NextDouble() * 2;
        attackStates["Grab"] += (1 - distanceBetweenX) - (distanceBetweenY * 2);
        attackStates["Mid"] = 0.75 + new System.Random().NextDouble() * 2;
    }

    void defend()
    {
        Debug.Log("defend state");
        if (pIsCrouching)
        {
            if (p1x - p2x < 0)
            {
                faceLeft = true;
                MaxInput.Crouch("Player2");
                MaxInput.MoveRight("Player2");
            }
            else
            {
                faceLeft = false;
                MaxInput.Crouch("Player2");
                MaxInput.MoveLeft("Player2");
            }
        }
        else
        {
            if (p1x - p2x < 0)
            {
                faceLeft = true;
                MaxInput.MoveRight("Player2");
            }
            else
            {
                faceLeft = false;
                MaxInput.MoveLeft("Player2");
            }
        }
    }

    void moveCloser()
    {
        Debug.Log("move closer state");
        var rand = new System.Random();

        //If ai is not crouching, back dashing, or combo-ing, move in direction of player
        if (!isCrouching && !finishMove && !finishDash)
        {
            if (p1x - p2x < 0)
            {
                faceLeft = true;
                MaxInput.MoveLeft("Player2");
            }
            else
            {
                faceLeft = false;
                MaxInput.MoveRight("Player2");
            }
        }

        //Foward Dash
        if (distanceBetweenX >= 1.5 && rand.Next(3) == 1)
        {
            if(faceLeft == true)
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
        if (distanceBetweenX < 2 && p2y < p1y - 0.5 && rand.Next(0,4) == 1)
        {
            MaxInput.Jump("Player2");
            isJumping = true;
            if(rand.Next(0,3) == 1)
        	{
                MaxInput.Jump("Player2");
            }
        }
    }

    void calculateWeights()
    {
        if (pIsBlocking) states["Attack"] += 1;
        if (pIsRecovering) states["Attack"] += 1;
        states["Attack"] -= distanceBetweenX - distanceBetweenY - 1;
        if (isAttacking) states["Attack"] -= 1;

        if (pIsAttacking) states["Defend"] += 3;

        // The farther away, the more likely to move closer
        states["MoveCloser"] += distanceBetweenX * 1 + distanceBetweenY * 2;
    }

    void updateProperties()
    {
        // Getting all the current player data
        pArmor = PlayerProp.armor;
        pDurability = PlayerProp.durability;

        GameObject playerInput = GameObject.Find("Player1").transform.GetChild(0).GetChild(0).gameObject;
        GameObject playerHit = GameObject.Find("Player1").transform.GetChild(0).GetChild(2).gameObject;

        pIsBlocking = playerInput.GetComponent<Animator>().GetBool("Blocked");  // only true if I've blocked an attack, not working as intended
        pIsAirborne = playerInput.GetComponent<AcceptInputs>().airborne;
        pIsCrouching = playerInput.GetComponent<Animator>().GetBool("Crouch");
        pIsAttacking = playerInput.GetComponent<AcceptInputs>().attacking;
        pIsRecovering = playerInput.GetComponent<AcceptInputs>().recovering;

        if (playerInput.GetComponent<Animator>().GetBool("HighGuard") == true) pGuard = "High";
        else if (playerInput.GetComponent<Animator>().GetBool("LowGuard") == true) pGuard = "Low";
        else pGuard = "None";

        Debug.Log("I am guarding " + pGuard);
        p1x = GameObject.Find("Player1").transform.GetChild(0).transform.position.x + 1.5;
        p1y = GameObject.Find("Player1").transform.GetChild(0).transform.position.y;

        GameObject aiInput = GameObject.Find("Player2").transform.GetChild(0).GetChild(0).gameObject;

        // Getting all the current AI data
        armor = AIProp.armor;
        durability = AIProp.durability;
        health = AIProp.currentHealth;
        isAttacking = aiInput.GetComponent<AcceptInputs>().attacking;
        //Debug.Log("AI is " + isAttacking);
        p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x + 1.0;
        p2y = GameObject.Find("Player2").transform.GetChild(0).transform.position.y;

        // AI location adjusted based on direction its facing)
        if (faceLeft == false)
        {
            p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x + 1.0 + 0.914;
        }
        distanceBetweenX = Math.Abs(p1x - p2x);
        distanceBetweenY = Math.Abs(p1y - p2y);
    }

    void resetStateValues()
    {
        states["Attack"] = 0;
        states["Defend"] = 0;
        states["MoveCloser"] = 0;

        attackStates["Zone"] = 0;
        attackStates["Overhead"] = 0;
        attackStates["Mid"] = 0;
        attackStates["Low"] = 0;
        attackStates["Grab"] = 0;
    }

    // Currently testing QCF inputs. Maybe just manually set Dhalia's QCF variable???
    void testActions()
    {
        Debug.Log("testAction");
        MaxInput.Crouch("Player2");
        keepAction = "Square";
        doingQCF = 1;
        delayTimer = .1f;
    }

    // IEnumerator Delay()
    // {
    //     Debug.Log("delayed");
    //     pauseAI = true;
    //     yield return new WaitForSeconds(5);
    //     pauseAI = false;
    // }
}

// //Setting random variable and manipulating timer variables
// var rand = new System.Random();

// if (crouchTimer > 0)
// {
//     crouchTimer -= Time.deltaTime;
// }
// if (grabTimer > 0)
// {
//     grabTimer -= Time.deltaTime;
// }
// if (lightTimer > 0)
// {
//     lightTimer -= Time.deltaTime;
// }
// if (mediumTimer > 0)
// {
//     mediumTimer -= Time.deltaTime;
// }
// if (heavyTimer > 0)
// {
//     heavyTimer -= Time.deltaTime;
// }
// if (specialTimer > 0)
// {
//     specialTimer -= Time.deltaTime;
// }
// if (backDashTimer > 0)
// {
//     backDashTimer -= Time.deltaTime;
// }
// if (multiInputTimer > 0)
// {
//     multiInputTimer -= Time.deltaTime;
// }
// if (moveTimer > 0)
// {
//     moveTimer -= Time.deltaTime;
// }
// timer += Time.deltaTime;
//
// //Down-Left/Right input of QCF combo
// if (multiInputTimer <= 0 && finishMove == true)
// {
//     if (faceLeft == true)
// 	{
//         MaxInput.DownLeft("Player2");
//     }
//     else
// 	{
//         MaxInput.DownRight("Player2");
//     }
//     //Setting a timer until next part of combo and setting boolean values appropriately based on characters position
//     multiInputTimer = 0.05f;
//     finishMove = false;
//     comboAttack = true;
//     isCrouching = false;
// }
//
// //Down-Move(Sets vertical to 0) into a Move-Left/Right
// if (multiInputTimer <= 0 && comboAttack == true)
// {
//     MaxInput.DownMove("Player2");
//     if (faceLeft == true)
// 	{
//         MaxInput.MoveLeft("Player2");
//     }
//     else
// 	{
//         MaxInput.MoveRight("Player2");
//     }
//     //Setting a timer until next part of combo and setting boolean values appropriately based on characters position
//     multiInputTimer = 0.15f;
//     comboAttack = false;
//     comboEnd = true;
// }
//
// //Finish with light or special attack depending on some good old rng
// if (multiInputTimer <= 0 && comboEnd == true)
// {
//     if (rand.Next(4) == 1)
// 	{
//         MaxInput.Cross("Player2");
//     }
//     else
// 	{
//         MaxInput.Square("Player2");
//     }
//     //Ending combo and setting a cd for when this combo can be executed again, and setting input to be cleared
//     comboEnd = false;
//     moveTimer = 4;
//     keepInput = false;
// }
//
// //After initial backdash is executed, finish back dash with another input in appropriate direction
// if (multiInputTimer <= 0 && backDashTimer >= 0 && finishDash == true)
// {
//     if (faceLeft == true)
// 	{
//         MaxInput.MoveRight("Player2");
//     }
//     else
// 	{
//         MaxInput.MoveLeft("Player2");
//     }
//     finishDash = false;
//     multiInputTimer = 0;
//     backDashTimer = 0;
// }
//
// //If ai is on ground set jumping to false
// if (GameObject.Find("Player2").transform.GetChild(0).transform.position.y <= 0)
// {
//     isJumping = false;
// }
//
// //When crouchTimer reaches 0 or less set stop crouching otherwise keep crouching
// if (crouchTimer <= 0 || Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) > 1)
// {
//     isCrouching = false;
// }
// else
// {
//     MaxInput.Crouch("Player2");
// }
//
// //When timer is greater than 4 then either crouch or jump and maybe jump twice
// if (timer > 4 && multiInputTimer <= 0)
// {
//     if (rand.Next(0,4) == 2 && Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) < 1)
// 	{
//        MaxInput.Crouch("Player2");
//        isCrouching = true;
//        crouchTimer = 3;
//        timer = 0;
//     }
//     else
// 	{
//        MaxInput.Jump("Player2");
//        isJumping = true;
//        if(rand.Next(0,4) == 4)
//        {
//             MaxInput.Jump("Player2");
//        }
//        timer = 0;
//     }
// }
//
// //If ai is not crouching, back dashing, or combo-ing, move in direction of player
// if (!isCrouching && backDashTimer <= 0 && multiInputTimer <= 0 && !finishMove && !finishDash)
// {
//     if (p1x - p2x < 0)
// 	{
//         faceLeft = true;
//         MaxInput.MoveLeft("Player2");
//     }
//     else
// 	{
//         faceLeft = false;
//         MaxInput.MoveRight("Player2");
//     }
// }
//
// //If player is above ai sometimes jump and maybe jump twice
// if (GameObject.Find("Player2").transform.GetChild(0).transform.position.y <
//     GameObject.Find("Player1").transform.GetChild(0).transform.position.y - 0.5 && rand.Next(0,4) == 1)
// {
//     MaxInput.Jump("Player2");
//     isJumping = true;
//     if(rand.Next(0,3) == 1)
// 	{
//         MaxInput.Jump("Player2");
//     }
// }
//
// //If ai is 3 x units away and jumping, do a double jump
// if (Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) > 3 && isJumping == true)
// {
//     MaxInput.Jump("Player2");
// }
//
// //AI does an attack based on distance from player in x units and some good old rng
// if (multiInputTimer <= 0 && finishMove == false)
// {
//     //Grab attack
//     if (Math.Abs(p1x - p2x) > 0.01 && Math.Abs(p1x - p2x) < 0.1 && grabTimer <= 0)
// 	{
//         MaxInput.LBumper("Player2");
//         if (rand.Next(0, 3) == 2)
// 		{
//             grabTimer = 2;
//         }
//
//     }
//     //Light attack
//     else if(Math.Abs(p1x - p2x) >= 0.1 && Math.Abs(p1x - p2x) < 0.3 && lightTimer <= 0)
// 	{
//         MaxInput.ClearInput("Player2");
//         MaxInput.Square("Player2");
//
//     }
//     //Medium attack
//     else if(Math.Abs(p1x - p2x) >= 0.3 && Math.Abs(p1x - p2x) < 0.6 && mediumTimer <= 0)
// 	{
//         MaxInput.Triangle("Player2");
//     }
//     //Heavy attack
//     else if(Math.Abs(p1x - p2x) >= 0.6 && Math.Abs(p1x - p2x) < 0.9 && heavyTimer <= 0)
// 	{
//         MaxInput.ClearInput("Player2");
//         MaxInput.Circle("Player2");
//         if (rand.Next(0, 10) == 2)
// 		{
//             MaxInput.Square("Player2");
//         }
//     }
//     //Special attack
//     else if(Math.Abs(p1x - p2x) >= 0.9 && Math.Abs(p1x - p2x) < 1 && specialTimer <= 0)
// 	{
//         MaxInput.Cross("Player2");
//         if (rand.Next(0, 5) == 2)
// 		{
//             specialTimer = 1;
//         }
//     }
//     //Light forward attack
//     else if(Math.Abs(p1x - p2x) >= 1 && Math.Abs(p1x - p2x) < 1.1 && lightTimer <= 0 && rand.Next(2) == 1)
// 	{
//         MaxInput.ClearInput("Player2");
//         lightTimer = 2;
//         if(faceLeft == true)
// 		{
//             MaxInput.MoveLeft("Player2");
//         }
//         else
// 		{
//             MaxInput.MoveRight("Player2");
//         }
//         MaxInput.Square("Player2");
//     }
//     //Back Dash
//     else if(Math.Abs(p1x - p2x) >= 1.1 && Math.Abs(p1x - p2x) < 1.5 && backDashTimer <= 0)
// 	{
//         backDashTimer = 10;
//         multiInputTimer = 0.15f;
//         if(faceLeft == true)
// 		{
//             MaxInput.MoveRight("Player2");
//         }
//         else
// 		{
//             MaxInput.MoveLeft("Player2");
//         }
//         finishDash = true;
//     }
//     //DownFoward(Any) attack
//     else if(Math.Abs(p1x - p2x) >= 1.5 && Math.Abs(p1x - p2x) < 1.7 && moveTimer <= 0)
// 	{
//         MaxInput.Stand("Player2");
//         multiInputTimer = 0.05f;
//         crouchTimer = 0.05f;
//         MaxInput.Crouch("Player2");
//         finishMove = true;
//         keepInput = true;
//     }
//     //Foward Dash
//     if (Math.Abs(p1x - p2x) >= 2 && rand.Next(3) == 1)
// 	{
//         if(faceLeft == true)
// 		{
//             MaxInput.MoveLeft("Player2");
//             MaxInput.LStick("Player2");
//         }
//         else
// 		{
//             MaxInput.MoveRight("Player2");
//             MaxInput.LStick("Player2");
//         }
//     }
//}
