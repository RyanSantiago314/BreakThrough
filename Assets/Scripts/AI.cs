﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI : MonoBehaviour
{
    float timer;
    double p1x;
    double p2x;
    private MaxInput MaxInput;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        //rand = new Random();
        MaxInput = GetComponent<MaxInput>();
        if (!MaxInput.AI)
        {
            enabled = false;
        }
    }
    void Update() {
        MaxInput.ClearInput();
        timer += Time.deltaTime;
        p1x = GameObject.Find("Player1").transform.GetChild(0).transform.position.x + 1.5;
        p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x + 1.0;
        //Debug.Log(p1x);

        if (timer > 5)
        {
            var rand = new System.Random();
            if (rand.Next(1,3) == 1) {
               MaxInput.Crouch();
               timer = 0;
            }
            else {
               MaxInput.Jump();
               timer = 0;
            }
        }

        if(p1x - p2x < 0) {
            MaxInput.moveLeft();
        }
        else {
            MaxInput.moveRight();
        }

        if (Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) > 0 && Math.Abs(Math.Abs(p1x) - Math.Abs(p2x)) < 0.15) {
            MaxInput.LBumper();
            
        }
        else if(Math.Abs(p1x - p2x) >= 0.15 && Math.Abs(p1x - p2x) < 0.5) {
            MaxInput.Square();
        }
        else if(Math.Abs(p1x - p2x) >= 0.5 && Math.Abs(p1x - p2x) < 1) {
            MaxInput.Triangle();
        }
        else if(Math.Abs(p1x - p2x) >= 1 && Math.Abs(p1x - p2x) < 1.25) {
            MaxInput.Circle();
            timer = 0;
        }
        else if(Math.Abs(p1x - p2x) >= 1.25 && Math.Abs(p1x - p2x) < 1.5) {
            MaxInput.Cross();
        }
        /*if (math.Abs(p1x - p2x) > 3) {
            if(p1x - p2x > 0) {
               MaxInput.dashLeft();
            }
            else {
               MaxInput.dashRight();
            }
        }*/
    }
}