using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDetection : MonoBehaviour
{
    public GameObject[] borders;
    public GameObject[] P1Models;
    public GameObject[] P2Models;
    public CursorMovement CursorMovement;
    public bool isOverlap;
    public string currentChar;
    private int charNum;
    public bool P1Selected;
    public bool P2Selected;

    private string p1Circle = "Circle_P1";
    private string p2Circle = "Circle_P2";

    private void OnTriggerEnter2D(Collider2D collider) {
        currentChar = collider.transform.parent.name;
        isOverlap = true;

        switch (currentChar)
        {
            case "Dhalia":
                charNum = 0;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        borders[charNum].SetActive(false);
        currentChar = "";
        isOverlap = false;
        if (this.name == "P1Cursor")
        {
            P1Models[charNum].SetActive(false);
        }
        else
        {
            P2Models[charNum].SetActive(false);
        }
    }

    private void Start()
    {
        p1Circle += UpdateControls(CheckXbox(0));
        p2Circle += UpdateControls(CheckXbox(1));
    }

    void Update()
    {
        if (isOverlap)
        {
            borders[charNum].SetActive(true);
            if (this.name == "P1Cursor")
            {
                P1Models[charNum].SetActive(true);
            }
            else
            {
                P2Models[charNum].SetActive(true);
            }
        }

        //Manage Character Deselection Interactions
        if (P1Selected && Input.GetButtonDown(p1Circle) && !CursorMovement.P1Ready)
        {
            P1Selected = false;
            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character = "";
        }

        if (P2Selected && Input.GetButtonDown(p2Circle) && !CursorMovement.P2Ready)
        {
            P2Selected = false;
            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character = "";
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
