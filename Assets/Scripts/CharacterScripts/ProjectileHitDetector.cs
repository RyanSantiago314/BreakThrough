using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitDetector : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public AcceptInputs Actions;
    public HitDetector HitDetect;
    public ProjectileProperties ProjProp;
    PauseMenu pauseScreen;

    public Collider2D hitBox1;

    public Vector2 currentVelocity;
    public float currentAngularVelocity;

    public int damage;
    public float initialProration;
    public float forcedProration;
    public Vector2 potentialKnockBack;
    public Vector2 potentialAirKnockBack;
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

    public float hitStop = 0;

    public bool blitz = false;
    public bool piercing = false;
    public bool launch = false;
    public bool crumple = false;
    public bool sweep = false;
    public bool forceCrouch = false;
    public bool forceStand = false;
    public bool shatter = false;
    public bool allowWallStick = false;
    public bool allowGroundBounce = false;
    public bool allowWallBounce = false;
    public bool usingSuper;
    public bool usingSpecial;

    public HitDetector OpponentDetector;

    bool allowHit = false;
    int collideCount = 0;
    public bool hit = false;
    public bool armorHit = false;
    float opponentValor;

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

    static int guardID;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

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

        guardID = Animator.StringToHash("Guard");

        pauseScreen = GameObject.Find("PauseManager").GetComponentInChildren<PauseMenu>();

        Actions = transform.root.GetChild(0).transform.GetComponentInChildren<AcceptInputs>();
        HitDetect = Actions.Move.HitDetect;
        OpponentDetector = HitDetect.OpponentDetector;
    }

    // Update is called once per frame
    void Update()
    {
        if (Actions == null || HitDetect == null || OpponentDetector == null)
        {
            Actions = transform.root.GetChild(0).transform.GetComponentInChildren<AcceptInputs>();
            HitDetect = Actions.Move.HitDetect;
            OpponentDetector = HitDetect.OpponentDetector;
        }
        opponentValor = Actions.Move.OpponentProperties.currentValor;

        if ((Input.GetButtonDown("Start_P1") || Input.GetButtonDown("Start_P2")) && pauseScreen.isPaused)
        {
            currentVelocity = rb.velocity;
            currentAngularVelocity = rb.angularVelocity;
        }

        if ((pauseScreen.isPaused && hitStop <= 0) || OpponentDetector.Actions.superFlash > 0)
        {
            hitStop = (float)1/60;
        }

        if (hitStop > 0)
        {
            //hitStop to give hits more impact and allow time to input next move
            anim.SetFloat(animSpeedID, 0f);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            if (!pauseScreen.isPaused)
                hitStop -= Time.deltaTime;
        }
        else
        {
            hitStop = 0;
            anim.SetFloat(animSpeedID, 1.0f);
            if (rb.constraints == RigidbodyConstraints2D.FreezeAll)
            {
                rb.constraints = RigidbodyConstraints2D.None;
            }

            if (OpponentDetector.Actions.blitzed > 0)
            {
                //simulate slow motion on projectile if within range of a blitz cancel or blitz attack
                if (OpponentDetector.Actions.blitzed <= (float)59/60  && OpponentDetector.Actions.blitzed >= (float)58 / 60)
                {
                    rb.velocity *= .5f;
                    rb.angularVelocity *= .75f;
                    rb.mass *= .5f;
                    rb.gravityScale *= .5f;
                }
                
                anim.SetFloat(animSpeedID, .5f);
            }
            else if (OpponentDetector.Actions.blitzed >= (float)1 / 60 && OpponentDetector.Actions.blitzed <= (float)2/60)
            {
                rb.velocity /=  .5f;
                rb.angularVelocity /= .5f;

                rb.mass /= .5f;
                rb.gravityScale /= .5f;
                anim.SetFloat(animSpeedID, 1f);
            }

            if (currentVelocity != Vector2.zero)
            {
                //retain velocity after hitStop occurs
                rb.velocity = currentVelocity;
                rb.angularVelocity = currentAngularVelocity;
                currentVelocity = Vector2.zero;
                currentAngularVelocity = 0;
            }
        }

        if (hitStop == 0)
        {
            anim.ResetTrigger(successID);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        collideCount++;
        if (allowHit && !OpponentDetector.Actions.projInvincible && !(OpponentDetector.anim.GetBool(crouchID) && guard == "High") && 
            !((guard == "Low" && OpponentDetector.Actions.lowInvincible) || ((guard == "Mid" || guard == "High" || guard == "Overhead") && OpponentDetector.Actions.hiInvincible)) &&
                other.gameObject.transform.parent.parent == Actions.Move.opponent && (potentialHitStun > 0 || blitz) && ProjProp.currentHits <= ProjProp.maxHits)
        {
            OpponentDetector.Actions.shattered = false;

            if ((guard == "Mid" && (OpponentDetector.anim.GetBool(LoGuard) || OpponentDetector.anim.GetBool(HiGuard) || OpponentDetector.anim.GetBool(AirGuard))) ||
                (guard == "Low" && (OpponentDetector.anim.GetBool(LoGuard) || OpponentDetector.anim.GetBool(AirGuard))) ||
                (guard == "Overhead" && OpponentDetector.anim.GetBool(HiGuard) || OpponentDetector.anim.GetBool(AirGuard)))
            {
                OpponentDetector.anim.SetTrigger("Blocked");
                OpponentDetector.blockStun = (float)(potentialHitStun - potentialHitStun / 600);
                if (OpponentDetector.blockStun > .5f)
                    OpponentDetector.blockStun = .5f;
                // guarding right as the attack lands (just defend) reduces blockstun and negates chip damage
                if (OpponentDetector.Actions.Move.justDefenseTime > 0 && OpponentDetector.Actions.standing)
                {
                    OpponentDetector.blockStun -= OpponentDetector.blockStun / 3;
                    OpponentDetector.Actions.CharProp.durability += 15;
                    Debug.Log("JUST DEFEND");
                }
                OpponentDetector.anim.SetFloat(blockStunID, OpponentDetector.blockStun);
                //what to do if an attack is blocked
                //mid can be guarded by any guard, lows must be guarded low, overheads must be guarded high
                //deal durability/chip damage equaling 10-20% of base damage
                //apply pushback to both by half of horizontal knockback value
                if (potentialKnockBack.x > potentialKnockBack.y)
                {
                    OpponentDetector.ProjectileKnockBack = potentialKnockBack * new Vector2(.5f, 0);
                }
                else
                {
                    OpponentDetector.ProjectileKnockBack = new Vector2(.5f * potentialKnockBack.y, 0);
                }

                if (OpponentDetector.anim.GetBool(AirGuard) && OpponentDetector.Actions.Move.justDefenseTime <= 0)
                    {
                        //apply special knockback to airborne guards
                        if (potentialAirKnockBack != Vector2.zero)
                        {
                            //guarding characters should never be spiked toward the ground
                            if (potentialAirKnockBack.y < 0)
                                OpponentDetector.ProjectileKnockBack = potentialAirKnockBack * new Vector2(.4f, 0) + new Vector2(0, .3f);
                            else
                                OpponentDetector.ProjectileKnockBack = potentialAirKnockBack * new Vector2(.4f, .5f);
                        }
                        else
                            OpponentDetector.ProjectileKnockBack += new Vector2(0f, .5f);

                        //double chip damage/durability damage on airguard
                        if (Actions.Move.OpponentProperties.armor > 0)
                        {
                            Actions.Move.OpponentProperties.durability -= damage / 3;
                        }
                        else
                        {
                            //guarding an attack without having any resolve results in getting shattered
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
                            ApplyHitStop(2 * potentialHitStop);

                            //chip damage
                            /*if (Actions.Move.OpponentProperties.currentHealth - damage/5 == 0 && Actions.Move.OpponentProperties.currentHealth > 1)
                                Actions.Move.OpponentProperties.currentHealth = 1;
                            else
                                Actions.Move.OpponentProperties.currentHealth -= damage/5;

                            if (Actions.Move.OpponentProperties.currentHealth <= 0)
                                OpponentDetector.anim.SetTrigger(hitID);*/

                        }
                    }
                    else if (OpponentDetector.Actions.Move.justDefenseTime <= 0 && OpponentDetector.Actions.standing)
                    {
                        if (Actions.Move.OpponentProperties.armor > 0)
                        {
                            //durability damage
                            Actions.Move.OpponentProperties.durability -= damage / 5;
                        }
                        else
                        {
                            //guarding an attack without having any resolve results in getting shattered
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
                            if (potentialHitStop > 0)
                                ApplyHitStop(2 * potentialHitStop);
                            else
                                ApplyHitStop(30);

                            //chip damage
                            /*if (Actions.Move.OpponentProperties.currentHealth - damage/10 == 0 && Actions.Move.OpponentProperties.currentHealth > 1)
                                Actions.Move.OpponentProperties.currentHealth = 1;
                            else
                                Actions.Move.OpponentProperties.currentHealth -= damage/10;

                            if (Actions.Move.OpponentProperties.currentHealth <= 0)
                                OpponentDetector.anim.SetTrigger(crumpleID);*/
                        }
                    }
                    if (OpponentDetector.blockStun > 0)
                    {
                        ApplyHitStop(0);

                        if (OpponentDetector.Actions.Move.justDefenseTime > 0 && OpponentDetector.Actions.standing)
                            OpponentDetector.ProjectileKnockBack *= .5f;

                        if (!Actions.Move.facingRight)
                            OpponentDetector.ProjectileKnockBack *= new Vector2(-1, 1);
                    }
            }
            else
            {
                if (jumpCancellable)
                {
                    Actions.jumpCancel = true;
                }

                if (shatter && (guard == "Unblockable" || Actions.Move.OpponentProperties.armor > 0) && (OpponentDetector.Actions.armorActive || OpponentDetector.Actions.recovering))
                {
                    //getting shattered means losing all your meter/armor
                    Actions.Move.OpponentProperties.armor = 0;
                    Actions.Move.OpponentProperties.durability = 0;
                    //trigger shatter effect
                    OpponentDetector.anim.SetTrigger(shatterID);
                    OpponentDetector.Actions.shattered = true;
                    Debug.Log("SHATTERED");
                    //damage, hitstun, etc.
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
                    HitSuccess(other);
                    ApplyHitStop(potentialHitStop / 2);
                }
                else if (!blitz && Actions.Move.OpponentProperties.armor > 0 && OpponentDetector.Actions.armorActive)
                {
                    //if the opponent has armor and is using it, deal armor and durability damage
                    Actions.Move.OpponentProperties.armor -= armorDamage;
                    Actions.Move.OpponentProperties.durability -= durabilityDamage;
                    ApplyHitStop(potentialHitStop/2);
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
        else if (allowHit && !blitz && other.transform.parent.CompareTag("Projectile") && other.CompareTag("HitBox"))
        {
            //clash/deflect system projectile vs projectile attack
            if ((other.transform.GetComponent<ProjectileHitDetector>().attackLevel - attackLevel) > 1 && potentialHitStun > 0)
            {
                //when one attack is more powerful than another, the weaker attack is deflected
                ApplyHitStop(2);
                Debug.Log("Projectile DEFLECTED!");
                other.transform.GetComponent<ProjectileHitDetector>().ProjProp.currentHits = other.transform.GetComponent<ProjectileHitDetector>().ProjProp.maxHits;
            }
            else if ((attackLevel - other.transform.GetComponent<ProjectileHitDetector>().attackLevel) <= 1 && potentialHitStun > 0)
            {
                //if the attacks are of similar strength both can immediately input another command
                Debug.Log("Projectile Clash!");
                ApplyHitStop(0);
                ProjProp.currentHits++;
                //no knockback on clashes
                Clash(other);
            }
            allowHit = false;
            hit = true;
        }
        else if (allowHit && !blitz && other.gameObject.transform.parent == Actions.Move.opponent && other.CompareTag("HitBox"))
        {
            //clash/deflect system projectile vs physical attack
            if (attackLevel > OpponentDetector.attackLevel && (attackLevel - OpponentDetector.attackLevel) > 1 && potentialHitStun > 0)
            {
                //when one attack is more powerful than another, the weaker attack is deflected and the winner is allowed to followup
                ApplyHitStop(2);
                Debug.Log("DEFLECTED! by projectile" + attackLevel);
                OpponentDetector.anim.SetTrigger(deflectID);
                HitDetect.anim.SetTrigger(parryID); 
                Actions.jumpCancel = true;
                OpponentDetector.Actions.CharProp.durabilityRefillTimer = 0;
                Contact(other);
            }
            else if ((attackLevel - OpponentDetector.attackLevel) <= 1 && potentialHitStun > 0)
            {
                //if the attacks are of similar strength both can immediately input another command
                Debug.Log("Clash!");
                ApplyHitStop(2);
                HitDetect.anim.SetTrigger(clashID);
                ProjProp.currentHits++;
                //no knockback on clashes
                Clash(other);
            }
            allowHit = false;
            hit = true;
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

    void HitSuccess(Collider2D other)
    {
        //if the attack successfully hit the opponent
        HitDetect.anim.SetTrigger(successID);
        OpponentDetector.Actions.Move.jumping = 0;
        OpponentDetector.Actions.TurnAroundCheck();
        OpponentDetector.Actions.superHit = false;

        //special properties if hitting a dizzied opponent
        if (OpponentDetector.anim.GetBool(dizzyID))
        {
            OpponentDetector.anim.SetBool(dizzyID, false);
            OpponentDetector.Actions.CharProp.refill = true;
            OpponentDetector.Actions.CharProp.comboTimer = 5;
            forceCrouch = true;
            HitDetect.specialProration = .55f;
        }

        if (forceCrouch && !OpponentDetector.Actions.airborne)
            OpponentDetector.anim.SetBool("Crouch", true);
        else if (forceStand && !OpponentDetector.Actions.airborne)
            OpponentDetector.anim.SetBool("Crouch", false);

        if (!(blitz && potentialHitStun == 0) && !OpponentDetector.Actions.grabbed)
        {
            OpponentDetector.anim.SetTrigger(hitID);
            if (OpponentDetector.Actions.standing && !launch && !sweep && !crumple && potentialKnockBack.y == 0)
            {
                OpponentDetector.anim.ResetTrigger(hitID);
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
            else if (!crumple && !sweep && !launch)
            {
                OpponentDetector.anim.SetTrigger(hitAirID);
            }
        }

        //calculate and deal damage
        damageToOpponent = damage * HitDetect.comboProration * opponentValor * HitDetect.specialProration;

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

        if ((piercing && OpponentDetector.Actions.armorActive) && OpponentDetector.Actions.CharProp.currentHealth - (int)damageToOpponent <= 0)
            OpponentDetector.Actions.CharProp.currentHealth = 1;
        else
            OpponentDetector.Actions.CharProp.currentHealth -= (int)damageToOpponent;

        minDamage = 0;

        //meter gain
        if (HitDetect.comboCount > 0)
        {
            OpponentDetector.Actions.CharProp.durability += damage / 10;
            if (Actions.CharProp.durabilityRefillTimer > 5)
                Actions.CharProp.durability += damage / 20;
        }

        // initialproration is applied if it is the first hit of a combo
        // some moves will force damage scaling in forcedProration
        if (HitDetect.comboCount == 0 && HitDetect.specialProration == 1)
            HitDetect.specialProration = initialProration;
        if (forcedProration > 0 && HitDetect.comboCount > 0)
            HitDetect.specialProration *= forcedProration;
        if (HitDetect.comboCount != 0 && HitDetect.comboCount < 11)
        {
            if (HitDetect.comboCount < 3)
                HitDetect.comboProration = 1;
            else if (HitDetect.comboCount < 4)
                HitDetect.comboProration = .8f;
            else if (HitDetect.comboCount < 5)
                HitDetect.comboProration = .7f;
            else if (HitDetect.comboCount < 6)
                HitDetect.comboProration = .6f;
            else if (HitDetect.comboCount < 7)
                HitDetect.comboProration = .5f;
            else if (HitDetect.comboCount < 8)
                HitDetect.comboProration = .4f;
            else if (HitDetect.comboCount < 9)
                HitDetect.comboProration = .3f;
            else if (HitDetect.comboCount < 10)
                HitDetect.comboProration = .2f;
            else if (HitDetect.comboCount < 11)
                HitDetect.comboProration = .1f;
        }



        //manipulate opponent's state based on attack properties
        //defender can enter unique states of stun if hit by an attack with corresponding property
        if (blitz && OpponentDetector.hitStun > 0)
        {
            OpponentDetector.Actions.blitzed = 60;
            Actions.Move.OpponentProperties.comboTimer -= 1.5f;
        }
        if (!OpponentDetector.Actions.grabbed)
        {
            if (launch)
            {
                OpponentDetector.anim.SetBool(launchID, true);
            }
            else if (crumple)
            {
                if (!OpponentDetector.Actions.airborne)
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
            OpponentDetector.Actions.groundBounce = allowGroundBounce;
            OpponentDetector.Actions.wallBounce = allowWallBounce;


            if (allowWallStick && !HitDetect.usedWallStick && OpponentDetector.Actions.wallStick == 0)
            {
                OpponentDetector.Actions.wallStick = 4;
                HitDetect.usedWallStick = true;
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
            OpponentDetector.hitStun += (float)1 /60;
        }
        else if (!OpponentDetector.Actions.grabbed)
        {
            OpponentDetector.hitStun = potentialHitStun/60;
            if (OpponentDetector.Actions.airborne && usingSpecial)
            {
                if (Actions.Move.OpponentProperties.comboTimer > 16)
                    OpponentDetector.hitStun = 7 * potentialHitStun / 600;
                else if (Actions.Move.OpponentProperties.comboTimer >= 13)
                    OpponentDetector.hitStun = 8 * potentialHitStun / 600;
                else if (Actions.Move.OpponentProperties.comboTimer > 10)
                    OpponentDetector.hitStun = 9 * potentialHitStun / 600;
            }
            else if (OpponentDetector.Actions.airborne && !usingSuper)
            {
                if (Actions.Move.OpponentProperties.comboTimer > 16)
                    OpponentDetector.hitStun = 6 * potentialHitStun / 600;
                else if (Actions.Move.OpponentProperties.comboTimer >= 13)
                    OpponentDetector.hitStun = 7 * potentialHitStun / 600;
                else if (Actions.Move.OpponentProperties.comboTimer > 10)
                    OpponentDetector.hitStun = 8 * potentialHitStun / 600;
                else if (Actions.Move.OpponentProperties.comboTimer > 7)
                    OpponentDetector.hitStun = 9 * potentialHitStun / 600;
            }

            if (OpponentDetector.anim.GetBool("Crouch"))
                OpponentDetector.hitStun += (float)1/30;
            //increase hitstun upon landing a shatter or counter hit
            if (OpponentDetector.Actions.shattered || OpponentDetector.Actions.attacking)
            {
                Actions.Move.OpponentProperties.comboTimer = 0;
                OpponentDetector.hitStun *= 2;
                if (OpponentDetector.Actions.shattered)
                {
                    //shatter graphic and voice clip
                }
                else
                {
                    //counter graphic on attacker's side and voice clip
                }
            }
            OpponentDetector.blockStun = 0;
        }

        //apply knockback
        if ((potentialAirKnockBack != Vector2.zero || potentialKnockBack != Vector2.zero) && !OpponentDetector.Actions.grabbed)
        {
            if (OpponentDetector.currentState.IsName("Crumple"))
            {
                OpponentDetector.anim.SetTrigger(hitAirID);
                if (potentialAirKnockBack.y < 0)
                {
                    OpponentDetector.ProjectileKnockBack = potentialKnockBack;
                    if (potentialKnockBack.y == 0)
                        OpponentDetector.ProjectileKnockBack += new Vector2(0, 1.5f);
                }
                else if (potentialAirKnockBack == Vector2.zero)
                {
                    OpponentDetector.ProjectileKnockBack = potentialKnockBack;
                    if (potentialKnockBack.y == 0)
                        OpponentDetector.ProjectileKnockBack += new Vector2(0, 1.5f);
                }
                else
                {
                    OpponentDetector.ProjectileKnockBack = potentialAirKnockBack;
                }
            }
            else if (OpponentDetector.Actions.airborne)
            {
                if (potentialAirKnockBack == Vector2.zero)
                {
                    OpponentDetector.ProjectileKnockBack = potentialKnockBack;
                    if (potentialKnockBack.y == 0)
                        OpponentDetector.ProjectileKnockBack += new Vector2(0, 1f);
                }
                else
                {
                    OpponentDetector.ProjectileKnockBack = potentialAirKnockBack;
                }
            }
            else
                OpponentDetector.ProjectileKnockBack = potentialKnockBack;

            if (Mathf.Abs(transform.position.x - OpponentDetector.Actions.Move.transform.position.x) < .3f || OpponentDetector.Actions.Move.hittingWall)
                OpponentDetector.ProjectileKnockBack *= new Vector2(0f, 1);
            else if (transform.position.x > OpponentDetector.Actions.Move.transform.position.x)
                OpponentDetector.ProjectileKnockBack *= new Vector2(-1f, 1);
        }

        if (potentialHitStun != 0)
            HitDetect.comboCount++;
    }

    public void Contact(Collider2D other)
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

        HitDetect.hitEffect.transform.position = other.bounds.ClosestPoint(transform.position + new Vector3(hitBox1.offset.x, hitBox1.offset.y, 0));
        if (piercing)
            HitDetect.hitEffect.SetInteger("AttackLevel", 4);
        else
            HitDetect.hitEffect.SetInteger("AttackLevel", attackLevel);

        HitDetect.HitFX.transform.rotation = Actions.Move.transform.rotation;
        if (OpponentDetector.Actions.shattered)
        {
            HitDetect.hitEffect.SetTrigger(shatterID);
            HitDetect.hitEffect.transform.position = OpponentDetector.Actions.Move.transform.position;
            if (OpponentDetector.Actions.Move.anim.GetBool("Crouch"))
                HitDetect.hitEffect.transform.position = new Vector3(HitDetect.hitEffect.transform.position.x, HitDetect.hitEffect.transform.position.y - .5f, HitDetect.hitEffect.transform.position.z);
            HitDetect.hitEffect.transform.GetChild(0).transform.localScale = new Vector3(2, 2, 1);
        }
        else if (OpponentDetector.armorHit)
        {
            HitDetect.hitEffect.SetTrigger(armorHitID);
        }
        else if (OpponentDetector.blockStun > 0)
        {
            HitDetect.hitEffect.SetTrigger(guardID);
        }
        else if (OpponentDetector.hitStun > 0)
        {
            if (HitDetect.vertSlash || HitDetect.horiSlash || HitDetect.slash)
            {
                HitDetect.hitEffect.SetTrigger("Slash");
                if (HitDetect.vertSlash)
                {
                    if (OpponentDetector.KnockBack.y > 2)
                        HitDetect.hitEffect.transform.localEulerAngles = new Vector3(0, 0, Random.Range(60f, 80f));
                    else
                        HitDetect.hitEffect.transform.localEulerAngles = new Vector3(0, 0, Random.Range(-60f, -80f));
                }
                else if (HitDetect.horiSlash)
                {
                    HitDetect.hitEffect.transform.localEulerAngles = new Vector3(0, 0, Random.Range(15f, 15f));
                }
                else
                {
                    HitDetect.hitEffect.transform.localEulerAngles = new Vector3(0, 0, Random.Range(-60f, 60f));
                    if (OpponentDetector.KnockBack.y > 2)
                        HitDetect.hitEffect.transform.localEulerAngles = new Vector3(0, 0, Random.Range(60f, 80f));
                }

                if (attackLevel < 2)
                    HitDetect.hitEffect.transform.GetChild(0).transform.localScale = new Vector3(Random.Range(.5f, .75f), Random.Range(-1f, 1f), 1);
                else
                    HitDetect.hitEffect.transform.GetChild(0).transform.localScale = new Vector3(Random.Range(1f, 1.5f), Random.Range(-1.5f, 1.5f), 1);

            }
            else
            {
                HitDetect.hitEffect.SetTrigger("Strike");
                if (attackLevel < 4)
                    HitDetect.hitEffect.transform.GetChild(0).transform.localScale = new Vector3(Random.Range(.75f, 1f), Random.Range(-1f, 1f), 1);
                else
                    HitDetect.hitEffect.transform.GetChild(0).transform.localScale = new Vector3(Random.Range(1f, 1.5f), Random.Range(-1.5f, 1.5f), 1);

                HitDetect.hitEffect.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0f, 359f));
            }
        }

        if (OpponentDetector.KnockBack.y > 2)
        {
            HitDetect.hitEffect.transform.GetChild(0).transform.eulerAngles = new Vector3(HitDetect.hitEffect.transform.eulerAngles.x, HitDetect.hitEffect.transform.eulerAngles.y, Random.Range(-30f, 30f));
        }

        if (OpponentDetector.hitStun <= 0)
            OpponentDetector.Actions.CharProp.durabilityRefillTimer = 0;

        anim.SetTrigger(successID);
        ProjProp.currentHits++;
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

        HitDetect.hitEffect.transform.position = other.bounds.ClosestPoint(transform.position + new Vector3(hitBox1.offset.x, hitBox1.offset.y, 0));

        HitDetect.hitEffect.transform.GetChild(0).transform.localScale = Vector3.one;
        HitDetect.hitEffect.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, -30);
        HitDetect.hitEffect.SetTrigger(clashID);
    }

    void ApplyHitStop(float i)
    {
        currentVelocity = rb.velocity;
        currentAngularVelocity = rb.angularVelocity;
        OpponentDetector.currentVelocity = OpponentDetector.rb.velocity;

        if (Actions.Move.OpponentProperties.currentHealth <= 0 && !OpponentDetector.anim.GetBool(KOID) && GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode != "Practice")
        {
            hitStop = 1.5f;
            HitDetect.hitStop = hitStop;
            OpponentDetector.hitStop = hitStop;
            Actions.Move.OpponentProperties.currentHealth = 0;
            HitDetect.hitEffect.SetFloat(animSpeedID, 0);
        }
        else if (Actions.Move.OpponentProperties.currentHealth > 0 || GameObject.Find("PlayerData").GetComponent<SelectedCharacterManager>().gameMode == "Practice")
        {
            hitStop = (potentialHitStop + i)/60;
            OpponentDetector.hitStop = hitStop;
            if (usingSuper)
                HitDetect.hitEffect.SetFloat(animSpeedID, 0);
        }
    }
}
