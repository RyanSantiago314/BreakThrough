using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperties : MonoBehaviour
{
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

    int durabilityRefillRate;

    public HitDetector HitDetect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (durability > 100)
        {
            if(armor < 4)
            {
                durability -= 100;
                armor++;
            }
            else
            {
                durability = 100;
            }
        }
        else if (durability < 0 && armor > 0)
        {
            armor--;
            durability = 100;
        }
        else if (durability < 0)
        {
            durability = 0;
        }

        if(HitDetect.hitStun > 0)
        {
            armor = 0;
            durability = 0;
            comboTimer += .6f;
        }
    }
}
