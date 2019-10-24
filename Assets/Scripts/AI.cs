﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{
    float timer;
    float crouchTimer;
    double p1x;
    double p2x;
    bool faceLeft;
    bool isJumping;
    bool isCrouching;

    private MaxInput MaxInput;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        crouchTimer = 0;
        isJumping = false;
        isCrouching = false;
        faceLeft = true;
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
        timer += Time.deltaTime;
        //Setting variables representing the x and y locations of player and ai (ai location adjusted based on direction)
        p1x = GameObject.Find("Player1").transform.GetChild(0).transform.position.x + 1.5;
        p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x + 1.0;
        if (!faceLeft) {
            p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x + 1.0 + 0.914;
        }
        
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

        //If player is above ai jump and maybe jump twice
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

        //7 in 8 chance that ai does an attack based on distance from player in x units
        if (rand.Next(0, 8) != 1) {
            if (Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) > 0.01 && Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) < 0.1) {
                MaxInput.LBumper();
            }
            else if(Math.Abs(p1x - p2x) >= 0.1 && Math.Abs(p1x - p2x) < 0.3) {
                MaxInput.Square();
            }
            else if(Math.Abs(p1x - p2x) >= 0.3 && Math.Abs(p1x - p2x) < 0.6) {
                MaxInput.Triangle();
            }
            else if(Math.Abs(p1x - p2x) >= 0.6 && Math.Abs(p1x - p2x) < .9) {
                MaxInput.Circle();
            }
            else if(Math.Abs(p1x - p2x) >= .9 && Math.Abs(p1x - p2x) < 1 && rand.Next(0,3) != 1) {
                MaxInput.Cross();
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