using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDetection : MonoBehaviour
{
    public Animator P1Animator;
    public Animator P2Animator;
    public GameObject[] borders;
    public GameObject[] P1Portraits;
    public GameObject[] P2Portraits;
    public CursorMovement CursorMovement;
    public bool isOverlap;
    public string currentChar;
    private int charNum;
    public bool P1Selected;
    public bool P2Selected;

    private string p1Circle = "Circle_P1";
    private string p2Circle = "Circle_P2";

    private void OnTriggerEnter2D(Collider2D collider) {
        currentChar = collider.transform.name;
        isOverlap = true;
        switch (currentChar)
        {
            case "Dhalia":
                charNum = 0;
                break;
            case "Achealis":
                charNum = 1;
                break;
        }

        //Play Slide Animation
        if (this.name == "P1Cursor")
        {
            if (!P1Selected)
            {
                switch (charNum)
                {
                    case 0:
                        P1Animator.Play("DhaliaModelSlide", -1, 0f);
                        break;
                    case 1:
                        P1Animator.Play("AchealisModelSlide", -1, 0f);
                        break;
                }
            }
        }
        else
        {
            if(!P2Selected)
            {
                switch (charNum)
                {
                    case 0:
                        P2Animator.Play("DhaliaModelSlide", -1, 0f);
                        break;
                    case 1:
                        P2Animator.Play("AchealisModelSlide", -1, 0f);
                        break;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        borders[charNum].SetActive(false);
        currentChar = "";
        isOverlap = false;
        if (this.name == "P1Cursor")
        {
            P1Portraits[charNum].SetActive(false);
        }
        else
        {
            P2Portraits[charNum].SetActive(false);
        }
    }

    private void Start()
    {
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "PvP")
        {
            //Check P1 Side
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Right")
            {
                p1Circle = "Circle_P2";
            }

            //Check P2Side
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Left")
            {
                p2Circle = "Circle_P1";
            }

        }
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "AI")
        {
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
            {
                p2Circle = "Circle_P1";
            }
            p1Circle += UpdateControls(CheckXbox(0));
            p2Circle += UpdateControls(CheckXbox(1));
        }
    }

    void Update()
    {
        if (isOverlap)
        {
            borders[charNum].SetActive(true);
            if (this.name == "P1Cursor")
            {
                P1Portraits[charNum].SetActive(true);
            }
            else
            {
                P2Portraits[charNum].SetActive(true);
            }
        }
    }

    private bool CheckXbox(int player)
    {
        if (Input.GetJoystickNames().Length > player)
        {
            if (Input.GetJoystickNames()[player].Contains("Xbox"))
            {
                return true;
            }
        }
        return false;
    }

    private string UpdateControls(bool xbox)
    {
        if (xbox)
            return "_Xbox";
        return "";
    }
}
