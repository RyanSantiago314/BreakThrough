﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandlerDHA : MonoBehaviour
{
    public Animator anim;
    public MovementHandler Move;
    public AcceptInputs Actions;
    public CharacterProperties CharProp;
    public MaxInput MaxInput;
    public HitboxDHA Hitboxes;

    public GameObject Prefab;
    public GameObject ToasterPrefab;
    public GameObject BlitzPrefab;

    public GameObject Projectile;
    public GameObject Toaster;
    public GameObject BlitzEffect;
    SpriteRenderer BlitzImage;
    Animator BlitzWave;
    

    ColorSwapDHA colorControl;

    private string Horizontal;
    private string Vertical;

    private string Light;
    private string Medium;
    private string Heavy;
    private string Break;
    private string LM;
    private string HB;
    private string MH;
    private string LB;
    private string Select;

    float bufferTime = .25f;
    float directionBufferTime = .35f;
    float lightButton;
    float mediumButton;
    float heavyButton;
    float breakButton;
    float QCF;
    float QCB;
    float HCB;

    //directional variables using numpad notation
    public float dir1;
    public float dir2;
    public float dir3;
    public float dir4;
    public float dir6;


    private int StandL = 3;
    private bool StandM = true;
    private bool StandH = true;
    private bool StandB = true;
    private int CrouchL = 3;
    private bool CrouchM = true;
    private bool CrouchH = true;
    private bool CrouchB = true;
    private int JumpL = 3;
    private bool JumpM = true;
    private bool JumpH1 = true;
    private bool JumpH2 = true;
    private bool JumpH3 = true;
    private bool JumpH4 = true;
    private bool JumpB = true;
    private bool FL = true;
    private int FLx = 2;
    private bool FLF = true;
    private bool FB = true;
    private int BCShots = 2;

    static int ID5L;
    static int ID2L;
    static int ID6L;
    static int ID5M;
    static int ID2M;
    static int ID5H;
    static int ID5H2;
    static int ID5H3;
    static int ID5H4;
    static int ID2H;
    static int ID5B;
    static int ID2B;
    static int ID6B;
    static int BreakCharge;
    static int IDBloodBrave;
    static int IDPatissiere;
    static int IDHeadRush;
    static int IDBasketCase;
    static int IDToaster;
    static int IDSabre;

    static int runID;
    static int IDRec;
    static int IDBlitz;
    static int IDBurst;
    static int IDThrow;

    static int lowGuardID;
    static int highGuardID;
    static int airGuardID;
    static int dizzyID;
    static int KOID;
    public int dizzyTime;
    int blitzActive;

    AnimatorStateInfo currentState;  

    void Start()
    {
        ID5L = Animator.StringToHash("5L");
        ID2L = Animator.StringToHash("2L");
        ID6L = Animator.StringToHash("6L");
        ID5M = Animator.StringToHash("5M");
        ID2M = Animator.StringToHash("2M");
        ID5H = Animator.StringToHash("5H");
        ID5H2 = Animator.StringToHash("5H2");
        ID5H3 = Animator.StringToHash("5H3");
        ID5H4 = Animator.StringToHash("5H4");
        ID2H = Animator.StringToHash("2H");
        ID5B = Animator.StringToHash("5B");
        ID2B = Animator.StringToHash("2B");
        ID6B = Animator.StringToHash("6B");
        BreakCharge = Animator.StringToHash("BreakCharge");
        IDBloodBrave = Animator.StringToHash("BloodBrave");
        IDPatissiere = Animator.StringToHash("Patissiere");
        IDHeadRush = Animator.StringToHash("HeadRush");
        IDBasketCase = Animator.StringToHash("BasketCase");

        IDToaster = Animator.StringToHash("Toaster");
        IDSabre = Animator.StringToHash("JudgmentSabre");

        lowGuardID = Animator.StringToHash("LowGuard");
        highGuardID = Animator.StringToHash("HighGuard");
        airGuardID = Animator.StringToHash("AirGuard");
        dizzyID = Animator.StringToHash("Dizzy");
        KOID = Animator.StringToHash("KOed");

        runID = Animator.StringToHash("Run");
        IDRec = Animator.StringToHash("Recover");
        IDBlitz = Animator.StringToHash("Blitz");
        IDBurst = Animator.StringToHash("BurstBreaker");
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
            Select = "Select_P1";
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
            Select = "Select_P2";
        }

        colorControl = transform.GetChild(0).GetComponent<ColorSwapDHA>();

        Projectile = Instantiate(Prefab, new Vector3(0, 5, -3), Quaternion.identity, transform.root);
        Toaster = Instantiate(ToasterPrefab, new Vector3(0, -5, -3), Quaternion.identity, transform.root);
        BlitzEffect = Instantiate(BlitzPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, transform.root);
        BlitzImage = BlitzEffect.transform.GetChild(0).GetComponent<SpriteRenderer>();
        BlitzWave = BlitzEffect.transform.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        anim.ResetTrigger(IDRec);

        if (StartText.startReady && !GameOver.lockInputs)
        {
            Move.playing = true;
            Move.opponent.GetComponent<MovementHandler>().playing = true;
        }
        if (!StartText.startReady && GameOver.lockInputs)
        {
            Move.playing = false;
            Move.opponent.GetComponent<MovementHandler>().playing = false;
        }

        if (Move.HitDetect.hitStun > 0)
        {
            anim.ResetTrigger(ID5L);
            anim.ResetTrigger(ID2L);
            anim.ResetTrigger(ID5M);
            anim.ResetTrigger(ID2M);
            anim.ResetTrigger(ID5H);
            anim.ResetTrigger(ID5H2);
            anim.ResetTrigger(ID5H3);
            anim.ResetTrigger(ID5H4);
            anim.ResetTrigger(ID6L);
            anim.ResetTrigger(ID2H);
            anim.ResetTrigger(ID5B);
            anim.ResetTrigger(ID2B);
            anim.ResetTrigger(IDThrow);
            anim.ResetTrigger(IDBloodBrave);
            anim.ResetTrigger(IDPatissiere);
            anim.ResetTrigger(IDHeadRush);
            anim.ResetTrigger(IDToaster);
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
            anim.ResetTrigger(ID5H2);
            anim.ResetTrigger(ID5H3);
            anim.ResetTrigger(ID5H4);
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
            anim.ResetTrigger(ID6B);
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
        if (HCB > 0)
            HCB -= Time.deltaTime;


        //record buttons pressed
        if (MaxInput.GetButtonDown(Light))
            lightButton = bufferTime;
        if (MaxInput.GetButtonDown(Medium))
            mediumButton = bufferTime;
        if (MaxInput.GetButtonDown(Heavy))
            heavyButton = bufferTime;
        if (MaxInput.GetButtonDown(Break))
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
        HCBCheck();

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
        if ((dizzyTime == 0 && anim.GetBool(dizzyID)) || anim.GetBool(KOID))
        {
            dizzyTime = 300;
        }
        else if (!anim.GetBool(dizzyID) || CharProp.currentHealth == CharProp.maxHealth)
        {
            dizzyTime = 0;
        }

        if (dizzyTime > 0)
        {
            anim.SetBool(dizzyID, true);
            dizzyTime--;
            if (MaxInput.GetButtonDown(Light))
            {
                dizzyTime -= 5;
            }
            if (MaxInput.GetButtonDown(Medium))
            {
                dizzyTime -= 5;
            }
            if (MaxInput.GetButtonDown(Heavy))
            {
                dizzyTime -= 5;
            }
            if (MaxInput.GetButtonDown(Break))
            {
                dizzyTime -= 5;
            }
        }

        if (dizzyTime <= 0 && anim.GetBool(dizzyID))
        {
            anim.SetBool(dizzyID, false);
            CharProp.refill = true;
            CharProp.comboTimer = 0;
        }

        //aerial recovery, press a button after hitstun ends
        if ((currentState.IsName("HitAir") || currentState.IsName("FallForward") || currentState.IsName("SweepHit") || currentState.IsName("LaunchTransition") ||
             currentState.IsName("LaunchFall")) && Move.HitDetect.hitStun == 0 && Move.transform.position.y > 1.4f &&
            (lightButton > 0 || mediumButton > 0 || heavyButton > 0 || breakButton > 0))
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

        
        if (blitzActive > 0)
            blitzActive--;
        else if (blitzActive == 1)
            Hitboxes.ClearHitBox();

        if (Actions.acceptBurst && Move.HitDetect.hitStop == 0 && breakButton > 0 && heavyButton > 0 && mediumButton > 0 && lightButton > 0)
        {
            //anim.SetTrigger(IDBurst);
            //Move.HitDetect.hitStun = 0;
            //Actions.blitzed = 0;

            lightButton = 0;
            mediumButton = 0;
            heavyButton = 0;
            breakButton = 0;
        }
        //blitz cancel mechanic, return to neutral position to extend combos, cancel recovery, make character safe, etc. at the cost of one hit of armor
        else if (Actions.blitzCancel && Move.HitDetect.hitStop == 0 && Move.HitDetect.hitStun == 0 && Move.HitDetect.blockStun == 0 && 
            heavyButton > 0 && mediumButton > 0 && Mathf.Abs(heavyButton - mediumButton) <= .1f && CharProp.armor >= 1)
        {
            anim.SetTrigger(IDBlitz);
            BlitzWave.SetTrigger(IDBlitz);
            Hitboxes.BlitzCancel();
            Actions.landingLag = 0;
            Move.HitDetect.KnockBack = Vector2.zero;

            anim.SetBool(runID, false);
            BlitzImage.sprite = anim.gameObject.GetComponent<SpriteRenderer>().sprite;
            BlitzImage.color = new Color(BlitzImage.color.r, BlitzImage.color.g, BlitzImage.color.b, .75f);
            BlitzEffect.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            BlitzEffect.transform.rotation = transform.rotation;

            if (Actions.airborne && MaxInput.GetAxis(Vertical) < 0)
                Move.rb.AddForce(new Vector2(0, -3), ForceMode2D.Impulse);

            //cost for executing blitz cancel
            CharProp.armor--;
            if (CharProp.armor > 0)
                CharProp.durability = 50;
            else
                CharProp.durability = 80;
            blitzActive = 5;
            CharProp.durabilityRefillTimer = 0;
            heavyButton = 0;
            mediumButton = 0;
        }    
        // basic throw performed by pressing both light and break attack
        else if (Actions.acceptMove && lightButton > 0 && breakButton > 0 && Move.HitDetect.hitStop == 0)
        {
            if(Actions.standing)
            {
                anim.SetTrigger(IDThrow);
                if (dir4 == directionBufferTime)
                    Actions.backThrow = true;
                else
                    Actions.backThrow = false;

                Actions.throwTech = true;
            }
        }
        else if (Actions.acceptSuper && lightButton > 0 && mediumButton > 0 && Move.HitDetect.hitStop == 0 && QCB > 0 && CharProp.armor >= 2 && Actions.standing)
        {
            Move.jumping = 0;
            // Judgment Sabre super attack, executed by doing a QCB and pressing L and M together
            anim.SetTrigger(IDSabre);
            CharProp.armor -= 2;
            if (CharProp.armor > 0)
                CharProp.durability = 50;
            else
                CharProp.durability = 70;
            CharProp.durabilityRefillTimer = 0;
            lightButton = 0;
            mediumButton = 0;
            QCB = 0;
        }
        else if (Actions.acceptSuper && heavyButton > 0 && breakButton > 0 && Move.HitDetect.hitStop == 0 && QCF > 0 && CharProp.armor >= 2 && Actions.standing && !Toaster.activeSelf)
        {
            Move.jumping = 0;
            // Toaster super attack, executed by doing a QCF and pressing H and B
            anim.SetTrigger(IDToaster);
            CharProp.armor -= 2;
            if (CharProp.armor > 0)
                CharProp.durability = 50;
            else
                CharProp.durability = 70;
            CharProp.durabilityRefillTimer = 0;
            breakButton = 0;
            heavyButton = 0;
            QCF = 0;
        }
        else if (Actions.acceptSpecial && breakButton > 0 && Move.HitDetect.hitStop == 0 && QCF > 0 && Actions.standing)
        {
            Move.jumping = 0;
            // Basket Case special attack, executed by doing a QCF and pressing B, can be used up to twice in succession
            anim.SetTrigger(IDBasketCase);
            Actions.TurnAroundCheck();
            breakButton = 0;
            QCF = 0;
            BCShots--;
        }
        else if (Actions.acceptSuper && currentState.IsName("BasketCaseFullShoot") && breakButton > 0 && Move.HitDetect.hitStop == 0 && QCF > 0 && Actions.standing && BCShots > 0)
        {
            Move.jumping = 0;
            // Basket special attack, executed by doing a QCF and pressing B (second shot)
            anim.SetTrigger(IDBasketCase);
            breakButton = 0;
            QCF = 0;
            BCShots--;
        }
        else if (Actions.acceptSpecial && heavyButton > 0 && Move.HitDetect.hitStop == 0 && QCB > 0)
        {
            Move.jumping = 0;
            // Blood Brave special attack, executed by doing a QCB and pressing H
            anim.SetTrigger(IDBloodBrave);
            Actions.TurnAroundCheck();
            heavyButton = 0;
            QCB = 0;
        }
        else if (Actions.acceptSpecial && mediumButton > 0 && Move.HitDetect.hitStop == 0 && HCB > 0 && Actions.standing)
        {
            Move.jumping = 0;
            // Head Rush special attack, executed by doing a HCB and pressing M
            anim.SetTrigger(IDHeadRush);
            mediumButton = 0;
            HCB = 0;
        }
        else if (Actions.acceptSpecial && lightButton > 0 && Move.HitDetect.hitStop == 0 && QCF > 0 && Actions.standing && !Projectile.activeSelf)
        {
            Move.jumping = 0;
            // Patissiere projectile special attack, executed by doing a QCF and pressing L
            anim.SetTrigger(IDPatissiere);
            Actions.TurnAroundCheck();
            lightButton = 0;
            QCF = 0;
        }
        else if (Actions.acceptBreak && breakButton > 0 && Move.HitDetect.hitStop == 0)
        {
            //break attacks
            if(Actions.standing)
            {
                if(MaxInput.GetAxis(Vertical) < 0)
                {
                    if(CrouchB)
                    {
                        anim.SetTrigger(ID2B);
                        CrouchB = false;
                        Actions.TurnAroundCheck();
                    }
                }
                else if (dir6 == directionBufferTime)
                {
                    if (FB)
                    {
                        anim.SetTrigger(ID6B);
                        FB = false;
                        anim.SetBool(runID, false);
                    }
                }
                else
                {
                    if(StandB)
                    {
                        anim.SetTrigger(ID5B);
                        StandB = false;
                        Actions.TurnAroundCheck();
                    }
                }
            }
            else
            {
                if(JumpB)
                {
                    anim.SetTrigger(ID5B);
                    JumpB = false;
                }
            }
            breakButton = 0;
        }
        else if (Actions.acceptHeavy && heavyButton > 0 && Move.HitDetect.hitStop == 0)
        {
            //heavy attacks
            if(Actions.standing)
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
                if (JumpH1)
                {
                    anim.SetTrigger(ID5H);
                    JumpH1 = false;
                }
               else if (JumpH2)
                {
                    anim.SetTrigger(ID5H2);
                    JumpH2 = false;
                }
                else if (JumpH3)
                {
                    anim.SetTrigger(ID5H3);
                    JumpH3 = false;
                }
                else if (JumpH4)
                {
                    anim.SetTrigger(ID5H4);
                    JumpH4 = false;
                }

            }
            heavyButton = 0;
        }
        else if (Actions.acceptMedium && mediumButton > 0 && Move.HitDetect.hitStop == 0)
        {
            //medium attacks
            if(Actions.standing)
            {
                if(MaxInput.GetAxis(Vertical) < 0)
                {
                    if(CrouchM)
                    {
                        anim.SetTrigger(ID2M);
                        CrouchM = false;
                    }
                }
                else
                {
                    if(StandM)
                    {
                        anim.SetTrigger(ID5M);
                        StandM = false;
                    }
                }
            }
            else
            {
                if(JumpM)
                {
                    anim.SetTrigger(ID5M);
                    JumpM = false;
                }
            }
            mediumButton = 0;
        }
        else if (Actions.acceptLight && lightButton > 0 && Move.HitDetect.hitStop == 0)
        {
            //light attacks
            if(Actions.standing)
            {
                if(MaxInput.GetAxis(Vertical) < 0 && !(currentState.IsName("6Lx") || currentState.IsName("6LF")))
                {
                    if(CrouchL > 0)
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
                    else if (FLx == 2)
                    {
                        anim.SetTrigger(ID5H2);
                        FLx--;
                    }
                    else if (FLx == 1)
                    {
                        anim.SetTrigger(ID5H3);
                        FLx--;
                    }
                    else if (FLF)
                    {
                        anim.SetTrigger(ID5H4);
                        FLF = false;
                    }
                }
                else
                {
                    if (currentState.IsName("6Lx"))
                    {
                        if (FLx > 0)
                        {
                            if (FLx == 2)
                            {
                                anim.SetTrigger(ID5H2);
                            }
                            else if (FLx < 2)
                            {
                                anim.SetTrigger(ID5H3);
                            }
                            FLx--;
                        }
                        else if (FLF)
                        {
                            anim.SetTrigger(ID5H4);
                            FLF = false;
                        }
                    }
                    else if (StandL > 0)
                    {
                        anim.SetTrigger(ID5L);
                        StandL--;
                    }
                }
            }

            else
            {
                if(JumpL > 0)
                {
                    anim.SetTrigger(ID5L);
                    JumpL--;
                }
            }
            lightButton = 0;
        }

        // DHA character specific property, can charge Break attacks to make them more powerful
        if(MaxInput.GetButton(Break))
        {
            anim.SetBool(BreakCharge, true);
        }
        else
        {
            anim.SetBool(BreakCharge, false);
        }

        
    }

    void RefreshMoveList()
    {
        StandL = 3;
        StandM = true;
        StandH = true;
        StandB = true;
        CrouchL = 3;
        CrouchM = true;
        CrouchH = true;
        CrouchB = true;
        JumpL = 3;
        JumpM = true;
        JumpH1 = true;
        JumpH2 = true;
        JumpH3 = true;
        JumpH4 = true;
        JumpB = true;
        FL = true;
        FLx = 2;
        FLF = true;
        FB = true;
        BCShots = 2;
    }

    void QCFCheck()
    {
        //check if the player has executed a quarter circle forward with the control stick
        if(dir2 > 0 && dir3 > 0 && dir6 > 0 && dir6 > dir3 && dir3 > dir2)
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

    void HCBCheck()
    {
        //check if the player has executed a half circle back with the control stick
        if (dir6 > 0 && (dir1 > 0 || dir2 > 0 || dir3 > 0) && dir4 > 0 && ((dir4 > dir2 && dir2 > dir6)|| (dir4 > dir1 && dir1 > dir6) || (dir4 > dir3 && dir3 > dir6)))
        {
            HCB = .15f;
            dir6 = 0;
            dir3 = 0;
            dir2 = 0;
            dir1 = 0;
            dir4 = 0;
        }
    }
}
