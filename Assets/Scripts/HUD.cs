using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Sprite DHAHeadshot;

    public Image P1HealthUI;
    public Image P1RedHealth;
    public Image P2HealthUI;
    public Image P2RedHealth;
    public SpriteRenderer P1Headshot;
    public SpriteRenderer P2Headshot;
    public Text P1Name;
    public Text P2Name;

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
    public Text Player1Hits;
    public Text Player2Hits;
    public Image combotimer1;
    public Image combotimer2;
    public Image combogauge1;
    public Image combogauge2;

    float displayTime1;
    float displayTime2;
    int hitNum1;
    int hitNum2;
    int hitNum1Memo;
    int hitNum2Memo;
    bool P1TrueCombo = true;
    bool P2TrueCombo = true;

    Vector2 P1HitsPos;
    Vector2 P1ComboPos;

    Vector2 P2HitsPos;
    Vector2 P2ComboPos;

    CharacterProperties P1Prop;
    CharacterProperties P2Prop;
    HitDetector P1hit;
    HitDetector P2hit;

    static int regen;
    static int shatter;
    int flickerTimer;

    bool P1ResolveWhite;
    bool P2ResolveWhite;
    Color32 P1ResolveColor;
    Color32 P2ResolveColor;
    float P1Transition;
    float P2Transition;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
        {
            P1Prop = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
            P2Prop = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();
            P1hit = GameObject.Find("Player1").transform.GetComponentInChildren<HitDetector>();
            P2hit = GameObject.Find("Player2").transform.GetComponentInChildren<HitDetector>();
        }
        else
        {
            P1Prop = GameObject.Find("Player2").transform.GetComponentInChildren<CharacterProperties>();
            P2Prop = GameObject.Find("Player1").transform.GetComponentInChildren<CharacterProperties>();
            P1hit = GameObject.Find("Player2").transform.GetComponentInChildren<HitDetector>();
            P2hit = GameObject.Find("Player1").transform.GetComponentInChildren<HitDetector>();
        }

        combotimer1.fillAmount = 0;
        combotimer2.fillAmount = 0;
        Player1Combo.text = "";
        Player2Combo.text = "";

        regen = Animator.StringToHash("Regen");
        shatter = Animator.StringToHash("Shatter");

        P1HitsPos = Player1Hits.rectTransform.anchoredPosition;
        P1ComboPos = Player1Combo.rectTransform.anchoredPosition;

        P2HitsPos = Player2Hits.rectTransform.anchoredPosition;
        P2ComboPos = Player2Combo.rectTransform.anchoredPosition;

        SetCharacterPortrait();
    }

    // Update is called once per frame
    void Update()
    {
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
                P1Dura4.fillAmount = (float)P1Prop.durability / 100;

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

        //causes the resolve gauge to flash when refilling, also changes color based on how many segments
        if (P1Prop.armor == 4)
            P1ResolveColor = new Color32(255, 205, 70, 255);
        else if (P1Prop.armor >= 2)
            P1ResolveColor = new Color32(0, 143, 255, 255);
        else if (P1Prop.armor <= 1)
        {
            P1ResolveColor = new Color32(255, 0, 85, 255);
        }

        if (P1Prop.durabilityRefillTimer >= 3 || (P1Prop.armor == 4 && P1Prop.durability == 100))
        {
            if (P1ResolveWhite)
                P1Transition -= .04f;
            else
                P1Transition += .04f;

            if (P1Transition >= 1.2f)
                P1ResolveWhite = true;
            else if (P1Transition <= -.2f)
                P1ResolveWhite = false;
        }
        else if (P1Prop.armor <= 1)
        {
            if (flickerTimer == 4 && P1Transition == 0)
                P1Transition = 1;
            else if (flickerTimer == 4 && P1Transition > 0)
                P1Transition = 0;
        }
        else
            P1Transition = 0;

        P1Dura1.color = Color32.Lerp(P1ResolveColor, Color.white, P1Transition);
        P1Dura2.color = Color32.Lerp(P1ResolveColor, Color.white, P1Transition);
        P1Dura3.color = Color32.Lerp(P1ResolveColor, Color.white, P1Transition);
        P1Dura4.color = Color32.Lerp(P1ResolveColor, Color.white, P1Transition);

        if (P2Prop.armor == 4)
            P2ResolveColor = new Color32(255, 205, 70, 255);
        else if (P2Prop.armor >= 2)
            P2ResolveColor = new Color32(0, 143, 255, 255);
        else
            P2ResolveColor = new Color32(255, 0, 85, 255);

        if (P2Prop.durabilityRefillTimer >= 3 || (P2Prop.armor == 4 && P2Prop.durability == 100))
        {
            if (P2ResolveWhite)
                P2Transition -= .04f;
            else
                P2Transition += .04f;

            if (P2Transition >= 1.2f)
                P2ResolveWhite = true;
            else if (P2Transition <= -.2f)
                P2ResolveWhite = false;
        }
        else if (P2Prop.armor <= 1)
        {
            if (flickerTimer == 4 && P2Transition == 0)
                P2Transition = 1;
            else if (flickerTimer == 4 && P2Transition > 0)
                P2Transition = 0;
        }
        else
            P2Transition = 0;

        P2Dura1.color = Color32.Lerp(P2ResolveColor, Color.white, P2Transition);
        P2Dura2.color = Color32.Lerp(P2ResolveColor, Color.white, P2Transition);
        P2Dura3.color = Color32.Lerp(P2ResolveColor, Color.white, P2Transition);
        P2Dura4.color = Color32.Lerp(P2ResolveColor, Color.white, P2Transition);

        //round icons
        if (RoundManager.p1Win > 0)
        {
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
            {
                if (!p1Icon1.GetCurrentAnimatorStateInfo(0).IsName("IconAppear") && !p1Icon1.GetCurrentAnimatorStateInfo(0).IsName("StillIcon") && P1Prop.HitDetect.hitStop == 0)
                {
                    p1Icon1.SetTrigger("Activate");
                }
                if (RoundManager.p1Win > 1 && !p1Icon2.GetCurrentAnimatorStateInfo(0).IsName("IconAppear") && !p1Icon2.GetCurrentAnimatorStateInfo(0).IsName("StillIcon") && P1Prop.HitDetect.hitStop == 0)
                {
                    p1Icon2.SetTrigger("Activate");
                }
            }
            else
            {
                if (!p2Icon1.GetCurrentAnimatorStateInfo(0).IsName("IconAppear") && !p2Icon1.GetCurrentAnimatorStateInfo(0).IsName("StillIcon") && P1Prop.HitDetect.hitStop == 0)
                {
                    p2Icon1.SetTrigger("Activate");
                }
                if (RoundManager.p1Win > 1 && !p2Icon2.GetCurrentAnimatorStateInfo(0).IsName("IconAppear") && !p2Icon2.GetCurrentAnimatorStateInfo(0).IsName("StillIcon") && P1Prop.HitDetect.hitStop == 0)
                {
                    p2Icon2.SetTrigger("Activate");
                }
            }
        }
        else
        {
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P1Side == "Left")
            {
                if (!p1Icon1.GetCurrentAnimatorStateInfo(0).IsName("BlankIcon"))
                    p1Icon1.SetTrigger("Disappear");
                if (!p1Icon2.GetCurrentAnimatorStateInfo(0).IsName("BlankIcon"))
                    p1Icon2.SetTrigger("Disappear");
            }
            else
            {
                if (!p2Icon1.GetCurrentAnimatorStateInfo(0).IsName("BlankIcon"))
                    p2Icon1.SetTrigger("Disappear");
                if (!p2Icon2.GetCurrentAnimatorStateInfo(0).IsName("BlankIcon"))
                    p2Icon2.SetTrigger("Disappear");
            }
        }

        if (RoundManager.p2Win > 0)
        {
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Left")
            {
                if (!p1Icon1.GetCurrentAnimatorStateInfo(0).IsName("IconAppear") && !p1Icon1.GetCurrentAnimatorStateInfo(0).IsName("StillIcon") && P2Prop.HitDetect.hitStop == 0)
                {
                    p1Icon1.SetTrigger("Activate");
                }
                if (RoundManager.p2Win > 1 && !p1Icon2.GetCurrentAnimatorStateInfo(0).IsName("IconAppear") && !p1Icon2.GetCurrentAnimatorStateInfo(0).IsName("StillIcon") && P2Prop.HitDetect.hitStop == 0)
                {
                    p1Icon2.SetTrigger("Activate");
                }
            }
            else
            {
                if (!p2Icon1.GetCurrentAnimatorStateInfo(0).IsName("IconAppear") && !p2Icon1.GetCurrentAnimatorStateInfo(0).IsName("StillIcon") && P2Prop.HitDetect.hitStop == 0)
                {
                    p2Icon1.SetTrigger("Activate");
                }
                if (RoundManager.p2Win > 1 && !p2Icon2.GetCurrentAnimatorStateInfo(0).IsName("IconAppear") && !p2Icon2.GetCurrentAnimatorStateInfo(0).IsName("StillIcon") && P2Prop.HitDetect.hitStop == 0)
                {
                    p2Icon2.SetTrigger("Activate");
                }
            }
        }
        else
        {
            if (GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().P2Side == "Left")
            {
                if (!p1Icon1.GetCurrentAnimatorStateInfo(0).IsName("BlankIcon"))
                    p1Icon1.SetTrigger("Disappear");
                if (!p1Icon2.GetCurrentAnimatorStateInfo(0).IsName("BlankIcon"))
                    p1Icon2.SetTrigger("Disappear");
            }
            else
            {
                if (!p2Icon1.GetCurrentAnimatorStateInfo(0).IsName("BlankIcon"))
                    p2Icon1.SetTrigger("Disappear");
                if (!p2Icon2.GetCurrentAnimatorStateInfo(0).IsName("BlankIcon"))
                    p2Icon2.SetTrigger("Disappear");
            }
        }

        //player 1 combo timer
        if (P2hit.hitStun > 0)
        {
            if (P2hit.hitStun > 60)
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
            Player1Combo.text = hitNum1.ToString();
            Player1Hits.text = "HITS";
            displayTime1 -= Time.fixedDeltaTime;
        }
        else
        {
            hitNum1 = 0;
            hitNum1Memo = 0;
            Player1Combo.text = "";
            Player1Hits.text = "";
        }
        //shakes the combocounter everytime a hit is added to the combo, combocounter also changes color based on whether or not the combo is true

        if (P1hit.comboCount == 0)
        {
            P1TrueCombo = true;
            combotimer1.color = Color.white;
        }
        else if (P1hit.comboCount > 0 && hitNum1Memo == hitNum1 && P2hit.hitStun == 0 && P2hit.Actions.airborne && !P2hit.anim.GetCurrentAnimatorStateInfo(0).IsName("Launch")
            && !P2hit.anim.GetCurrentAnimatorStateInfo(0).IsName("WallStick"))
        {
            P1TrueCombo = false;
        }

        if (hitNum1Memo < hitNum1)
        {
            Player1Combo.rectTransform.anchoredPosition = new Vector2(P1ComboPos.x + Random.Range(-150f, 150f), P1ComboPos.y + Random.Range(-150f, 150f));
            Player1Hits.rectTransform.anchoredPosition = new Vector2(P1HitsPos.x + Random.Range(-150f, 150f), P1HitsPos.y + Random.Range(-150f, 150f));
            if (P1TrueCombo)
            {
                Player1Combo.color = new Color32(185, 0, 30, 255);
                Player1Hits.color = new Color32(230, 230, 230, 255);
                combotimer1.color = Color.white;
            }
            else
            {
                Player1Combo.color = new Color32(50, 50, 50, 255);
                Player1Hits.color = new Color32(100, 100, 100, 255);
                combotimer1.color = new Color32(70, 250, 255, 255);
            }
            hitNum1Memo = hitNum1;
        }
        

        Player1Combo.rectTransform.anchoredPosition = Vector2.Lerp(Player1Combo.rectTransform.anchoredPosition, P1ComboPos, 40*Time.deltaTime);
        Player1Hits.rectTransform.anchoredPosition = Vector2.Lerp(Player1Hits.rectTransform.anchoredPosition, P1HitsPos, 40*Time.deltaTime);

        //player 2 combo timer
        if (P1hit.hitStun > 0)
        {
            if (P1hit.hitStun > 60)
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
            Player2Combo.text = hitNum2.ToString();
            Player2Hits.text = "HITS";
            displayTime2 -= Time.fixedDeltaTime;
        }
        else
        {
            hitNum2 = 0;
            hitNum2Memo = 0;
            Player2Combo.text = "";
            Player2Hits.text = "";
        }

        if (P2hit.comboCount == 0)
        {
            P2TrueCombo = true;
            combotimer2.color = Color.white;
        }
        else if (P2hit.comboCount > 0 && hitNum2Memo == hitNum2 && P1hit.hitStun == 0 && P1hit.Actions.airborne && !P1hit.anim.GetCurrentAnimatorStateInfo(0).IsName("Launch")
            && !P1hit.anim.GetCurrentAnimatorStateInfo(0).IsName("WallStick"))
        {
            P2TrueCombo = false;
        }

        if (hitNum2Memo < hitNum2)
        {
            Player2Combo.rectTransform.anchoredPosition = new Vector2(P2ComboPos.x + Random.Range(-150f, 150f), P2ComboPos.y + Random.Range(-150f, 150f));
            Player2Hits.rectTransform.anchoredPosition = new Vector2(P2HitsPos.x + Random.Range(-150f, 150f), P2HitsPos.y + Random.Range(-150f, 150f));
            if (P2TrueCombo)
            {
                Player2Combo.color = new Color32(185, 0, 30, 255);
                Player2Hits.color = new Color32(230, 230, 230, 255);
                combotimer2.color = Color.white;
            }
            else
            {
                Player2Combo.color = new Color32(50, 50, 50, 255);
                Player2Hits.color = new Color32(100, 100, 100, 255);
                combotimer2.color = new Color32(70, 250, 255, 255);
            }
            hitNum2Memo = hitNum2;
        }
    }

    public void SetCharacterPortrait()
    {
        if (P1Prop.transform.root.GetChild(0).name.Contains("Dhalia") || P1Prop.transform.root.GetChild(0).name.Contains("DHA"))
        {
            P1Headshot.sprite = DHAHeadshot;
            P1Name.text = "Dhalia Thorne";
        }

        if (P2Prop.transform.root.GetChild(0).name.Contains("Dhalia") || P2Prop.transform.root.GetChild(0).name.Contains("DHA"))
        {
            P2Headshot.sprite = DHAHeadshot;
            P2Name.text = "Dhalia Thorne";
        }
    }
}
