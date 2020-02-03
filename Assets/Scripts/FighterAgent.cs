using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

/*
 * Things that can be done to improve AI:
 *
 * Doublecheck what it is the network is seeing, might be
 * getting some cropping on their image
 *
 * train more
 *
 * try to get imitation learning to work
 *
 * check out ways to only show the fighters
 * (Camera from the opposite angle?)
 *
 * Command used: mlagents-learn ModelsBrains/fighter.yaml --run-id=IterationXX --train --slow
 * The --slow is needed so the training and the fighting system run at the same speed.
 */

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

    //private float totalTime;

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
            float penalty = -0.25f * (healthLoss / myChar.maxHealth);
            AddReward(penalty);
            //Debug.Log("Hit Penalty: " + penalty);
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
            float reward = damage / opponent.maxHealth;
            AddReward(reward);
            //Debug.Log("Hit Reward: " + reward);
            theirHealth = opponent.currentHealth;
            hitRegistered = true;
        }

        if (!hitRegistered)
        {
            float penalty = -1 * timer / (matchTime * 2000); //the 2000 in the bottom is to keep the sum around 1
            AddReward(penalty);
            //totalTime += penalty;
            //Debug.Log("Time Penalty: " + penalty + " Total time penalty: " + totalTime + " Timer: " + timer);
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
                MaxInput.MoveLeft(Name);
                break;
            case 2:
                MaxInput.MoveRight(Name);
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
