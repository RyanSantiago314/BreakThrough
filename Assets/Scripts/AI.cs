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
        MaxInput.ClearInput();
        if (crouchTimer > 0) {
            crouchTimer -= Time.deltaTime;
        }
        timer += Time.deltaTime;
        p1x = GameObject.Find("Player1").transform.GetChild(0).transform.position.x + 1.5;
        p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x + 1.0;
        if (!faceLeft) {
            p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x + 1.0 + 0.914;
        }
        //Debug.Log(Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)));
        Debug.Log(crouchTimer);
        if (GameObject.Find("Player2").transform.GetChild(0).transform.position.y == 0) {
            isJumping = false;
        }

        if (crouchTimer <= 0) {
            isCrouching = false;
        }
        else {
            MaxInput.Crouch();
        }

        if (timer > 4)
        {
            var rand = new System.Random();
            if (rand.Next(1,5) == 1) {
               MaxInput.Crouch();
               isCrouching = true;
               crouchTimer = 3;
               timer = 0;
            }
            else {
               MaxInput.Jump();
               isJumping = true;
               timer = 0;
            }
        }

        if (!isCrouching) {
        if(p1x - p2x < 0) {
            faceLeft = true;
            MaxInput.moveLeft();
        }
        else {
            faceLeft = false;
            MaxInput.moveRight();
        }
        }

            if (Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) > 0.01 && Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) < 0.1) {
                MaxInput.LBumper();
            }
            else if(Math.Abs(p1x - p2x) >= 0.1 && Math.Abs(p1x - p2x) < 0.3) {
                MaxInput.Square();
            }
            else if(Math.Abs(p1x - p2x) >= 0.3 && Math.Abs(p1x - p2x) < 0.6) {
                MaxInput.Triangle();
            }
            else if(Math.Abs(p1x - p2x) >= 0.6 && Math.Abs(p1x - p2x) < 1) {
                MaxInput.Circle();
            }
            else if(Math.Abs(p1x - p2x) >= 1 && Math.Abs(p1x - p2x) < 1.2) {
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