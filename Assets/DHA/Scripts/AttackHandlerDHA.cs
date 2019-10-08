using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandlerDHA : MonoBehaviour
{
    public Animator anim;
    public MovementHandler Move;
    public AcceptInputs Actions;
    public CharacterProperties CharProp;

    public string Light;
    public string Medium;
    public string Heavy;
    public string Break;

    float lightButton;
    float mediumButton;
    float heavyButton;
    float breakButton;
    float bufferTime = .25f;

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

    static int ID5L;
    static int ID2L;
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

    static int IDRec;
    static int IDBlitz;

    AnimatorStateInfo currentState;  

    // Start is called before the first frame update
    void Start()
    {
        ID5L = Animator.StringToHash("5L");
        ID2L = Animator.StringToHash("2L");
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

        IDRec = Animator.StringToHash("Recover");
        IDBlitz = Animator.StringToHash("Blitz");

        if (transform.parent.name == "Player1")
        {
            Light = "Light_P1";
            Medium = "Medium_P1";
            Heavy = "Heavy_P1";
            Break = "Break_P1";
        }
        else
        {
            Light = "Light_P2";
            Medium = "Medium_P2";
            Heavy = "Heavy_P2";
            Break = "Break_P2";
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0);
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

        if (Input.GetButtonDown(Light))
            lightButton = bufferTime;
        if (Input.GetButtonDown(Medium))
            mediumButton = bufferTime;
        if (Input.GetButtonDown(Heavy))
            heavyButton = bufferTime;
        if (Input.GetButtonDown(Break))
            breakButton = bufferTime;

        if (currentState.IsName("IdleStand") || currentState.IsName("IdleCrouch") || currentState.IsName("StandUp") || Move.jumped)
        {
            RefreshMoveList();
        }

        //aerial recovery
        if ((currentState.IsName("HitAir") || currentState.IsName("FallForward") || currentState.IsName("SweepHit") ||
             currentState.IsName("LaunchFall")) && Move.HitDetect.hitStun == 0 && 
            (lightButton > 0 || mediumButton > 0 || heavyButton > 0 || breakButton > 0))
        {
            anim.SetTrigger(IDRec);
            lightButton = 0;
            mediumButton = 0;
            heavyButton = 0;
            breakButton = 0;
        }

        //blitz cancel mechanic
        if (Actions.blitzCancel && Move.HitDetect.hitStun == 0 && Move.HitDetect.blockStun == 0 && heavyButton > 0 && mediumButton > 0) // && CharProp.armor >= 1)
        {
            if (!Actions.airborne)
                Move.rb.velocity = new Vector2(0, Move.rb.velocity.y);
            anim.SetTrigger(IDBlitz);
            Debug.Log("BLITZ CANCEL");
            //CharProp.armor--; //cost for executing blitz cancel
            heavyButton = 0;
            mediumButton = 0;
        }
        else if (Actions.acceptBreak && breakButton > 0 && Move.HitDetect.hitStop == 0)
        {
            if(Actions.standing)
            {
                if(Input.GetAxis(Move.Vertical) < 0)
                {
                    if(CrouchB)
                    {
                        anim.SetTrigger(ID2B);
                        CrouchB = false;
                    }
                }
                else
                {
                    if(StandB)
                    {
                        anim.SetTrigger(ID5B);
                        StandB = false;
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
            if(Actions.standing)
            {
                if(Input.GetAxis(Move.Vertical) < 0)
                {
                    if(CrouchH)
                    {
                        anim.SetTrigger(ID2H);
                        CrouchH = false;
                    }
                }
                else
                {
                    if(StandH)
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
            if(Actions.standing)
            {
                if(Input.GetAxis(Move.Vertical) < 0)
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
            if(Actions.standing)
            {
                if(Input.GetAxis(Move.Vertical) < 0)
                {
                    if(CrouchL > 0)
                    {
                        anim.SetTrigger(ID2L);
                        CrouchL--;
                    }
                }
                else
                {
                    if(StandL > 0)
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

        if(Input.GetButton(Break))
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

        Move.jumped = false;
    }
}
