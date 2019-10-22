using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperties : MonoBehaviour
{
    public HitDetector HitDetect;

    public int maxHealth;
    public int currentHealth;
    public int armor;
    public int durability;
    public float comboTimer;

    //valor scales down damage based on current health
    public float valor100;
    public float valor50;
    public float valor25;
    public float currentValor;

    public float durabilityRefillTimer;
    public int durabilityRefillRate = 1;
    public int refillInterval;
    int refillCounter;

    static int crouchID;
    static int dizzyID;
    static int aliveID;

    // Start is called before the first frame update
    void Start()
    {
        crouchID = Animator.StringToHash("Crouch");
        dizzyID = Animator.StringToHash("Dizzy");
        aliveID = Animator.StringToHash("Alive");

        armor = 4;
        durability = 100;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth == 0)
        {
            HitDetect.Actions.anim.SetBool(aliveID, false);
            HitDetect.Actions.DisableAll();
        }
        else
            HitDetect.Actions.anim.SetBool(aliveID, true);

        if (HitDetect.hitStun > 0 && armor <= 0)
            comboTimer += .6f;
        if (HitDetect.Actions.grabbed)
            comboTimer = 0;

        if (HitDetect.Actions.anim.GetCurrentAnimatorStateInfo(0).IsName("FUGetup") || HitDetect.Actions.anim.GetCurrentAnimatorStateInfo(0).IsName("FDGetup") ||
            HitDetect.Actions.anim.GetCurrentAnimatorStateInfo(0).IsName("AirRecovery"))
        {
            if(armor < 0)
            {
                //make character dizzy if armor is less than zero, usually triggered by throws but also possible through other means
                comboTimer = 0;
                armor = 0;
                Debug.Log("DIZZY");
                HitDetect.Actions.DisableAll();
                HitDetect.Actions.anim.SetBool(dizzyID, true);
                
            }
            else if (armor == 0 && comboTimer > 50)
            {
                //refill armor based on amount of time spent in hitstun on wake-up or aerial recovery
                if (comboTimer < 300)
                    armor = 2;
                else if (comboTimer < 400)
                    armor = 3;
                else
                    armor = 4;

                durability = 100;
                comboTimer = 0;
            }
        }

        //increase durability refill rate and damage scaling based on health remaining
        if (currentHealth <= maxHealth / 4)
        {
            currentValor = valor25;
            if(durabilityRefillRate == 1)
                refillInterval = 3;
        }
        else if (currentHealth <= maxHealth / 2)
        {
            currentValor = valor50;
            if (durabilityRefillRate == 1)
                refillInterval = 6;
        }
        else
        {
            currentValor = valor100;
            if (durabilityRefillRate == 1)
                refillInterval = 10;
        }

        //durability starts refilling after not being in hit or blockstun for three seconds
        if (HitDetect.hitStun == 0 && HitDetect.blockStun == 0)
            durabilityRefillTimer += Time.deltaTime;
        else
            durabilityRefillTimer = 0;

        if (durabilityRefillTimer >= 3 && refillCounter == refillInterval)
        {
            durability += durabilityRefillRate;
            refillCounter = 0;
        }
        else if(durabilityRefillTimer > 3)
        {
            refillCounter++;
        }
        else
        {
            refillCounter = 0;
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
        else if (armor == 0 && durabilityRefillTimer < 3)
        {
            durability = 0;
        }
    }
}
