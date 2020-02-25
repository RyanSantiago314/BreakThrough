using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public MaxInput MaxInput;
    public GameObject MaxInputObject;
    public GameObject GameOverManager;

    private bool P1inCombo;
    private bool P2inCombo;
    private float InputTimer;
    double p1x;
    double p2x;

    public bool enableArmorRefill = true;
    public bool enableP2Controller = false;
    public string dummyState = "Stand";

    public int P1ValorSetting;
    public int P2ValorSetting;

    private int P1CurrentValor;
    private int P2CurrentValor;
    private float P1PrevHealth;
    private float P2PrevHealth;
    private float P1CurrentHitDamage;
    private float P2CurrentHitDamage;
    private float P1CurrentComboTotalDamage;
    private float P2CurrentComboTotalDamage;
    private float P1HighestComboDamage;
    private float P2HighestComboDamage;

    public Text P1HitDamage;
    public Text P2HitDamage;
    public Text P1ComboDamage;
    public Text P2ComboDamage;
    public Text P1HighComboDamage;
    public Text P2HighComboDamage;

    public GameObject DamageDisplays;

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
        P1PrevHealth = P1Prop.maxHealth;
        P2PrevHealth = P2Prop.maxHealth;
        P1HitDamage.text = "";
        P2HitDamage.text = "";
        P1ComboDamage.text = "Total Damage: ";
        P2ComboDamage.text = "Total Damage: ";
        P1HighComboDamage.text = "Highest Combo Damage: 0";
        P2HighComboDamage.text = "Highest Combo Damage: 0";

        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            GameOverManager = GameObject.Find("GameOverManager");
            GameOverManager.SetActive(false);
            DamageDisplays.SetActive(true);
        }       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Player1.GetComponent<MovementHandler>().Actions.superFlash);
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
                P2CurrentHitDamage = 0;
                P1PrevHealth = P1Prop.currentHealth;
                P2CurrentComboTotalDamage = 0;
            }
            //Refill P2 HP after P1 combo finishes  
            if (P2Prop.HitDetect.hitStun > 0)
            {
                P2inCombo = true;
                InputTimer = 0.0f;
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
                P1CurrentHitDamage = 0;
                P2PrevHealth = P2Prop.currentHealth;
                P1CurrentComboTotalDamage = 0;
            }

            //Manage Hit/Combo Damage Display
            //Display P1 current hit damage
            if (P2Prop.currentHealth < P2PrevHealth)
            {
                P1CurrentHitDamage = P2PrevHealth - P2Prop.currentHealth;
                P1CurrentComboTotalDamage += P1CurrentHitDamage;
                P1HitDamage.text = "";
                P1HitDamage.text = "Damage: ";
                P1HitDamage.text += P1CurrentHitDamage;
                P1ComboDamage.text = "";
                P1ComboDamage.text = "Total Damage : ";
                P1ComboDamage.text += P1CurrentComboTotalDamage;
                P2PrevHealth = P2Prop.currentHealth;
            }
            if (HUD.Player1Combo.text == "" && P1Prop.HitDetect.comboCount != 1)
            {
                P1HitDamage.text = "";
            }
            //Display P2 current hit damage
            if (P1Prop.currentHealth < P1PrevHealth)
            {
                P2CurrentHitDamage = P1PrevHealth - P1Prop.currentHealth;
                P2CurrentComboTotalDamage += P2CurrentHitDamage;
                P2HitDamage.text = "";
                P2HitDamage.text = "Damage: ";
                P2HitDamage.text += P2CurrentHitDamage;
                P2ComboDamage.text = "";
                P2ComboDamage.text = "Total Damage : ";
                P2ComboDamage.text += P2CurrentComboTotalDamage;
                P1PrevHealth = P1Prop.currentHealth;
            }
            if (HUD.Player2Combo.text == "" && P2Prop.HitDetect.comboCount != 1)
            {
                P2HitDamage.text = "";
            }
            //Update Highest Combo Damage
            if (P1CurrentComboTotalDamage > P1HighestComboDamage)
            {
                P1HighestComboDamage = P1CurrentComboTotalDamage;
                P1HighComboDamage.text = "";
                P1HighComboDamage.text = "Highest Combo Damage: ";
                P1HighComboDamage.text += P1HighestComboDamage;
            }
            if (P2CurrentComboTotalDamage > P2HighestComboDamage)
            {
                P2HighestComboDamage = P2CurrentComboTotalDamage;
                P2HighComboDamage.text = "";
                P2HighComboDamage.text = "Highest Combo Damage: ";
                P2HighComboDamage.text += P2HighestComboDamage;
            }

            //Handle Dummy State
            p1x = GameObject.Find("Player1").transform.GetChild(0).transform.position.x;
            p2x = GameObject.Find("Player2").transform.GetChild(0).transform.position.x;
            if (InputTimer > 0)
            {
                InputTimer -= Time.deltaTime;
            }
            else
            {
                InputTimer = 0;
            }
            switch (dummyState)
            {
                case "CPU":
                    MaxInput.enableAI();
                    MaxInputObject.GetComponent<AI>().enabled = true;
                    break;
                case "Stand":
                    MaxInput.ClearInput("Player2");
                    MaxInput.enableAI();
                    MaxInputObject.GetComponent<AI>().enabled = false;
                    break;
                case "Crouch":
                    MaxInput.ClearInput("Player2");
                    MaxInput.enableAI();
                    MaxInputObject.GetComponent<AI>().enabled = false;
                    MaxInput.Crouch("Player2");
                    break;
                case "Jump":
                    MaxInput.ClearInput("Player2");
                    MaxInput.enableAI();
                    MaxInputObject.GetComponent<AI>().enabled = false;
                   
                    if (InputTimer == 0 && !P2inCombo)
                    {
                        MaxInput.Jump("Player2");
                        InputTimer = 1.0f;
                    }                  
                    break;
                case "Guard":
                    MaxInput.ClearInput("Player2");
                    MaxInput.enableAI();
                    MaxInputObject.GetComponent<AI>().enabled = false;
                    if (p1x - p2x < 0)
                    {
                        MaxInput.MoveRight("Player2");
                    }
                    else
                    {
                        MaxInput.MoveLeft("Player2");
                    }                        
                    break;
                case "LowGuard":
                    MaxInput.ClearInput("Player2");
                    MaxInput.enableAI();                    
                    MaxInputObject.GetComponent<AI>().enabled = false;
                    if (p1x - p2x < 0)
                    {
                        MaxInput.DownRight("Player2");
                    }
                    else
                    {
                        MaxInput.DownLeft("Player2");
                    }
                    break;
                case "Player": //Player Complete
                    MaxInput.ClearInput("Player2");
                    MaxInput.disableAI();
                    break;
            }

            //Reset Positions back to start **Still needs Refinement
            if (Input.GetButtonDown("Select_P1"))
            {
                resetPositions();
            }
        }
    }

    void resetPositions()
    {
        InputTimer = 0.0f;

        //Setting players to starting location vectors
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
        {
            Vector3 p1Start = new Vector3(-1.3f, 1.05f, -3);
            GameObject.Find("Player1").transform.GetChild(0).transform.position = p1Start;
        }
        else
        {
            Vector3 p1Start = new Vector3(1.3f, 1.05f, -3);
            GameObject.Find("Player1").transform.GetChild(0).transform.position = p1Start;
        }
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Right")
        {
            Vector3 p2Start = new Vector3(1.3f, 1.05f, -3);
            GameObject.Find("Player2").transform.GetChild(0).transform.position = p2Start;
        }
        else
        {
            Vector3 p2Start = new Vector3(-1.3f, 1.05f, -3);
            GameObject.Find("Player2").transform.GetChild(0).transform.position = p2Start;
        }

        //Reset Player Knockback
        Player1.transform.GetChild(0).GetComponent<MovementHandler>().HitDetect.KnockBack = new Vector2(0, 0);
        Player1.transform.GetChild(0).GetComponent<MovementHandler>().HitDetect.ProjectileKnockBack = new Vector2(0, 0);
        Player1.transform.GetChild(0).GetComponent<MovementHandler>().rb.velocity = Vector2.zero;
        Player2.transform.GetChild(0).GetComponent<MovementHandler>().HitDetect.KnockBack = new Vector2(0, 0);
        Player2.transform.GetChild(0).GetComponent<MovementHandler>().HitDetect.ProjectileKnockBack = new Vector2(0, 0);
        Player2.transform.GetChild(0).GetComponent<MovementHandler>().rb.velocity = Vector2.zero;
        //DISABLE BOUNCE

        //Reset Camera
        Player1.transform.GetChild(0).GetComponent<MovementHandler>().Actions.superFlash = 0;
        Player2.transform.GetChild(0).GetComponent<MovementHandler>().Actions.superFlash = 0;
        GameObject.Find("CameraPos").transform.GetChild(1).transform.position = GameObject.Find("CameraPos").transform.position;
       
        //Reset Character Specific things
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
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerDHA>().anim.SetTrigger(Animator.StringToHash("Blitz"));
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerDHA>().Hitboxes.BlitzCancel();
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerDHA>().Actions.landingLag = 0;
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerDHA>().Move.HitDetect.KnockBack = Vector2.zero;
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerDHA>().anim.SetBool(Animator.StringToHash("Run"), false);
        player.transform.GetChild(1).gameObject.SetActive(false);
        player.transform.GetChild(2).gameObject.SetActive(false);
    }
}
