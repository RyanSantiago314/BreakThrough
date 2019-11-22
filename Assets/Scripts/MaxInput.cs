using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxInput : MonoBehaviour
{
    static public bool AI; // Turns on traditional AI for player 2
    public bool neural; // Turns on neural network AI for player 2
    public bool training; //Take over both players with neural networks, for training purposes

    private float horizontal;
    private float vertical;
    private bool square;
    private bool triangle;
    private bool circle;
    private bool cross;
    private bool rBumper;
    private bool rTrigger;
    private bool lBumper;
    private bool lTrigger;
    private bool lstick;
    public FighterAgent player2;

    private float horizontal1;
    private float vertical1;
    private bool square1;
    private bool triangle1;
    private bool circle1;
    private bool cross1;
    private bool rBumper1;
    private bool rTrigger1;
    private bool lBumper1;
    private bool lTrigger1;
    private bool lstick1;
    public FighterAgent player1;

    static public void enableAI() {
        AI = true;
    }

    static public void disableAI() {
        AI = false;
    }

    void Start()
    {
        ClearInput("Player1");
        ClearInput("Player2");

        if (AI && neural)
        {
            player2.enabled = false;
        }
        else if (neural)
        {
            player1.enabled = false;
        }
        else if (!training)
        {
            player1.enabled = false;
            player2.enabled = false;
            GetComponent<FighterAcademy>().enabled = false;
        }
    }

    void Update() {
        
    }

    public float GetAxisRaw(string axis)
    {
        if ((!training && !AI && !neural) || axis.Contains("P1") && !training && (!neural || !AI))
        {
            return Input.GetAxisRaw(axis);
        }
        else
        {
            if (axis.Contains("Horizontal_P2"))
            {
                return horizontal;
            }
            else if (axis.Contains("Vertical_P2"))
            {
                return vertical;
            }
            else if (axis.Contains("Horizontal_P1"))
            {
                return horizontal1;
            }
            else if (axis.Contains("Vertical_P1"))
            {
                return vertical1;
            }
            else
            {
                throw new System.Exception("Axis " + axis + " is unknown axis.");
            }
        }
    }

    public float GetAxis(string axis)
    {
        if ((!training && !AI && !neural) || axis.Contains("P1") && !training && (!neural || !AI))
        {
            return Input.GetAxis(axis);
        }
        else
        {
            if (axis.Contains("Horizontal_P2"))
            {
                return horizontal;
            }
            else if (axis.Contains("Vertical_P2"))
            {
                return vertical;
            }
            else if (axis.Contains("Horizontal_P1"))
            {
                return horizontal1;
            }
            else if (axis.Contains("Vertical_P1"))
            {
                return vertical1;
            }
            else
            {
                throw new System.Exception("Axis " + axis + " is unknown axis.");
            }
        }
    }

    public bool GetButtonDown(string button)
    {
        if ((!training && !AI && !neural) || button.Contains("P1") && !training && (!neural || !AI))
        {
            return Input.GetButtonDown(button);
        }
        else
        {
            switch (button)
            {
                case "Square_P2":
                    return square;
                case "Triangle_P2":
                    return triangle;
                case "Circle_P2":
                    return circle;
                case "Cross_P2":
                    return cross;
                case "R1_P2":
                    return rBumper;
                case "R2_P2":
                    return rTrigger;
                case "L1_P2":
                    return lBumper;
                case "L2_P2":
                    return lTrigger;
                case "L3_P2":
                    return lstick;
                case "Square_P1":
                    return square1;
                case "Triangle_P1":
                    return triangle1;
                case "Circle_P1":
                    return circle1;
                case "Cross_P1":
                    return cross1;
                case "R1_P1":
                    return rBumper1;
                case "R2_P1":
                    return rTrigger1;
                case "L1_P1":
                    return lBumper1;
                case "L2_P1":
                    return lTrigger1;
                case "L3_P1":
                    return lstick1;
                case "Start_P2":
                case "Select_P2":
                    return false;
                default:
                    throw new System.Exception("Button " + button + " is an invalid button.");
            }
        }
    }

    public bool GetButton(string button)
    {
        if ((!training && !AI && !neural) || button.Contains("P1") && !training && (!neural || !AI))
        {
            return Input.GetButton(button);
        }
        else
        {
            switch (button)
            {
                case "Square_P2":
                    return square;
                case "Triangle_P2":
                    return triangle;
                case "Circle_P2":
                    return circle;
                case "Cross_P2":
                    return cross;
                case "R1_P2":
                    return rBumper;
                case "R2_P2":
                    return rTrigger;
                case "L1_P2":
                    return lBumper;
                case "L2_P2":
                    return lTrigger;
                case "L3_P2":
                    return lstick;
                case "Square_P1":
                    return square1;
                case "Triangle_P1":
                    return triangle1;
                case "Circle_P1":
                    return circle1;
                case "Cross_P1":
                    return cross1;
                case "R1_P1":
                    return rBumper1;
                case "R2_P1":
                    return rTrigger1;
                case "L1_P1":
                    return lBumper1;
                case "L2_P1":
                    return lTrigger1;
                case "L3_P1":
                    return lstick1;
                case "Start_P2":
                case "Select_P2":
                    return false;
                default:
                    throw new System.Exception("Button " + button + " is an invalid button.");
            }
        }
    }

    public void ClearInput(string name)
    {
        if (name == "Player2")
        {
            horizontal = 0;
            vertical = 0;
            square = false;
            triangle = false;
            circle = false;
            cross = false;
            rBumper = false;
            rTrigger = false;
            lBumper = false;
            lTrigger = false;
            lstick = false;
        }
        else if (name == "Player1")
        {
            horizontal1 = 0;
            vertical1 = 0;
            square1 = false;
            triangle1 = false;
            circle1 = false;
            cross1 = false;
            rBumper1 = false;
            rTrigger1 = false;
            lBumper1 = false;
            lTrigger1 = false;
            lstick1 = false;
        }
    }

    public void moveLeft(string name)
    {
        if (name == "Player1")
        {
            horizontal1 = -1;
        }
        else
        {
            horizontal = -1;
        }
    }

    public void moveRight(string name)
    {
        if (name == "Player1")
        {
            horizontal1 = 1;
        }
        else
        {
            horizontal = 1;
        }
    }

    public void Jump(string name)
    {
        if (name == "Player1")
        {
            vertical1 = 1;
        }
        else
        {
            vertical = 1;
        }
    }

    public void Crouch(string name)
    {
        if (name == "Player1")
        {
            vertical1 = -1;
            horizontal1 = 0;
        }
        else
        {
            horizontal = 0;
            vertical = -1;
        }
    }

    public void Stand(string name)
    {
        if (name == "Player1")
        {
            horizontal1 = 0;
            vertical1 = 0;
        }
        else
        {
            horizontal = 0;
            vertical = 0;
        }
    }

    public void DownLeft(string name)
    {
        if (name == "Player1")
        {
            vertical1 = -1;
            horizontal = -1;
        }
        else
        {
            vertical = -1;
            horizontal = -1;
        }
    }

    public void DownRight(string name)
    {
        if (name == "Player1")
        {
            vertical1 = -1;
            horizontal = 1;
        }
        else
        {
            vertical = -1;
            horizontal = 1;
        }
    }

    public void DownMove(string name)
    {
        if (name == "Player1")
        {
            vertical1 = 0;
        }
        else
        {
            vertical = 0;
        }
    }

    public void Square(string name)
    {
        if (name == "Player1")
        {
            square1 = true;
        }
        else
        {
            square = true;
        }
    }

    public void Triangle(string name)
    {
        if (name == "Player1")
        {
            triangle1 = true;
        }
        else
        {
            triangle = true;
        }
    }

    public void Circle(string name)
    {
        if (name == "Player1")
        {
            circle1 = true;
        }
        else
        {
            circle = true;
        }
    }

    public void Cross(string name)
    {
        if (name == "Player1")
        {
            cross1 = true;
        }
        else
        {
            cross = true;
        }
    }

    public void RBumper(string name)
    {
        if (name == "Player1")
        {
            rBumper1 = true;
        }
        else
        {
            rBumper = true;
        }
    }

    public void RTrigger(string name)
    {
        if (name == "Player1")
        {
            rTrigger1 = true;
        }
        else
        {
            rTrigger = true;
        }
    }

    public void LBumper(string name)
    {
        if (name == "Player1")
        {
            lBumper1 = true;
        }
        else
        {
            lBumper = true;
        }
    }

    public void LTrigger(string name)
    {
        if (name == "Player1")
        {
            lTrigger1 = true;
        }
        else
        {
            lTrigger = true;
        }
    }

    public void LStick(string name)
    {
        if (name == "Player1")
        {
            lstick1 = true;
        }
        else
        {
            lstick = true;
        }
    }
}
