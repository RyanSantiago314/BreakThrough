﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{
    float timer;
    float crouchTimer;
    float grabTimer;
    float lightTimer;
    float mediumTimer;
    float heavyTimer;
    float specialTimer;
    float backDashTimer;
    float multiInputTimer;
    float moveTimer;
    double p1x;
    double p2x;
    bool faceLeft;
    bool isJumping;
    bool isCrouching;
    bool finishMove;
    bool comboAttack;
    bool comboEnd;
    bool finishDash;
    bool pauseAI;
    bool keepInput;

    private MaxInput MaxInput;
    private CharacterProperties PlayerProp;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        crouchTimer = 0;
        grabTimer = 0;
        lightTimer = 0;
        mediumTimer = 0;
        heavyTimer = 0;
        specialTimer = 0;
        backDashTimer = 0;
        multiInputTimer = 0;
        moveTimer = 0;
        isJumping = false;
        isCrouching = false;
        faceLeft = true;
        pauseAI = false;
        finishMove = false;
        comboAttack = false;
        comboEnd = false;
        finishDash = false;
        keepInput = false;

        MaxInput = GetComponent<MaxInput>();
        if (!MaxInput.AI)
        {
            enabled = false;
        }
        PlayerProp = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
    }

    void Update() {

        //Setting random variable and manipulating timer variables
        var rand = new System.Random();
        if (!keepInput)
        {
            MaxInput.ClearInput("Player2");
        }
        if (crouchTimer > 0)
        {
            crouchTimer -= Time.deltaTime;
        }
        if (grabTimer > 0)
        {
            grabTimer -= Time.deltaTime;
        }
        if (lightTimer > 0)
        {
            lightTimer -= Time.deltaTime;
        }
        if (mediumTimer > 0)
        {
            mediumTimer -= Time.deltaTime;
        }
        if (heavyTimer > 0)
        {
            heavyTimer -= Time.deltaTime;
        }
        if (specialTimer > 0)
        {
            specialTimer -= Time.deltaTime;
        }
        if (backDashTimer > 0)
        {
            backDashTimer -= Time.deltaTime;
        }
        if (multiInputTimer > 0)
        {
            multiInputTimer -= Time.deltaTime;
        }
        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
        }
        timer += Time.deltaTime;

        //Setting variables representing the x and y locations of player and ai (ai location adjusted based on direction its facing)
        p1x = GameObject.Find("Player1").transform.GetChild(0).transform.position.x + 1.5;
        p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x + 1.0;
        //faceLeft = false;
        if (faceLeft == false)
        {
            p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x + 1.0 + 0.914;
        }

        //Stops ai if player has lost
        if (PlayerProp.currentHealth <= 0 || !StartText.startReady)
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

            //Down-Left/Right input of of QCF combo
            if (multiInputTimer <= 0 && finishMove == true)
            {
                if (faceLeft == true)
                {
                    MaxInput.DownLeft("Player2");
                }
                else
                {
                    MaxInput.DownRight("Player2");
                }
                //Setting a timer until next part of combo and setting boolean values appropriately based on characters position
                multiInputTimer = 0.05f;
                finishMove = false;
                comboAttack = true;
                isCrouching = false;
            }

            //Down-Move(Sets vertical to 0) into a Move-Left/Right
            if (multiInputTimer <= 0 && comboAttack == true)
            {
                MaxInput.DownMove("Player2");
                if (faceLeft == true)
                {
                    MaxInput.moveLeft("Player2");
                }
                else
                {
                    MaxInput.moveRight("Player2");
                }
                //Setting a timer until next part of combo and setting boolean values appropriately based on characters position
                multiInputTimer = 0.15f;
                comboAttack = false;
                comboEnd = true;
            }

            //Finish with light or special attack depending on some good old rng
            if (multiInputTimer <= 0 && comboEnd == true)
            {
                if (rand.Next(4) == 1)
                {
                    MaxInput.Cross("Player2");
                }
                else
                {
                    MaxInput.Square("Player2");
                }
                //Ending combo and setting a cd for when this combo can be executed again, and setting input to be cleared
                comboEnd = false;
                moveTimer = 4;
                keepInput = false;
            }

            //After initial backdash is executed, finish back dash with another input in appropriate direction
            if (multiInputTimer <= 0 && backDashTimer >= 0 && finishDash == true)
            {
                if (faceLeft == true)
                {
                    MaxInput.moveRight("Player2");
                }
                else
                {
                    MaxInput.moveLeft("Player2");
                }
                finishDash = false;
                multiInputTimer = 0;
                backDashTimer = 0;
            }

            //If ai is on ground set jumping to false
            if (GameObject.Find("Player2").transform.GetChild(0).transform.position.y <= 0)
            {
                isJumping = false;
            }

            //When crouchTimer reaches 0 or less set stop crouching otherwise keep crouching
            if (crouchTimer <= 0 || Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) > 1)
            {
                isCrouching = false;
            }
            else
            {
                MaxInput.Crouch("Player2");
            }

            //When timer is greater than 4 then either crouch or jump and maybe jump twice
            if (timer > 4 && multiInputTimer <= 0)
            {
                if (rand.Next(0,4) == 2 && Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) < 1)
                {
                   MaxInput.Crouch("Player2");
                   isCrouching = true;
                   crouchTimer = 3;
                   timer = 0;
                }
                else
                {
                   MaxInput.Jump("Player2");
                   isJumping = true;
                   if(rand.Next(0,4) == 4)
                   {
                        MaxInput.Jump("Player2");
                    }
                   timer = 0;
                }
            }

            //If ai is not crouching, back dashing, or combo-ing, move in direction of player
            if (!isCrouching && backDashTimer <= 0 && multiInputTimer <= 0 && !finishMove && !finishDash)
            {
                if (p1x - p2x < 0)
                {
                    faceLeft = true;
                    MaxInput.moveLeft("Player2");
                }
                else
                {
                    faceLeft = false;
                    MaxInput.moveRight("Player2");
                }
            }

            //If player is above ai sometimes jump and maybe jump twice
            if (GameObject.Find("Player2").transform.GetChild(0).transform.position.y <
                GameObject.Find("Player1").transform.GetChild(0).transform.position.y - 0.5 && rand.Next(0,4) == 1){
                MaxInput.Jump("Player2");
                isJumping = true;
                if(rand.Next(0,3) == 1)
                {
                    MaxInput.Jump("Player2");
                }
            }

            //If ai is 3 x units away and jumping, do a double jump
            if (Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) > 3 && isJumping == true)
            {
                MaxInput.Jump("Player2");
            }

            //AI does an attack based on distance from player in x units and some good old rng
            if (multiInputTimer <= 0 && finishMove == false)
            {
                //Grab attack
                if (Math.Abs(p1x - p2x) > 0.01 && Math.Abs(p1x - p2x) < 0.1 && grabTimer <= 0)
                {
                    MaxInput.LBumper("Player2");
                    if (rand.Next(0, 3) == 2)
                    {
                        grabTimer = 2;
                    }
                    
                }
                //Light attack
                else if(Math.Abs(p1x - p2x) >= 0.1 && Math.Abs(p1x - p2x) < 0.3 && lightTimer <= 0)
                {
                    MaxInput.ClearInput("Player2");
                    MaxInput.Square("Player2");
                    
                }
                //Medium attack
                else if(Math.Abs(p1x - p2x) >= 0.3 && Math.Abs(p1x - p2x) < 0.6 && mediumTimer <= 0)
                {
                    MaxInput.Triangle("Player2");
                }
                //Heavy attack
                else if(Math.Abs(p1x - p2x) >= 0.6 && Math.Abs(p1x - p2x) < 0.9 && heavyTimer <= 0)
                {
                    MaxInput.ClearInput("Player2");
                    MaxInput.Circle("Player2");
                    if (rand.Next(0, 10) == 2)
                    {
                        MaxInput.Square("Player2");
                    }
                }
                //Special attack
                else if(Math.Abs(p1x - p2x) >= 0.9 && Math.Abs(p1x - p2x) < 1 && specialTimer <= 0)
                {
                    MaxInput.Cross("Player2");
                    if (rand.Next(0, 5) == 2)
                    {
                        specialTimer = 1;
                    }
                }
                //Light forward attack
                else if(Math.Abs(p1x - p2x) >= 1 && Math.Abs(p1x - p2x) < 1.1 && lightTimer <= 0 && rand.Next(2) == 1)
                {
                    MaxInput.ClearInput("Player2");
                    lightTimer = 2;
                    if(faceLeft == true)
                    {
                        MaxInput.moveLeft("Player2");
                    }
                    else
                    {
                        MaxInput.moveRight("Player2");
                    }
                    MaxInput.Square("Player2");
                }
                //Back Dash
                else if(Math.Abs(p1x - p2x) >= 1.1 && Math.Abs(p1x - p2x) < 1.5 && backDashTimer <= 0)
                {
                    backDashTimer = 10;
                    multiInputTimer = 0.15f;
                    if(faceLeft == true)
                    {
                        MaxInput.moveRight("Player2");
                    }
                    else
                    {
                        MaxInput.moveLeft("Player2");
                    }
                    finishDash = true;
                }
                //DownFoward(Any) attack
                else if(Math.Abs(p1x - p2x) >= 1.5 && Math.Abs(p1x - p2x) < 1.7 && moveTimer <= 0)
                {
                    MaxInput.Stand("Player2");
                    multiInputTimer = 0.05f;
                    crouchTimer = 0.05f;
                    MaxInput.Crouch("Player2");
                    finishMove = true;
                    keepInput = true;
                }
                //Foward Dash
                if (Math.Abs(p1x - p2x) >= 2 && rand.Next(3) == 1)
                {
                    if(faceLeft == true)
                    {
                        MaxInput.moveLeft("Player2");
                        MaxInput.LStick("Player2");
                    }
                    else
                    {
                        MaxInput.moveRight("Player2");
                        MaxInput.LStick("Player2");
                    }
                }
            }
        }
    }
}