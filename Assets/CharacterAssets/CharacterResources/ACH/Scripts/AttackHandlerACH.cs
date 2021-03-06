﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandlerACH : MonoBehaviour
{
    public Animator anim;
    public MovementHandler Move;
    public AcceptInputs Actions;
    public CharacterProperties CharProp;
    public MaxInput MaxInput;
    public HitboxACH Hitboxes;

    public GameObject HCPrefab;
    public GameObject LHSlidePrefab;
    public GameObject LHPrefab;
    public GameObject SFPrefab;
    public GameObject GenesisPrefab;
    public GameObject FMPrefab;
    public GameObject BlitzPrefab;

    public GameObject HCWave;
    public GameObject LHSlide;
    public GameObject LHWave;
    public GameObject SFWave;
    public GameObject GenesisEdge;
    public GameObject BlitzEffect;
    public GameObject ForsythiaReticle;
    SpriteRenderer BlitzImage;
    Animator BlitzWave;


    ColorSwapACH colorControl;

    public string Horizontal;
    public string Vertical;

    public string Light;
    public string Medium;
    public string Heavy;
    public string Break;
    public string LM;
    public string HB;
    public string MH;
    public string LB;

    float bufferTime = .25f;
    float directionBufferTime = .3f;
    float lightButton;
    float mediumButton;
    float heavyButton;
    float breakButton;
    float QCF;
    float QCB;
    float HCB;
    float HCF;
    float DP;

    //directional variables using numpad notation
    public float dir1;
    public float dir2;
    public float dir3;
    public float dir4;
    public float dir6;


    private bool StandL = true;
    private bool StandM = true;
    private bool StandH = true;
    private bool StandB = true;
    private int CrouchL = 3;
    private bool CrouchM = true;
    private bool CrouchH = true;
    private bool CrouchB = true;
    private int JumpL = 3;
    private bool JumpM = true;
    private bool JumpH = true;
    private bool JumpB = true;
    private bool FL = true;
    private bool FM = true;
    private bool DFB = true;
    private bool Starfall = true;
    private bool HClimber = true;
    private bool LHell = true;
    private bool TLeap = true;
    private bool GEdge = true;

    static int ID5L;
    static int ID2L;
    static int ID5M;
    static int ID2M;
    static int ID5H;
    static int ID2H;
    static int ID5B;
    static int ID2B;
    static int ID3B;
    static int ID6L;
    static int ID6M;
    static int IDTowerLeapL;
    static int IDTowerLeapM;
    static int IDHeavenClimberH;
    static int IDHeavenClimberB;
    static int IDLevelHell;
    static int IDStarfall;
    static int IDGenesis;
    static int BreakCharge;
    static int IDForsythia;

    /*
    static int IDHeadRush;
    static int IDBasketCase;
    static int IDToaster;
    static int IDSabre;*/

    static int runID;
    static int IDRec;
    static int IDBlitz;
    static int IDThrow;

    static int lowGuardID;
    static int highGuardID;
    static int airGuardID;
    static int dizzyID;
    static int KOID;
    public float dizzyTime;
    float blitzActive;

    AnimatorStateInfo currentState;

    void Start()
    {
        ID5L = Animator.StringToHash("5L");
        ID2L = Animator.StringToHash("2L");
        ID5M = Animator.StringToHash("5M");
        ID2M = Animator.StringToHash("2M");
        ID5H = Animator.StringToHash("5H");
        ID2H = Animator.StringToHash("2H");
        ID5B = Animator.StringToHash("5B");
        ID2B = Animator.StringToHash("2B");
        ID3B = Animator.StringToHash("3B");
        ID6L = Animator.StringToHash("6L");
        ID6M = Animator.StringToHash("6M");
        IDTowerLeapL = Animator.StringToHash("TowerLeapL");
        IDTowerLeapM = Animator.StringToHash("TowerLeapM");
        IDHeavenClimberH = Animator.StringToHash("HeavenClimberH");
        IDHeavenClimberB = Animator.StringToHash("HeavenClimberB");
        IDLevelHell = Animator.StringToHash("LevelHell");
        BreakCharge = Animator.StringToHash("BreakCharge");
        IDStarfall = Animator.StringToHash("Starfall");
        IDGenesis = Animator.StringToHash("GenesisEdge");
        IDForsythia = Animator.StringToHash("Forsythia");
        /*
        IDPatissiere = Animator.StringToHash("Patissiere");
        IDHeadRush = Animator.StringToHash("HeadRush");
        IDBasketCase = Animator.StringToHash("BasketCase");

        
        IDSabre = Animator.StringToHash("JudgmentSabre");*/

        lowGuardID = Animator.StringToHash("LowGuard");
        highGuardID = Animator.StringToHash("HighGuard");
        airGuardID = Animator.StringToHash("AirGuard");
        dizzyID = Animator.StringToHash("Dizzy");
        KOID = Animator.StringToHash("KOed");

        runID = Animator.StringToHash("Run");
        IDRec = Animator.StringToHash("Recover");
        IDBlitz = Animator.StringToHash("Blitz");
        IDThrow = Animator.StringToHash("Throw");

        if (transform.parent.name == "Player1")
        {

            Horizontal = "Horizontal_P1";
            Vertical = "Vertical_P1";

            Light = "Square_P1";
            Medium = "Triangle_P1";
            Heavy = "Circle_P1";
            Break = "Cross_P1";
            LM = "R1_P1";
            HB = "R2_P1";
            LB = "L1_P1";
            MH = "L2_P1";
        }
        else
        {
            Horizontal = "Horizontal_P2";
            Vertical = "Vertical_P2";

            Light = "Square_P2";
            Medium = "Triangle_P2";
            Heavy = "Circle_P2";
            Break = "Cross_P2";
            LM = "R1_P2";
            HB = "R2_P2";
            LB = "L1_P2";
            MH = "L2_P2";
        }

        colorControl = transform.GetChild(0).GetComponent<ColorSwapACH>();

        HCWave = Instantiate(HCPrefab, new Vector3(0, -10, -3), Quaternion.identity, transform.root);
        HCWave.SetActive(false);

        LHSlide = Instantiate(LHSlidePrefab, new Vector3(0, -10, -3), Quaternion.identity, transform.root);
        LHSlide.SetActive(false);

        LHWave = Instantiate(LHPrefab, new Vector3(0, -10, -3), Quaternion.identity, transform.root);
        LHWave.SetActive(false);

        SFWave = Instantiate(SFPrefab, new Vector3(0, -10, -3), Quaternion.identity, transform.root);
        SFWave.SetActive(false);

        GenesisEdge = Instantiate(GenesisPrefab, new Vector3(0, -5, -3), Quaternion.identity, transform.root);

        ForsythiaReticle = Instantiate(FMPrefab, new Vector3(0, -5, -3), Quaternion.identity, transform.root);

        BlitzEffect = Instantiate(BlitzPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, transform.root);
        BlitzImage = BlitzEffect.transform.GetChild(0).GetComponent<SpriteRenderer>();
        BlitzWave = BlitzEffect.transform.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        anim.ResetTrigger(IDRec);

        if (Move.HitDetect.hitStun > 0)
        {
            anim.ResetTrigger(ID5L);
            anim.ResetTrigger(ID2L);
            anim.ResetTrigger(ID6L);
            anim.ResetTrigger(ID5M);
            anim.ResetTrigger(ID2M);
            anim.ResetTrigger(ID5H);
            anim.ResetTrigger(ID2H);
            anim.ResetTrigger(ID5B);
            anim.ResetTrigger(ID2B);
            anim.ResetTrigger(IDThrow);
            anim.ResetTrigger(IDTowerLeapL);
            anim.ResetTrigger(IDTowerLeapM);
            anim.ResetTrigger(IDHeavenClimberH);
            anim.ResetTrigger(IDHeavenClimberB);
            anim.ResetTrigger(IDLevelHell);
            anim.ResetTrigger(IDStarfall);
            anim.ResetTrigger(IDGenesis);
            anim.ResetTrigger(IDForsythia);
        }

        if (lightButton > 0)
        {
            lightButton -= Time.deltaTime;
        }
        else
        {
            lightButton = 0;
            anim.ResetTrigger(ID5L);
            anim.ResetTrigger(ID2L);
            anim.ResetTrigger(ID6L);
        }

        if (mediumButton > 0)
        {
            mediumButton -= Time.deltaTime;
        }
        else
        {
            mediumButton = 0;
            anim.ResetTrigger(ID5M);
            anim.ResetTrigger(ID2M);
        }

        if (heavyButton > 0)
        {
            heavyButton -= Time.deltaTime;
        }
        else
        {
            heavyButton = 0;
            anim.ResetTrigger(ID5H);
            anim.ResetTrigger(ID2H);
        }

        if (breakButton > 0)
        {
            breakButton -= Time.deltaTime;
        }
        else
        {
            breakButton = 0;
            anim.ResetTrigger(ID5B);
            anim.ResetTrigger(ID2B);
            anim.ResetTrigger(ID3B);
        }

        if (dir1 > 0)
            dir1 -= Time.deltaTime;
        if (dir2 > 0)
            dir2 -= Time.deltaTime;
        if (dir3 > 0)
            dir3 -= Time.deltaTime;
        if (dir4 > 0)
            dir4 -= Time.deltaTime;
        if (dir6 > 0)
            dir6 -= Time.deltaTime;
        if (QCF > 0)
            QCF -= Time.deltaTime;
        if (QCB > 0)
            QCB -= Time.deltaTime;
        if (HCF > 0)
            HCF -= Time.deltaTime;
        if (HCB > 0)
            HCB -= Time.deltaTime;
        if (DP > 0)
            DP -= Time.deltaTime;


        //record buttons pressed
        if (MaxInput.GetButtonDown(Light) && !anim.GetCurrentAnimatorStateInfo(0).IsName("ForsythiaAim"))
            lightButton = bufferTime;
        if (MaxInput.GetButtonDown(Medium))
            mediumButton = bufferTime;
        if (MaxInput.GetButtonDown(Heavy))
            heavyButton = bufferTime;
        if (MaxInput.GetButtonDown(Break) && !anim.GetCurrentAnimatorStateInfo(0).IsName("ForsythiaAim"))
            breakButton = bufferTime;
        if (MaxInput.GetButtonDown(LM))
        {
            lightButton = bufferTime;
            mediumButton = bufferTime;
        }
        if (MaxInput.GetButtonDown(HB))
        {
            heavyButton = bufferTime;
            breakButton = bufferTime;
        }
        if (MaxInput.GetButtonDown(LB))
        {
            lightButton = bufferTime;
            breakButton = bufferTime;
        }
        if (MaxInput.GetButtonDown(MH))
        {
            mediumButton = bufferTime;
            heavyButton = bufferTime;
        }
        QCFCheck();
        QCBCheck();
        HCFCheck();
        HCBCheck();
        DPCheck();

        //record directional input
        //float dir# corresponds to numpad notation for character facing to the right
        //special moves' directional input for DHA will never use 7 8 or 9, and 5 is the neutral position so no variables for those directions is necessary,
        /*           up
                     ^
                     |
                   7 8 9
         left <--- 4 5 6 ---> right
                   1 2 3
                     |
                     v
                    down
         */
        // pressing left on the d pad or stick, considered backward if facing right, considered forward if facing left
        if (MaxInput.GetAxis(Horizontal) < 0 && MaxInput.GetAxis(Vertical) < 0)
        {
            if (Move.transform.position.x < Move.opponent.position.x)
                // 1 : pressing down-back
                dir1 = directionBufferTime;
            else if (Move.transform.position.x > Move.opponent.position.x)// 3 : pressing down-forward
                dir3 = directionBufferTime;
        }
        else if (MaxInput.GetAxis(Horizontal) > 0 && MaxInput.GetAxis(Vertical) < 0)
        {
            if (Move.transform.position.x < Move.opponent.position.x)
                // pressing down-forward
                dir3 = directionBufferTime;
            else if (Move.transform.position.x > Move.opponent.position.x)
                // pressing down-back
                dir1 = directionBufferTime;
        }
        else if (MaxInput.GetAxis(Horizontal) < 0)
        {
            if (Move.transform.position.x < Move.opponent.position.x)
                // pressing back if facing right
                dir4 = directionBufferTime;
            else if (Move.transform.position.x > Move.opponent.position.x)
                // pressing forward if facing left
                dir6 = directionBufferTime;
        }
        // pressing right on the d pad/stick, considered forward if facing right, considered backward if facing left
        else if (MaxInput.GetAxis(Horizontal) > 0)
        {
            if (Move.transform.position.x < Move.opponent.position.x)
                //forward if facing right
                dir6 = directionBufferTime;
            else if (Move.transform.position.x > Move.opponent.position.x)
                //back if facing left
                dir4 = directionBufferTime;
        }
        else if (MaxInput.GetAxis(Vertical) < 0)
        {
            //only pressing down
            dir2 = directionBufferTime;
        }

        if (Actions.acceptMove || currentState.IsName("StandUp") || currentState.IsName("Jump"))
        {
            //refresh possible moves when in certain states
            RefreshMoveList();
        }

        //dizzy state, mash buttons to get out of it faster
        if ((dizzyTime <= 0 && anim.GetBool(dizzyID)) || anim.GetBool(KOID))
        {
            dizzyTime = 5;
        }
        else if (!anim.GetBool(dizzyID))
        {
            dizzyTime = 0;
        }

        if (dizzyTime > 0)
        {
            anim.SetBool(dizzyID, true);
            dizzyTime -= Time.deltaTime;
            if (MaxInput.GetButtonDown(Light))
            {
                dizzyTime -= .08f;
            }
            if (MaxInput.GetButtonDown(Medium))
            {
                dizzyTime -= .08f;
            }
            if (MaxInput.GetButtonDown(Heavy))
            {
                dizzyTime -= .08f;
            }
            if (MaxInput.GetButtonDown(Break))
            {
                dizzyTime -= .08f;
            }
        }

        if (dizzyTime <= 0 && anim.GetBool(dizzyID))
        {
            anim.SetBool(dizzyID, false);
            CharProp.refill = true;
            CharProp.comboTimer = 0;
        }

        //aerial recovery, press a button after hitstun ends
        if((currentState.IsName("HitAir") || currentState.IsName("FallForward") || currentState.IsName("SweepHit") || currentState.IsName("LaunchTransition") ||
            currentState.IsName("LaunchFall") || currentState.IsName("Unstick")) && Move.HitDetect.hitStun <= 0 &&
            Move.transform.position.y > 1.1f && (lightButton > 0 || mediumButton > 0 || heavyButton > 0 || breakButton > 0))
        {
            anim.SetTrigger(IDRec);
        }

        if (currentState.IsName("AirRecovery"))
        {
            lightButton = 0;
            mediumButton = 0;
            heavyButton = 0;
            breakButton = 0;
        }


        if (blitzActive > 0 && Hitboxes.HitDetect.OpponentDetector.Actions.blitzed <= 0)
        {
            Hitboxes.BlitzCancel();
            blitzActive -= Time.deltaTime;

        }
        else if (Hitboxes.HitDetect.OpponentDetector.Actions.blitzed > 0)
            blitzActive = 0;

        //blitz cancel mechanic, return to neutral position to extend combos, cancel recovery, make character safe, etc. at the cost of one hit of armor
        if ((Actions.blitzCancel && Actions.blitzed <= 0 && Move.HitDetect.hitStun <= 0 && Move.HitDetect.blockStun <= 0 && CharProp.armor >= 1) &&
            Move.HitDetect.hitStop <= 0 && heavyButton > 0 && mediumButton > 0 && Mathf.Abs(heavyButton - mediumButton) <= .1f)
        {
            RefreshMoveList();
            BlitzWave.SetTrigger(IDBlitz);
            Hitboxes.BlitzCancel();
            Actions.landingLag = 0;

            ForsythiaReticle.SetActive(false);
            anim.SetBool(runID, false);
            BlitzImage.sprite = anim.gameObject.GetComponent<SpriteRenderer>().sprite;
            BlitzImage.color = new Color(BlitzImage.color.r, BlitzImage.color.g, BlitzImage.color.b, .75f);
            BlitzEffect.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            BlitzEffect.transform.rotation = transform.rotation;

            anim.SetTrigger(IDBlitz);
            if (Actions.airborne && MaxInput.GetAxis(Vertical) < 0)
            {
                Move.rb.velocity = Vector2.zero;
                Move.rb.AddForce(new Vector2(0, -4), ForceMode2D.Impulse);
            }
            else if (Actions.airborne && MaxInput.GetAxis(Horizontal) < 0)
            {
                if (Move.rb.velocity.y < 0)
                    Move.rb.velocity = Vector2.zero;
                else
                    Move.rb.velocity = new Vector2(0, Move.rb.velocity.y);
                Move.rb.AddForce(new Vector2(-2.7f, 0), ForceMode2D.Impulse);
            }
            else if (Actions.airborne && MaxInput.GetAxis(Horizontal) > 0)
            {
                if (Move.rb.velocity.y < 0)
                    Move.rb.velocity = Vector2.zero;
                else
                    Move.rb.velocity = new Vector2(0, Move.rb.velocity.y);
                Move.rb.AddForce(new Vector2(2.7f, 0), ForceMode2D.Impulse);
            }

            //cost for executing blitz cancel
            CharProp.armor--;
            CharProp.durability = 70;
            blitzActive = 5f / 60f;
            CharProp.durabilityRefillTimer = 0;
            heavyButton = 0;
            mediumButton = 0;
        }
        else if (!(Actions.landingLag > 0 && Actions.standing))
        {
            // basic throw performed by pressing both light and break attack
            if ((Actions.acceptMove || currentState.IsName("Brake")) && lightButton > 0 && breakButton > 0 && Move.HitDetect.hitStop <= 0)
            {
                Hitboxes.ClearHitBox();
                if (Actions.standing)
                {
                    anim.SetTrigger(IDThrow);
                    if (dir4 == directionBufferTime)
                        Actions.backThrow = true;
                    else
                        Actions.backThrow = false;

                    Actions.throwTech = true;
                }
            }
            else if (Actions.acceptSuper && lightButton > 0 && mediumButton > 0 && Move.HitDetect.hitStop <= 0 && QCF > 0 && CharProp.armor >= 2 && Actions.standing)
            {
                Move.jumping = 0;
                Hitboxes.ClearHitBox();
                // Forsythia Marduk super attack, executed by doing a QCF and pressing L and M together
                anim.SetTrigger(IDForsythia);
                CharProp.armor -= 2;
                CharProp.durability = 70;
                CharProp.durabilityRefillTimer = 0;
                lightButton = 0;
                mediumButton = 0;
                QCF = 0;
            }
            else if (Actions.acceptSpecial && breakButton > 0 && GEdge && Move.HitDetect.hitStop <= 0 && HCF > 0 && !GenesisEdge.activeSelf)
            {
                Move.jumping = 0;
                Hitboxes.ClearHitBox();
                // Genesis Edge special attack, executed by doing a HCF and pressing B
                anim.SetTrigger(IDGenesis);
                Actions.TurnAroundCheck();
                breakButton = 0;
                HCF = 0;
                GEdge = false;
            }
            else if (Actions.acceptSpecial && breakButton > 0 && Starfall && Move.HitDetect.hitStop <= 0 && QCB > 0 && Actions.airborne && Hitboxes.HitDetect.Actions.Move.transform.position.y > 1.5f)
            {
                Move.jumping = 0;
                Hitboxes.ClearHitBox();
                // Starfall special attack, executed by doing a QCB and pressing B
                anim.SetTrigger(IDStarfall);
                Actions.TurnAroundCheck();
                breakButton = 0;
                QCB = 0;
                Starfall = false;
            }
            else if (Actions.acceptSpecial && mediumButton > 0 && Move.HitDetect.hitStop <= 0 && QCF > 0 && !Actions.airborne)
            {
                Move.jumping = 0;
                Hitboxes.ClearHitBox();
                // Level Hell special attack, executed by doing a QCF and pressing M
                anim.SetTrigger(IDLevelHell);
                Actions.TurnAroundCheck();
                mediumButton = 0;
                QCF = 0;
                LHell = false;
            }
            else if (Actions.acceptSpecial && (lightButton > 0 || mediumButton > 0) && Move.HitDetect.hitStop <= 0 && QCB > 0 && !Actions.airborne)
            {
                Move.jumping = 0;
                Hitboxes.ClearHitBox();
                // Tower Leap special attack, executed by doing a QCB and pressing L or M, L version leaps higher, M version leaps farther forward
                if (lightButton > 0)
                    anim.SetTrigger(IDTowerLeapL);
                else
                    anim.SetTrigger(IDTowerLeapM);
                Actions.TurnAroundCheck();
                lightButton = 0;
                mediumButton = 0;
                QCB = 0;
                TLeap = false;
            }
            else if (Actions.acceptSpecial && HClimber && (heavyButton > 0 || breakButton > 0) && Move.HitDetect.hitStop <= 0 && DP > 0)
            {
                Move.jumping = 0;
                Hitboxes.ClearHitBox();
                // Heaven Climber special attack, executed by doing a DP and pressing H or B
                if (Actions.airborne || heavyButton > 0)
                    anim.SetTrigger(IDHeavenClimberH);
                else
                    anim.SetTrigger(IDHeavenClimberB);
                Actions.TurnAroundCheck();
                heavyButton = 0;
                breakButton = 0;
                DP = 0;
                HClimber = false;
            }
            else if (Actions.acceptBreak && breakButton > 0 && Move.HitDetect.hitStop <= 0)
            {
                //break attacks
                if (Actions.standing)
                {
                    if (MaxInput.GetAxis(Vertical) < 0)
                    {
                        if (CrouchB)
                        {
                            anim.SetTrigger(ID2B);
                            CrouchB = false;
                            Actions.TurnAroundCheck();
                            Hitboxes.ClearHitBox();
                        }
                    }
                    else if (dir3 == directionBufferTime)
                    {
                        if (DFB)
                        {
                            anim.SetTrigger(ID3B);
                            DFB = false;
                            anim.SetBool(runID, false);
                            Hitboxes.ClearHitBox();
                        }
                    }
                    else
                    {
                        if (StandB)
                        {
                            anim.SetTrigger(ID5B);
                            StandB = false;
                            Actions.TurnAroundCheck();
                            Hitboxes.ClearHitBox();
                        }
                    }
                }
                else
                {
                    if (JumpB)
                    {
                        anim.SetTrigger(ID5B);
                        JumpB = false;
                        Hitboxes.ClearHitBox();
                    }
                }
                Hitboxes.ClearHitBox();
                breakButton = 0;
            }
            else if (Actions.acceptHeavy && heavyButton > 0 && Move.HitDetect.hitStop <= 0)
            {
                //heavy attacks
                if (Actions.standing)
                {
                    if (MaxInput.GetAxis(Vertical) < 0)
                    {
                        if (CrouchH)
                        {
                            anim.SetTrigger(ID2H);
                            CrouchH = false;
                        }
                    }
                    else
                    {
                        if (StandH)
                        {
                            anim.SetTrigger(ID5H);
                            StandH = false;
                        }
                    }
                }
                else
                {
                    if (JumpH)
                    {
                        anim.SetTrigger(ID5H);
                        JumpH = false;
                    }

                }
                Hitboxes.ClearHitBox();
                heavyButton = 0;
            }
            else if (Actions.acceptHeavy && mediumButton > 0 && Move.HitDetect.hitStop <= 0 && dir6 == directionBufferTime && FM && Actions.standing)
            {
                anim.SetTrigger(ID6M);
                FM = false;
                anim.SetBool(runID, false);
            }
            else if (Actions.acceptMedium && mediumButton > 0 && Move.HitDetect.hitStop <= 0)
            {
                //medium attacks
                if (Actions.standing)
                {
                    if (MaxInput.GetAxis(Vertical) < 0)
                    {
                        if (CrouchM)
                        {
                            anim.SetTrigger(ID2M);
                            CrouchM = false;
                        }
                    }
                    else
                    {
                        if (StandM)
                        {
                            anim.SetTrigger(ID5M);
                            StandM = false;
                        }
                    }
                }
                else
                {
                    if (JumpM)
                    {
                        anim.SetTrigger(ID5M);
                        JumpM = false;
                    }
                }
                Hitboxes.ClearHitBox();
                mediumButton = 0;
            }
            else if (Actions.acceptLight && lightButton > 0 && Move.HitDetect.hitStop <= 0)
            {
                //light attacks
                if (Actions.standing)
                {
                    if (MaxInput.GetAxis(Vertical) < 0)
                    {
                        if (CrouchL > 0)
                        {
                            anim.SetTrigger(ID2L);
                            CrouchL--;
                        }
                    }
                    else if (dir6 == directionBufferTime)
                    {
                        if (FL)
                        {
                            anim.SetTrigger(ID6L);
                            FL = false;
                            anim.SetBool(runID, false);
                        }
                    }
                    else
                    {
                        if (StandL)
                        {
                            if (CrouchL < 3)
                                CrouchL = 0;
                            anim.SetTrigger(ID5L);
                            StandL = false;
                        }
                    }
                }
                else
                {
                    if (JumpL > 0)
                    {
                        anim.SetTrigger(ID5L);
                        JumpL--;
                    }
                }
                Hitboxes.ClearHitBox();
                lightButton = 0;
            }
        }

        // ACH character specific, can charge or delay certain special attacks
        if (MaxInput.GetButton(Break))
        {
            anim.SetBool(BreakCharge, true);

            if (currentState.IsName("StarfallDelay"))
            {
                Hitboxes.HitDetect.rb.gravityScale = .35f * Hitboxes.HitDetect.Actions.originalGravity;
            }
                
        }
        else
        {
            anim.SetBool(BreakCharge, false);
        }


    }

    void RefreshMoveList()
    {
        StandL = true;
        StandM = true;
        StandH = true;
        StandB = true;
        CrouchL = 3;
        CrouchM = true;
        CrouchH = true;
        CrouchB = true;
        JumpL = 3;
        JumpM = true;
        JumpH = true;
        JumpB = true;
        FL = true;
        FM = true;
        DFB = true;
        Starfall = true;
        HClimber = true;
        LHell = true;
        TLeap = true;
        GEdge = true;
    }

    void QCFCheck()
    {
        //check if the player has executed a quarter circle forward with the control stick
        if (dir2 > 0 && dir3 > 0 && dir6 > 0 && dir6 > dir3 && dir3 > dir2)
        {
            QCF = .15f;
            dir2 = 0;
            dir3 = 0;
            dir6 = 0;
        }
    }

    void QCBCheck()
    {
        //check if the player has executed a quarter circle back with the control stick
        if (dir2 > 0 && dir1 > 0 && dir4 > 0 && dir4 > dir1 && dir1 > dir2)
        {
            QCB = .15f;
            dir2 = 0;
            dir1 = 0;
            dir4 = 0;
        }
    }

    void DPCheck()
    {
        //check if the player has executed a quarter circle back with the control stick
        if (dir2 > 0 && dir3 > 0 && dir6 > 0 && dir3 > dir2 && dir2 > dir6)
        {
            DP = .15f;
        }
    }

    void HCFCheck()
    {
        //check if the player has executed a half circle forward with the control stick
        if (dir6 > 0 && (dir1 > 0 || dir2 > 0 || dir3 > 0) && dir4 > 0 && ((dir4 < dir2 && dir2 < dir6) || (dir4 < dir1 && dir1 < dir6) || (dir4 < dir3 && dir3 < dir6)))
        {
            HCF = .15f;
            dir6 = 0;
            dir3 = 0;
            dir2 = 0;
            dir1 = 0;
            dir4 = 0;
        }
    }

    void HCBCheck()
    {
        //check if the player has executed a half circle back with the control stick
        if (dir6 > 0 && (dir1 > 0 || dir2 > 0 || dir3 > 0) && dir4 > 0 && ((dir4 > dir2 && dir2 > dir6) || (dir4 > dir1 && dir1 > dir6) || (dir4 > dir3 && dir3 > dir6)))
        {
            HCB = .15f;
            dir6 = 0;
            dir3 = 0;
            dir2 = 0;
            dir1 = 0;
            dir4 = 0;
        }
    }

    public void switchActions(bool switchPlayer)
    {
        if (switchPlayer)
        {
            if (transform.parent.name == "Player1")
            {
                Horizontal = "Horizontal_P2";
                Vertical = "Vertical_P2";

                Light = "Square_P2";
                Medium = "Triangle_P2";
                Heavy = "Circle_P2";
                Break = "Cross_P2";
                LM = "R1_P2";
                HB = "R2_P2";
                LB = "L1_P2";
                MH = "L2_P2";
            }
            else
            {
                Horizontal = "Horizontal_P1";
                Vertical = "Vertical_P1";

                Light = "Square_P1";
                Medium = "Triangle_P1";
                Heavy = "Circle_P1";
                Break = "Cross_P1";
                LM = "R1_P1";
                HB = "R2_P1";
                LB = "L1_P1";
                MH = "L2_P1";
            }
        }
        else
        {
            if (transform.parent.name == "Player1")
            {
                Horizontal = "Horizontal_P1";
                Vertical = "Vertical_P1";

                Light = "Square_P1";
                Medium = "Triangle_P1";
                Heavy = "Circle_P1";
                Break = "Cross_P1";
                LM = "R1_P1";
                HB = "R2_P1";
                LB = "L1_P1";
                MH = "L2_P1";
            }
            else
            {
                Horizontal = "Horizontal_P2";
                Vertical = "Vertical_P2";

                Light = "Square_P2";
                Medium = "Triangle_P2";
                Heavy = "Circle_P2";
                Break = "Cross_P2";
                LM = "R1_P2";
                HB = "R2_P2";
                LB = "L1_P2";
                MH = "L2_P2";
            }
        }
    }
}
