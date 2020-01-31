using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDetection : MonoBehaviour
{
    public GameObject[] borders;
    private bool isOverlap;
    private string currentChar;
    private int charNum;
    public bool P1Selected;
    public bool P2Selected;

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
        isOverlap = false;
    }

    void Update()
    {
        if (isOverlap)
        {
            borders[charNum].SetActive(true);
            if (Input.GetButtonDown("Cross_P1") && this.name == "P1Cursor")
            {
                P1Selected = true;
            }

            if (Input.GetButtonDown("Cross_P2") && this.name == "P2Cursor")
            {
                P2Selected = true;
            }
        }

        //Manage Character Deselection Interactions
        if (P1Selected && Input.GetButtonDown("Circle_P1"))
        {
            P1Selected = false;
        }

        if (P2Selected && Input.GetButtonDown("Circle_P2"))
        {
            P2Selected = false;
        }
    }
}
