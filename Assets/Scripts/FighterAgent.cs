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

    public override void CollectObservations()
    {
        AddVectorObs(number);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

        if (MaxInput.LastHit(Name))
        {
            AddReward(-0.05f);
        }
        else if (MaxInput.LastHit(opponent.GetComponentInParent<Transform>().name))
        {
            AddReward(.2f);
        }
        else
        {
            AddReward(-0.01f);
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
        if (timer > 120)
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
                Debug.Log(name + " is taking actions: " + horizontal + vertical + ", Square");
                break;
            case 2:
                MaxInput.Triangle(Name);
                Debug.Log(name + " is taking actions: " + horizontal + vertical + ", Triangle");
                break;
            case 3:
                MaxInput.Circle(Name);
                Debug.Log(name + " is taking actions: " + horizontal + vertical + ", Circle");
                break;
            case 4:
                MaxInput.Cross(Name);
                Debug.Log(name + " is taking actions: " + horizontal + vertical + ", Cross");
                break;
            case 5:
                MaxInput.RBumper(Name);
                Debug.Log(name + " is taking actions: " + horizontal + vertical + ", RBumper");
                break;
            case 6:
                MaxInput.RTrigger(Name);
                Debug.Log(name + " is taking actions: " + horizontal + vertical + ", RTrigger");
                break;
            case 7:
                MaxInput.LBumper(Name);
                Debug.Log(name + " is taking actions: " + horizontal + vertical + ", LBumper");
                break;
            case 8:
                MaxInput.LTrigger(Name);
                Debug.Log(name + " is taking actions: " + horizontal + vertical + ", LTrigger");
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
    }
}
