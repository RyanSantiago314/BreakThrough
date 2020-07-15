using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetector : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public AcceptInputs Actions;
    public PauseMenu pauseScreen;

    public Collider2D hitBox1;

    public GameObject HitFXPrefab;
    public GameObject HitFX;
    public Animator hitEffect;

    public AnimatorStateInfo currentState;
    public Vector2 currentVelocity;

    public int damage;
    public float initialProration;
    public float forcedProration;
    public Vector2 potentialKnockBack;
    public Vector2 potentialAirKnockBack;
    public float potentialBlockStun;
    public float potentialHitStun;
    public float potentialHitStop;
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

    public Vector2 KnockBack;
    public Vector2 ProjectileKnockBack;
    public float hitStop = 0;
    public float hitStun = 0;
    public float blockStun = 0;

    public bool grab = false;
    public bool commandGrab = false;

    public bool blitz = false;
    public bool turnOffEffect = false;

    public bool piercing = false;
    public bool launch = false;
    public bool crumple = false;
    public bool sweep = false;
    public bool forceCrouch = false;
    public bool forceStand = false;
    public bool shatter = false;
    public bool forceShatter = false;
    public bool allowWallStick = false;
    public bool allowGroundBounce = false;
    public bool allowWallBounce = false;
    public bool usingSuper;
    public bool usingSpecial;
    public bool guardCancel;
    public bool disableBlitz = false;

    public bool pierceSuccess;
    public bool counterSuccess;
    public bool shatterSuccess;

    public bool slash = false;
    public bool vertSlash = false;
    public bool horiSlash = false;

    public HitDetector OpponentDetector;

    bool allowHit = false;
    int collideCount = 0;
    public bool hit = false;
    public bool armorHit = false;
    public bool justDefense = false;
    public int comboCount;
    public float specialProration;
    public float comboProration;
    public bool usedWallStick = false;
    float opponentValor;
    float pushBackScale;

    float minDamage;
    float damageToOpponent;

    static int HiGuard;
    static int LoGuard;
    static int AirGuard;

    static int crouchID;
    static int runID;
    static int animSpeedID;
    static int hitStunID;
    static int blockStunID;
    static int clashID;
    static int deflectID;
    static int parryID;
    static int successID;
    static int hitID;
    static int hitAirID;
    static int hitBodyID;
    static int hitLegsID;
    static int launchID;
    static int crumpleID;
    static int sweepID;
    static int shatterID;
    static int armorHitID;
    static int throwRejectID;
    static int dizzyID;
    static int KOID;
    static int KDID;

    static int guardID;

    void Start()
    {
        Application.targetFrameRate = 60;

        OpponentDetector = Actions.Move.OpponentProperties.HitDetect;

        LoGuard = Animator.StringToHash("LowGuard");
        HiGuard = Animator.StringToHash("HighGuard");
        AirGuard = Animator.StringToHash("AirGuard");

        crouchID = Animator.StringToHash("Crouch");
        runID = Animator.StringToHash("Run");
        animSpeedID = Animator.StringToHash("AnimSpeed");
        hitStunID = Animator.StringToHash("HitStun");
        blockStunID = Animator.StringToHash("BlockStun");
        clashID = Animator.StringToHash("Clash");
        deflectID = Animator.StringToHash("Deflected");
        parryID = Animator.StringToHash("Parry");
        successID = Animator.StringToHash("HitSuccess");
        hitID = Animator.StringToHash("Hit");
        hitAirID = Animator.StringToHash("HitAir");
        hitBodyID = Animator.StringToHash("HitBody");
        hitLegsID = Animator.StringToHash("HitLegs");
        launchID = Animator.StringToHash("Launch");
        crumpleID = Animator.StringToHash("Crumple");
        sweepID = Animator.StringToHash("Sweep");
        shatterID = Animator.StringToHash("Shatter");
        armorHitID = Animator.StringToHash("ArmorHit");
        throwRejectID = Animator.StringToHash("ThrowReject");
        dizzyID = Animator.StringToHash("Dizzy");
        KOID = Animator.StringToHash("KOed");
        KDID = Animator.StringToHash("KnockDown");

        guardID = Animator.StringToHash("Guard");

        pauseScreen = GameObject.Find("PauseManager").GetComponentInChildren<PauseMenu>();

        HitFX = Instantiate(HitFXPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity, transform.root);//.GetChild(0));
        hitEffect = HitFX.GetComponent<Animator>();
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
            pushBackScale = 0;
            usedWallStick = false;
        }

        if (currentState.IsName("Launch"))
            anim.SetBool(launchID, false);
        else if (currentState.IsName("SweepHit"))
            anim.SetBool(sweepID, false);
        else if (currentState.IsName("Deflected"))
        {
            if (Actions.standing)
                Actions.Move.rb.velocity = Vector2.zero;
            else
                Actions.Move.rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (hitStun <= 0 && blockStun <= 0 && hitStop <= 0)
        {
            hitStun = 0;
            hitStop = 0;
            blockStun = 0;
            anim.SetFloat(hitStunID, -1);
            anim.SetFloat(blockStunID, -1);
            anim.ResetTrigger(deflectID);
            anim.ResetTrigger(parryID);
            anim.ResetTrigger(successID);
            anim.ResetTrigger(hitID);
            anim.ResetTrigger(hitAirID);
            anim.ResetTrigger(hitBodyID);
            anim.ResetTrigger(hitLegsID);
            anim.ResetTrigger(crumpleID);
            anim.SetBool(launchID, false);
            anim.SetBool(sweepID, false);
        }

        if (hitStun > 0 && hitStop <= 0)
        {
            anim.SetBool(runID, false);
            //hitStun only counts down if not in the groundbounce or crumple animations
            if (!currentState.IsName("GroundBounce") && !currentState.IsName("Crumple") && !currentState.IsName("SweepHit") && !pauseScreen.isPaused)
            {
                if (Actions.blitzed > 0)
                    hitStun -= Time.deltaTime / 2;
                else
                    hitStun -= Time.deltaTime;
            }
            anim.SetFloat(hitStunID, hitStun);
        }
        if(blockStun > 0 && hitStop <= 0)
        {
            Actions.Guard();
            if (!pauseScreen.isPaused)
            {
                    blockStun -= Time.deltaTime;
            }
            anim.SetFloat(blockStunID, blockStun);
        }

        if (hitStop > 0)
        {
            //hitStop to give hits more impact and allow time to input next move
            anim.SetFloat(animSpeedID, 0f);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            if (!pauseScreen.isPaused)
                hitStop -= Time.deltaTime;
        }
        else if (Actions.grabbed)
        {
            //lock character to allow throw animation to work correctly
            anim.SetFloat(animSpeedID, .8f);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            anim.SetFloat(animSpeedID, 1.0f);
            hitEffect.SetFloat(animSpeedID, 1.0f);
            if (rb.constraints == RigidbodyConstraints2D.FreezeAll)
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (Actions.superFlash > 0)
            {
                OpponentDetector.hitStop = (float)2/60;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            if(currentState.IsName("WallStick") && Actions.Move.hittingWall)
            {
                rb.velocity = Vector2.zero;
            }
            else if (Actions.blitzed > 0)
            {
                //simulate slow motion if within range of a blitz cancel or blitz attack
                if (Actions.blitzed > (float)58/60)
                {
                     rb.velocity *= new Vector2(.65f, 1f);
                    if (rb.velocity.y < .5f)
                        rb.velocity *= new Vector2(1f, .5f);
                }

                if (hitStun == 0 && !Actions.attacking && !Actions.active && !Actions.recovering)
                    Actions.blitzed = (float)1/60;

                anim.SetFloat(animSpeedID, .5f);

                rb.mass = Actions.Move.weight * .65f;
                if (rb.velocity.y < 0.5f)
                    rb.gravityScale = .6f * Actions.gravScale * Actions.originalGravity;

                if (!pauseScreen.isPaused)
                    Actions.blitzed -= Time.deltaTime;

            }
            else if (Actions.shattered && hitStun > 0)
            {
                //reward attacker for landing a shattering attack
                rb.gravityScale = .8f * Actions.originalGravity;
                anim.SetFloat(animSpeedID, .75f);
            }
            else
            {
                if (Actions.blitzed <= (float)1/60 && Actions.blitzed > 0)
                {
                    rb.velocity *= new Vector2(1.35f, 1f);
                    Actions.blitzed = 0;
                }
                rb.mass = Actions.Move.weight;
                rb.gravityScale = Actions.originalGravity * Actions.gravScale;
                anim.SetFloat(animSpeedID, 1f);
            }

            if (currentVelocity != Vector2.zero)
            {
                //retain velocity after hitStop occurs
                rb.velocity = currentVelocity;
                currentVelocity = Vector2.zero;
            }

            if (KnockBack != Vector2.zero || ProjectileKnockBack != Vector2.zero)
            {
                //apply knockback/pushback once hitstop has ceased
                if ((hitStun > 0 || blockStun > 0) && (Actions.airborne || (Actions.comboHits <= 1 && Actions.standing)))
                    rb.velocity = Vector2.zero;
                else if (comboCount > 5)
                    rb.velocity = new Vector2(.65f * rb.velocity.x, rb.velocity.y);

                if (Mathf.Abs(ProjectileKnockBack.x) > Mathf.Abs(KnockBack.x) || Mathf.Abs(ProjectileKnockBack.y) > Mathf.Abs(KnockBack.y))
                {
                    if (Actions.blitzed > 0)
                        rb.AddForce(ProjectileKnockBack * .6f, ForceMode2D.Impulse);
                    else
                        rb.AddForce(ProjectileKnockBack, ForceMode2D.Impulse);
                }
                else
                {
                    if (Actions.blitzed > 0)
                        rb.AddForce(KnockBack * .6f, ForceMode2D.Impulse);
                    else
                        rb.AddForce(KnockBack, ForceMode2D.Impulse);
                }
                KnockBack = Vector2.zero;
                ProjectileKnockBack = Vector2.zero;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        collideCount++;
        if (!(OpponentDetector.anim.GetBool(crouchID) && guard == "High") && !((guard == "Low" && OpponentDetector.Actions.lowInvincible) ||
                ((guard == "Mid" || guard == "High" || guard == "Overhead") && OpponentDetector.Actions.hiInvincible)) && hitStop <= 0 && !hit)
        {
            if (allowHit && !grab && !commandGrab && !blitz && !usingSuper && ((guard == "Low" && OpponentDetector.Actions.lowCounter) || 
                ((guard == "Mid" || guard == "High" || guard == "Overhead") && OpponentDetector.Actions.hiCounter)))
            {
                ApplyHitStop(potentialHitStop / 2);
                Debug.Log("COUNTERED!");
                OpponentDetector.anim.SetTrigger(parryID);
                anim.SetTrigger(deflectID);
            }
            else if (allowHit && !grab && !commandGrab && other.gameObject.transform.parent.parent == Actions.Move.opponent && (potentialHitStun > 0 || blitz))
            {
                OpponentDetector.Actions.shattered = false;

                if ((guard == "Mid" && (OpponentDetector.anim.GetBool(LoGuard) || OpponentDetector.anim.GetBool(HiGuard) || OpponentDetector.anim.GetBool(AirGuard))) ||
                    (guard == "Low" && (OpponentDetector.anim.GetBool(LoGuard) || OpponentDetector.anim.GetBool(AirGuard))) ||
                    (guard == "Overhead" && (OpponentDetector.anim.GetBool(HiGuard) || OpponentDetector.anim.GetBool(AirGuard))) ||
                    (guard == "High" && (OpponentDetector.anim.GetBool(HiGuard) || OpponentDetector.anim.GetBool(AirGuard))))
                {
                    OpponentDetector.anim.SetTrigger("Blocked");
                    if (potentialBlockStun > 0)
                        OpponentDetector.blockStun = potentialBlockStun/60;
                    else
                    {
                        if (potentialHitStun <= 19)
                            OpponentDetector.blockStun = (potentialHitStun - 1) / 60;
                        else if (guardCancel)
                            OpponentDetector.blockStun = (potentialHitStun - potentialHitStun / 2) / 60;
                        else
                            OpponentDetector.blockStun = (potentialHitStun - potentialHitStun / 10) / 60;
                    }

                    if (OpponentDetector.blockStun > .5f)
                        OpponentDetector.blockStun = .5f;
                    // guarding right as the attack lands (just defend) reduces blockstun and negates chip damage
                    if (OpponentDetector.Actions.Move.justDefenseTime > 0)
                    {
                        OpponentDetector.blockStun -= (float)(OpponentDetector.blockStun / 3);
                        if (OpponentDetector.Actions.CharProp.armor > 0)
                            OpponentDetector.Actions.CharProp.durability += 25;
                        else
                        {
                            OpponentDetector.Actions.CharProp.armor = 1;
                            OpponentDetector.Actions.CharProp.durability += 25;
                        }
                        OpponentDetector.justDefense = true;
                        Debug.Log("JUST DEFEND");
                    }
                    OpponentDetector.anim.SetFloat(blockStunID, OpponentDetector.blockStun);
                    //what to do if an attack is blocked
                    //mid can be guarded by any guard, lows must be guarded low, overheads must be guarded high
                    //deal durability/chip damage equaling 10-20% of base damage
                    //apply pushback to both by half of horizontal knockback value
                    if (OpponentDetector.Actions.Move.hittingWall)
                    {
                        if (potentialKnockBack.x > potentialKnockBack.y)
                            KnockBack = new Vector2(potentialKnockBack.x, 0);
                        else
                            KnockBack = new Vector2((potentialKnockBack.y + potentialKnockBack.x) / 2, 0);
                    }
                    else if (Actions.Move.hittingWall)
                    {
                        if (potentialKnockBack.x > potentialKnockBack.y)
                            OpponentDetector.KnockBack = potentialKnockBack * new Vector2(1f, 0);
                        else
                            OpponentDetector.KnockBack = new Vector2((potentialKnockBack.y + potentialKnockBack.x) / 2, 0);
                    }
                    else
                    {
                        if (potentialKnockBack.x > potentialKnockBack.y)
                        {
                            KnockBack = potentialKnockBack * new Vector2(.8f, 0);
                            OpponentDetector.KnockBack = potentialKnockBack * new Vector2(1f, 0);
                        }
                        else
                        {
                            KnockBack = new Vector2(.9f * (potentialKnockBack.y + potentialKnockBack.x) / 2, 0);
                            OpponentDetector.KnockBack = new Vector2((potentialKnockBack.y + potentialKnockBack.x) / 2, 0);
                        }

                        if (Actions.airborne && KnockBack.x > 1f)
                        {
                            KnockBack = new Vector2(1f, KnockBack.y);
                        }

                        if (usingSpecial)
                        {
                            KnockBack = new Vector2(.5f * KnockBack.x, KnockBack.y);
                        }

                    }

                    if (OpponentDetector.anim.GetBool(AirGuard) && OpponentDetector.Actions.Move.justDefenseTime <= 0 && !guardCancel)
                    {
                        //apply special knockback to airborne guards
                        if (potentialAirKnockBack != Vector2.zero)
                        {
                            //guarding characters should never be spiked toward the ground
                            if (potentialAirKnockBack.y < 0)
                                OpponentDetector.KnockBack = potentialAirKnockBack * new Vector2(.4f, 0) + new Vector2(0, .3f);
                            else
                                OpponentDetector.KnockBack = potentialAirKnockBack * new Vector2(.4f, .5f);
                        }
                        else
                            OpponentDetector.KnockBack += new Vector2(0f, .5f);

                        //double chip damage/durability damage on airguard
                        if (Actions.Move.OpponentProperties.armor > 0)
                        {
                            Actions.Move.OpponentProperties.durability -= damage / 3;
                        }
                        else
                        {
                            //guarding an attack in the air without having any resolve results in getting shattered
                            shatter = true;
                            OpponentDetector.blockStun = 0;
                            Actions.Move.OpponentProperties.armor = 0;
                            Actions.Move.OpponentProperties.durability = 0;
                            potentialHitStun = 30;
                            //trigger shatter effect
                            OpponentDetector.anim.SetTrigger(shatterID);
                            OpponentDetector.Actions.shattered = true;
                            Debug.Log("SHATTERED");
                            //damage, hitstun, etc.
                            HitSuccess(other);
                            if (potentialHitStop > 9)
                                ApplyHitStop(2 * potentialHitStop);
                            else
                                ApplyHitStop(30);

                            //chip damage
                            /*if (Actions.Move.OpponentProperties.currentHealth - damage/5 == 0 && Actions.Move.OpponentProperties.currentHealth > 1)
                                Actions.Move.OpponentProperties.currentHealth = 1;
                            else
                                Actions.Move.OpponentProperties.currentHealth -= damage/5;

                            if (Actions.Move.OpponentProperties.currentHealth <= 0)
                                OpponentDetector.anim.SetTrigger(hitID);*/

                        }
                    }
                    else if (OpponentDetector.Actions.Move.justDefenseTime <= 0 && OpponentDetector.Actions.standing && !guardCancel)
                    {
                        if (Actions.Move.OpponentProperties.armor > 0)
                        {
                            //durability damage
                            Actions.Move.OpponentProperties.durability -= damage / 5;
                        }
                        else
                        {
                            //guarding an attack on the ground without having any resolve results in chip damage
                            Actions.Move.OpponentProperties.armor = 0;
                            Actions.Move.OpponentProperties.durability = 0;

                            //chip damage
                            if (Actions.Move.OpponentProperties.currentHealth - damage/2 * opponentValor <= 0 && Actions.Move.OpponentProperties.currentHealth > 1)
                                Actions.Move.OpponentProperties.currentHealth = 1;
                            else
                                Actions.Move.OpponentProperties.currentHealth -= damage/2 * opponentValor;

                            if (Actions.Move.OpponentProperties.currentHealth <= 0)
                                OpponentDetector.anim.SetTrigger(crumpleID);
                        }
                    }
                    if (OpponentDetector.blockStun > 0)
                    {
                        ApplyHitStop(0);

                        if (OpponentDetector.Actions.Move.justDefenseTime > 0 && OpponentDetector.Actions.standing)
                            OpponentDetector.KnockBack *= .5f;

                        if (usingSpecial || usingSuper)
                            KnockBack *= .5f;
                        if (KnockBack.x < -2)
                            KnockBack = new Vector2(-2, KnockBack.y);
                        if (OpponentDetector.KnockBack.x > 2)
                            OpponentDetector.KnockBack = new Vector2(2, KnockBack.y);

                        if (Actions.Move.facingRight)
                            KnockBack *= new Vector2(-1, 1);
                        else
                            OpponentDetector.KnockBack *= new Vector2(-1, 1);
                    }
                }
                else
                {
                    if (jumpCancellable)
                    {
                        Actions.jumpCancel = true;
                    }

                    if (forceShatter || (shatter && (guard == "Unblockable" || Actions.Move.OpponentProperties.armor > 0 || OpponentDetector.anim.GetBool(dizzyID)) && (OpponentDetector.Actions.armorActive || OpponentDetector.Actions.attacking || OpponentDetector.Actions.active || OpponentDetector.Actions.recovering || OpponentDetector.anim.GetBool(dizzyID))))
                    {
                        //getting shattered means losing all your meter/armor
                        if (comboCount < 1)
                        {
                            Actions.Move.OpponentProperties.armor = 0;
                            Actions.Move.OpponentProperties.durability = 0;
                        }
                        //trigger shatter effect
                        OpponentDetector.anim.SetTrigger(shatterID);
                        OpponentDetector.Actions.shattered = true;
                        if (!forceShatter)
                        {
                            shatterSuccess = true;
                            Actions.Move.OpponentProperties.comboTimer = 0;
                        }
                        Debug.Log("SHATTERED");
                        //damage, hitstun, etc.
                        specialProration *= 1.1f;
                        HitSuccess(other);
                        ApplyHitStop(2 * potentialHitStop);
                    }
                    else if (piercing && Actions.Move.OpponentProperties.armor > 0 && OpponentDetector.Actions.armorActive)
                    {
                        if (armorDamage > 0 || durabilityDamage > 0)
                        {
                            Actions.Move.OpponentProperties.armor -= armorDamage;
                            Actions.Move.OpponentProperties.durability -= durabilityDamage;
                            OpponentDetector.armorHit = true;
                        }
                        pierceSuccess = true;
                        HitSuccess(other);
                        ApplyHitStop(0);
                    }
                    else if (!blitz && Actions.Move.OpponentProperties.armor > 0 && OpponentDetector.Actions.armorActive)
                    {
                        //if the opponent has armor and is using it, deal armor and durability damage
                        Actions.Move.OpponentProperties.armor -= armorDamage;
                        Actions.Move.OpponentProperties.durability -= durabilityDamage;
                        ApplyHitStop(0);
                        OpponentDetector.armorHit = true;
                        Debug.Log("HitArmor");
                    }
                    else
                    {
                        //otherwise deal damage, hitstun, and knockback
                        HitSuccess(other);
                        ApplyHitStop(0);
                    }
                }
                Contact(other);
            }
            else if (allowHit && (grab || commandGrab) && other.CompareTag("Body") && !OpponentDetector.Actions.throwInvincible &&
                other.gameObject.transform.parent.parent == Actions.Move.opponent &&
                ((Actions.standing && OpponentDetector.Actions.standing) || (Actions.airborne && OpponentDetector.Actions.airborne)))
            {
                if ((OpponentDetector.Actions.throwTech && !commandGrab))
                {
                    anim.SetTrigger(throwRejectID);
                    OpponentDetector.anim.SetTrigger(throwRejectID);
                    KnockBack = new Vector2(2, 0);
                    OpponentDetector.KnockBack = new Vector2(2, 0);
                    if (Actions.Move.facingRight)
                        KnockBack *= new Vector2(-1, 0);
                    else
                        OpponentDetector.KnockBack *= new Vector2(-1, 0);
                }
                else if (((OpponentDetector.hitStun <= 0 && OpponentDetector.blockStun <= 0) || OpponentDetector.Actions.grabbed) && hitStun <= 0)
                {
                    Actions.throwTech = false;

                    if (!OpponentDetector.anim.GetBool(dizzyID) && armorDamage > 0)
                    {
                        Actions.Move.OpponentProperties.armor -= armorDamage;
                        Actions.Move.OpponentProperties.durability -= durabilityDamage;
                        if (Actions.Move.OpponentProperties.armor < 0)
                            OpponentDetector.anim.SetBool(dizzyID, true);
                        if (Actions.Move.OpponentProperties.armor <= 0)
                            Actions.Move.OpponentProperties.durability = 0;
                    }

                    HitSuccess(other);
                    ApplyHitStop(0);
                }
                allowHit = false;
                hit = true;
            }
            else if (allowHit && !grab && !commandGrab && !blitz && !OpponentDetector.blitz && !OpponentDetector.grab && !OpponentDetector.commandGrab && !guardCancel && 
                other.gameObject.transform.parent == Actions.Move.opponent && other.CompareTag("HitBox"))
            {
                //clash/deflect system
                if (attackLevel > OpponentDetector.attackLevel && (attackLevel - OpponentDetector.attackLevel) > 1 && potentialHitStun > 0)
                {
                    //when one attack is more powerful than another, the weaker attack is deflected and the winner is allowed to followup
                    ApplyHitStop(potentialHitStop/2);
                    Debug.Log("DEFLECTED!");
                    anim.SetTrigger(parryID);
                    OpponentDetector.anim.SetTrigger(deflectID);
                    Actions.jumpCancel = true;
                    Actions.CharProp.durabilityRefillTimer = 0;
                    Actions.EnableAll();

                    hitEffect.transform.position = other.bounds.ClosestPoint(transform.position + new Vector3(hitBox1.offset.x, hitBox1.offset.y, 0));

                    hitEffect.transform.GetChild(0).transform.localScale = Vector3.one;
                    hitEffect.transform.rotation = Actions.Move.transform.rotation; ;
                    hitEffect.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, -30);
                    hitEffect.SetTrigger(parryID);
                    if (usingSuper)
                    {
                        HitSuccess(other);
                        ApplyHitStop(0);
                    }
                }
                else if (Mathf.Abs(attackLevel - OpponentDetector.attackLevel) <= 1 && potentialHitStun > 0)
                {
                    //if the attacks are of similar strength both can immediately input another command
                    Debug.Log("Clash!");
                    ApplyHitStop(potentialHitStop/2);
                    //no knockback on clashes
                    Clash(other);
                }
                allowHit = false;
                hit = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        collideCount--;
        if (collideCount == 0)
        {
            allowHit = true;
        }
    }

    public void Contact(Collider2D other)
    {
        //execute if an attack makes contact with a opponent
        if (allowLight && !OpponentDetector.armorHit)
            Actions.acceptLight = true;
        if (allowMedium && !OpponentDetector.armorHit)
            Actions.acceptMedium = true;
        if (allowHeavy && !OpponentDetector.armorHit)
            Actions.acceptHeavy = true;
        if (allowBreak)
            Actions.acceptBreak = true;
        if (allowSpecial)
            Actions.acceptSpecial = true;
        if (allowSuper)
            Actions.acceptSuper = true;
        if (!disableBlitz)
            Actions.blitzCancel = true;

        allowHit = false;
        hit = true;

        if (OpponentDetector.hitStun <= 0)
            OpponentDetector.Actions.CharProp.durabilityRefillTimer = 0;

        hitEffect.transform.position = other.bounds.ClosestPoint(transform.position + new Vector3(hitBox1.offset.x, hitBox1.offset.y, 0));
        OpponentDetector.anim.ResetTrigger("Jump");

        if (piercing)
            hitEffect.SetInteger("AttackLevel", 4);
        else
            hitEffect.SetInteger("AttackLevel", attackLevel);

        //set hit effect to play based on attack properties
        if (!blitz && !grab && !turnOffEffect)
        {
            HitFX.transform.rotation = Actions.Move.transform.rotation;
            if (OpponentDetector.Actions.shattered)
            {
                hitEffect.SetTrigger(shatterID);
                hitEffect.transform.position = OpponentDetector.Actions.Move.transform.position;
                if (OpponentDetector.Actions.Move.anim.GetBool("Crouch"))
                    hitEffect.transform.position = new Vector3(hitEffect.transform.position.x, hitEffect.transform.position.y - .5f, hitEffect.transform.position.z);
                hitEffect.transform.GetChild(0).transform.localScale = new Vector3(2, 2, 1);
            }
            else if (OpponentDetector.armorHit)
            {
                hitEffect.SetTrigger(armorHitID);
            }
            else if (OpponentDetector.blockStun > 0)
            {
                hitEffect.SetTrigger(guardID);
            }
            else if (OpponentDetector.hitStun > 0)
            {
                if (vertSlash || horiSlash || slash)
                {
                    hitEffect.SetTrigger("Slash");
                    if (vertSlash)
                    {
                        if (OpponentDetector.KnockBack.y > 2)
                            hitEffect.transform.eulerAngles = new Vector3(0, Actions.Move.transform.eulerAngles.y, Random.Range(60f, 80f));
                        else
                            hitEffect.transform.eulerAngles = new Vector3(0, Actions.Move.transform.eulerAngles.y, Random.Range(-60f, -80f));
                    }
                    else if (horiSlash)
                    {
                        hitEffect.transform.eulerAngles = new Vector3(0, Actions.Move.transform.eulerAngles.y, Random.Range(15f, 15f));
                    }
                    else
                    {
                        hitEffect.transform.eulerAngles = new Vector3(0, Actions.Move.transform.eulerAngles.y, Random.Range(-60f, 60f));
                        if (OpponentDetector.KnockBack.y > 2)
                            hitEffect.transform.eulerAngles = new Vector3(0, Actions.Move.transform.eulerAngles.y, Random.Range(60f, 80f));
                    }

                    if (attackLevel < 2)
                        hitEffect.transform.GetChild(0).transform.localScale = new Vector3(Random.Range(.5f, .75f), Random.Range(-1f, 1f), 1);
                    else
                        hitEffect.transform.GetChild(0).transform.localScale = new Vector3(Random.Range(1f, 1.5f), Random.Range(-1.5f, 1.5f), 1);

                }
                else
                {
                    hitEffect.SetTrigger("Strike");
                    if (attackLevel < 4)
                        hitEffect.transform.GetChild(0).transform.localScale = new Vector3(Random.Range(.75f, 1f), Random.Range(-1f, 1f), 1);
                    else
                        hitEffect.transform.GetChild(0).transform.localScale = new Vector3(Random.Range(1f, 1.5f), Random.Range(-1.5f, 1.5f), 1);

                    hitEffect.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0f, 359f));
                }
            }
            else if (attackLevel > OpponentDetector.attackLevel && (attackLevel - OpponentDetector.attackLevel) > 1 && potentialHitStun > 0)
            {
                hitEffect.transform.GetChild(0).transform.localScale = Vector3.one;
                if (Actions.Move.facingRight)
                    hitEffect.transform.position = new Vector3(OpponentDetector.Actions.Move.transform.position.x - .5f, OpponentDetector.Actions.Move.transform.position.y - .5f, 
                                                               OpponentDetector.Actions.Move.transform.position.z);
                else
                    hitEffect.transform.position = new Vector3(OpponentDetector.Actions.Move.transform.position.x + .5f, OpponentDetector.Actions.Move.transform.position.y - .5f, 
                                                               OpponentDetector.Actions.Move.transform.position.z);
                hitEffect.SetTrigger(parryID);
            }

            if (OpponentDetector.KnockBack.y > 2)
            {
                hitEffect.transform.GetChild(0).transform.eulerAngles = new Vector3(hitEffect.transform.eulerAngles.x, hitEffect.transform.eulerAngles.y, Random.Range(-30f, 30f));
            }
        }
    }

    void HitSuccess(Collider2D other)
    {
        //if the attack successfully hit the opponent
        anim.SetTrigger(successID);
        OpponentDetector.Actions.Move.jumping = 0;
        OpponentDetector.Actions.TurnAroundCheck();
        OpponentDetector.Actions.superHit = false;

        if (comboCount == 0 && OpponentDetector.Actions.standing)
            OpponentDetector.rb.velocity = Vector2.zero;

        //special properties if hitting a dizzied opponent
        if(OpponentDetector.currentState.IsName("DizzyIn") || OpponentDetector.currentState.IsName("Dizzy") || (comboCount >= 1 && anim.GetBool(dizzyID)))
        {
            OpponentDetector.anim.SetBool(dizzyID, false);
            OpponentDetector.Actions.CharProp.refill = true;
            if (!shatter)
            {
                OpponentDetector.Actions.CharProp.armor = 1;
            }
            OpponentDetector.Actions.CharProp.durability = 50;
            forceCrouch = true;
        }
        if(Actions.Move.OpponentProperties.armor < 0 && !grab && !piercing)
        {
            Actions.Move.OpponentProperties.armor = 0;
        }

        if (forceCrouch && !OpponentDetector.Actions.airborne)
            OpponentDetector.anim.SetBool("Crouch", true);
        else if (forceStand && !OpponentDetector.Actions.airborne)
            OpponentDetector.anim.SetBool("Crouch", false);

        if (OpponentDetector.Actions.airborne && transform.position.y < 1.2f)
            transform.position = new Vector3(transform.position.x, 1.2f, transform.position.z);

        if (!(blitz && potentialHitStun == 0))
        {
            OpponentDetector.anim.SetTrigger(hitID);
            if (OpponentDetector.Actions.standing && !launch && !sweep && !crumple && potentialKnockBack.y == 0)
            {
                OpponentDetector.anim.ResetTrigger(hitID);
                //determine whether to play a low hit or high hit animation
                if (other.CompareTag("Legs") || guard == "Low")
                {
                    OpponentDetector.anim.SetTrigger(hitLegsID);
                }
                else if (other.CompareTag("Body") || other.CompareTag("HurtBox"))
                {
                    OpponentDetector.anim.SetTrigger(hitBodyID);
                }
            }
            else if ((!crumple && !sweep && !launch) || OpponentDetector.currentState.IsName("FDKnockdown") || OpponentDetector.currentState.IsName("FUKnockdown"))
            {
                OpponentDetector.anim.SetTrigger(hitAirID);
            }
        }

        //calculate and deal damage
        damageToOpponent = damage * comboProration * opponentValor * specialProration;

        if (usingSuper)
        {
            minDamage = damage * .2f * opponentValor;
            OpponentDetector.Actions.superHit = true;
        }
        else if (damage > 1)
        {
            minDamage = 1;
        }

        if (damageToOpponent < minDamage)
            damageToOpponent = minDamage;

        if (((piercing && OpponentDetector.Actions.armorActive) || guardCancel) && OpponentDetector.Actions.CharProp.currentHealth - (int)damageToOpponent <= 0)
            OpponentDetector.Actions.CharProp.currentHealth = 1;
        else
            OpponentDetector.Actions.CharProp.currentHealth -= (int)damageToOpponent;

        minDamage = 0;

        //meter gain
        if (!grab && comboCount > 0 && !OpponentDetector.currentState.IsName("FUKnockdown") && !OpponentDetector.currentState.IsName("FDKnockdown"))
        {
            OpponentDetector.Actions.CharProp.durability += damage / 10;
            if (Actions.CharProp.durabilityRefillTimer > 5)
                Actions.CharProp.durability += damage / 20;
        }

        // initialproration is applied if it is the first hit of a combo
        // some moves will force damage scaling in forcedProration
        if (comboCount == 0 && specialProration == 1)
            specialProration = initialProration;
        if (forcedProration > 0 && comboCount > 0)
            specialProration *= forcedProration;
        if (comboCount != 0 && comboCount < 11)
        {
            if (comboCount < 2)
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
        if (blitz && (OpponentDetector.Actions.attacking || OpponentDetector.Actions.active || OpponentDetector.Actions.recovering || 
            (OpponentDetector.Actions.armorActive && !OpponentDetector.Actions.acceptGuard) ||
            OpponentDetector.hitStun > 0 || OpponentDetector.anim.GetCurrentAnimatorStateInfo(0).IsName("Deflected")))
        {
            OpponentDetector.Actions.blitzed = 1;
            if (Actions.Move.OpponentProperties.comboTimer > 0)
                Actions.Move.OpponentProperties.comboTimer -= 1f;
        }
        if (!OpponentDetector.currentState.IsName("FDKnockdown") && !OpponentDetector.currentState.IsName("FUKnockdown"))
        {
            if (launch)
            {
                OpponentDetector.anim.SetBool(launchID, true);
                OpponentDetector.Actions.standing = false;
            }
            else if (crumple)
            {
                if (OpponentDetector.Actions.standing)
                {
                    OpponentDetector.anim.ResetTrigger(hitID);
                    OpponentDetector.anim.SetTrigger(crumpleID);
                }
                else
                {
                    OpponentDetector.anim.ResetTrigger(hitID);
                    OpponentDetector.anim.SetTrigger(hitAirID);
                }
            }
            else if (sweep)
            {
                OpponentDetector.anim.SetBool(sweepID, true);
                OpponentDetector.Actions.airborne = true;
            }
            else if (OpponentDetector.Actions.CharProp.currentHealth <= 0 && !OpponentDetector.Actions.airborne && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice")
                OpponentDetector.anim.SetTrigger(crumpleID);
        }
        

        if (!blitz && potentialHitStun > 0)
        {
            anim.ResetTrigger(KDID);
            OpponentDetector.Actions.groundBounce = allowGroundBounce;
            OpponentDetector.Actions.wallBounce = allowWallBounce;


            if (allowWallStick && !usedWallStick && OpponentDetector.Actions.wallStick == 0)
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
        }

        //apply hitstun
        if (blitz && potentialHitStun == 0 && OpponentDetector.hitStun > 0)
        {
            OpponentDetector.hitStun += (float)(1/60);
        }
        else if (OpponentDetector.currentState.IsName("FDKnockdown") || OpponentDetector.currentState.IsName("FUKnockdown"))
        {
            OpponentDetector.hitStun = potentialHitStun / 120;
            OpponentDetector.Actions.groundBounce = false;
            OpponentDetector.Actions.wallBounce = false;
            OpponentDetector.Actions.wallStick = 0;
        }
        else
        {
            OpponentDetector.hitStun = potentialHitStun/60f;
            if (OpponentDetector.Actions.airborne && (usingSpecial||allowGroundBounce))
            {
                if (Actions.Move.OpponentProperties.comboTimer > 16)
                    OpponentDetector.hitStun *= .6f;
                else if (Actions.Move.OpponentProperties.comboTimer > 14)
                    OpponentDetector.hitStun *= .7f;
                else if (Actions.Move.OpponentProperties.comboTimer > 10)
                    OpponentDetector.hitStun *= .8f;
                else if (Actions.Move.OpponentProperties.comboTimer > 7)
                    OpponentDetector.hitStun = .9f;
            }
            else if (OpponentDetector.Actions.airborne && !usingSuper)
            {
                if (Actions.Move.OpponentProperties.comboTimer > 16)
                    OpponentDetector.hitStun = (float)(1/60);
                else if (Actions.Move.OpponentProperties.comboTimer > 14)
                    OpponentDetector.hitStun *= .6f;
                else if (Actions.Move.OpponentProperties.comboTimer > 10)
                    OpponentDetector.hitStun *= .7f;
                else if (Actions.Move.OpponentProperties.comboTimer > 7)
                    OpponentDetector.hitStun *= .8f;
                else if (Actions.Move.OpponentProperties.comboTimer > 5)
                    OpponentDetector.hitStun *= .9f;
            }

            if (OpponentDetector.anim.GetBool("Crouch"))
                OpponentDetector.hitStun += (float)(1/30);

            Debug.Log("Hit " + comboCount + "; Hitstun: " + OpponentDetector.hitStun*60 + " CurrentTime: " + Actions.Move.OpponentProperties.comboTimer);
            //increase hitstun upon landing a shatter or counter hit
            if ((OpponentDetector.Actions.shattered || OpponentDetector.Actions.attacking) && !piercing)
            {
                OpponentDetector.hitStun *= 2;
                if (OpponentDetector.Actions.shattered)
                {
                    //shatter graphic and voice clip
                }
                else
                {
                    counterSuccess = true;
                }
            }
            OpponentDetector.blockStun = 0;
        }

        if (usingSuper || blitz)
            usedWallStick = false;

        //apply knockback
        if ((potentialAirKnockBack != Vector2.zero || potentialKnockBack != Vector2.zero) && ProjectileKnockBack == Vector2.zero)
        {
            if (OpponentDetector.currentState.IsName("WallStick") && (potentialAirKnockBack.x < 0 || potentialKnockBack.x < 0))
            {
                if (potentialKnockBack.x < 0)
                    potentialKnockBack = new Vector2(-potentialKnockBack.x, potentialKnockBack.y);
                if (potentialAirKnockBack.x < 0)
                    potentialAirKnockBack = new Vector2(-potentialAirKnockBack.x, potentialAirKnockBack.y);
            }
            if (OpponentDetector.currentState.IsName("FDKnockdown")|| OpponentDetector.currentState.IsName("FUKnockdown"))
            {
                OpponentDetector.KnockBack = new Vector2(2f, 1.5f);
            }
            else if (OpponentDetector.currentState.IsName("Crumple"))
            {
                if (launch)
                {
                    OpponentDetector.anim.SetBool(launchID, true);
                    OpponentDetector.anim.SetTrigger(hitID);
                }
                else
                    OpponentDetector.anim.SetTrigger(hitAirID);

                if (potentialAirKnockBack.y < 0)
                {
                    OpponentDetector.KnockBack = potentialKnockBack;
                    if (potentialKnockBack.y == 0)
                        OpponentDetector.KnockBack += new Vector2(0, 2f);
                }
                else if (potentialAirKnockBack == Vector2.zero)
                {
                    OpponentDetector.KnockBack = potentialKnockBack;
                    if (potentialKnockBack.y == 0)
                        OpponentDetector.KnockBack += new Vector2(0, 2f);
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
                    if (OpponentDetector.KnockBack.y == 0)
                        OpponentDetector.KnockBack = new Vector2(OpponentDetector.KnockBack.x, 2f);
                }
                else
                {
                    OpponentDetector.KnockBack = potentialAirKnockBack;
                }
            }
            else
                OpponentDetector.KnockBack = potentialKnockBack;

            //apply pushback based on certain conditions
            if (!usingSpecial && !usingSuper && guard != "Unblockable")
            {
                if (Actions.Move.OpponentProperties.comboTimer < 2)
                    pushBackScale = .9f;
                else if (Actions.Move.OpponentProperties.comboTimer < 3)
                    pushBackScale = .95f;
                else if (Actions.Move.OpponentProperties.comboTimer < 5)
                    pushBackScale = 1f;
                else if (Actions.Move.OpponentProperties.comboTimer < 7)
                    pushBackScale = 1.05f;
                else if (Actions.Move.OpponentProperties.comboTimer < 10)
                    pushBackScale = 1.1f;
                else if (Actions.Move.OpponentProperties.comboTimer < 14)
                    pushBackScale = 1.15f;
                else if (Actions.Move.OpponentProperties.comboTimer < 16)
                    pushBackScale = 1.2f;
                else
                    pushBackScale = 1.25f;

                if (Actions.airborne)
                {
                    if (OpponentDetector.Actions.Move.hittingWall && OpponentDetector.Actions.airborne)
                        KnockBack = OpponentDetector.KnockBack * new Vector2(.35f, 0);
                    if (OpponentDetector.Actions.airborne && rb.velocity.y < 0)
                        KnockBack += OpponentDetector.KnockBack * new Vector2(0, .5f);
                }
                else
                {
                    if (OpponentDetector.KnockBack.x >= OpponentDetector.KnockBack.y)
                    {
                        if (OpponentDetector.KnockBack.x * pushBackScale > 2.5)
                            KnockBack = new Vector2(2.5f, 0);
                        else
                            KnockBack = OpponentDetector.KnockBack * new Vector2(pushBackScale, 0);
                    }
                    else
                    {
                        if (OpponentDetector.KnockBack.y * pushBackScale > 2.5f)
                            KnockBack = new Vector2(2.5f, 0);
                        else
                            KnockBack = new Vector2(OpponentDetector.KnockBack.y * pushBackScale, 0);
                    }

                    if (OpponentDetector.Actions.wallStick > 0 && OpponentDetector.Actions.Move.hittingWall)
                    {
                        KnockBack *= new Vector2(.7f, 1);
                    }
                    else if (OpponentDetector.Actions.Move.hittingWall)
                    {
                        //KnockBack *= new Vector2(1.5f, 1);
                        if (KnockBack.x < 1.6f && potentialKnockBack.x > 0)
                        {
                            KnockBack = new Vector2(1.6f, KnockBack.y);
                        }
                    }
                    else if (!OpponentDetector.Actions.Move.hittingWall)
                    {
                        KnockBack *= new Vector2(.9f, 1);
                    }
                }
            }


            if (Actions.airborne)
            {
                if (Actions.Move.transform.position.x > OpponentDetector.Actions.Move.transform.position.x)
                    OpponentDetector.KnockBack *= new Vector2(-1f, 1);
                else if (Actions.Move.transform.position.x < OpponentDetector.Actions.Move.transform.position.x)
                    KnockBack *= new Vector2(-1f, 1);
            }
            else
            {
                if (Actions.Move.facingRight)
                    KnockBack *= new Vector2(-1f, 1);
                else if (OpponentDetector.Actions.Move.facingRight)
                    OpponentDetector.KnockBack *= new Vector2(-1f, 1);
            }
        }

        if (!grab && potentialHitStun != 0 && !turnOffEffect)
            comboCount++;
    }

    void Clash(Collider2D other)
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

        hitEffect.transform.position = other.bounds.ClosestPoint(transform.position + new Vector3(hitBox1.offset.x, hitBox1.offset.y, 0));

        hitEffect.transform.GetChild(0).transform.localScale = Vector3.one;
        hitEffect.transform.rotation = Actions.Move.transform.rotation; ;
        hitEffect.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, -30);
        hitEffect.SetTrigger(clashID);
    }


    void ApplyHitStop(float i)
    {
        currentVelocity = rb.velocity;
        OpponentDetector.currentVelocity = OpponentDetector.rb.velocity;

        if ((Actions.Move.OpponentProperties.currentHealth <= 0|| RoundManager.suddenDeath) && !OpponentDetector.anim.GetBool(KOID) && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice")
        {
            hitStop = 1.5f;
            OpponentDetector.hitStop = hitStop;
            Actions.Move.OpponentProperties.currentHealth = 0;
            OpponentDetector.anim.SetBool(KOID, true);
            hitEffect.SetFloat(animSpeedID, 0);
        }
        else if (Actions.Move.OpponentProperties.currentHealth > 0 || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            hitStop = (potentialHitStop + i)/60;
            OpponentDetector.hitStop = hitStop;
            if (usingSuper || hitStop > 2f/5f)
                hitEffect.SetFloat(animSpeedID, 0);
        }
    }
}
