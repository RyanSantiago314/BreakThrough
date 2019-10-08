using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public AcceptInputs Actions;

    AnimatorStateInfo currentState;
    public Vector2 currentVelocity;

    public int damage;
    public float initialProration;
    public Vector2 potentialKnockBack;
    public Vector2 potentialAirKnockBack;
    public int potentialHitStun;
    public int potentialHitStop;
    public int attackLevel;
    public string guard;
    public int durabilityDamage;
    public int armorDamage;
    
    public bool allowLight;
    public bool allowMedium;
    public bool allowHeavy;
    public bool allowBreak;
    public bool allowSpecial;
    public bool allowSuper;
    public bool jumpCancellable;

    Vector2 KnockBack;
    public int hitStop = 0;
    public int hitStun = 0;
    public int blockStun = 0;

    public bool grab = false;
    public bool commandGrab = false;
    public bool piercing = false;
    public bool launch = false;
    public bool crumple = false;
    public bool sweep = false;
    public bool forceCrouch = false;
    public bool shatter = false;
    public bool allowWallStick = false;
    public bool allowGroundBounce = false;

    HitDetector OpponentDetector;

    bool allowHit = false;
    int collideCount = 0;
    public bool hit = false;
    public int comboCount;
    float startProration;
    float comboProration;

    static int HiGuard;
    static int LoGuard;
    static int AirGuard;

    static int animSpeedID;
    static int hitStunID;
    static int blockStunID;
    static int clashID;
    static int deflectID;
    static int parryID;
    static int successID;
    static int hitID;   
    static int hitBodyID;
    static int hitLegsID;
    static int launchID;
    static int crumpleID;
    static int sweepID;
    static int shatterID;
    static int armorHitID;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        OpponentDetector = Actions.Move.OpponentProperties.HitDetect;

        LoGuard = Animator.StringToHash("LowGuard");
        HiGuard = Animator.StringToHash("HighGuard");
        AirGuard = Animator.StringToHash("AirGuard");

        animSpeedID = Animator.StringToHash("AnimSpeed");
        hitStunID = Animator.StringToHash("HitStun");
        blockStunID = Animator.StringToHash("BlockStun");
        clashID = Animator.StringToHash("Clash");
        deflectID = Animator.StringToHash("Deflected");
        parryID = Animator.StringToHash("Parry");
        successID = Animator.StringToHash("HitSuccess");
        hitID = Animator.StringToHash("Hit");   
        hitBodyID = Animator.StringToHash("HitBody");
        hitLegsID = Animator.StringToHash("HitLegs");
        launchID = Animator.StringToHash("Launch");
        crumpleID = Animator.StringToHash("Crumple");
        sweepID = Animator.StringToHash("Sweep");
        shatterID = Animator.StringToHash("Shatter");
        armorHitID = Animator.StringToHash("ArmorHit");
    }

    // Update is called once per frame
    void Update()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0); 

        if ((OpponentDetector.hitStun == 0 && !OpponentDetector.Actions.airborne) || currentState.IsName("AirRecovery"))
        {
            comboCount = 0;
            initialProration = 1;
        }
        if(currentState.IsName("Launch"))
            anim.SetBool(launchID, false);
        else if (currentState.IsName("SweepHit"))
            anim.SetBool(sweepID, false);

        if(hitStun > 0 && hitStop == 0)
        {
            Actions.DisableAll();
            if(!currentState.IsName("GroundBounce") && !currentState.IsName("Crumple"))
                hitStun--;
            anim.SetInteger(hitStunID, hitStun);
        }
        if(blockStun > 0 && hitStop == 0)
        {
            Actions.Guard();
            blockStun--;
            anim.SetInteger(blockStunID, blockStun);
        }

        if (hitStop > 0)
        {
            anim.SetFloat(animSpeedID, 0);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            hitStop--;
        }
        else 
        {
            anim.SetFloat(animSpeedID, 1.0f);

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if(currentState.IsName("Deflected") || currentState.IsName("WallStick"))
            {
                rb.velocity = Vector2.zero;
                if (currentState.IsName("Deflected"))
                    rb.gravityScale = .5f;
            }
            else if (Actions.shattered && hitStun > 0)
            {
                rb.gravityScale = .7f;
                anim.SetFloat(animSpeedID, .75f);
            }
            else
            {
                rb.gravityScale = Actions.gravScale;
            }

            if (currentVelocity != Vector2.zero)
            {
                rb.velocity = currentVelocity;
                currentVelocity = Vector2.zero;
            }

            if (KnockBack != Vector2.zero)
            {
                if ((hitStun > 0 || blockStun > 0) && Actions.airborne)
                    rb.velocity = Vector2.zero;
                
                rb.AddForce(KnockBack, ForceMode2D.Impulse);
                KnockBack = Vector2.zero;
            }
        }

        if (hitStun == 0 && blockStun == 0 && hitStop == 0)
        {
            anim.SetInteger(hitStunID, 0);
            anim.SetInteger(blockStunID, 0);
            anim.ResetTrigger(clashID);
            anim.ResetTrigger(deflectID);
            anim.ResetTrigger(parryID);
            anim.ResetTrigger(successID);
            anim.ResetTrigger(hitID);
            anim.ResetTrigger(hitBodyID);
            anim.ResetTrigger(hitLegsID);
            anim.SetBool(launchID, false);
            anim.SetBool(sweepID, false);
            anim.ResetTrigger(shatterID);
            anim.ResetTrigger(armorHitID);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        collideCount++;
        currentVelocity = rb.velocity;
        if (allowHit && other.gameObject.transform.parent == Actions.Move.opponent && other.CompareTag("HitBox"))
        {
            OpponentDetector.Actions.shattered = false;
            //use for counter stances
            if ((attackLevel - OpponentDetector.attackLevel) > 1)
            {
                ApplyHitStop(0);
                Debug.Log("DEFLECTED!");
                OpponentDetector.anim.SetTrigger(deflectID);
                anim.SetTrigger(parryID);
                Actions.jumpCancel = true;
                Contact();
            }
            else if ((attackLevel - OpponentDetector.attackLevel) <= 1)
            {
                Debug.Log("Clash!");
                ApplyHitStop(0);
                anim.SetTrigger(clashID);
                //no knockback on clashes
                Clash();
            }
            allowHit = false;
        }
        else if (allowHit && other.gameObject.transform.parent.parent == Actions.Move.opponent && potentialHitStun > 0)
        {

            if ((guard == "Mid" && (OpponentDetector.anim.GetBool(LoGuard) || OpponentDetector.anim.GetBool(HiGuard) || OpponentDetector.anim.GetBool(AirGuard))) ||
                (guard == "Low" && (OpponentDetector.anim.GetBool(LoGuard) || OpponentDetector.anim.GetBool(AirGuard))) ||
                (guard == "Overhead" && OpponentDetector.anim.GetBool(HiGuard) || OpponentDetector.anim.GetBool(AirGuard)))
            {
                OpponentDetector.anim.SetTrigger("Blocked");
                OpponentDetector.blockStun = potentialHitStun - potentialHitStun/10;
                if (OpponentDetector.blockStun > 30)
                    OpponentDetector.blockStun = 30;
                anim.SetInteger(blockStunID, blockStun);
                ApplyHitStop(0);
                //what to do if an attack is blocked
                //mid can be guarded by any guard, lows must be guarded low, overheads must be guarded high
                //deal durability/chip damage equaling 20% of base damage
                //apply pushback to both by half of horizontal knockback value
                KnockBack = potentialKnockBack * new Vector2(.7f, 0);
                OpponentDetector.KnockBack = potentialKnockBack * new Vector2(.4f, 0);

                if(OpponentDetector.anim.GetBool(AirGuard))
                {
                    //apply special knockback to airborne guards
                    if (potentialAirKnockBack != Vector2.zero)
                    {
                        //guarding characters should never be spiked toward the ground
                        if(potentialAirKnockBack.y < 0)
                            OpponentDetector.KnockBack = potentialAirKnockBack * new Vector2(.4f, 0) + new Vector2(0, .3f);
                        else
                            OpponentDetector.KnockBack = potentialAirKnockBack * new Vector2(.4f, .5f);
                    }
                    else
                        OpponentDetector.KnockBack += new Vector2(0f, .5f);

                    //double chip damage/durability damage on airguard
                    if(Actions.Move.OpponentProperties.armor > 0)
                    {
                        //durability damage
                        if (attackLevel >= 3)
                        {
                            //Actions.Move.OpponentProperties.durability -= 25;
                        }
                        else if (attackLevel == 2)
                        {
                            //Actions.Move.OpponentProperties.durability -= 20;
                        }
                        else if (attackLevel == 1)
                        {
                            //Actions.Move.OpponentProperties.durability -= 15;
                        }
                        else
                        {
                            //Actions.Move.OpponentProperties.durability -= 10;
                        }
                        //Actions.Move.OpponentProperties.durability -= durabilityDamage/5;
                    }
                    else
                    {
                        //chip damage
                    }
                }
                else
                {
                    if(Actions.Move.OpponentProperties.armor > 0)
                    {
                        //durability damage
                        if (attackLevel >= 3)
                        {
                            //Actions.Move.OpponentProperties.durability -= 20;
                        }
                        else if (attackLevel == 2)
                        {
                            //Actions.Move.OpponentProperties.durability -= 15;
                        }
                        else if (attackLevel == 1)
                        {
                            //Actions.Move.OpponentProperties.durability -= 10;
                        }
                        else
                        {
                            //Actions.Move.OpponentProperties.durability -= 5;
                        }
                    }
                    else
                    {
                        //chip damage
                    }
                }

                if (Actions.Move.facingRight)
                    KnockBack *= new Vector2(-1, 1);
                else
                    OpponentDetector.KnockBack *= new Vector2(-1, 1);
            }
            else
            {
                if (jumpCancellable)
                {
                    Actions.jumpCancel = true;
                }
                if (shatter && Actions.Move.OpponentProperties.armor > 0)
                {
                    //trigger shatter effect
                    OpponentDetector.anim.SetTrigger(shatterID);
                    OpponentDetector.Actions.shattered = true;
                    Debug.Log("Shattered");
                    ApplyHitStop(5);
                    HitSuccess(other);
                    Actions.Move.OpponentProperties.armor = 0;
                    Actions.Move.OpponentProperties.durability = 0;
                    //damage and hitstun;
                    
                }
                else if (piercing && Actions.Move.OpponentProperties.armor > 0)
                {
                    ApplyHitStop(-2);
                    //Actions.Move.OpponentProperties.armor -= armorDamage;
                    //Actions.Move.OpponentProperties.durability -= durabilityDamage;
                    HitSuccess(other);   
                }
                else if(Actions.Move.OpponentProperties.armor > 0)
                {
                    //if the opponent has armor, deal armor and durability damage
                    //Actions.Move.OpponentProperties.armor -= armorDamage;
                    //Actions.Move.OpponentProperties.durability -= durabilityDamage;
                    ApplyHitStop(-2);
                    OpponentDetector.anim.SetTrigger(armorHitID);
                }
                else
                {
                    //otherwise deal damage, hitstun, and knockback
                    ApplyHitStop(0);
                    HitSuccess(other);
                }
            }
            Contact();
        }
        anim.ResetTrigger(clashID);
        anim.ResetTrigger(deflectID);
        anim.ResetTrigger(parryID);
        anim.ResetTrigger(successID);
        anim.ResetTrigger(hitID);
        anim.ResetTrigger(hitBodyID);
        anim.ResetTrigger(hitLegsID);
        anim.SetBool(launchID, false);
        anim.SetBool(sweepID, false);
        anim.ResetTrigger(shatterID);
        anim.ResetTrigger(armorHitID);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        collideCount--;
        if (collideCount == 0)
        {
            allowHit = true;
        }
    }

    void Contact()
    {
        if (allowLight)
            Actions.acceptLight = true;
        if (allowMedium)
            Actions.acceptMedium = true;
        if (allowHeavy)
            Actions.acceptHeavy = true;
        if (allowBreak)
            Actions.acceptBreak = true;
        if (allowSpecial)
            Actions.acceptSpecial = true;
        if (allowSuper)
            Actions.acceptSuper = true;
        Actions.blitzCancel = true;
        allowHit = false;
        hit = true;
    }

    void HitSuccess(Collider2D other)
    {
        //if the attack hit the opponent
        anim.SetTrigger(successID);



        //manipulate opponent's state based on attack properties
        if (forceCrouch && OpponentDetector.Actions.standing)
            OpponentDetector.anim.SetBool("Crouch", true);
        else if (OpponentDetector.Actions.airborne && transform.position.y < 1.2f)
            transform.position = new Vector3(transform.position.x, 1.2f, transform.position.z);

        if (launch)
        {
            OpponentDetector.anim.SetBool(launchID, true);
        }
        else if ((crumple || OpponentDetector.Actions.CharProp.currentHealth == 0) && !OpponentDetector.Actions.airborne)
        {
            OpponentDetector.anim.SetBool(crumpleID, true);
        }
        else if (sweep && !OpponentDetector.Actions.airborne)
        {
            OpponentDetector.anim.SetBool(sweepID, true);
            OpponentDetector.Actions.airborne = true;
        }
        
        OpponentDetector.anim.SetTrigger(hitID);
        if(OpponentDetector.Actions.standing && !launch && !sweep && !crumple)
        {
            if (other.CompareTag("Body") || other.CompareTag("HurtBox"))
            {
                OpponentDetector.anim.SetTrigger(hitBodyID);
            }
            else if (other.CompareTag("Legs"))
            {
                OpponentDetector.anim.SetTrigger(hitLegsID);
            }
        }
        OpponentDetector.Actions.groundBounce = allowGroundBounce;

        if(allowWallStick && OpponentDetector.Actions.wallStick == 0)
        {
            OpponentDetector.Actions.wallStick = 4;
        }
        else if (OpponentDetector.Actions.wallStick > 0)
        {
            OpponentDetector.Actions.wallStick--;
        }
        else
        {
            OpponentDetector.Actions.wallStick = 0;
        }

        //calculate and deal damage

        //apply hitstun
        OpponentDetector.hitStun = potentialHitStun;

        if (OpponentDetector.anim.GetBool("Crouch"))
            OpponentDetector.hitStun += 2;
        if (OpponentDetector.Actions.shattered)
            OpponentDetector.hitStun += OpponentDetector.hitStun/5;
        OpponentDetector.blockStun = 0;

        //apply knockback
        if(OpponentDetector.currentState.IsName("Crumple"))
        {
            if (potentialAirKnockBack.y < 0)
            {
                OpponentDetector.KnockBack = potentialKnockBack;
                if (potentialKnockBack.y == 0)
                    OpponentDetector.KnockBack += new Vector2(0, 1.5f);
            }
            else if (potentialAirKnockBack == Vector2.zero)
            {
                OpponentDetector.KnockBack = potentialKnockBack;
                if (potentialKnockBack.y == 0)
                    OpponentDetector.KnockBack += new Vector2(0, 1.5f);
            }
            else
            {
                OpponentDetector.KnockBack = potentialAirKnockBack;
            }
        }
        else if (OpponentDetector.Actions.airborne)
        {
            if (potentialAirKnockBack == Vector2.zero)
            {
                OpponentDetector.KnockBack = potentialKnockBack;
                if(potentialKnockBack.y == 0)
                    OpponentDetector.KnockBack += new Vector2(0, 1.5f);
            }
            else
            {    
                OpponentDetector.KnockBack = potentialAirKnockBack;
            }
        }
        else
            OpponentDetector.KnockBack = potentialKnockBack;

        //apply pushback based on certain conditions
        if (Actions.airborne)
        {
            if (OpponentDetector.Actions.Move.hittingWall && OpponentDetector.Actions.airborne)
                KnockBack = potentialAirKnockBack * new Vector2(.25f, 0);
            if (OpponentDetector.Actions.airborne && rb.velocity.y < 0)
                KnockBack += potentialAirKnockBack * new Vector2(0, .35f);
        }
        else if (OpponentDetector.Actions.Move.hittingWall)
        {
            if (potentialKnockBack.x > potentialKnockBack.y)
                KnockBack = potentialKnockBack * new Vector2(.8f, 0);
            else if (potentialKnockBack.y > 2)
            {
                KnockBack = new Vector2(2, 0);
            }
            else
            {
                KnockBack = new Vector2(potentialKnockBack.y, 0);
            }
        }

        if (Actions.Move.facingRight)
        {
            KnockBack *= new Vector2(-1f, 1);
        }
        else
        {
            OpponentDetector.KnockBack *= new Vector2(-1f, 1);
        }

        comboCount++;
    }

    void Clash()
    {
        Actions.acceptLight = true;
        Actions.acceptMedium = true;
        Actions.acceptHeavy = true;
        Actions.acceptBreak = true;
        Actions.acceptSpecial = true;
        Actions.acceptSuper = true;
        Actions.jumpCancel = true;
        allowHit = false;
        hit = true;
    }


    void ApplyHitStop(int i)
    {
        currentVelocity = rb.velocity;
        OpponentDetector.currentVelocity = OpponentDetector.rb.velocity;
        hitStop = potentialHitStop + i;
        OpponentDetector.hitStop = potentialHitStop + i;
    }
}
