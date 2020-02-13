using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : MonoBehaviour
{
    public AI AI;
    private MaxInput MaxInput;

    void Start()
    {
        MaxInput = GetComponent<MaxInput>();
    }

    public void QCF()
    {
        if (AI.doingQCF == 1)
        {
            if (AI.faceLeft == true)
            {
                Debug.Log("DownLeft");
                MaxInput.DownLeft("Player2");
            }
            else
            {
                Debug.Log("DownRight");
                MaxInput.DownRight("Player2");
            }
            AI.doingQCF = 2;
            AI.delayTimer = .1f;
        }
        else if (AI.doingQCF == 2)
        {
            MaxInput.DownMove("Player2");
            if (AI.faceLeft == true)
            {
                Debug.Log("Left");
                MaxInput.MoveLeft("Player2");
            }
            else
            {
                Debug.Log("Right");
                MaxInput.MoveRight("Player2");
            }
            if (AI.keepAction == "Square") MaxInput.Square("Player2");
            if (AI.keepAction == "Triangle") MaxInput.Triangle("Player2");
            if (AI.keepAction == "Circle") MaxInput.Circle("Player2");
            if (AI.keepAction == "Cross") MaxInput.Cross("Player2");
            if (AI.keepAction == "RTrigger") MaxInput.RTrigger("Player2");

            AI.doingQCF = 0;
            AI.keepAction = "";
            AI.keepInput = false;
        }
    }

    public void QCB()
    {
        if (AI.doingQCB == 1)
        {
            if (AI.faceLeft == true)
            {
                Debug.Log("DownRight");
                MaxInput.DownRight("Player2");
            }
            else
            {

                Debug.Log("DownLeft");
                MaxInput.DownLeft("Player2");
            }
            AI.doingQCB = 2;
            AI.delayTimer = .1f;
        }
        else if (AI.doingQCB == 2)
        {
            MaxInput.DownMove("Player2");
            if (AI.faceLeft == true)
            {
                Debug.Log("Right");
                MaxInput.MoveRight("Player2");
            }
            else
            {

                Debug.Log("Left");
                MaxInput.MoveLeft("Player2");
            }
            if (AI.keepAction == "Square") MaxInput.Square("Player2");
            if (AI.keepAction == "Triangle") MaxInput.Triangle("Player2");
            if (AI.keepAction == "Circle") MaxInput.Circle("Player2");
            if (AI.keepAction == "Cross") MaxInput.Cross("Player2");
            if (AI.keepAction == "RTrigger") MaxInput.RTrigger("Player2");

            AI.doingQCB = 0;
            AI.keepAction = "";
            AI.keepInput = false;
        }
    }
}
