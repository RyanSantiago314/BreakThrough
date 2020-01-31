using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDetection : MonoBehaviour
{
    public GameObject[] borders;
    private bool isOverlap;
    public string currentChar;
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
        currentChar = "";
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
                GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character = currentChar; 
            }

            if (Input.GetButtonDown("Cross_P2") && this.name == "P2Cursor")
            {
                P2Selected = true;
                GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character = currentChar;

            }
        }

        //Manage Character Deselection Interactions
        if (P1Selected && Input.GetButtonDown("Circle_P1"))
        {
            P1Selected = false;
            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character = "";
        }

        if (P2Selected && Input.GetButtonDown("Circle_P2"))
        {
            P2Selected = false;
            GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character = "";
        }
    }
}
