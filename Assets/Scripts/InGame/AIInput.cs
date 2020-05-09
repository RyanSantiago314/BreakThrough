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
                MaxInput.DownLeft("Player2");
            }
            else
            {
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
                MaxInput.MoveLeft("Player2");
            }
            else
            {
                MaxInput.MoveRight("Player2");
            }
            AI.doingQCF = 4;
            AI.delayTimer = .1f;
        }
        // Step 4: Pressing Attack button
        else if (AI.doingQCF == 4)
        {
            if (AI.keepAction == "Square") MaxInput.Square("Player2");
            if (AI.keepAction == "Triangle") MaxInput.Triangle("Player2");
            if (AI.keepAction == "Circle") MaxInput.Circle("Player2");
            if (AI.keepAction == "Cross") MaxInput.Cross("Player2");
            if (AI.keepAction == "RTrigger") MaxInput.RTrigger("Player2");
            if (AI.keepAction == "RBumper") MaxInput.RBumper("Player2");

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
            AI.doingQCB = 4;
            AI.delayTimer = .1f;
        }
        // Step 4: Pressing attack button
        else if (AI.doingQCB == 4)
        {
            var rand = new System.Random().Next(101);    // Random int from 0 to 100
            if (AI.keepAction == "Square") MaxInput.Square("Player2");
            if (AI.keepAction == "Triangle")
            {
                MaxInput.Triangle("Player2");
                bool headRush = rand <= 50;
            }
            if (AI.keepAction == "Circle") MaxInput.Circle("Player2");
            if (AI.keepAction == "Cross") MaxInput.Cross("Player2");
            if (AI.keepAction == "RTrigger") MaxInput.RTrigger("Player2");
            if (AI.keepAction == "RBumper") MaxInput.RBumper("Player2");

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

        // Step 2: Down movement
        else if (AI.doingHCF == 2)
        {
            MaxInput.Crouch("Player2");
            AI.doingHCF = 3;
            AI.delayTimer = .1f;
        }

        // Step 3: Left/Right movement
        else if (AI.doingHCF == 3)
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
            AI.doingHCF = 4;
            AI.delayTimer = .1f;
        }
        // Step 4: Pressing attack button
        else if (AI.doingHCF == 4)
        {
            if (AI.keepAction == "Square") MaxInput.Square("Player2");
            if (AI.keepAction == "Triangle") MaxInput.Triangle("Player2");
            if (AI.keepAction == "Circle") MaxInput.Circle("Player2");
            if (AI.keepAction == "Cross") MaxInput.Cross("Player2");
            if (AI.keepAction == "RTrigger") MaxInput.RTrigger("Player2");
            if (AI.keepAction == "RBumper") MaxInput.RBumper("Player2");

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
            AI.doingHCB = 2;
            AI.delayTimer = .1f;
            AI.keepInput = true;
        }

        // Step 2: Down movement
        else if (AI.doingHCB == 2)
        {
            MaxInput.Crouch("Player2");
            AI.doingHCB = 3;
            AI.delayTimer = .1f;
        }

        // Step 3: Left/Right movement
        else if (AI.doingHCB == 3)
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
            AI.doingHCB = 4;
            AI.delayTimer = .1f;
        }
        // Step 4: Pressing Attack button
        else if (AI.doingHCB == 4)
        {
            if (AI.keepAction == "Square") MaxInput.Square("Player2");
            if (AI.keepAction == "Triangle") MaxInput.Triangle("Player2");
            if (AI.keepAction == "Circle") MaxInput.Circle("Player2");
            if (AI.keepAction == "Cross") MaxInput.Cross("Player2");
            if (AI.keepAction == "RTrigger") MaxInput.RTrigger("Player2");
            if (AI.keepAction == "RBumper") MaxInput.RBumper("Player2");

            AI.doingHCB = 0;
            AI.keepAction = "";
            AI.keepInput = false;
        }
    }

    public void combo5L_1()
    {
        // Step 1: 5L
        if (AI.doing5L_1 == 1)
        {
            MaxInput.Square("Player2");

            AI.doing5L_1 = 2;
            AI.delayTimer = .2f;
            //AI.keepInput = true;
        }

        // Step 2: 5M
        else if (AI.doing5L_1 == 2 && (AI.pIsHitstun || AI.pIsBlockstun))
        {
            MaxInput.Triangle("Player2");

            AI.doing5L_1 = 3;
            AI.delayTimer = .2f;
        }

        // Step 3: 5H
        else if (AI.doing5L_1 == 3 && (AI.pIsHitstun || AI.pIsBlockstun))
        {
            MaxInput.Circle("Player2");

            AI.doing5L_1 = 4;
            AI.delayTimer = .7f;
        }

        // Step 4: 5B
        else if (AI.doing5L_1 == 4 && (AI.pIsHitstun || AI.pIsBlockstun))
        {
            MaxInput.Cross("Player2");

            AI.doing5L_1 = 0;
            //AI.delayTimer = 5f;
        }

        else AI.doing5L_1 = 0;
    }

    public void combo2H_1()
    {
        // Step 1: Crouching
        if (AI.doing2H_1 == 1)
        {
            MaxInput.Crouch("Player2");

            AI.doing2H_1 = 2;
            AI.delayTimer = .1f;
            AI.keepInput = true;
        }

        // Step 2: 2H
        else if (AI.doing2H_1 == 2)
        {
            MaxInput.Circle("Player2");

            AI.doing2H_1 = 3;
            AI.delayTimer = .5f;
        }

        // Step 3: Jump Left/Right
        else if (AI.doing2H_1 == 3 && AI.pIsHitstun)
        {
            if (AI.faceLeft == true)
            {
                MaxInput.MoveLeft("Player2");
            }
            else
            {
                MaxInput.MoveRight("Player2");
            }
            MaxInput.Jump("Player2");

            AI.doing2H_1 = 4;
            AI.delayTimer = .1f;
            AI.keepInput = false;
        }

        // Step 4 - 7: J.H
        else if (AI.doing2H_1 >= 4 && AI.doing2H_1 <= 7 && AI.pIsHitstun)
        {
            MaxInput.Circle("Player2");

            AI.doing2H_1++;
            AI.delayTimer = .25f;
            //if (AI.doing2H_1 == 8) AI.delayTimer = .3f;
        }

        // Step 8: Jump Left/Right
        else if (AI.doing2H_1 == 8 && AI.pIsHitstun)
        {
            if (AI.faceLeft == true)
            {
                MaxInput.MoveLeft("Player2");
            }
            else
            {
                MaxInput.MoveRight("Player2");
            }
            MaxInput.Jump("Player2");

            AI.doing2H_1 = 9;
            AI.delayTimer = .1f;
        }

        // Step 9 - 11: J.H
        else if (AI.doing2H_1 >= 9 && AI.doing2H_1 <= 11 && AI.pIsHitstun)
        {
            MaxInput.Circle("Player2");

            AI.doing2H_1++;
            AI.delayTimer = .25f;
        }

        else if (AI.doing2H_1 == 12 && AI.pIsHitstun)
        {
            MaxInput.Cross("Player2");

            AI.doing2H_1 = 13;
            AI.delayTimer = .18f;
        }

        else if (AI.doing2H_1 == 13 && AI.pIsHitstun)
        {
            AI.doing2H_1 = 14;

            AI.keepAction = "Circle";
            AI.doingQCB = 1;
            QCB();
        }

        else if (AI.doing2H_1 == 14 && AI.pIsHitstun)
        {
            AI.doing2H_1 = 15;
            AI.delayTimer = 1.25f;
        }

        else if (AI.doing2H_1 == 15 && AI.pIsHitstun && AI.armor >= 2)
        {
            AI.doing2H_1 = 0;

            AI.keepAction = "RTrigger";
            AI.doingQCF = 1;
            AI.AIInput.QCF();
        }

        else
        {
            AI.doing2H_1 = 0;
            //AI.delayTimer = 5f;
            AI.keepInput = false;
        }
    }
}
