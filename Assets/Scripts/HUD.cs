using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Sprite DHAHeadshot;

    public Text Player1Health;
    public Text Player2Health;
    public Text Player1Armor;
    public Text Player2Armor;
    public Text Player1Durability;
    public Text Player2Durability;
    public Image P1HealthUI;
    public Image P1RedHealth;
    public Image P2HealthUI;
    public Image P2RedHealth;
    public SpriteRenderer P1Headshot;
    public SpriteRenderer P2Headshot;

    public Animator p1Icon1;
    public Animator p1Icon2;

    public Animator p2Icon1;
    public Animator p2Icon2;

    public Image P1Dura1;
    public Image P1Dura2;
    public Image P1Dura3;
    public Image P1Dura4;

    public Image P2Dura1;
    public Image P2Dura2;
    public Image P2Dura3;
    public Image P2Dura4;

    public Animator P1Seg1;
    public Animator P1Seg2;
    public Animator P1Seg3;
    public Animator P1Seg4;

    public Animator P2Seg1;
    public Animator P2Seg2;
    public Animator P2Seg3;
    public Animator P2Seg4;

    public Text Player1Combo;
	public Text Player2Combo;
	public Image combotimer1;
	public Image combotimer2;
    public Image combogauge1;
    public Image combogauge2;

    float displayTime1;
    float displayTime2;
    int hitNum1;
    int hitNum2;

    CharacterProperties P1Prop;
    CharacterProperties P2Prop;
	HitDetector P1hit;
	HitDetector P2hit;

    static int regen;
    static int shatter;
    int flickerTimer;

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

        regen = Animator.StringToHash("Regen");
        shatter = Animator.StringToHash("Shatter");

        SetCharacterPortrait();
    }

    // Update is called once per frame
    void Update()
    {

        Player1Health.text = P1Prop.currentHealth + " / " + P1Prop.maxHealth;
        Player2Health.text = P2Prop.currentHealth + " / " + P2Prop.maxHealth;
        Player1Armor.text = "Armor:" + P1Prop.armor;
        Player2Armor.text = "Armor:" + P2Prop.armor;
        Player1Durability.text = "Durability:" + P1Prop.durability;
        Player2Durability.text = "Durability:" + P2Prop.durability;
        if (P1HealthUI.fillAmount > (float)(P1Prop.currentHealth / P1Prop.maxHealth))
            P1HealthUI.fillAmount = (float)(P1Prop.currentHealth / P1Prop.maxHealth);
        else if (P1HealthUI.fillAmount < (float)(P1Prop.currentHealth / P1Prop.maxHealth))
            P1HealthUI.fillAmount += .015f;

        if (P2HealthUI.fillAmount > (float)(P2Prop.currentHealth / P2Prop.maxHealth))
            P2HealthUI.fillAmount = (float)(P2Prop.currentHealth / P2Prop.maxHealth);
        else if (P2HealthUI.fillAmount < (float)(P2Prop.currentHealth / P2Prop.maxHealth))
            P2HealthUI.fillAmount += .015f;

        if ((float)(P1Prop.currentHealth / P1Prop.maxHealth) == 1)
            P1HealthUI.color = new Color32(93, 255, 175, 255);
        else if ((float)(P1Prop.currentHealth / P1Prop.maxHealth) <= .1f)
        {
            if (P1HealthUI.color == new Color32(255, 175, 175, 255) && flickerTimer <= 0)
            {
                P1HealthUI.color = new Color32(255, 76, 98, 255);
            }
            else if (flickerTimer <= 0)
            {
                P1HealthUI.color = new Color32(255, 175, 175, 255);
            }
        }
        else if ((float)(P1Prop.currentHealth / P1Prop.maxHealth) <= .25f)
            P1HealthUI.color = new Color32(255, 76, 98, 255);
        else if ((float)(P1Prop.currentHealth / P1Prop.maxHealth) < 1f)
            P1HealthUI.color = new Color32(255, 223, 105, 255);

        if ((float)(P2Prop.currentHealth / P2Prop.maxHealth) == 1)
            P2HealthUI.color = new Color32(93, 255, 175, 255);
        else if ((float)(P2Prop.currentHealth / P2Prop.maxHealth) <= .1f)
        {
            if (P2HealthUI.color == new Color32(255, 175, 175, 255) && flickerTimer <= 0)
            {
                P2HealthUI.color = new Color32(255, 76, 98, 255);
            }
            else if (flickerTimer <= 0)
            {
                P2HealthUI.color = new Color32(255, 175, 175, 255);
            }
        }
        else if ((float)(P2Prop.currentHealth / P2Prop.maxHealth) <= .25f)
            P2HealthUI.color = new Color32(255, 76, 98, 255);
        else if ((float)(P2Prop.currentHealth / P2Prop.maxHealth) < 1f)
            P2HealthUI.color = new Color32(255, 223, 105, 255);

        if (flickerTimer <= 0)
            flickerTimer = 4;
        else
            flickerTimer--;

        if (P1Prop.armor > 0)
        {
            if (P1Prop.armor == 4)
            {
                P1Dura1.fillAmount = 1;
                P1Dura2.fillAmount = 1;
                P1Dura3.fillAmount = 1;
                P1Dura4.fillAmount = (float)P1Prop.durability/100;

                if (!P1Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P1Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P1Seg1.SetTrigger(regen);
                if (!P1Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P1Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P1Seg2.SetTrigger(regen);
                if (!P1Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P1Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P1Seg3.SetTrigger(regen);
                if (!P1Seg4.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P1Seg4.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P1Seg4.SetTrigger(regen);
            }
            else if (P1Prop.armor == 3)
            {
                P1Dura1.fillAmount = 1;
                P1Dura2.fillAmount = 1;
                P1Dura3.fillAmount = (float)P1Prop.durability / 100;
                P1Dura4.fillAmount = 0;

                if (!P1Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P1Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P1Seg1.SetTrigger(regen);
                if (!P1Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P1Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P1Seg2.SetTrigger(regen);
                if (!P1Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P1Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P1Seg3.SetTrigger(regen);
                if (!P1Seg4.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                    P1Seg4.SetTrigger(shatter);
            }
            else if (P1Prop.armor == 2)
            {
                P1Dura1.fillAmount = 1;
                P1Dura2.fillAmount = (float)P1Prop.durability / 100;
                P1Dura3.fillAmount = 0;
                P1Dura4.fillAmount = 0;

                if (!P1Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P1Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P1Seg1.SetTrigger(regen);
                if (!P1Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P1Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P1Seg2.SetTrigger(regen);
                if (!P1Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                    P1Seg3.SetTrigger(shatter);
                if (!P1Seg4.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                    P1Seg4.SetTrigger(shatter);
            }
            else if (P1Prop.armor == 1)
            {
                P1Dura1.fillAmount = (float)P1Prop.durability / 100;
                P1Dura2.fillAmount = 0;
                P1Dura3.fillAmount = 0;
                P1Dura4.fillAmount = 0;

                if (!P1Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P1Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P1Seg1.SetTrigger(regen);
                if (!P1Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                    P1Seg2.SetTrigger(shatter);
                if (!P1Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                    P1Seg3.SetTrigger(shatter);
                if (!P1Seg4.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                    P1Seg4.SetTrigger(shatter);
            }
        }
        else
        {
            P1Dura1.fillAmount = 0;
            P1Dura2.fillAmount = 0;
            P1Dura3.fillAmount = 0;
            P1Dura4.fillAmount = 0;

            if (!P1Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                P1Seg1.SetTrigger(shatter);
            if (!P1Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                P1Seg2.SetTrigger(shatter);
            if (!P1Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                P1Seg3.SetTrigger(shatter);
            if (!P1Seg4.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                P1Seg4.SetTrigger(shatter);
        }

        if (P2Prop.armor > 0)
        {
            if (P2Prop.armor == 4)
            {
                P2Dura1.fillAmount = 1;
                P2Dura2.fillAmount = 1;
                P2Dura3.fillAmount = 1;
                P2Dura4.fillAmount = (float)P2Prop.durability / 100;

                if (!P2Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P2Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P2Seg1.SetTrigger(regen);
                if (!P2Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P2Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P2Seg2.SetTrigger(regen);
                if (!P2Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P2Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P2Seg3.SetTrigger(regen);
                if (!P2Seg4.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P2Seg4.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P2Seg4.SetTrigger(regen);
            }
            else if (P2Prop.armor == 3)
            {
                P2Dura1.fillAmount = 1;
                P2Dura2.fillAmount = 1;
                P2Dura3.fillAmount = (float)P2Prop.durability / 100;
                P2Dura4.fillAmount = 0;

                if (!P2Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P2Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P2Seg1.SetTrigger(regen);
                if (!P2Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P2Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P2Seg2.SetTrigger(regen);
                if (!P2Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P2Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P2Seg3.SetTrigger(regen);
                if (!P2Seg4.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                    P2Seg4.SetTrigger(shatter);
            }
            else if (P2Prop.armor == 2)
            {
                P2Dura1.fillAmount = 1;
                P2Dura2.fillAmount = (float)P2Prop.durability / 100;
                P2Dura3.fillAmount = 0;
                P2Dura4.fillAmount = 0;

                if (!P2Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P2Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P2Seg1.SetTrigger(regen);
                if (!P2Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P2Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P2Seg2.SetTrigger(regen);
                if (!P2Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                    P2Seg3.SetTrigger(shatter);
                if (!P2Seg4.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                    P2Seg4.SetTrigger(shatter);
            }
            else if (P2Prop.armor == 1)
            {
                P2Dura1.fillAmount = (float)P2Prop.durability / 100;
                P2Dura2.fillAmount = 0;
                P2Dura3.fillAmount = 0;
                P2Dura4.fillAmount = 0;

                if (!P2Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentStill") && !P2Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentRegen"))
                    P2Seg1.SetTrigger(regen);
                if (!P2Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                    P2Seg2.SetTrigger(shatter);
                if (!P2Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                    P2Seg3.SetTrigger(shatter);
                if (!P2Seg4.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                    P2Seg4.SetTrigger(shatter);
            }
        }
        else
        {
            P2Dura1.fillAmount = 0;
            P2Dura2.fillAmount = 0;
            P2Dura3.fillAmount = 0;
            P2Dura4.fillAmount = 0;

            if (!P2Seg1.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                P2Seg1.SetTrigger(shatter);
            if (!P2Seg2.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                P2Seg2.SetTrigger(shatter);
            if (!P2Seg3.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                P2Seg3.SetTrigger(shatter);
            if (!P2Seg4.GetCurrentAnimatorStateInfo(0).IsName("SegmentShatter"))
                P2Seg4.SetTrigger(shatter);
        }

        if (P1hit.comboCount == 0 && P2RedHealth.fillAmount > P2HealthUI.fillAmount)
            P2RedHealth.fillAmount -= .03f;//(float)(P2Prop.currentHealth / P2Prop.maxHealth);
        else if (P1hit.comboCount == 0 && P2RedHealth.fillAmount < P2HealthUI.fillAmount)
            P2RedHealth.fillAmount = P2HealthUI.fillAmount;
        if (P2hit.comboCount == 0 && P1RedHealth.fillAmount > P1HealthUI.fillAmount)
            P1RedHealth.fillAmount -= .03f; // (float)(P1Prop.currentHealth / P1Prop.maxHealth);
        else if (P2hit.comboCount == 0 && P1RedHealth.fillAmount < P1HealthUI.fillAmount)
            P1RedHealth.fillAmount = P1HealthUI.fillAmount;

        //round icons
        if (GameOver.p1Win > 0)
        {
            if (!p1Icon1.GetCurrentAnimatorStateInfo(0).IsName("IconAppear") && !p1Icon1.GetCurrentAnimatorStateInfo(0).IsName("StillIcon") && P1Prop.HitDetect.hitStop == 0)
            {
                p1Icon1.SetTrigger("Activate");
            }
            if (GameOver.p1Win > 1 && !p1Icon2.GetCurrentAnimatorStateInfo(0).IsName("IconAppear") && !p1Icon2.GetCurrentAnimatorStateInfo(0).IsName("StillIcon") && P1Prop.HitDetect.hitStop == 0)
                p1Icon2.SetTrigger("Activate");
        }
        else
        {
            if (!p1Icon1.GetCurrentAnimatorStateInfo(0).IsName("BlankIcon"))
                p1Icon1.SetTrigger("Disappear");
            if (!p1Icon2.GetCurrentAnimatorStateInfo(0).IsName("BlankIcon"))
                p1Icon2.SetTrigger("Disappear");
        }

        if (GameOver.p2Win > 0)
        {
            if (!p2Icon1.GetCurrentAnimatorStateInfo(0).IsName("IconAppear") && !p2Icon1.GetCurrentAnimatorStateInfo(0).IsName("StillIcon") && P2Prop.HitDetect.hitStop == 0)
            {
                p2Icon1.SetTrigger("Activate");
            }
            if (GameOver.p2Win > 1 && !p2Icon2.GetCurrentAnimatorStateInfo(0).IsName("IconAppear") && !p2Icon2.GetCurrentAnimatorStateInfo(0).IsName("StillIcon") && P2Prop.HitDetect.hitStop == 0)
                p2Icon2.SetTrigger("Activate");
        }
        else
        {
            if (!p2Icon1.GetCurrentAnimatorStateInfo(0).IsName("BlankIcon"))
                p2Icon1.SetTrigger("Disappear");
            if (!p2Icon2.GetCurrentAnimatorStateInfo(0).IsName("BlankIcon"))
                p2Icon2.SetTrigger("Disappear");
        }

        //player 1 combo timer

        if (P2hit.hitStun > 0)
		{
			if(P2hit.hitStun > 60)
				combotimer1.fillAmount = 1;
			else
			{
				combotimer1.fillAmount = P2hit.hitStun / 60f;
			}
            hitNum1 = P1hit.comboCount;
            combogauge1.enabled = true;

            if (P1hit.comboCount > 1)
                displayTime1 = 1;
        }
		else
		{
			combotimer1.fillAmount = 0;
            combogauge1.enabled = false;
        }

        if (displayTime1 > 0)
        {
            Player1Combo.text = hitNum1 + " hits";
            displayTime1 -= Time.fixedDeltaTime;
        }
        else
        {
            hitNum1 = 0;
            Player1Combo.text = "";
        }

        //player 2 combo timer
        if (P1hit.hitStun > 0)
		{
			if(P1hit.hitStun > 60)
				combotimer2.fillAmount = 1;
			else
				combotimer2.fillAmount = P1hit.hitStun / 60f;

            hitNum2 = P2hit.comboCount;
            combogauge2.enabled = true;
            if (P2hit.comboCount > 1)
                displayTime2 = 1;
        }
		else
		{
            combogauge2.enabled = false;
            combotimer2.fillAmount = 0;
		}

        if (displayTime2 > 0)
        {
            Player2Combo.text = hitNum2 + " hits";
            displayTime2 -= Time.fixedDeltaTime;
        }
        else
        {
            hitNum2 = 0;
            Player2Combo.text = "";
        }
    }

    public void SetCharacterPortrait()
    {
        if (P1Prop.transform.root.GetChild(0).name.Contains("Dhalia") || P1Prop.transform.root.GetChild(0).name.Contains("DHA"))
            P1Headshot.sprite = DHAHeadshot;

        if (P2Prop.transform.root.GetChild(0).name.Contains("Dhalia") || P2Prop.transform.root.GetChild(0).name.Contains("DHA"))
            P2Headshot.sprite = DHAHeadshot;    
    }
}
