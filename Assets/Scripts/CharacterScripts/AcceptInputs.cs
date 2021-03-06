using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptInputs : MonoBehaviour
{
    //class that dictates what the character is currently capable of doing
    public bool acceptMove = true;
    public bool acceptGuard = true;
    public bool acceptLight = true;
    public bool acceptMedium = true;
    public bool acceptHeavy = true;
    public bool acceptBreak = true;
    public bool acceptSpecial = true;
    public bool acceptSuper = true;
    public bool jumpCancel = true;
    public bool blitzCancel = true;
    public bool airborne = false;
    public bool standing = true;
    public float superFlash;
    public bool armorActive = false;
    public bool attacking = false;
    public bool active = false;
    public bool recovering = false;
    public bool throwInvincible = false;
    public bool projInvincible = false;
    public bool hiInvincible = false;
    public bool lowInvincible = false;
    

    public bool hiCounter = false;
    public bool lowCounter = false;

    public bool shattered = false;
    public bool superHit = false;
    public float blitzed = 0;
    public int wallStick = 0;
    public float landingLag = 0;
    public bool groundBounce = false;
    public bool wallBounce = false;

    public bool grabbed = false;
    public float grabZoom;
    public bool throwTech = false;
    public bool backThrow = false;

    float throwInvulnCounter;
    public float originalGravity;
    public float gravScale = 1f;
    public int comboHits = 0;
    public string hitType = "";
    public bool screenShake;

    public Animator anim;
    public MovementHandler Move;
    public CharacterProperties CharProp;

    MaxInput MaxInput;
    SpriteRenderer sprite;
    MovementHandler opponentMove;

    AnimatorStateInfo currentState;

    static int airID;
    static int standID;
    static int crouchID;
    static int dizzyID;
    static int lowGuardID;
    static int highGuardID;
    static int airGuardID;
    static int runID;
    static int landLagID;

    float zPos;

    void Start()
    {
        Application.targetFrameRate = 60;

        airID = Animator.StringToHash("Airborne");
        standID = Animator.StringToHash("Standing");
        crouchID = Animator.StringToHash("Crouch");
        dizzyID = Animator.StringToHash("Dizzy");
        lowGuardID = Animator.StringToHash("LowGuard");
        highGuardID = Animator.StringToHash("HighGuard");
        airGuardID = Animator.StringToHash("AirGuard");
        runID = Animator.StringToHash("Run");
        landLagID = Animator.StringToHash("LandingLag");
        zPos = transform.position.z;

        sprite = GetComponent<SpriteRenderer>();
        opponentMove = Move.opponent.GetComponent<MovementHandler>();
        MaxInput = GameObject.Find("MaxInput").GetComponent<MaxInput>();
    }

    void Update()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        //draws the defending character first to allow visibility on attacking character
        if (shattered && CharProp.currentHealth > 0)
            sprite.sortingOrder = 2;
        else if (comboHits > 0 || grabbed)
            sprite.sortingOrder = -2;
        else
            sprite.sortingOrder = 1;

        if (landingLag > 0)
        {
            if (standing)
            {
                DisableAll();
                StopAttacking();
                anim.SetBool(crouchID, true);
                anim.SetFloat(landLagID, landingLag);
                landingLag -= Time.deltaTime;
            }
        }
        else
            anim.SetFloat(landLagID, -1);

        if (anim.GetBool(dizzyID) || grabbed || Move.HitDetect.hitStun > 0 || currentState.IsName("Deflected"))
        {
            DisableAll();
            DisableBlitz();
            if (!currentState.IsName("Crumple"))
            {
                hiInvincible = false;
                lowInvincible = false;
            }
            if (!currentState.IsName("Deflected"))
            {
                armorActive = false;
                attacking = false;
                recovering = false;
                active = false;
            }
        }
        else if (Move.HitDetect.blockStun > 0)
        {
            Guard();
        }

            if ((attacking || anim.GetBool(highGuardID) || anim.GetBool(lowGuardID)) && CharProp.armor > 0)
            armorActive = true;
        else
            armorActive = false;

        if (active)
        {
            hiInvincible = false;
            lowInvincible = false;
        }

        //characters are throw invincible for eight frames after throw teching
        if (throwInvulnCounter > 0)
        {
            throwInvincible = true;
            throwInvulnCounter -= Time.deltaTime;
        }
        else if (throwInvulnCounter <= 0)
        {
            throwInvincible = false;
        }

        if (currentState.IsName("ThrowReject") || currentState.IsName("FUGetup") || currentState.IsName("FDGetup"))
            throwInvulnCounter = (float)1/10;

        if (superFlash > 0 && !Move.HitDetect.pauseScreen.isPaused)
        {
            superFlash -= Time.deltaTime;
        }

        if (grabZoom > 0)
            grabZoom -= Time.deltaTime;


        //change character properties based on current animation state
        if (airborne || currentState.IsName("SweepHit"))
            standing = false;

        if (currentState.IsName("IdleStand") || currentState.IsName("IdleCrouch") || currentState.IsName("StandUp"))
        {
            standing = true;
        }
        else if ((currentState.IsName("FUKnockdown") || currentState.IsName("FDKnockdown")) && Move.HitDetect.hitStun > .5f)
        {
            Move.HitDetect.hitStun = .5f;
        }
        else if (currentState.IsName("FUGetup") || currentState.IsName("FDGetup"))
        {
            standing = true;
            Move.HitDetect.hitStun = 0;
            hiInvincible = true;
            lowInvincible = true;
        }

        if(wallStick == 0)
            anim.SetBool("WallStick", false);

        anim.SetBool(airID, airborne);
        anim.SetBool(standID, standing);

        //increase gravScale based on hitting certain numbers with comboHits
        //keep track of hits in combo for damage and gravity scaling
        comboHits = Move.OpponentProperties.HitDetect.comboCount;
        if (comboHits == 0 || superHit)
            gravScale = 1;
        else if (comboHits > 24)
            gravScale = 1.3f;
        else if (comboHits > 20)
            gravScale = 1.25f;
        else if (comboHits > 16)
            gravScale = 1.2f;
        else if (comboHits > 12)
            gravScale = 1.15f;
        else if (comboHits > 8)
            gravScale = 1.1f;
    }

    public void DisableAll()
    {
        acceptMove = false;
        acceptGuard = false;
        acceptLight = false;
        acceptMedium = false;
        acceptHeavy = false;
        acceptBreak = false;        
        acceptSpecial = false;
        acceptSuper = false;
        jumpCancel = false;
        throwTech = false;
        throwInvincible = false;
        CharProp.HitDetect.allowLight = false;
        CharProp.HitDetect.allowMedium = false;
        CharProp.HitDetect.allowHeavy = false;
        CharProp.HitDetect.allowBreak = false;
        CharProp.HitDetect.allowSpecial = false;
        CharProp.HitDetect.allowSuper = false;
        CharProp.HitDetect.jumpCancellable = false;
        CharProp.HitDetect.anim.SetBool(runID, false);
    }
    public void EnableAll()
    {
        if (landingLag > 0 && currentState.IsName("IdleCrouch"))
        {
            DisableAll();
        }
        else
        {
            acceptMove = true;
            acceptGuard = true;
            acceptLight = true;
            acceptMedium = true;
            acceptHeavy = true;
            acceptBreak = true;
            acceptSpecial = true;
            acceptSuper = true;
            jumpCancel = true;
            blitzCancel = true;
            gravScale = 1f;
            Move.HitDetect.hitStun = 0;
            Move.HitDetect.blockStun = 0;
            shattered = false;
            superHit = false;
            wallStick = 0;
            groundBounce = false;
            wallBounce = false;
            grabbed = false;
            throwInvincible = false;
            recovering = false;
            attacking = false;
            hiCounter = false;
            lowCounter = false;
            hiInvincible = false;
            lowInvincible = false;
        }
    }

    public void Attacking()
    {
        attacking = true;
    }

    public void AttackActive()
    {
        attacking = false;
        active = true;
    }

    public void StopAttacking()
    {
        attacking = false;
        active = false;
        recovering = true;
    }

    public void EnableScreenShake()
    {
        screenShake = true;
    }

    public void SigilJump()
    {
        if (airborne)
        {
            Move.sigil.GetComponent<Sigil>().colorChange = 0;
            Move.sigil.GetComponent<Sigil>().scaleChange = 0;
            Move.sigil.transform.position = new Vector3(Move.transform.position.x, Move.transform.position.y - .5f * Move.pushBox.size.y, Move.transform.position.z);
            Move.sigil.transform.eulerAngles = new Vector3(80, 0, 0);
        }
    }

    public void StartSuperFlash(float i)
    {
        superFlash = i/60;

        Move.HitDetect.OpponentDetector.currentVelocity = Move.HitDetect.OpponentDetector.rb.velocity;
        //Move.HitDetect.OpponentDetector.Actions.blitzed = (float)1/30;
    }

    public void DisableMovement()
    {
        acceptMove = false;
        acceptGuard = false;
    }

    public void InvincibleHigh()
    {
        hiInvincible = true;
    }

    public void InvincibleLow()
    {
        lowInvincible = true;
    }

    public void CounterHigh()
    {
        hiCounter = true;
    }

    public void CounterLow()
    {
        lowCounter = true;
    }

    public void CounterAll()
    {
        hiCounter = true;
        lowCounter = true;
    }

    public void StopCountering()
    {
        hiCounter = false;
        lowCounter = false;
    }

    public void SetLandLag(float x)
    {
        landingLag += x/60;
        anim.SetFloat(landLagID, landingLag);
    }

    public void DisableBlitz()
    {
        blitzCancel = false;
    }

    public void EnableBlitz()
    {
        blitzCancel = true;
    }

    public void EnableLight()
    {
        acceptLight = true;
    }

    public void EnableHeavy()
    {
        acceptHeavy = true;
    }

    public void Launch()
    {
        airborne = true;
        standing = false;
    }

    public void Grounded()
    {
        airborne = false;
        standing = true;
        Move.jumps = 0;
    }

    public void TurnAroundCheck()
    {
        if(Move.opponent.transform.position.x < transform.position.x - .1f)
            Move.facingRight = false;
        else if (Move.opponent.transform.position.x > transform.position.x + .1f)
            Move.facingRight = true;
    }

    public void Guard()
    {
        acceptMove = false;
        acceptGuard = true;
        acceptLight = false;
        acceptMedium = false;
        acceptHeavy = false;
        acceptBreak = false;        
        acceptSpecial = false;
        acceptSuper = false;
        jumpCancel = false;
        DisableBlitz();

        wallStick = 0;
        groundBounce = false;
        wallBounce = false;
        grabbed = false;
        throwInvincible = false;
    }

    public void ChangeHVelocity(float x)
    {
        if (Move.facingRight)
            Move.rb.velocity = new Vector2(x, Move.rb.velocity.y);
        else
            Move.rb.velocity = new Vector2(-x, Move.rb.velocity.y);
    }

    public void Advance(float x)
    {
        Move.rb.velocity = new Vector2(0, Move.rb.velocity.y);
        if (blitzed > 0)
        {
            if (Move.facingRight)
                Move.rb.AddForce(new Vector2(x * .5f, 0), ForceMode2D.Impulse);
            else
                Move.rb.AddForce(new Vector2(-x * .5f, 0), ForceMode2D.Impulse);
        }
        else
        {
            if (Move.facingRight)
                Move.rb.AddForce(new Vector2(x, 0), ForceMode2D.Impulse);
            else
                Move.rb.AddForce(new Vector2(-x, 0), ForceMode2D.Impulse);
        }
    }

    public void NormalAdvance(float x)
    {
        Move.rb.velocity = new Vector2(.75f * Move.rb.velocity.x, Move.rb.velocity.y);
        if (blitzed > 0)
        {
            if (Move.facingRight)
                Move.rb.AddForce(new Vector2(x * .5f, 0), ForceMode2D.Impulse);
            else
                Move.rb.AddForce(new Vector2(-x * .5f, 0), ForceMode2D.Impulse);
        }
        else
        {
            if (Move.facingRight)
                Move.rb.AddForce(new Vector2(x, 0), ForceMode2D.Impulse);
            else
                Move.rb.AddForce(new Vector2(-x, 0), ForceMode2D.Impulse);
        }
    }

    public void Rise(float y)
    {
        if (blitzed > 0)
            y /= 2;
        Move.rb.velocity = new Vector2(Move.rb.velocity.x, 0);
        Move.rb.AddForce(new Vector2(0, y), ForceMode2D.Impulse);
    }

    public void ForceCrouch()
    {
        anim.SetBool(crouchID, true);
    }

    public void ForceStand()
    {
        anim.SetBool(crouchID, false);
    }

    public void Recover()
    {
        Move.rb.velocity = new Vector2(.5f * Move.rb.velocity.x, .2f * Move.rb.velocity.y);
            if (MaxInput.GetAxis(Move.Horizontal) > 0)
                Move.HitDetect.KnockBack = new Vector2(.5f * Move.backDashForce, .5f * Move.jumpPower);
            else if (MaxInput.GetAxis(Move.Horizontal) < 0)
                Move.HitDetect.KnockBack = new Vector2(-.5f * Move.backDashForce, .5f * Move.jumpPower);
            else if (MaxInput.GetAxis(Move.Vertical) < 0 && transform.position.y > 1.5f)
                Move.HitDetect.KnockBack = new Vector2(0, -.3f * Move.jumpPower);
            else
                Move.HitDetect.KnockBack = new Vector2(0, .5f * Move.jumpPower);
    }

    public void Throwing()
    {
        DisableAll();
        throwTech = true;
    }

    public void ThrowTechFalse()
    {
        throwTech = false;
    }

    public void StartGrab(float i)
    {
        opponentMove.Actions.grabbed = true;
        grabZoom = i / 60;
    }

    public void EndGrab()
    {
        opponentMove.Actions.grabbed = false;
    }

    public void SetPosX(float distance)
    {
        if (Move.facingRight)
            Move.opponent.position = new Vector3(Move.transform.position.x + distance, Move.opponent.position.y, Move.opponent.position.z);
        else
            Move.opponent.position = new Vector3(Move.transform.position.x - distance, Move.opponent.position.y, Move.opponent.position.z);
    }

    public void SetPosY(float distance)
    {
        Move.opponent.position = new Vector3(Move.opponent.position.x, Move.transform.position.y + distance, Move.opponent.position.z);
    }

    public void BackThrowCheck(float distance)
    {
        if (backThrow)
        {
            if (Move.hittingWall || Move.transform.position.x + distance > 9.9f || Move.transform.position.x - distance < -9.9f)
            {
                if (Move.facingRight)
                {
                    Move.transform.position = new Vector3(Move.transform.position.x + distance, Move.transform.position.y, Move.transform.position.z);
                    Move.opponent.position = new Vector3(Move.transform.position.x - distance, Move.transform.position.y, Move.transform.position.z);
                }
                else
                {
                    Move.transform.position = new Vector3(Move.transform.position.x - distance, Move.transform.position.y, Move.transform.position.z);
                    Move.opponent.position = new Vector3(Move.transform.position.x + distance, Move.transform.position.y, Move.transform.position.z);
                }
            }
            else
            {
                if (Move.facingRight)
                    Move.opponent.position = new Vector3(Move.transform.position.x - distance, Move.opponent.position.y, Move.opponent.position.z);
                else
                    Move.opponent.position = new Vector3(Move.transform.position.x + distance, Move.opponent.position.y, Move.opponent.position.z);
            }
            backThrow = false;
        }
        else
        {
            if (opponentMove.hittingWall || Move.opponent.position.x + distance > 9.9f ||Move.opponent.position.x - distance < -9.9f)
            {
                if (Move.facingRight)
                {
                    Move.transform.position = new Vector3(Move.opponent.transform.position.x - 1.75f*distance, Move.transform.position.y, Move.transform.position.z);
                    Move.opponent.position = new Vector3(Move.transform.position.x + distance, Move.transform.position.y, Move.transform.position.z);
                }
                else
                {
                    Move.transform.position = new Vector3(Move.opponent.transform.position.x + 1.75f*distance, Move.transform.position.y, Move.transform.position.z);
                    Move.opponent.position = new Vector3(Move.transform.position.x - distance, Move.transform.position.y, Move.transform.position.z);
                }
            }
            else
            {
                if (Move.facingRight)
                    Move.opponent.position = new Vector3(Move.transform.position.x + distance, Move.transform.position.y, Move.transform.position.z);
                else
                    Move.opponent.position = new Vector3(Move.transform.position.x - distance, Move.transform.position.y, Move.transform.position.z);
            }

        }
        TurnAroundCheck();
        opponentMove.Actions.TurnAroundCheck();
    }

    public void setLow()
    {
        hitType = "Low";
    }

    public void setThrow()
    {
        hitType = "Throw";
    }

    public void clearHitType()
    {
        hitType = "";
    }
}
