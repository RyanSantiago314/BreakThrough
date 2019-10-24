using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text Player1Health;
    public Text Player2Health;
    public Text Player1Armor;
    public Text Player2Armor;
    public Text Player1Durability;
    public Text Player2Durability;
	
	public Text Player1Combo;
	public Text Player2Combo;
	public Image combotimer1;
	public Image combotimer2;

    CharacterProperties P1Prop;
    CharacterProperties P2Prop;
	HitDetector P1hit;
	HitDetector P2hit;

    // Start is called before the first frame update
    void Start()
    {
        P1Prop = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
        P2Prop = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();
		P1hit = GameObject.Find("Player1").transform.GetComponentInChildren<HitDetector>();
		P2hit = GameObject.Find("Player2").transform.GetComponentInChildren<HitDetector>();
		combotimer1.fillAmount = 0;
		combotimer2.fillAmount = 0;
		Player1Combo.text = "";
		Player2Combo.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        Player1Health.text = "HP:" + P1Prop.currentHealth;
        Player2Health.text = "HP:" + P2Prop.currentHealth;
        Player1Armor.text = "Armor:" + P1Prop.armor;
        Player2Armor.text = "Armor:" + P2Prop.armor;
        Player1Durability.text = "Durability:" + P1Prop.durability;
        Player2Durability.text = "Durability:" + P2Prop.durability;
		
		//player 1 combo timer
		
		if(P2hit.hitStun > 0 && P1hit.comboCount > 0)
		{
			if(P2hit.hitStun > 60)
				combotimer1.fillAmount = 1;
			else
			{
				combotimer1.fillAmount = P2hit.hitStun / 60f;
			}

            if(P1hit.comboCount > 1)
			    Player1Combo.text = P1hit.comboCount.ToString() + " hits";
		}
		else
		{
			Player1Combo.text = "";
			combotimer1.fillAmount = 0;
		}

		//player 2 combo timer
		if(P1hit.hitStun > 0 && P2hit.comboCount > 0)
		{
			if(P1hit.hitStun > 60)
				combotimer2.fillAmount = 1;
			else
				combotimer2.fillAmount = P1hit.hitStun / 60f;
            if(P2hit.comboCount > 1)
			    Player2Combo.text = P2hit.comboCount.ToString() + " hits";
		}
		else
		{
			Player2Combo.text = "";
			combotimer2.fillAmount = 0;
		}
		

    }
}
