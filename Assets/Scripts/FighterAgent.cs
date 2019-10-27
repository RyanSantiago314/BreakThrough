using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class FighterAgent : Agent
{
    private float timer;
    public float matchTime;
    public MaxInput MaxInput;
    public string Name;
    public int number;
    public CharacterProperties myChar;
    public CharacterProperties opponent;
    private float myHealth;
    private float theirHealth;

    private bool hitRegistered;

    public override void CollectObservations()
    {
        AddVectorObs(number);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        

        if (myChar.currentHealth == 0)
        {
            SetReward(-0.5f);
            Done();
        }
        else if (myChar.currentHealth < myHealth)
        {
            float healthLoss = myHealth - myChar.currentHealth;
            AddReward(-0.25f * healthLoss / myChar.maxHealth);
            myHealth = myChar.currentHealth;
            hitRegistered = true;
        }

        if (opponent.currentHealth == 0)
        {
            SetReward(1f);
            Done();
        }
        else if (opponent.currentHealth < theirHealth)
        {
            float damage = theirHealth - opponent.currentHealth;
            AddReward(damage / opponent.maxHealth);
            theirHealth = opponent.currentHealth;
            hitRegistered = true;
        }

        if (!hitRegistered)
        {
            AddReward(-1 * timer / (matchTime * 60)); //the 60 in the bottom is to keep the sum around 1
        }
        else
        {
            hitRegistered = false;
        }

        timer += Time.deltaTime;
        if (timer > matchTime)
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
        if (MaxInput.training)
        {
            myChar.transform.position = myChar.transform.parent.position;
            myChar.currentHealth = myChar.maxHealth;
            timer = 0;
        }
        myHealth = myChar.currentHealth;
        theirHealth = opponent.currentHealth;
    }
}
