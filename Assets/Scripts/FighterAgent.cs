using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class FighterAgent : Agent
{
    private float timer;
    public MaxInput MaxInput;
    public string Name;
    public int number;
    public CharacterProperties myChar;
    public CharacterProperties opponent;

    private bool hitRegistered;

    public override void CollectObservations()
    {
        AddVectorObs(number);
    }

    public void GotHit()
    {
        AddReward(-0.05f);
        hitRegistered = true;
    }

    public void HitEnemy()
    {
        Debug.Log(Name + " hit Enemy");
        AddReward(.2f);
        hitRegistered = true;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

        if (!hitRegistered)
        {
            AddReward(-0.001f);
        }
        else
        {
            hitRegistered = false;
        }

        if (myChar.currentHealth == 0)
        {
            SetReward(-0.5f);
            Done();
        }
        if (opponent.currentHealth == 0)
        {
            SetReward(1f);
            Done();
        }

        timer += Time.deltaTime;
        if (timer > 240)
        {
            timer = 0;
            Done();
        }

        var horizontal = Mathf.FloorToInt(vectorAction[0]);
        var vertical = Mathf.FloorToInt(vectorAction[1]);
        var attack = Mathf.FloorToInt(vectorAction[2]);

        MaxInput.ClearInput(Name);

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

    public override void AgentReset()
    {
        Debug.Log("Resetting");
        myChar.transform.position = new Vector3(0, 0, 0);
        myChar.currentHealth = myChar.maxHealth;
        timer = 0;
    }
}
