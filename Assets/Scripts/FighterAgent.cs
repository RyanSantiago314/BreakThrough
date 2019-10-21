using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class FighterAgent : Agent
{
    public MaxInput MaxInput;
    public string Name;

    private void Update()
    {
        if (MaxInput.LastHit() == Name)
        {
            AddReward(-0.1f);
        }
        else if (MaxInput.LastHit() != "")
        {
            AddReward(.2f);
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        AddReward(-0.01f);
        var horizontal = Mathf.FloorToInt(vectorAction[0]);
        var vertical = Mathf.FloorToInt(vectorAction[1]);
        var attack = Mathf.FloorToInt(vectorAction[2]);

        MaxInput.ClearInput();

        switch (horizontal)
        {
            case 1:
                MaxInput.moveLeft(Name);
                break;
            case 2:
                MaxInput.moveRight(Name);
                break;
            case 0:
                //Not moving side to side
                break;
        }

        switch (vertical)
        {
            case 1:
                MaxInput.Jump(Name);
                break;
            case 2:
                MaxInput.Crouch(Name);
                break;
            case 0:
                //Not jumping or crouching
                break;
        }

        switch (attack)
        {
            case 1:
                MaxInput.Square(Name);
                break;
            case 2:
                MaxInput.Triangle(Name);
                break;
            case 3:
                MaxInput.Circle(Name);
                break;
            case 4:
                MaxInput.Cross(Name);
                break;
            case 5:
                MaxInput.RBumper(Name);
                break;
            case 6:
                MaxInput.RTrigger(Name);
                break;
            case 7:
                MaxInput.LBumper(Name);
                break;
            case 8:
                MaxInput.LTrigger(Name);
                break;
            case 0:
                //Not attacking
                break;
        }
    }
}
