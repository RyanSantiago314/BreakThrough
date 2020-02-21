using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeMode : MonoBehaviour
{
    GameObject Player1;
    GameObject Player2;

    CharacterProperties P1Prop;
    CharacterProperties P2Prop;
    HitDetector P1hit;
    HitDetector P2hit;
    AcceptInputs P1Input;
    AcceptInputs P2Input;
    HUD HUD;
    public GameObject GameOverManager;

    private bool P1inCombo;
    private bool P2inCombo;

    public bool enableArmorRefill = true;

    public int P1ValorSetting;
    public int P2ValorSetting;

    private int P1CurrentValor;
    private int P2CurrentValor;

    // Start is called before the first frame update
    void Start()
    {
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
        P1Prop = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
        P2Prop = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();
        P1hit = GameObject.Find("Player1").transform.GetComponentInChildren<HitDetector>();
        P2hit = GameObject.Find("Player2").transform.GetComponentInChildren<HitDetector>();
        P1Input = GameObject.Find("Player1").transform.GetChild(0).transform.GetComponentInChildren<AcceptInputs>();
        P2Input = GameObject.Find("Player2").transform.GetChild(0).transform.GetComponentInChildren<AcceptInputs>();
        HUD = GameObject.Find("HUD").GetComponent<HUD>();
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
            //Refill Armor Meters Option
            if (enableArmorRefill)
            {
                //Refill P1 Armor when P1 combo finishes
                if (HUD.combogauge1.enabled == false && P2inCombo)
                {
                    P1Prop.armor = 4;
                    P1Prop.durability = 100;
                }
                //Refill P2 Armor when P2 combo finishes
                if (HUD.combogauge2.enabled == false && P2inCombo)
                {
                    P2Prop.armor = 4;
                    P2Prop.durability = 100;
                }
                //Refill P1 armor after move whiffed
                if (P1Prop.HitDetect.Actions.acceptSuper && !P2inCombo)
                {
                    P1Prop.armor = 4;
                    P1Prop.durability = 100;
                }
                //Refill P2 armor after move whiffed
                if (P2Prop.HitDetect.Actions.acceptSuper && !P2inCombo)
                {
                    P2Prop.armor = 4;
                    P2Prop.durability = 100;
                }
            }

            //Reset Health if Valor setting has changed
            if (P1CurrentValor != P1ValorSetting)
            {
                P1CurrentValor = P1ValorSetting;
                if (P1CurrentValor == 0)
                {
                    P1Prop.currentHealth = P1Prop.maxHealth;
                }
                else if (P1CurrentValor == 1)
                {
                    P1Prop.currentHealth = P1Prop.maxHealth / 2;
                }
                else if (P1CurrentValor == 2)
                {
                    P1Prop.currentHealth = P1Prop.maxHealth / 4;
                }
                else if (P1CurrentValor == 3)
                {
                    P1Prop.currentHealth = P1Prop.maxHealth / 10;
                }               
            }

            if (P2CurrentValor != P2ValorSetting)
            {
                P2CurrentValor = P2ValorSetting;
                if (P2CurrentValor == 0)
                {
                    P2Prop.currentHealth = P2Prop.maxHealth;
                }
                else if (P2CurrentValor == 1)
                {
                    P2Prop.currentHealth = P2Prop.maxHealth / 2;
                }
                else if (P2CurrentValor == 2)
                {
                    P2Prop.currentHealth = P2Prop.maxHealth / 4;
                }
                else if (P2CurrentValor == 3)
                {
                    P2Prop.currentHealth = P2Prop.maxHealth / 10;
                }
            }

            //Refill Health Meters/Manage whiff detection for Armor refill            
            //Refill P1 HP after P2 combo finishes
            if (P1Prop.HitDetect.hitStun > 0)
            {
                P1inCombo = true;
            }
            if (P2Prop.HitDetect.comboCount == 0)
            {
                if (P1CurrentValor == 0)
                {
                    P1Prop.currentHealth = P1Prop.maxHealth;
                }
                else if (P1CurrentValor == 1)
                {
                    P1Prop.currentHealth = P1Prop.maxHealth / 2;
                }
                else if (P1CurrentValor == 2)
                {
                    P1Prop.currentHealth = P1Prop.maxHealth / 4;
                }
                else if (P1CurrentValor == 3)
                {
                    P1Prop.currentHealth = P1Prop.maxHealth / 10;
                }
                P1inCombo = false;
            }
            //Refill P2 HP after P1 combo finishes  
            if (P2Prop.HitDetect.hitStun > 0)
            {
                P2inCombo = true;
            }
            if (P1Prop.HitDetect.comboCount == 0)
            {
                if (P2CurrentValor == 0)
                {
                    P2Prop.currentHealth = P2Prop.maxHealth;
                }
                else if (P2CurrentValor == 1)
                {
                    P2Prop.currentHealth = P2Prop.maxHealth / 2;
                }
                else if (P2CurrentValor == 2)
                {
                    P2Prop.currentHealth = P2Prop.maxHealth / 4;
                }
                else if (P2CurrentValor == 3)
                {
                    P2Prop.currentHealth = P2Prop.maxHealth / 10;
                }
                P2inCombo = false;
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

        //MAKE CHARACTERS IN NEUTRAL POSITION
        switch (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character)
        {
            case "Dhalia":
                resetDhalia(Player1);
                break;
        }

        switch (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character)
        {
            case "Dhalia":
                resetDhalia(Player2);
                break;
        }
    }

    //Character Specific Reset Properties
    private void resetDhalia(GameObject player)
    {
        //reset velocity??
        //camera reset for toaster
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerDHA>().anim.SetTrigger(Animator.StringToHash("Blitz"));
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerDHA>().Hitboxes.BlitzCancel();
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerDHA>().Actions.landingLag = 0;
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerDHA>().Move.HitDetect.KnockBack = Vector2.zero;
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerDHA>().anim.SetBool(Animator.StringToHash("Run"), false);
        player.transform.GetChild(1).gameObject.SetActive(false);
        player.transform.GetChild(2).gameObject.SetActive(false);
    }
}
