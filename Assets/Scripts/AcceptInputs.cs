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
    public bool acceptBurst = true;
    public bool jumpCancel = true;
    public bool blitzCancel = true;
    public bool airborne = false;
    public bool standing = true;
    public int superFlash;
    public bool armorActive = false;
    public bool attacking = false;
    public bool recovering = false;
    public bool throwInvincible = false;

    public bool bursting = false;
    public bool counterBursting = false;

    public bool shattered = false;
    public bool superHit = false;
    public int blitzed = 0;
    public int wallStick = 0;
    public int landingLag = 0;
    public bool groundBounce = false;
    public bool wallBounce = false;

    public bool grabbed = false;
    public bool throwTech = false;
    public bool backThrow = false;

    int throwInvulnCounter;

    public float gravScale = 1f;
    public int comboHits = 0;

    public Animator anim;
    public MovementHandler Move;
    public CharacterProperties CharProp;

    MaxInput MaxInput;
    SpriteRenderer sprite;
    MovementHandler opponentMove;

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
        //draws the defending character first to allow visibility on attacking character
        if (shattered && CharProp.currentHealth > 0)
            sprite.sortingOrder = 2;
        else if (comboHits > 0 || grabbed)
            sprite.sortingOrder = -2;
        else
            sprite.sortingOrder = 1;

        if (landingLag > 0)
        {
            DisableAll();
            anim.SetBool(crouchID, true);
            if (!airborne)
                landingLag--;
        }
        anim.SetInteger(landLagID, landingLag);

        if (anim.GetBool(dizzyID) || grabbed || Move.HitDetect.hitStun > 0 || anim.GetCurrentAnimatorStateInfo(0).IsName("Deflected"))
        {
            DisableAll();
            DisableBlitz();
            bursting = false;
            counterBursting = false;
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Deflected"))
            {
                armorActive = false;
                attacking = false;
                recovering = false;
            }
        }

        if (anim.GetBool(dizzyID) || grabbed || shattered || superHit)
            acceptBurst = false;

            if ((attacking || anim.GetBool(highGuardID) || anim.GetBool(lowGuardID) || anim.GetBool(runID)) && CharProp.armor > 0)
            armorActive = true;
        else
            armorActive = false;

        //characters are throw invincible for ten frames after throw teching
        if (throwInvulnCounter > 0)
        {
            throwInvincible = true;
            throwInvulnCounter--;
        }
        else if (throwInvulnCounter <= 0)
        {
            throwInvincible = false;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("ThrowReject"))
            throwInvulnCounter = 10;

        if (superFlash > 0)
            superFlash--;


        //change character properties based on current animation state
        if (airborne || anim.GetCurrentAnimatorStateInfo(0).IsName("SweepHit"))
            standing = false;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("IdleStand") || anim.GetCurrentAnimatorStateInfo(0).IsName("IdleCrouch") || anim.GetCurrentAnimatorStateInfo(0).IsName("StandUp"))
        {
            standing = true;           
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("FUGetup") || anim.GetCurrentAnimatorStateInfo(0).IsName("FDGetup"))
        {
            Guard();
            standing = true;
            Move.HitDetect.hitStun = 0;
        }

        if(wallStick == 0)
            anim.SetBool("WallStick", false);

        if (blitzed > 0 && Move.HitDetect.hitStop == 0 && !Move.HitDetect.pauseScreen.isPaused)
            blitzed--;

        anim.SetBool(airID, airborne);
        anim.SetBool(standID, standing);

        //increase gravScale based on hitting certain numbers with comboHits
        //keep track of hits in combo for damage and gravity scaling
        comboHits = Move.OpponentProperties.HitDetect.comboCount;
        if (comboHits == 0)
            gravScale = 1;
        else if (comboHits > 40)
            gravScale = 1.3f;
        else if (comboHits > 30)
            gravScale = 1.25f;
        else if (comboHits > 20)
            gravScale = 1.2f;
        else if (comboHits > 15)
            gravScale = 1.15f;
        else if (comboHits > 10)
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
        anim.SetBool(runID, false);
    }
    public void EnableAll()
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
        wallStick = 0;
        groundBounce = false;
        wallBounce = false;
        grabbed = false;
        throwInvincible = false;
        recovering = false;
        attacking = false;
    }

    public void Attacking()
    {
        attacking = true;
    }

    public void AttackActive()
    {
        attacking = false;
    }

    public void StopAttacking()
    {
        attacking = false;
        recovering = true;
    }

    public void StartBurst()
    {
        bursting = true;
    }

    public void EndBurst()
    {
        bursting = false;
    }

    public void StartCounterBurst()
    {
        counterBursting = true;
    }

    public void EndCounterBurst()
    {
        counterBursting = false;
    }

    public void StartSuperFlash(int i)
    {
        superFlash = i;
        Move.HitDetect.OpponentDetector.currentVelocity = Move.HitDetect.OpponentDetector.rb.velocity;
        Move.HitDetect.OpponentDetector.Actions.blitzed = 1;
    }

    public void DisableMovement()
    {
        acceptMove = false;
        acceptGuard = false;
    }

    public void SetLandLag(int x)
    {
        landingLag = x;
        anim.SetInteger(landLagID, landingLag);
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

    public void Rise(float y)
    {
        if (blitzed > 0)
            y /= 2;
        Move.rb.velocity = new Vector2(Move.rb.velocity.x, 0);
        Move.rb.AddForce(new Vector2(0, y), ForceMode2D.Impulse);
    }

    public void BurstRise(float y)
    {
        if (!airborne)
        {
            Move.rb.velocity = new Vector2(Move.rb.velocity.x, 0);
            Move.rb.AddForce(new Vector2(0, y), ForceMode2D.Impulse);
        }
    }

    public void ForceCrouch()
    {
        anim.SetBool(crouchID, true);
    }

    public void Recover()
    {
        Move.rb.velocity = new Vector2(.2f * Move.rb.velocity.x, 0);
        
        if (blitzed > 0)
        {
            if (MaxInput.GetAxis(Move.Horizontal) > 0)
                Move.HitDetect.KnockBack = new Vector2(.25f * Move.backDashForce, .25f * Move.jumpPower);
            else if (MaxInput.GetAxis(Move.Horizontal) < 0)
                Move.HitDetect.KnockBack = new Vector2(-.25f * Move.backDashForce, .25f * Move.jumpPower);
            else if (MaxInput.GetAxis(Move.Vertical) < 0 && transform.position.y > 1.5f)
                Move.HitDetect.KnockBack = new Vector2(0, -.25f * Move.jumpPower);
            else
                Move.HitDetect.KnockBack = new Vector2(0, .25f * Move.jumpPower);
        }
        else
        {
            if (MaxInput.GetAxis(Move.Horizontal) > 0)
                Move.HitDetect.KnockBack = new Vector2(.5f * Move.backDashForce, .5f * Move.jumpPower);
            else if (MaxInput.GetAxis(Move.Horizontal) < 0)
                Move.HitDetect.KnockBack = new Vector2(-.5f * Move.backDashForce, .5f * Move.jumpPower);
            else if (MaxInput.GetAxis(Move.Vertical) < 0 && transform.position.y > 1.5f)
                Move.HitDetect.KnockBack = new Vector2(0, -.5f * Move.jumpPower);
            else
                Move.HitDetect.KnockBack = new Vector2(0, .5f * Move.jumpPower);
        }  
    }

    public void Dash()
    {
        if (Move.facingRight)
            Move.rb.AddForce(new Vector2(-Move.backDashForce, .4f*Move.backDashForce), ForceMode2D.Impulse);
        else
            Move.rb.AddForce(new Vector2(Move.backDashForce, .4f*Move.backDashForce), ForceMode2D.Impulse);
            Move.backDash = false;
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

    public void StartGrab()
    {
        opponentMove.Actions.grabbed = true;
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
            if (Move.hittingWall || Move.transform.position.x - 10 < distance + 1 || Move.transform.position.x + 10 > distance + 1)
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
            if (opponentMove.hittingWall || Move.opponent.position.x - 9.8 < distance + 1 ||Move.opponent.position.x + 9.8 > distance + 1)
            {
                if (Move.facingRight)
                    Move.transform.position = new Vector3(Move.opponent.transform.position.x - distance, Move.transform.position.y, Move.transform.position.z);
                else
                    Move.transform.position = new Vector3(Move.opponent.transform.position.x + distance, Move.transform.position.y, Move.transform.position.z);
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

    public void GetUp()
    {
        if (CharProp.comboTimer < 200)
        {
            CharProp.armor = 2;
            CharProp.durability = 100;
        }
        else
        {
            for (float i = CharProp.comboTimer; i >= 100; i -= 100)
            {
                CharProp.armor++;
            }
            CharProp.durability = (int)CharProp.comboTimer % 100;
        }
    }
}
