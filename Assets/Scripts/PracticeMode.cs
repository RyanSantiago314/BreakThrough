using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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
    public GameObject PracticeModeSettings;

    private bool P1inCombo;
    private bool P2inCombo;
    private bool P2inAirTrueCombo;
    private bool P2inGroundTrueCombo;
    private bool guardAfterTrueCombo;
    private bool fixAnimBug;
    private float InputTimer;
    private string guardLevel;
    double p1x;
    double p2x;

    public bool enableArmorRefill = true;
    public bool enableCPUAirTech;
    public bool enableGuardAfterFirstHit;
    public string dummyState = "Stand";

    public int P1ValorSetting = 100;
    public int P2ValorSetting = 100;

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
    public Text P1HitType;
    public Text P2HitType;

    private string path = "Assets/Resources/test.txt";
    private int recording = 0;
    private int recordingFrame = 0;
    private List<List<float>> movement = new List<List<float>>();
    private List<List<bool>> inputs = new List<List<bool>>();

    public GameObject DamageDisplays;
    public GameObject P1Displays;
    public GameObject P2Displays;

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
        P1ComboDamage.text = "Total Damage: 0";
        P2ComboDamage.text = "Total Damage: 0";
        P1HighComboDamage.text = "Highest Combo Damage: 0";
        P2HighComboDamage.text = "Highest Combo Damage: 0";

        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
        {
            P2Displays.SetActive(false);
        }
        else
        {
            P1Displays.SetActive(false);
        }

        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            DamageDisplays.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Practice Mode Handler
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            //Check Settings from Practice Pause Menu
            //CPUState Check
            switch (PracticeModeSettings.GetComponent<PauseMenu>().CPUState)
            {
                case 0:
                    dummyState = "Stand";
                    break;
                case 1:
                    dummyState = "Crouch";
                    break;
                case 2:
                    dummyState = "Jump";
                    break;
                case 3:
                    dummyState = "StandGuard";
                    break;
                case 4:
                    dummyState = "LowGuard";
                    break;
                case 5:
                    dummyState = "GuardAll";
                    break;
                case 6:
                    dummyState = "CPU";
                    break;
                case 7:
                    dummyState = "Player";
                    break;
            }

            switch(PracticeModeSettings.GetComponent<PauseMenu>().ArmorRefill)
            {
                case 0:
                    enableArmorRefill = true;
                    break;
                case 1:
                    enableArmorRefill = false;
                    break;
            }

            switch (PracticeModeSettings.GetComponent<PauseMenu>().CPUAirRecover)
            {
                case 0:
                    enableCPUAirTech = false;
                    break;
                case 1:
                    enableCPUAirTech = true;
                    break;
            }

            switch (PracticeModeSettings.GetComponent<PauseMenu>().CPUGroundGuard)
            {
                case 0:
                    enableGuardAfterFirstHit = false;
                    break;
                case 1:
                    enableGuardAfterFirstHit = true;
                    break;
            }

            // If we are NOT paused
            if (!PracticeModeSettings.GetComponent<PauseMenu>().isPaused)
            {
                //Refill Armor Meters Option
                if (enableArmorRefill)
                {
                    //Refill P1 Armor when P1 combo finishes
                    if (HUD.combogauge1.enabled == false && P2inCombo && P1Prop.HitDetect.comboCount == 0)
                    {
                        P1Prop.armor = 4;
                        P1Prop.durability = 100;
                    }
                    //Refill P2 Armor when P2 combo finishes
                    if (HUD.combogauge1.enabled == false && P2inCombo && P2Prop.HitDetect.comboCount == 0)
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
                    if (P2Prop.HitDetect.Actions.acceptSuper && !P1inCombo)
                    {
                        P2Prop.armor = 4;
                        P2Prop.durability = 100;
                    }
                }

                //Update Valor settings from menu
                P1ValorSetting = PracticeModeSettings.GetComponent<PauseMenu>().P1Valor;
                P2ValorSetting = PracticeModeSettings.GetComponent<PauseMenu>().P2Valor;

                //Refill Health Meters/Manage whiff detection for Armor refill
                //Refill P1 HP after P2 combo finishes
                if (P1Prop.HitDetect.hitStun > 0)
                {
                    //P1inCombo = true;
                }
                if (P2Prop.HitDetect.comboCount == 0)
                {
                    P1Prop.currentHealth = P1Prop.maxHealth * (P1ValorSetting / 100f);
                    //P1inCombo = false;
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
                    P2Prop.currentHealth = P2Prop.maxHealth * (P2ValorSetting / 100f);
                    P2inCombo = false;
                    P1CurrentHitDamage = 0;
                    P2PrevHealth = P2Prop.currentHealth;
                    P1CurrentComboTotalDamage = 0;
                }

                //Manage Hit/Combo Damage/Hit Type Display
                //Display Current hit damage/Current Combo damage/Current Hit Type
                if (P2Prop.currentHealth < P2PrevHealth)
                {
                    P1CurrentHitDamage = P2PrevHealth - P2Prop.currentHealth;
                    P1CurrentComboTotalDamage += P1CurrentHitDamage;
                    P1HitDamage.text = "Damage: ";
                    P1HitDamage.text += P1CurrentHitDamage;
                    P1ComboDamage.text = "Total Damage: ";
                    P1ComboDamage.text += P1CurrentComboTotalDamage;
                    P1HitType.text = "Guard Level: ";
                    P1HitType.text += P1Prop.HitDetect.guard;
                    P2PrevHealth = P2Prop.currentHealth;

                    P2CurrentHitDamage = P2PrevHealth - P2Prop.currentHealth;
                    P2CurrentComboTotalDamage += P1CurrentHitDamage;
                    P2HitDamage.text = "Damage: ";
                    P2HitDamage.text += P1CurrentHitDamage;
                    P2ComboDamage.text = "Total Damage: ";
                    P2HitType.text = "Guard Level: ";
                    P2HitType.text += P1Prop.HitDetect.guard;
                    P2ComboDamage.text += P1CurrentComboTotalDamage;
                }
                if (HUD.Player1Combo.text == "" && P1Prop.HitDetect.comboCount != 1)
                {
                    P1HitDamage.text = "";
                    P1HitType.text = "";
                }
                if (HUD.Player2Combo.text == "" && P1Prop.HitDetect.comboCount != 1)
                {
                    P2HitDamage.text = "";
                    P2HitType.text = "";
                }
                //Update Highest Combo Damage
                if (P1CurrentComboTotalDamage > P1HighestComboDamage)
                {
                    P1HighestComboDamage = P1CurrentComboTotalDamage;
                    P1HighComboDamage.text = "Highest Combo Damage: ";
                    P1HighComboDamage.text += P1HighestComboDamage;

                    P2HighestComboDamage = P1CurrentComboTotalDamage;
                    P2HighComboDamage.text = "Highest Combo Damage: ";
                    P2HighComboDamage.text += P1HighestComboDamage;
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
                //Determine P1 Current attack type to determine proper Guard
                if (Player1.transform.GetComponentInChildren<AcceptInputs>().attacking)
                {
                    guardLevel = P1Prop.HitDetect.guard;
                    Debug.Log(guardLevel);
                }
                else
                {
                    guardLevel = "";
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
                    case "StandGuard":
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
                    case "GuardAll":
                        MaxInput.ClearInput("Player2");
                        MaxInput.enableAI();
                        MaxInputObject.GetComponent<AI>().enabled = false;
                        if (p1x - p2x < 0)
                        {
                            if (guardLevel == "Low")
                            {
                                MaxInput.DownRight("Player2");
                            }
                            else
                            {
                                MaxInput.MoveRight("Player2");
                            }
                        }
                        else
                        {
                            if (P1Prop.HitDetect.guard == "Low")
                            {
                                MaxInput.DownLeft("Player2");
                            }
                            else
                            {
                                MaxInput.MoveLeft("Player2");
                            }
                        }
                        break;
                    case "Player":
                        MaxInput.ClearInput("Player2");
                        MaxInput.disableAI();
                        break;
                }

                //CPU Air Tech Option (True Combo Test)
                if (enableCPUAirTech && dummyState != "Player" && dummyState != "CPU")
                {
                    //Air tech if in combo and hitstun = 0
                    if (P2Prop.HitDetect.hitStun > 0 && Player2.transform.GetComponentInChildren<AcceptInputs>().airborne)
                    {
                        P2inAirTrueCombo = true;
                    }
                    else if (P2Prop.HitDetect.hitStun == 0 && Player2.GetComponentInChildren<AcceptInputs>().airborne && P2inAirTrueCombo)
                    {
                        MaxInput.Cross("Player2");
                        P2inAirTrueCombo = false;
                    }
                }

                //CPU ground guard after first hit
                if (enableGuardAfterFirstHit && dummyState != "Player" && dummyState != "CPU")
                {
                    //(On Ground) Guard if in combo, hitstun = 0, and Player is still in the middle of an attack
                    if (P2Prop.HitDetect.hitStun > 0)
                    {
                        P2inGroundTrueCombo = true;
                    }
                    if (P2Prop.HitDetect.hitStun == 0 && P2inGroundTrueCombo)
                    {
                        guardAfterTrueCombo = true;
                        P2inGroundTrueCombo = false;
                    }
                    if (guardAfterTrueCombo)
                    {
                        if (p1x - p2x < 0)
                        {
                            MaxInput.MoveRight("Player2");
                        }
                        else
                        {
                            MaxInput.MoveLeft("Player2");
                        }
                        if (!P1Prop.HitDetect.Actions.attacking)
                        {
                            guardAfterTrueCombo = false;
                        }
                    }
                }

                //Fix Animation bug with resetting positions
                if (fixAnimBug)
                {
                    switch (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character)
                    {
                        case "Dhalia":
                            resetDhalia(Player1);
                            break;
                        case "Achealis":
                            resetAchealis(Player1);
                            break;
                    }

                    switch (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character)
                    {
                        case "Dhalia":
                            resetDhalia(Player2);
                            break;
                        case "Achealis":
                            resetAchealis(Player2);
                            break;
                    }
                    fixAnimBug = false;
                    InputTimer = 0.0f;
                }

                //Reset Positions back to start
                if (Input.GetButtonDown("Select_P2"))
                {
                    resetPositions();
                    //Reset Character Specific things
                    switch (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Character)
                    {
                        case "Dhalia":
                            resetDhalia(Player1);
                            break;
                        case "Achealis":
                            resetAchealis(Player1);
                            break;
                    }

                    switch (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Character)
                    {
                        case "Dhalia":
                            resetDhalia(Player2);
                            break;
                        case "Achealis":
                            resetAchealis(Player2);
                            break;
                    }
                    fixAnimBug = true;
                }

                // https://support.unity3d.com/hc/en-us/articles/115000341143-How-do-I-read-and-write-data-from-a-text-file-
                if (Input.GetButtonDown("Select_P1")) recording++;

                switch (recording)
                {
                    case 1:
                        Debug.Log("Recording armed");
                        // Switch controls over to Player2
                        break;
                    case 2:
                        Debug.Log("Now Recording");
                        recordingFrame++;
                        List<float> getMoves = MaxInput.returnMovement("Player2");
                        List<bool> getInputs = MaxInput.returnInputs("Player2");
                        movement.Add(getMoves);
                        inputs.Add(getInputs);
                        // Get inputs from MaxInput. returnMovement() returnInputs()
                        break;
                    case 3:
                        Debug.Log("Recording Saved");
                        saveRecording();
                        // Create a txt file that has all the inputs for each frame
                        recording = 0;
                        recordingFrame = 0;
                        break;
                }

                // Replay Recording
                //if (Input.GetButtonDown("R_Push"))
            }
        }
    }

    void resetPositions()
    {
        //Reset Player Knockback
        Player1.transform.GetChild(0).GetComponent<MovementHandler>().HitDetect.KnockBack = new Vector2(0, 0);
        Player1.transform.GetChild(0).GetComponent<MovementHandler>().HitDetect.ProjectileKnockBack = new Vector2(0, 0);
        Player1.transform.GetChild(0).GetComponent<MovementHandler>().rb.velocity = Vector2.zero;
        Player2.transform.GetChild(0).GetComponent<MovementHandler>().HitDetect.KnockBack = new Vector2(0, 0);
        Player2.transform.GetChild(0).GetComponent<MovementHandler>().HitDetect.ProjectileKnockBack = new Vector2(0, 0);
        Player2.transform.GetChild(0).GetComponent<MovementHandler>().rb.velocity = Vector2.zero;

        //Reset Camera
        Player1.transform.GetChild(0).GetComponent<MovementHandler>().Actions.superFlash = 0;
        Player2.transform.GetChild(0).GetComponent<MovementHandler>().Actions.superFlash = 0;
        GameObject.Find("CameraPos").transform.GetChild(1).transform.position = GameObject.Find("CameraPos").transform.position;

        //Refill Armor
        P1Prop.armor = 4;
        P1Prop.durability = 100;
        P2Prop.armor = 4;
        P2Prop.durability = 100;

        //Setting players to starting location vectors
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
        {
            GameObject.Find("Player1").transform.GetChild(0).transform.position = GameObject.Find("Player1").transform.position;
        }
        else
        {
            GameObject.Find("Player1").transform.GetChild(0).transform.position = GameObject.Find("Player1").transform.position;
        }
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Right")
        {
            GameObject.Find("Player2").transform.GetChild(0).transform.position = GameObject.Find("Player2").transform.position;
        }
        else
        {
            GameObject.Find("Player2").transform.GetChild(0).transform.position = GameObject.Find("Player2").transform.position;
        }

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
        player.transform.GetChild(2).gameObject.SetActive(false);
        player.transform.GetChild(3).gameObject.SetActive(false);
    }

    private void resetAchealis(GameObject player)
    {
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerACH>().anim.SetTrigger(Animator.StringToHash("Blitz"));
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerACH>().Hitboxes.BlitzCancel();
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerACH>().Actions.landingLag = 0;
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerACH>().Move.HitDetect.KnockBack = Vector2.zero;
        player.transform.GetChild(0).GetComponentInChildren<AttackHandlerACH>().anim.SetBool(Animator.StringToHash("Run"), false);
    }

    private void saveRecording()
    {
        // Clears previous recording
        File.WriteAllText(path, string.Empty);

        StreamWriter writer = new StreamWriter(path, true);
        for (int i = 1; i <= recordingFrame; i++)
        {
            string move = "";
            string input = "";
            for (int j = 0; j < 2; j++)
            {
                move += movement[i-1][j] + " ";
            }
            for (int k = 0; k < 9; k++)
            {
                input += inputs[i-1][k] + " ";
            }
            writer.WriteLine(i + ": " + move + input);
        }
        writer.Close();

        Debug.Log("File Written");

        // //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load("test") as TextAsset;

        //Print the text from the file
        //Debug.Log(asset.text);
    }
}
