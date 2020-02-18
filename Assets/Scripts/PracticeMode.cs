using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeMode : MonoBehaviour
{
    CharacterProperties P1Prop;
    CharacterProperties P2Prop;
    HitDetector P1hit;
    HitDetector P2hit;
    AcceptInputs P1Input;
    AcceptInputs P2Input;
    public GameObject GameOverManager;

    private bool P1inHitstun;
    private bool P2inHitstun;

    private bool enableArmorRefill = true;

    // Start is called before the first frame update
    void Start()
    {
        P1Prop = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
        P2Prop = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();
        P1hit = GameObject.Find("Player1").transform.GetComponentInChildren<HitDetector>();
        P2hit = GameObject.Find("Player2").transform.GetComponentInChildren<HitDetector>();
        P1Input = GameObject.Find("Player1").transform.GetChild(0).transform.GetComponentInChildren<AcceptInputs>();
        P2Input = GameObject.Find("Player2").transform.GetChild(0).transform.GetComponentInChildren<AcceptInputs>();
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            GameOverManager = GameObject.Find("GameOverManager");
            GameOverManager.SetActive(false);
        }       
    }

    // Update is called once per frame
    void Update()
    {
        //Practice Mode Handler
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            //Refill Armor Meters
            if (enableArmorRefill)
            {
                //Refill P1 Armor when P1 (true) combo finishes
                if (P2Prop.HitDetect.hitStun == 0 && P2inHitstun)
                {
                    P1Prop.armor = 4;
                    P1Prop.durability = 100;
                }
                //Refill P2 Armor when P2 (true) combo finishes
                if (P1Prop.HitDetect.hitStun == 0 && P1inHitstun)
                {
                    P2Prop.armor = 4;
                    P2Prop.durability = 100;
                }
                //Refill P1 armor if moves whiffed
                if (P1Prop.HitDetect.Actions.acceptSuper && !P2inHitstun)
                {
                    P1Prop.armor = 4;
                    P1Prop.durability = 100;
                }
                //Refill P2 armor if moves whiffed
                if (P2Prop.HitDetect.Actions.acceptSuper && !P1inHitstun)
                {
                    P2Prop.armor = 4;
                    P2Prop.durability = 100;
                }
            }
            
            //Refill Health Meters            
            //Refill P1 HP after P2 (true) combo finishes
            if (P1Prop.HitDetect.hitStun >= 0)
            {
                if (P1Prop.HitDetect.hitStun > 0)
                {
                    P1inHitstun = true;
                }
                else if (P1Prop.HitDetect.hitStun == 0 && P1inHitstun)
                {
                    P1Prop.currentHealth = P2Prop.maxHealth;
                    P1inHitstun = false;
                }
            }
            //Refill P2 HP after P1 (true) combo finishes           
            if (P2Prop.HitDetect.hitStun >= 0)
            {
                if (P2Prop.HitDetect.hitStun > 0)
                {
                    P2inHitstun = true;
                }
                else if (P2Prop.HitDetect.hitStun == 0 && P2inHitstun)
                {
                    P2Prop.currentHealth = P2Prop.maxHealth;
                    //P1Prop.armor = 4;
                    //P1Prop.durability = 100;
                    P2inHitstun = false;
                }
            }

            //Reset Positions back to start
            if (Input.GetButtonDown("Select_P1"))
            {
                resetPositions();
            }
        }
    }

    void resetPositions()
    {
        //MAKE CHARACTERS IN NEUTRAL POSITION

        //Sets players meters (health, armor, durability) to full
        P1Prop.currentHealth = P1Prop.maxHealth;
        P1Prop.armor = 4;
        P1Prop.durability = 100;
        P1Prop.HitDetect.currentVelocity = Vector2.zero;
        P1Input.anim.SetBool(Animator.StringToHash("Standing"), true);

        P2Prop.currentHealth = P2Prop.maxHealth;
        P2Prop.armor = 4;
        P2Prop.durability = 100;
        P2Prop.HitDetect.currentVelocity = Vector2.zero;
        P2Input.anim.SetBool(Animator.StringToHash("Standing"), true);

        //Setting players to starting location vectors
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
        {
            Vector3 p1Start = new Vector3(-1.3f, 1.10f, -3);
            GameObject.Find("Player1").transform.GetChild(0).transform.position = p1Start;
        }
        else
        {
            Vector3 p1Start = new Vector3(1.3f, 1.10f, -3);
            GameObject.Find("Player1").transform.GetChild(0).transform.position = p1Start;
        }
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Right")
        {
            Vector3 p2Start = new Vector3(1.3f, 1.10f, -3);
            GameObject.Find("Player2").transform.GetChild(0).transform.position = p2Start;
        }
        else
        {
            Vector3 p2Start = new Vector3(-1.3f, 1.10f, -3);
            GameObject.Find("Player2").transform.GetChild(0).transform.position = p2Start;
        }
        GameObject.Find("CameraPos").transform.GetChild(1).transform.position = GameObject.Find("CameraPos").transform.position;
    }
}
