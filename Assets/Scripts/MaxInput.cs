using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxInput : MonoBehaviour
{
    public bool AI;

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

    void Start()
    {
        ClearInput();
    }

    public float GetAxisRaw(string axis)
    {
        if (!AI || axis.Contains("P1"))
        {
            return Input.GetAxisRaw(axis);
        }
        else
        {
            if (axis.Contains("Horizontal"))
            {
                return horizontal;
            }
            else if (axis.Contains("Vertical"))
            {
                return vertical;
            }
            else
            {
                throw new System.Exception("Axis " + axis + " is unknown axis.");
            }
        }
    }

    public float GetAxis(string axis)
    {
        if (!AI || axis.Contains("P1"))
        {
            return Input.GetAxis(axis);
        }
        else
        {
            if (axis.Contains("Horizontal"))
            {
                return horizontal;
            }
            else if (axis.Contains("Vertical"))
            {
                return vertical;
            }
            else
            {
                throw new System.Exception("Axis " + axis + " is unknown axis.");
            }
        }
    }

    public bool GetButtonDown(string button)
    {
        if (!AI || button.Contains("P1"))
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
        if (!AI || button.Contains("P1"))
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
                case "Start_P2":
                case "Select_P2":
                    return false;
                default:
                    throw new System.Exception("Button " + button + " is an invalid button.");
            }
        }
    }

    public void ClearInput()
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
}

    public void moveLeft()
    {
        horizontal = -1;
    }

    public void moveRight()
    {
        horizontal = 1;
    }

    public void Jump()
    {
        vertical = 1;
    }

    public void Crouch()
    {
        vertical = -1;
    }

    public void Square()
    {
        square = true;
    }
    public void Triangle()
    {
        triangle = true;
    }

    public void Circle()
    {
        circle = true;
    }

    public void Cross()
    {
        cross = true;
    }

    public void RBumper()
    {
        rBumper = true;
    }

    public void RTrigger()
    {
        rTrigger = true;
    }

    public void LBumper()
    {
        lBumper = true;
    }

    public void LTrigger()
    {
        lTrigger = true;
    }

}
