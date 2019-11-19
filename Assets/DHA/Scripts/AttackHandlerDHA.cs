using System.Collections;
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
    public GameObject Projectile;
    

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
    static int BreakCharge;
    static int IDBloodBrave;
    static int IDPatissiere;

    static int runID;
    static int IDRec;
    static int IDBlitz;
    static int IDThrow;

    static int lowGuardID;
    static int highGuardID;
    static int airGuardID;
    static int dizzyID;
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
        BreakCharge = Animator.StringToHash("BreakCharge");
        IDBloodBrave = Animator.StringToHash("BloodBrave");
        IDPatissiere = Animator.StringToHash("Patissiere");

        lowGuardID = Animator.StringToHash("LowGuard");
        highGuardID = Animator.StringToHash("HighGuard");
        airGuardID = Animator.StringToHash("AirGuard");
        dizzyID = Animator.StringToHash("Dizzy");

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
        Projectile.gameObject.SetActive(false);
    }

    void Update()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        anim.ResetTrigger(IDRec);
        if (Input.GetButtonDown(Select))
        {
            CharProp.currentHealth = CharProp.maxHealth;
            CharProp.armor = 4;
            CharProp.durability = 100;
            Move.OpponentProperties.currentHealth = Move.OpponentProperties.maxHealth;
            Move.OpponentProperties.armor = 4;
            Move.OpponentProperties.durability = 100;

            //will take out later
            Move.playing = true;
            Move.opponent.GetComponent<MovementHandler>().playing = true;
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
        if (MaxInput.GetAxis(Horizontal) < 0) 
        {
            if(MaxInput.GetAxis(Vertical) < 0) // diagonal directions
            {
                if (Move.facingRight) 
                    // 1 : pressing down-back
                    dir1 = directionBufferTime;
                else // 3 : pressing down-forward
                    dir3 = directionBufferTime;
            }
            else if (Move.facingRight) 
                // pressing back if facing right
                dir4 = directionBufferTime;
            else 
                // pressing forward if facing left
                dir6 = directionBufferTime;
        }
        // pressing right on the d pad/stick, considered forward if facing right, considered backward if facing left
        else if (MaxInput.GetAxis(Horizontal) > 0) 
        {
            if (MaxInput.GetAxis(Vertical) < 0)
            {
                if (Move.transform.position.x < Move.opponent.position.x)
                    // pressing down-forward
                    dir3 = directionBufferTime;
                else if (Move.transform.position.x > Move.opponent.position.x)
                    // pressing down-back
                    dir1 = directionBufferTime;
            }
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

        if (Actions.acceptMove || currentState.IsName("StandUp") || Move.jumped)
        {
            //refresh possible moves when in certain states
            RefreshMoveList();
        }

        //dizzy state, mash buttons to get out of it faster
        if (dizzyTime == 0 && anim.GetBool(dizzyID))
        {
            dizzyTime = 300;
        }
        else if (!anim.GetBool(dizzyID))
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
            CharProp.comboTimer = 400;
        }

        //aerial recovery, press a button after hitstun ends
        if ((currentState.IsName("HitAir") || currentState.IsName("FallForward") || currentState.IsName("SweepHit") || currentState.IsName("LaunchTransition") ||
             currentState.IsName("LaunchFall")) && Move.HitDetect.hitStun == 0 && Move.transform.position.y > 1.4f &&
            (lightButton > 0 || mediumButton > 0 || heavyButton > 0 || breakButton > 0))
        {
            anim.SetTrigger(IDRec);
            lightButton = 0;
            mediumButton = 0;
            heavyButton = 0;
            breakButton = 0;
        }

        //blitz cancel mechanic, return to neutral position to extend combos, cancel recovery, make character safe, etc. at the cost of one hit of armor
        if (blitzActive > 0)
            blitzActive--;
        else if (blitzActive == 1)
            Hitboxes.ClearHitBox();

        if (Actions.blitzCancel && Move.HitDetect.hitStop == 0 && Move.HitDetect.hitStun == 0 && Move.HitDetect.blockStun == 0 && 
            heavyButton > 0 && mediumButton > 0 && CharProp.armor >= 2)
        {
            anim.SetTrigger(IDBlitz);
            Hitboxes.BlitzCancel();

            //cost for executing blitz cancel
            CharProp.armor -= 2;
            if (CharProp.armor > 0)
                CharProp.durability = 50;
            else
                CharProp.durability = 0;
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
            }
        }
        else if (Actions.acceptSpecial && breakButton > 0 && Move.HitDetect.hitStop == 0 && QCF > 0)
        {
            // Blood Brave special attack, executed by doing a QCF and pressing B
            anim.SetTrigger(IDBloodBrave);
            Actions.TurnAroundCheck();
            breakButton = 0;
            QCF = 0;
        }
        else if (Actions.acceptSpecial && lightButton > 0 && Move.HitDetect.hitStop == 0 && QCF > 0 && Actions.standing && !Projectile.activeSelf)
        {
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

        Move.jumped = false;
    }

    void QCFCheck()
    {
        //check if the player has executed a quarter circle forward with the control stick
        if(dir2 > 0 && dir3 > 0 && dir6 > 0 && dir6 > dir3 && dir3 > dir2)
        {
            QCF = directionBufferTime;
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
            QCB = directionBufferTime;
            dir2 = 0;
            dir1 = 0;
            dir4 = 0;
        }
    }
}
