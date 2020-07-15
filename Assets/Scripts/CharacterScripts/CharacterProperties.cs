using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperties : MonoBehaviour
{
    public HitDetector HitDetect;

    public float maxHealth;
    public float currentHealth;
    public int armor;
    public int durability;
    public float comboTimer;

    //valor scales down damage based on current health
    public float valor50;
    public float valor25;
    public float valor10;
    public float currentValor;

    public float durabilityRefillTimer;
    public int durabilityRefillRate = 1;
    public float refillInterval;
    public bool refill = false;
    float refillCounter;

    AnimatorStateInfo currentState;

    static int crouchID;
    static int dizzyID;
    static int runID;
    static int KOID;

    // Start is called before the first frame update
    void Start()
    {
        crouchID = Animator.StringToHash("Crouch");
        dizzyID = Animator.StringToHash("Dizzy");
        runID = Animator.StringToHash("Run");
        KOID = Animator.StringToHash("KOed");

        armor = 4;
        durability = 100;
        currentHealth = maxHealth;
        durabilityRefillRate = 1;

        HitDetect.anim.SetBool(dizzyID, false);
        HitDetect.anim.SetBool(KOID, false);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = HitDetect.anim.GetCurrentAnimatorStateInfo(0);
        if (currentHealth <= 0 && HitDetect.hitStop <= 0)
        {
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice")
            {
                currentHealth = 0;
                if (RoundManager.dizzyKO)
                {
                    HitDetect.anim.SetBool(crouchID, false);
                    HitDetect.anim.SetBool(dizzyID, true);
                }
                else
                    HitDetect.anim.SetBool(KOID, true);

                RoundManager.dizzyKO = false;

                HitDetect.Actions.DisableAll();
                HitDetect.Actions.DisableBlitz();
                HitDetect.Actions.Move.OpponentProperties.HitDetect.anim.SetBool(crouchID, false);
            }           
        }
        else if (currentHealth > 0)
        {
            HitDetect.anim.SetBool(KOID, false);
            if (HitDetect.hitStun > 0 && HitDetect.Actions.comboHits > 0 && !HitDetect.Actions.superHit && !HitDetect.Actions.shattered && HitDetect.OpponentDetector.Actions.superFlash <= 0)
                comboTimer += Time.deltaTime;
            else if (HitDetect.Actions.comboHits <= 0)
                comboTimer = 0;
            if (HitDetect.Actions.grabbed || HitDetect.OpponentDetector.Actions.grabZoom > 0)
            {
                comboTimer = 0;
                durabilityRefillTimer = 0;
            }

            if (currentState.IsName("FUGetup") || currentState.IsName("FDGetup") || currentState.IsName("AirRecovery") || currentState.IsName("DizzyOut"))
            {
                if (armor == 0 && !HitDetect.anim.GetBool(dizzyID))
                {
                    armor = 2;
                    durability = 50;
                }
                comboTimer = 0;
            }

            //increase durability refill rate and damage scaling based on health remaining
            if (currentHealth <= maxHealth / 10)
            {
                currentValor = valor10;
                if (durabilityRefillRate > 0)
                    refillInterval = 1f/60f;
            }
            else if (currentHealth <= maxHealth / 4)
            {
                currentValor = valor25;
                if (durabilityRefillRate > 0)
                    refillInterval = 1f/30f;
            }
            else if (currentHealth <= maxHealth / 2)
            {
                currentValor = valor50;
                if (durabilityRefillRate == 1)
                    refillInterval = 3f/60f;
            }
            else
            {
                currentValor = 1;
                if (durabilityRefillRate > 0)
                    refillInterval = 5f/60f;
            }

            //durability starts refilling after not being in blockstun for three seconds
            if (HitDetect.blockStun <= 0 && !HitDetect.anim.GetBool(dizzyID) && HitDetect.Actions.superFlash <= 0)
                durabilityRefillTimer += Time.deltaTime;
            else if (HitDetect.Actions.superFlash > 0)
            { }
            else
                durabilityRefillTimer = 0;

            if (!HitDetect.pauseScreen.isPaused && !HitDetect.anim.GetBool(KOID) && !HitDetect.OpponentDetector.anim.GetBool(KOID) && RoundManager.gameActive)
            {
                if (durabilityRefillTimer >= 3 && refillCounter >= refillInterval)
                {
                    durability += durabilityRefillRate;
                    refillCounter = 0;
                }
                else if (durabilityRefillTimer >= 3)
                {
                    refillCounter += Time.deltaTime;
                }
                else
                {
                    refillCounter = 0;
                }
            }
        }

        //gain armor when durability has refilled completely
        if (durability > 100)
        {
            if (armor < 4)
            {
                durability -= 100;
                armor++;
            }
            else
            {
                durability = 100;
            }
        }
        else if (durability <= 0 && armor > 0)
        {
            //lose armor when durability has been depleted
            armor--;
            durability = 100;
        }
        
        if (armor > 4)
            armor = 4;

        // increase durability recovery if running forward to promote offensive play
        if (durabilityRefillRate >= 0)
        {
            if (HitDetect.anim.GetBool(runID))
                durabilityRefillRate = 2;
            else
                durabilityRefillRate = 1;
        }
    }
}
