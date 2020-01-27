using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public GameObject P1Character;
    public GameObject P2Character;
    public GameObject HitMarker;

    void Awake()
    {
        //**Make Switch Statements when more characters are added
        setP1Properties();
        setP2Properties();
    }

    void start() {
        
    }

    void setP1Properties()
    {
        //Load Character and set name
        P1Character = Instantiate(Resources.Load("Characters/Dhalia", typeof(GameObject)), GameObject.Find("Player1").transform) as GameObject;
        P1Character.name = "Dhalia";

        //Set Character color
        P1Character.transform.GetChild(0).GetComponent<ColorSwapDHA>().colorNum = 1;

        //Assign CharacterHandlers
        GameObject.Find("Player1").GetComponent<FighterAgent>().myChar = P1Character.GetComponent<CharacterProperties>();
        P1Character.GetComponent<MovementHandler>().MaxInput = GameObject.Find("MaxInput").GetComponent<MaxInput>();
        P1Character.GetComponent<AttackHandlerDHA>().MaxInput = GameObject.Find("MaxInput").GetComponent<MaxInput>();
        P1Character.transform.GetChild(2).GetComponent<HitDetector>().hitTrack = HitMarker.transform;
        
    }

    void setP2Properties()
    {
        //Load Character and set name
        P2Character = Instantiate(Resources.Load("Characters/Dhalia", typeof(GameObject)), GameObject.Find("Player2").transform) as GameObject;
        P2Character.name = "Dhalia";

        //Set Character color
        P2Character.transform.GetChild(0).GetComponent<ColorSwapDHA>().colorNum = 2;

        //Assign CharacterHandlers
        P2Character.GetComponent<MovementHandler>().MaxInput = GameObject.Find("MaxInput").GetComponent<MaxInput>();
        P2Character.GetComponent<AttackHandlerDHA>().MaxInput = GameObject.Find("MaxInput").GetComponent<MaxInput>();
        P2Character.transform.GetChild(2).GetComponent<HitDetector>().hitTrack = HitMarker.transform;
        GameObject.Find("Player2").GetComponent<FighterAgent>().opponent = P1Character.GetComponent<CharacterProperties>(); //<- Is this needed?
    }
}
