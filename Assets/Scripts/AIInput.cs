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

    // Quarter Circle Forward
    public void QCF()
    {
        // Step 1: Down movement
        if (AI.doingQCF == 1)
        {
            MaxInput.Crouch("Player2");
            AI.doingQCF = 2;
            AI.delayTimer = .1f;
            AI.keepInput = true;
        }
        // Step 2: Down Left/Right movement
        else if (AI.doingQCF == 2)
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
            AI.doingQCF = 3;
            AI.delayTimer = .1f;
        }
        // Step 3: Left/Right movement
        else if (AI.doingQCF == 3)
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

    // Quarter Circle Backward
    public void QCB()
    {
        // Step 1: Down movement
        if (AI.doingQCB == 1)
        {
            MaxInput.Crouch("Player2");
            AI.doingQCB = 2;
            AI.delayTimer = .1f;
            AI.keepInput = true;
        }
        // Step 2: Down Left/Right movement
        else if (AI.doingQCB == 2)
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
            AI.doingQCB = 3;
            AI.delayTimer = .1f;
        }
        // Step 3: Left/Right movement
        else if (AI.doingQCB == 3)
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

    // Half Circle Forward
    public void HCF()
    {
        // Step 1: Right/Left movement
        if (AI.doingHCF == 1)
        {
            if (AI.faceLeft == true)
            {
                MaxInput.MoveRight("Player2");
            }
            else
            {
                MaxInput.MoveLeft("Player2");
            }
            AI.doingHCF = 2;
            AI.delayTimer = .1f;
            AI.keepInput = true;
        }
        // Step 2: Down Right/Left movement
        else if (AI.doingHCF == 2)
        {
            if (AI.faceLeft == true)
            {
                MaxInput.DownRight("Player2");
            }
            else
            {
                MaxInput.DownLeft("Player2");
            }
            AI.doingHCF = 3;
            AI.delayTimer = .1f;
        }
        // Step 3: Down movement
        else if (AI.doingHCF == 3)
        {
            MaxInput.Crouch("Player2");
            AI.doingHCF = 4;
            AI.delayTimer = .1f;
        }
        // Step 4: Down Left/Right movement
        else if (AI.doingHCF == 4)
        {
            if (AI.faceLeft == true)
            {
                MaxInput.DownLeft("Player2");
            }
            else
            {
                MaxInput.DownRight("Player2");
            }
            AI.doingHCF = 5;
            AI.delayTimer = .1f;
        }
        // Step 5: Left/Right movement
        else if (AI.doingHCF == 5)
        {
            MaxInput.DownMove("Player2");
            if (AI.faceLeft == true)
            {
                MaxInput.MoveLeft("Player2");
            }
            else
            {
                MaxInput.MoveRight("Player2");
            }
            if (AI.keepAction == "Square") MaxInput.Square("Player2");
            if (AI.keepAction == "Triangle") MaxInput.Triangle("Player2");
            if (AI.keepAction == "Circle") MaxInput.Circle("Player2");
            if (AI.keepAction == "Cross") MaxInput.Cross("Player2");
            if (AI.keepAction == "RTrigger") MaxInput.RTrigger("Player2");

            AI.doingHCF = 0;
            AI.keepAction = "";
            AI.keepInput = false;
        }
    }

    // Half Circle Backward
    public void HCB()
    {
        // Step 1: Right/Left movement
        if (AI.doingHCB == 1)
        {
            if (AI.faceLeft == true)
            {
                MaxInput.MoveLeft("Player2");
            }
            else
            {
                MaxInput.MoveRight("Player2");
            }
            AI.doingHCB = 3;                ////////////////
            AI.delayTimer = .1f;
            AI.keepInput = true;
        }
        // Step 2: Down Right/Left movement
        else if (AI.doingHCB == 2)
        {
            if (AI.faceLeft == true)
            {
                MaxInput.DownLeft("Player2");
            }
            else
            {
                MaxInput.DownRight("Player2");
            }
            AI.doingHCB = 3;
            AI.delayTimer = .1f;
        }
        // Step 3: Down movement
        else if (AI.doingHCB == 3)
        {
            MaxInput.Crouch("Player2");
            AI.doingHCB = 5;          ////////////////////////////
            AI.delayTimer = .1f;
        }
        // Step 4: Down Left/Right movement
        else if (AI.doingHCB == 4)
        {
            if (AI.faceLeft == true)
            {
                MaxInput.DownRight("Player2");
            }
            else
            {
                MaxInput.DownLeft("Player2");
            }
            AI.doingHCB = 5;
            AI.delayTimer = .1f;
        }
        // Step 5: Left/Right movement
        else if (AI.doingHCB == 5)
        {
            MaxInput.DownMove("Player2");
            if (AI.faceLeft == true)
            {
                MaxInput.MoveRight("Player2");
            }
            else
            {
                MaxInput.MoveLeft("Player2");
            }
            AI.doingHCB = 6 ;
            AI.delayTimer = .1f;
        }
        // Pressing Attack button
        else if (AI.doingHCB == 6)
        {
            if (AI.keepAction == "Square") MaxInput.Square("Player2");
            if (AI.keepAction == "Triangle") MaxInput.Triangle("Player2");
            if (AI.keepAction == "Circle") MaxInput.Circle("Player2");
            if (AI.keepAction == "Cross") MaxInput.Cross("Player2");
            if (AI.keepAction == "RTrigger") MaxInput.RTrigger("Player2");

            AI.doingHCB = 0;
            AI.keepAction = "";
            AI.keepInput = false;
        }
    }
}
