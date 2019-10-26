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
    float deadTimer;
    double deadCheck;
    double p1x;
    double p2x;
    bool faceLeft;
    bool isJumping;
    bool isCrouching;
    bool pauseAI;

    private MaxInput MaxInput;

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
        deadTimer = 5;
        deadCheck = 0;
        isJumping = false;
        isCrouching = false;
        faceLeft = true;
        pauseAI = false;
        MaxInput = GetComponent<MaxInput>();
        if (!MaxInput.AI)
        {
            enabled = false;
        }
    }
    void Update() {

        //Setting random variable and manipulating timer variables
        var rand = new System.Random();
        MaxInput.ClearInput();
        if (crouchTimer > 0) {
            crouchTimer -= Time.deltaTime;
        }
        if (grabTimer > 0) {
            grabTimer -= Time.deltaTime;
        }
        if (lightTimer > 0) {
            lightTimer -= Time.deltaTime;
        }
        if (mediumTimer > 0) {
            mediumTimer -= Time.deltaTime;
        }
        if (heavyTimer > 0) {
            heavyTimer -= Time.deltaTime;
        }
        if (specialTimer > 0) {
            specialTimer -= Time.deltaTime;
        }
        if (deadTimer > 0) {
            deadTimer -= Time.deltaTime;
        }
        timer += Time.deltaTime;

        //Setting variables representing the x and y locations of player and ai (ai location adjusted based on direction its facing)
        p1x = GameObject.Find("Player1").transform.GetChild(0).transform.position.x + 1.5;
        p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x + 1.0;
        if (!faceLeft) {
            p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x + 1.0 + 0.914;
        }

        //Stops ai if player has not moved x position within time frame
        if (deadTimer <= 0) {
            if (deadCheck == p1x && p1x - 1.5 >= -8 && p1x - 1.5 <= 11) {
                pauseAI = true;
                deadCheck = p1x;
                deadTimer = 1;

            }
            else {
                pauseAI = false;
                deadCheck = p1x;
                deadTimer = 3;
            }
        }

        if (!pauseAI) {
        
        //If ai is on ground set jumping to false
        if (GameObject.Find("Player2").transform.GetChild(0).transform.position.y <= 0) {
            isJumping = false;
        }

        //When crouchTimer reaches 0 or less set stop crouching otherwise keep crouching
        if (crouchTimer <= 0 || Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) > 1) {
            isCrouching = false;
        }
        else {
            MaxInput.Crouch();
        }

        //When timer is greater than 4 then either crouch or jump and maybe jump twice
        if (timer > 4)
        {
            if (rand.Next(0,4) == 2 && Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) < 1) {
               MaxInput.Crouch();
               isCrouching = true;
               crouchTimer = 3;
               timer = 0;
            }
            else {
               MaxInput.Jump();
               isJumping = true;
               if(rand.Next(0,4) == 4) {
                    MaxInput.Jump();
                }
               timer = 0;
            }
        }

        //If ai is not crouching move in direction of player
        if (!isCrouching) {
            if (p1x - p2x < 0) {
                faceLeft = true;
                MaxInput.moveLeft();
            }
            else {
                faceLeft = false;
                MaxInput.moveRight();
            }
        }

        //If player is above ai sometimes jump and maybe jump twice
        if (GameObject.Find("Player2").transform.GetChild(0).transform.position.y < 
            GameObject.Find("Player1").transform.GetChild(0).transform.position.y - 0.5 && rand.Next(0,4) == 1){
            MaxInput.Jump();
            isJumping = true;
            if(rand.Next(0,3) == 1) {
                MaxInput.Jump();
            }
        }

        //If ai is 3 units away and jumping, do a double jump
        if (Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) > 3 && isJumping == true) {
            MaxInput.Jump();
        }

        //7 in 8 chance that ai does an attack based on distance from player in x units and some good old rng
        if (rand.Next(0, 8) != 1) {
            //Grab attack
            if (Math.Abs(p1x - p2x) > 0.01 && Math.Abs(p1x - p2x) < 0.1 && grabTimer <= 0) {
                MaxInput.LBumper();
                if (rand.Next(0, 3) == 2) {
                    grabTimer = 2;
                }
            }
            //Light attack
            else if(Math.Abs(p1x - p2x) >= 0.1 && Math.Abs(p1x - p2x) < 0.3 && lightTimer <= 0) {
                MaxInput.Square();
                /*if (rand.Next(0, 30) == 2) {
                    lightTimer = 1;
                }*/
            }
            //Medium attack
            else if(Math.Abs(p1x - p2x) >= 0.3 && Math.Abs(p1x - p2x) < 0.6 && mediumTimer <= 0) {
                MaxInput.Triangle();
                /*if (rand.Next(0, 30) == 2) {
                    mediumTimer = 1;
                }*/
            }
            //Heavy attack
            else if(Math.Abs(p1x - p2x) >= 0.6 && Math.Abs(p1x - p2x) < 0.9 && heavyTimer <= 0) {
                MaxInput.ClearInput();
                MaxInput.Circle();
                /*if (rand.Next(0, 30) == 2) {
                    heavyTimer = 1;
                }*/
            }
            //Special attack
            else if(Math.Abs(p1x - p2x) >= 0.9 && Math.Abs(p1x - p2x) < 1 && specialTimer <= 0) {
                MaxInput.Cross();
                if (rand.Next(0, 5) == 2) {
                    specialTimer = 1;
                }
            }
                /*if (Math.Abs(p1x - p2x) > 2.5) {
                if(p1x - p2x < 0) {
                    MaxInput.moveLeft();
                    MaxInput.moveLeft();
                }
                else {
                    MaxInput.moveRight();
                    MaxInput.moveRight();
                }
            }*/
        }
        }
    }
}