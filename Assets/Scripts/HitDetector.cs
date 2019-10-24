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
    public float forcedProration;
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
    public bool allowWallBounce = false;
    public bool usingSuper;

    HitDetector OpponentDetector;

    bool allowHit = false;
    int collideCount = 0;
    public bool hit = false;
    public int comboCount;
    float specialProration;
    float comboProration;
    float opponentValor;

    float minDamage;
    float damageToOpponent;

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
    static int throwRejectID;
    static int dizzyID;

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
        throwRejectID = Animator.StringToHash("ThrowReject");
        dizzyID = Animator.StringToHash("Dizzy");
    }

    void Update()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        opponentValor = Actions.Move.OpponentProperties.currentValor;

        //reset combo count and damage scaling once combo has ended
        if ((OpponentDetector.hitStun == 0 && OpponentDetector.Actions.standing) || OpponentDetector.currentState.IsName("AirRecovery"))
        {
            comboCount = 0;
            specialProration = 1;
            comboProration = 1;
        }

        if(currentState.IsName("Launch"))
            anim.SetBool(launchID, false);
        else if (currentState.IsName("SweepHit"))
            anim.SetBool(sweepID, false);

        if(hitStun > 0 && hitStop == 0)
        {
            Actions.DisableAll();
            //hitStun only counts down if not in the groundbounce or crumple animations
            if(!currentState.IsName("GroundBounce") && !currentState.IsName("Crumple") && !currentState.IsName("SweepHit"))
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
            //hitStop to give hits more impact and allow time to input next move
            anim.SetFloat(animSpeedID, 0f);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            hitStop--;
        }
        else if (Actions.grabbed)
        {
            //lock character to allow throw animation to work correctly
            anim.SetFloat(animSpeedID, 0.5f);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else 
        {
            anim.SetFloat(animSpeedID, 1.0f);
            if (rb.constraints == RigidbodyConstraints2D.FreezeAll)
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if(currentState.IsName("WallStick"))
            {
                rb.velocity = Vector2.zero;
            }
            else if (Actions.shattered && hitStun > 0)
            {
                //reward attacker for landing a shattering attack
                rb.gravityScale = .7f;
                anim.SetFloat(animSpeedID, .85f);
            }
            else
            {
                rb.gravityScale = Actions.gravScale;
            }

            if (currentVelocity != Vector2.zero)
            {
                //retain velocity after hitStop occurs
                rb.velocity = currentVelocity;
                currentVelocity = Vector2.zero;
            }

            if (KnockBack != Vector2.zero)
            {
                //apply knockback/pushback once hitstop has ceased
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
            anim.ResetTrigger(crumpleID);
            anim.SetBool(launchID, false);
            anim.SetBool(sweepID, false);
            anim.ResetTrigger(shatterID);
            anim.ResetTrigger(armorHitID);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        collideCount++;
        if (allowHit && !grab && !commandGrab && other.gameObject.transform.parent.parent == Actions.Move.opponent && potentialHitStun > 0)
        {
            OpponentDetector.Actions.shattered = false;

            if ((guard == "Mid" && (OpponentDetector.anim.GetBool(LoGuard) || OpponentDetector.anim.GetBool(HiGuard) || OpponentDetector.anim.GetBool(AirGuard))) ||
                (guard == "Low" && (OpponentDetector.anim.GetBool(LoGuard) || OpponentDetector.anim.GetBool(AirGuard))) ||
                (guard == "Overhead" && OpponentDetector.anim.GetBool(HiGuard) || OpponentDetector.anim.GetBool(AirGuard)))
            {
                OpponentDetector.anim.SetTrigger("Blocked");
                OpponentDetector.blockStun = potentialHitStun - potentialHitStun/10;
                if (OpponentDetector.blockStun > 30)
                    OpponentDetector.blockStun = 30;
                anim.SetInteger(blockStunID, blockStun);
                //what to do if an attack is blocked
                //mid can be guarded by any guard, lows must be guarded low, overheads must be guarded high
                //deal durability/chip damage equaling 10-20% of base damage
                //apply pushback to both by half of horizontal knockback value
                if(OpponentDetector.Actions.Move.hittingWall)
                    KnockBack = potentialKnockBack * new Vector2(1.2f,0);
                else
                {
                    KnockBack = potentialKnockBack * new Vector2(1f, 0);
                    OpponentDetector.KnockBack = potentialKnockBack * new Vector2(.4f, 0);
                }

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
                        Actions.Move.OpponentProperties.durability -= damage/5;
                    }
                    else
                    {
                        //chip damage
                        if (Actions.Move.OpponentProperties.currentHealth - damage/5 == 0 && Actions.Move.OpponentProperties.currentHealth > 1)
                            Actions.Move.OpponentProperties.currentHealth = 1;
                        else
                            Actions.Move.OpponentProperties.currentHealth -= damage/5;

                        if (Actions.Move.OpponentProperties.currentHealth <= 0)
                            OpponentDetector.anim.SetTrigger(hitID);

                    }
                }
                else
                {
                    if (Actions.Move.OpponentProperties.armor > 0)
                    {
                        //durability damage
                        Actions.Move.OpponentProperties.durability -= damage/10;
                    }
                    else
                    {
                        //chip damage
                        if (Actions.Move.OpponentProperties.currentHealth - damage/10 == 0 && Actions.Move.OpponentProperties.currentHealth > 1)
                            Actions.Move.OpponentProperties.currentHealth = 1;
                        else
                            Actions.Move.OpponentProperties.currentHealth -= damage/10;

                        if (Actions.Move.OpponentProperties.currentHealth <= 0)
                            OpponentDetector.anim.SetTrigger(crumpleID);
                    }
                }
                ApplyHitStop(0);
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
                    Actions.Move.OpponentProperties.armor = 0;
                    Actions.Move.OpponentProperties.durability = 0;
                    //trigger shatter effect
                    OpponentDetector.anim.SetTrigger(shatterID);
                    OpponentDetector.Actions.shattered = true;
                    Debug.Log("Shattered");
                    //damage, hitstun, etc.
                    HitSuccess(other);
                    ApplyHitStop(5);  
                }
                else if (piercing && Actions.Move.OpponentProperties.armor > 0)
                { 
                    Actions.Move.OpponentProperties.armor -= armorDamage;
                    Actions.Move.OpponentProperties.durability -= durabilityDamage;
                    HitSuccess(other);
                    ApplyHitStop(-2);
                }
                else if(Actions.Move.OpponentProperties.armor > 0)
                {
                    //if the opponent has armor, deal armor and durability damage
                    Actions.Move.OpponentProperties.armor -= armorDamage;
                    Actions.Move.OpponentProperties.durability -= durabilityDamage;
                    ApplyHitStop(-2);
                    OpponentDetector.anim.SetTrigger(armorHitID);
                }
                else
                {
                    //otherwise deal damage, hitstun, and knockback
                    HitSuccess(other);
                    ApplyHitStop(0);
                }
            }
            Contact();
        }
        else if (allowHit && (grab || commandGrab) && other.CompareTag("Body") && !OpponentDetector.Actions.throwInvincible &&
            ((Actions.standing && OpponentDetector.Actions.standing) || (Actions.airborne && OpponentDetector.Actions.airborne)))
        {
            if ((OpponentDetector.Actions.throwTech && !commandGrab))
            {
                anim.SetTrigger(throwRejectID);
                OpponentDetector.anim.SetTrigger(throwRejectID);
                KnockBack = new Vector2(2, 0);
                if (Actions.Move.facingRight)
                    KnockBack *= new Vector2(-1, 0);
            }
            else if (((OpponentDetector.hitStun == 0 && OpponentDetector.blockStun == 0) || OpponentDetector.Actions.grabbed) && hitStun == 0 && !currentState.IsName("Deflected"))
            {
                Actions.throwTech = false;
                if(!OpponentDetector.anim.GetBool(dizzyID))
                {
                    Actions.Move.OpponentProperties.armor -= armorDamage;
                    if (Actions.Move.OpponentProperties.armor > 0)
                        Actions.Move.OpponentProperties.durability = 100;
                    else
                        Actions.Move.OpponentProperties.durability = 0;
                }
                HitSuccess(other);
                ApplyHitStop(0);
            }
            allowHit = false;
            hit = true;
        }
        else if (allowHit && !grab && other.gameObject.transform.parent == Actions.Move.opponent && other.CompareTag("HitBox"))
        {
            //clash/deflect system
            if ((OpponentDetector.attackLevel - attackLevel) > 1 && potentialHitStun > 0)
            {
                //when one attack is more powerful than another, the weaker attack is deflected and the winner is allowed to followup
                ApplyHitStop(0);
                Debug.Log("DEFLECTED!");
                OpponentDetector.anim.SetTrigger(parryID);
                anim.SetTrigger(deflectID);
                Actions.jumpCancel = true;
                Actions.CharProp.durabilityRefillTimer = 0;
            }
            else if ((attackLevel - OpponentDetector.attackLevel) <= 1 && potentialHitStun > 0)
            {
                //if the attacks are of similar strength both can immediately input another command
                Debug.Log("Clash!");
                ApplyHitStop(0);
                anim.SetTrigger(clashID);
                //no knockback on clashes
                Clash();
            }
            allowHit = false;
        }
        anim.ResetTrigger(hitID);
        anim.ResetTrigger(hitBodyID);
        anim.ResetTrigger(hitLegsID);
        anim.ResetTrigger(crumpleID);
        anim.SetBool(launchID, false);
        anim.SetBool(sweepID, false);
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
        //execute if an attack makes contact with a opponent
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

        OpponentDetector.Actions.CharProp.durabilityRefillTimer = 0;
    }

    void HitSuccess(Collider2D other)
    {
        //if the attack successfully hit the opponent
        anim.SetTrigger(successID);

        //special properties if hitting a dizzied opponent
        if(OpponentDetector.anim.GetBool(dizzyID))
        {
            OpponentDetector.anim.SetBool(dizzyID, false);
            OpponentDetector.Actions.CharProp.refill = true;
            forceCrouch = true;
            specialProration = .85f;
        }
        if(Actions.Move.OpponentProperties.armor < 0 && !grab && !piercing)
        {
            Actions.Move.OpponentProperties.armor = 0;
        }

        if (forceCrouch && !OpponentDetector.Actions.airborne)
            OpponentDetector.anim.SetBool("Crouch", true);

        if (OpponentDetector.Actions.airborne && transform.position.y < 1.2f)
            transform.position = new Vector3(transform.position.x, 1.2f, transform.position.z);

        OpponentDetector.anim.SetTrigger(hitID);
        if (OpponentDetector.Actions.standing && !launch && !sweep && !crumple)
        {
            //determine whether to play a low hit or high hit animation
            if (other.CompareTag("Body") || other.CompareTag("HurtBox"))
            {
                OpponentDetector.anim.SetTrigger(hitBodyID);
            }
            else if (other.CompareTag("Legs"))
            {
                OpponentDetector.anim.SetTrigger(hitLegsID);
            }
        }

        //calculate and deal damage
        damageToOpponent = damage * comboProration * opponentValor * specialProration;

        if (usingSuper)
        {
            minDamage = damage * .2f * opponentValor;
        }
        else if (damage > 1)
        {
            minDamage = 1;
        }

        if (damageToOpponent < minDamage)
            damageToOpponent = minDamage;

        OpponentDetector.Actions.CharProp.currentHealth -= (int)damageToOpponent;
        minDamage = 0;

        // initialproration is applied if it is the first hit of a combo
        // some moves will force damage scaling in forcedProration
        if (comboCount == 0)
            specialProration = initialProration;
        if (forcedProration > 0 && comboCount > 0)
            specialProration *= forcedProration;
        if (comboCount != 0 && comboCount < 10)
        {
            if (comboCount < 3)
                comboProration = 1;
            else if (comboCount < 4)
                comboProration = .8f;
            else if (comboCount < 5)
                comboProration = .7f;
            else if (comboCount < 6)
                comboProration = .6f;
            else if (comboCount < 7)
                comboProration = .5f;
            else if (comboCount < 8)
                comboProration = .4f;
            else if (comboCount < 9)
                comboProration = .3f;
            else if (comboCount < 10)
                comboProration = .2f;
            else if (comboCount < 11)
                comboProration = .1f;
        }
            


        //manipulate opponent's state based on attack properties
        //defender can enter unique states of stun if hit by an attack with corresponding property

        if (launch)
        {
            OpponentDetector.anim.SetBool(launchID, true);
        }
        else if ((crumple || OpponentDetector.Actions.CharProp.currentHealth <= 0) && !OpponentDetector.Actions.airborne)
        {
            OpponentDetector.anim.SetTrigger(crumpleID);
        }
        else if (sweep && !OpponentDetector.Actions.airborne)
        {
            OpponentDetector.anim.SetBool(sweepID, true);
            OpponentDetector.Actions.airborne = true;
        }

        OpponentDetector.Actions.groundBounce = allowGroundBounce;
        OpponentDetector.Actions.wallBounce = allowWallBounce;

        if (allowWallStick && OpponentDetector.Actions.wallStick == 0)
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

        //apply hitstun
        OpponentDetector.hitStun = potentialHitStun;
        if (Actions.Move.OpponentProperties.comboTimer >= 400)
            OpponentDetector.hitStun = 1;

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
                    OpponentDetector.KnockBack += new Vector2(0, 1f);
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
                KnockBack = potentialAirKnockBack * new Vector2(.35f, 0);
            if (OpponentDetector.Actions.airborne && rb.velocity.y < 0)
                KnockBack += potentialAirKnockBack * new Vector2(0, .3f);
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

        if (!OpponentDetector.Actions.Move.facingRight)
        {
            KnockBack *= new Vector2(-1f, 1);
        }
        else
        {
            OpponentDetector.KnockBack *= new Vector2(-1f, 1);
        }
        if (!grab)
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
        if (Actions.Move.OpponentProperties.currentHealth <= 0)
        {
            hitStop = 90;
            OpponentDetector.hitStop = 90;
        }
        else
        {
            hitStop = potentialHitStop + i;
            OpponentDetector.hitStop = potentialHitStop + i;
        }
    }
}
